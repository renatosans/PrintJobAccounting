using System;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.Security;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;


namespace WebAccounting
{
    public partial class Licenses : System.Web.UI.Page
    {
        private AccountingMasterPage accountingMasterPage;


        private void ShowWarning(String warningMessage)
        {
            // Remove todos os controles da página
            displayArea.Controls.Clear();

            // Renderiza a mensagem na página
            WarningMessage.Show(displayArea, warningMessage);
        }

        protected void Page_Load(Object sender, EventArgs e)
        {
            accountingMasterPage = (AccountingMasterPage)Page.Master;
            accountingMasterPage.InitializeMasterPageComponents();

            if (!Authorization.AuthorizedAsAdministrator(Session))
            {
                // Mostra aviso de falta de autorização
                ShowWarning(Authorization.GetWarning());
                return;
            }

            Tenant tenant = (Tenant)Session["tenant"];
            LicenseDAO licenseDAO = new LicenseDAO(accountingMasterPage.dataAccess.GetConnection());
            List<Object> licenseList = licenseDAO.GetAllLicenses(tenant.id);

            String[] columnNames = new String[] { "Id da Licença", "Chave Instalação(CPU/HD)", "Data Instalação", "Nome do Computador" };
            String downloadScript = "window.location.replace('LicenseFile.aspx?licenseId=' + {0});";
            EditableListButton[] buttons = new EditableListButton[]
            {
                // Botões que devem aparecer para os items da lista
                new EditableListButton("Download", downloadScript, ButtonTypeEnum.Download)
            };
            EditableList editableList = new EditableList(displayArea, columnNames, buttons);
            foreach (License license in licenseList)
            {
                String licenseId = license.id.ToString();
                if (license.id < 10000) licenseId = String.Format("{0:0000}", license.id);

                String installationKey = "-";
                if (license.installationKey != null) installationKey = license.installationKey;
                String installationDate = "-";
                if (license.installationDate != null) installationDate = license.installationDate.Value.ToString("dd/MM/yyyy");
                String computerName = "-";
                if (license.computerName != null) computerName = license.computerName;

                String[] licenseProperties = new String[]
                {
                    licenseId,
                    installationKey,
                    installationDate,
                    computerName
                };
                editableList.InsertItem(license.id, false, licenseProperties);
            }
            editableList.DrawList();
        }
    }

}
