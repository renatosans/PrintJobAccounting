using System;
using System.Web.UI;


namespace DocMageFramework.WebUtils
{
    public static class EmbedClientScript
    {
        public static void ShowErrorMessage(Page page, String message)
        {
            // por default "não fecha" a janela após exibir a mensagem de erro
            ShowErrorMessage(page, message, false);
        }

        public static void ShowErrorMessage(Page page, String message, Boolean closeWindow)
        {
            // Remove aspas simples e aspas duplas da mensagem para evitar erros de javascript
            message = message.Replace("'", "");
            message = message.Replace("\"", "");
            message = message.Replace(Environment.NewLine, "");

            ClientScriptManager script = page.ClientScript;
            if (!script.IsClientScriptBlockRegistered(page.GetType(), "ShowErrorMessage"))
            {
                String closeScript = "";
                if (closeWindow) closeScript = "self.close();";

                script.RegisterClientScriptBlock(page.GetType(), "ShowErrorMessage",
                "<script type='text/javascript'>window.onload=function(){ alert('" + message + "');" + closeScript + " };</script>");
            }
        }

        public static void CloseWindow(Page page)
        {
            // por default recarrega a janela principal (refresh na página) ao fechar a janela filha
            CloseWindow(page, true);
        }

        public static void CloseWindow(Page page, Boolean reloadMainWindow)
        {
            ClientScriptManager script = page.ClientScript;
            if (!script.IsClientScriptBlockRegistered(page.GetType(), "CloseWindow"))
            {
                // A função window.reload tem um efeito indesejado, por isso é usado o replace em seu lugar
                String reloadScript = "";
                if (reloadMainWindow) reloadScript = "opener.location.replace(opener.location);";

                script.RegisterClientScriptBlock(page.GetType(), "CloseWindow",
                "<script type='text/javascript'>" + reloadScript + "self.close();</script>");
            }
        }

        public static void PrintWindow(Page page)
        {
            ClientScriptManager script = page.ClientScript;
            if (!script.IsClientScriptBlockRegistered(page.GetType(), "PrintWindow"))
            {
                script.RegisterClientScriptBlock(page.GetType(), "PrintWindow",
                "<script type='text/javascript'>window.print();</script>");
            }
        }

        public static void AddButtonClickHandler(Page page, String action)
        {
            // Cria o form action para o click do botão (desconsidera a queryString pré-existente)
            String formAction = URLHandler.InsertQueryVariable(page.Request.Path, "action=" + action);

            String scriptBody = "function " + action + "Click() {" +
                                "    document.forms[0].action = '" + formAction + "';" +
                                "    document.forms[0].submit();" +
                                "}";

            ClientScriptManager script = page.ClientScript;
            if (!script.IsClientScriptBlockRegistered(page.GetType(), "AddButtonClickHandler"))
            {
                script.RegisterClientScriptBlock(page.GetType(), "AddButtonClickHandler",
                "<script type='text/javascript'>" + scriptBody + "</script>");
            }
        }

        public static void AddElementClickHandler(Page page, String elementName, String commands)
        {
            String scriptBody = "function " + elementName + "Click() {" + commands + "}";

            ClientScriptManager script = page.ClientScript;
            if (!script.IsClientScriptBlockRegistered(page.GetType(), "AddElementClickHandler"))
            {
                script.RegisterClientScriptBlock(page.GetType(), "AddElementClickHandler",
                "<script type='text/javascript'>" + scriptBody + "</script>");
            }
        }
    }

}
