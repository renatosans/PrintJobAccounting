using System;
using System.Net.Mail;
using AccountingLib.Security;
using AccountingLib.Management;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;
using DocMageFramework.DataManipulation;


namespace WebAccounting
{
    public partial class ConfigPreferences : System.Web.UI.Page
    {
        private AccountingMasterPage accountingMasterPage;

        private DataAccess dataAccess;

        private PreferenceDAO preferenceDAO;


        protected void Page_Load(object sender, EventArgs e)
        {
            accountingMasterPage = (AccountingMasterPage)Page.Master;
            accountingMasterPage.InitializeMasterPageComponents();
            dataAccess = accountingMasterPage.dataAccess;

            if (!Authorization.AuthorizedAsAdministrator(Session))
            {
                // Remove todos os controles da página
                configurationArea.Controls.Clear();
                controlArea.Controls.Clear();

                // Mostra aviso de falta de autorização
                WarningMessage.Show(controlArea, Authorization.GetWarning());
                return;
            }
            
            Tenant tenant = (Tenant)Session["tenant"];
            SettingsInput tenantInput = new SettingsInput(pnlTenant, null);
            tenantInput.Add("txtTenantAlias", "Nome amigável", tenant.alias);

            preferenceDAO = new PreferenceDAO(dataAccess.GetConnection());
            Preference sysSender = preferenceDAO.GetTenantPreference(tenant.id, "sysSender");
            Preference exportFormat = preferenceDAO.GetTenantPreference(tenant.id, "exportFormat");
            if (exportFormat == null)
            {
                // Se não existe no banco cria a entrada
                exportFormat = new Preference();
                exportFormat.id = 0;
                exportFormat.value = "0"; 
            }
            Preference periodEndDate = preferenceDAO.GetTenantPreference(tenant.id, "periodEndDate");
            if (periodEndDate == null)
            {
                // Se não existe no banco cria a entrada
                periodEndDate = new Preference();
                periodEndDate.id = 0;
                periodEndDate.value = "1";
            }

            SettingsInput tenantPreferencesInput = new SettingsInput(pnlTenantPreferences, null);
            tenantPreferencesInput.AddHidden("txtSysSenderId", sysSender.id.ToString());
            tenantPreferencesInput.Add("txtSysSenderValue", "Remetente e-mails do sistema", sysSender.value);
            tenantPreferencesInput.AddHidden("txtExportFormatId", exportFormat.id.ToString());
            tenantPreferencesInput.AddDropDownList("cmbExportFormatValue", "Formato de exportação", int.Parse(exportFormat.value), typeof(ExportFormatEnum));
            tenantPreferencesInput.AddHidden("txtPeriodEndDateId", periodEndDate.id.ToString());
            tenantPreferencesInput.AddDropDownList("cmbPeriodEndDateValue", "Fechamento de período", int.Parse(periodEndDate.value), typeof(PeriodDelimiterEnum));
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Tenant tenant = (Tenant)Session["tenant"];

            Preference sysSender = new Preference();
            sysSender.tenantId = tenant.id;
            sysSender.name = "sysSender";            
            sysSender.type = "System.String";

            Preference exportFormat = new Preference();
            exportFormat.tenantId = tenant.id;
            exportFormat.name = "exportFormat";
            exportFormat.type = "System.Int32";

            Preference periodEndDate = new Preference();
            periodEndDate.tenantId = tenant.id;
            periodEndDate.name = "periodEndDate";
            periodEndDate.type = "System.Int32";

            foreach (String fieldName in Request.Form)
            {
                if (fieldName.Contains("txtTenantAlias"))
                    tenant.alias = Request.Form[fieldName];
                
                if (fieldName.Contains("txtSysSenderId"))
                    sysSender.id = int.Parse(Request.Form[fieldName]);
                if (fieldName.Contains("txtSysSenderValue"))
                    sysSender.value = Request.Form[fieldName];

                if (fieldName.Contains("txtExportFormatId"))
                    exportFormat.id = int.Parse(Request.Form[fieldName]);
                if (fieldName.Contains("cmbExportFormatValue"))
                    exportFormat.value = Request.Form[fieldName];

                if (fieldName.Contains("txtPeriodEndDateId"))
                    periodEndDate.id = int.Parse(Request.Form[fieldName]);
                if (fieldName.Contains("cmbPeriodEndDateValue"))
                    periodEndDate.value = Request.Form[fieldName];
            }

            // Verifica se os campos foram preenchidos
            if ((String.IsNullOrEmpty(tenant.alias)) || (String.IsNullOrEmpty(sysSender.value)))
            {
                EmbedClientScript.ShowErrorMessage(this, "Favor preencher todos os campos!");
            }

            try
            {
                // Verifica o formato do endereço de e-mail
                MailAddress mailAddress = new MailAddress(sysSender.value);
            }
            catch
            {
                EmbedClientScript.ShowErrorMessage(this, "O endereço de e-mail não está em um formato válido!");
            }

            TenantDAO tenantDAO = new TenantDAO(dataAccess.GetConnection());
            tenantDAO.SetTenant(tenant);
            preferenceDAO.SetTenantPreference(sysSender);
            preferenceDAO.SetTenantPreference(exportFormat);
            preferenceDAO.SetTenantPreference(periodEndDate);
        }
    }

}
