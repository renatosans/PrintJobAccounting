using System;
using AccountingLib.Management;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.Parsing;
using DocMageFramework.WebUtils;
using DocMageFramework.DataManipulation;


namespace WebAdministrator
{
    public partial class TenantSettings : System.Web.UI.Page
    {
        private AdministratorMasterPage administratorMasterPage;

        private DataAccess dataAccess;


        protected void Page_Load(Object sender, EventArgs e)
        {
            administratorMasterPage = (AdministratorMasterPage)Page.Master;
            administratorMasterPage.InitializeMasterPageComponents();
            dataAccess = administratorMasterPage.dataAccess;

            int tenantId;
            Boolean isNumeric = int.TryParse(Request.QueryString["tenantId"], out tenantId);
            if (!isNumeric)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os parâmetros passados para a página não estão em um formato válido.", true);
                return;
            }

            TenantDAO tenantDAO = new TenantDAO(dataAccess.GetConnection());
            Tenant tenant = tenantDAO.GetTenant(tenantId);
            // Caso a empresa não exista cria uma nova
            if (tenant == null) tenant = new Tenant();

            SettingsInput settingsInput = new SettingsInput(settingsArea, null);
            settingsInput.AddHidden("txtId", tenant.id.ToString());
            settingsInput.Add("txtName", "Identificador", tenant.name);
            settingsInput.Add("txtAlias", "Nome amigável", tenant.alias);
        }

        protected void btnSubmit_Click(Object sender, EventArgs e)
        {
            Tenant tenant = new Tenant();
            foreach (String fieldName in Request.Form)
            {
                if (fieldName.Contains("txtId"))
                    tenant.id = int.Parse(Request.Form[fieldName]);
                if (fieldName.Contains("txtName"))
                    tenant.name = Request.Form[fieldName];
                if (fieldName.Contains("txtAlias"))
                    tenant.alias = Request.Form[fieldName];
            }

            if ((String.IsNullOrEmpty(tenant.name)) || (String.IsNullOrEmpty(tenant.alias)))
            {
                EmbedClientScript.ShowErrorMessage(this, "Os valores informados não estão em um formato válido!");
                return;
            }

            if (!FieldParser.IsAlphaNumeric(tenant.name)) 
            {
                EmbedClientScript.ShowErrorMessage(this, "O identificador deve conter apenas letras e números!");
                return;
            }

            // Executa o conjunto de operações para a criação do Tenant
            TenantTransaction transaction = new TenantTransaction(tenant, dataAccess.GetConnection());
            transaction.Execute();
            EmbedClientScript.CloseWindow(this);
        }
    }

}
