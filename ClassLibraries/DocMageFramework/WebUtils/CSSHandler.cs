using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;


namespace DocMageFramework.WebUtils
{
    public static class CSSHandler
    {
        public static void ReplaceStyles(WebControl control, NameValueCollection styles)
        {
            // Sai do médodo caso a lista de estilos seja nula
            if (styles == null)
                return;

            control.Style.Clear();
            foreach (String key in styles)
                control.Style.Add(key, styles[key]);
        }
    }

}
