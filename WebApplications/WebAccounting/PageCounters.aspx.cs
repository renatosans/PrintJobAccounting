using System;
using System.Collections.Generic;
using AccountingLib.Security;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;
using DocMageFramework.Reflection;


namespace WebAccounting
{
    public partial class PageCounters : System.Web.UI.Page
    {
        private AccountingMasterPage accountingMasterPage;

        private PrintingDeviceDAO printingDeviceDAO;


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
            //    null -  Sem ação, apenas lista os dispositivos
            //    0    -  Excluir dispositivo, lista os restantes
            int? action = null;
            int? deviceId = null;
            try
            {
                if (!String.IsNullOrEmpty(Request.QueryString["action"]))
                    action = int.Parse(Request.QueryString["action"]);

                if (!String.IsNullOrEmpty(Request.QueryString["deviceId"]))
                    deviceId = int.Parse(Request.QueryString["deviceId"]);
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
            printingDeviceDAO = new PrintingDeviceDAO(accountingMasterPage.dataAccess.GetConnection());

            if (deviceId != null)
                switch (action)
                {
                    case 0:
                        printingDeviceDAO.RemovePrintingDevice(deviceId.Value);
                        Response.Redirect("PageCounters.aspx");  // Limpa a QueryString para evitar erros
                        break;
                    default:
                        break;
                }

            List<Object> deviceList = printingDeviceDAO.GetAllPrintingDevices(tenant.id);

            String[] columnNames = new String[] { "Endereço IP", "Descrição", "Número de série", "Contador", "Atualizado Em" };
            String viewScript = "window.open('PageCounterHistory.aspx?deviceId=' + {0}, 'Histórico do contador', 'width=540,height=600');";
            String removeScript = "var confirmed = confirm('Deseja realmente excluir este dispositivo?'); if (confirmed) window.location='PageCounters.aspx?action=0&deviceId=' + {0};";
            EditableListButton[] buttons = new EditableListButton[]
            {
                // Botões que devem aparecer para os items da lista
                new EditableListButton("Histórico", viewScript, ButtonTypeEnum.Edit),
                new EditableListButton("Excluir", removeScript, ButtonTypeEnum.Remove)
            };
            EditableList editableList = new EditableList(configurationArea, columnNames, buttons);
            foreach(PrintingDevice device in deviceList)
            {
                String[] deviceProperties = new String[]
                {
                    device.ipAddress,
                    device.description,
                    device.serialNumber,
                    device.counter.ToString(),
                    String.Format("{0:dd/MM/yyyy HH:mm}", device.lastUpdated)
                };
                editableList.InsertItem(device.id, false, deviceProperties);
            }
            editableList.DrawList();
        }
    }

}
