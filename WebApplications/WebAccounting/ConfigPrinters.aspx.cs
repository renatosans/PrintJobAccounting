using System;
using System.Collections.Generic;
using AccountingLib.Security;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;
using DocMageFramework.Reflection;


namespace WebAccounting
{
    public partial class ConfigPrinters : System.Web.UI.Page
    {
        private AccountingMasterPage accountingMasterPage;

        private PrinterDAO printerDAO;


        private void ShowWarning(String warningMessage)
        {
            // Remove todos os controles da página
            configurationArea.Controls.Clear();
            controlArea.Controls.Clear();

            // Renderiza a mensagem na página
            WarningMessage.Show(controlArea, warningMessage);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            accountingMasterPage = (AccountingMasterPage)Page.Master;
            accountingMasterPage.InitializeMasterPageComponents();

            if (!Authorization.AuthorizedAsAdministrator(Session))
            {
                // Mostra aviso de falta de autorização
                ShowWarning(Authorization.GetWarning());
                return;
            }

            // action:
            //    null -  Sem ação, apenas lista as inpressoras
            //    0    -  Excluir impressora, lista as restantes
            int? action = null;
            int? printerId = null;
            try
            {
                if (!String.IsNullOrEmpty(Request.QueryString["action"]))
                    action = int.Parse(Request.QueryString["action"]);

                if (!String.IsNullOrEmpty(Request.QueryString["printerId"]))
                    printerId = int.Parse(Request.QueryString["printerId"]);
            }
            catch (System.FormatException)
            {
                // Remove todos os controles da página
                configurationArea.Controls.Clear();
                controlArea.Controls.Clear();

                // Mostra aviso de inconsistência nos parâmetros
                WarningMessage.Show(controlArea, ArgumentBuilder.GetWarning());
                return;
            }

            Tenant tenant = (Tenant)Session["tenant"];
            printerDAO = new PrinterDAO(accountingMasterPage.dataAccess.GetConnection());

            if (printerId != null)
                switch (action)
                {
                    case 0:
                        printerDAO.RemovePrinter(printerId.Value);
                        Response.Redirect("ConfigPrinters.aspx"); // Limpa a QueryString para evitar erros
                        break;
                    default:
                        break;
                }

            List<Object> printerList = printerDAO.GetAllPrinters(tenant.id);

            String[] columnNames = new String[] { "Impressora", "Custo página Pb", "Custo página Cor" };
            String alterScript = "window.open('PrinterSettings.aspx?printerId=' + {0}, 'Settings', 'width=540,height=600');";
            String removeScript = "var confirmed = confirm('Deseja realmente excluir esta impressora?'); if (confirmed) window.location='ConfigPrinters.aspx?action=0&printerId=' + {0};";
            EditableListButton[] buttons = new EditableListButton[]
            {
                // Botões que devem aparecer para os items da lista
                new EditableListButton("Editar", alterScript, ButtonTypeEnum.Edit),
                new EditableListButton("Excluir", removeScript, ButtonTypeEnum.Remove)
            };
            EditableList editableList = new EditableList(configurationArea, columnNames, buttons);
            foreach (Printer printer in printerList)
            {
                String[] printerProperties = new String[]
                {
                    printer.alias,
                    String.Format("{0:0.000}", printer.pageCost),
                    String.Format("{0:0.000}", printer.pageCost + printer.colorCostDiff)
                };
                editableList.InsertItem(printer.id, false, printerProperties);
            }
            editableList.DrawList();
        }
    }

}
