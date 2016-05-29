using System;
using System.Net.Mail;
using System.Web.UI.WebControls;
using AccountingLib.Entities;
using AccountingLib.ReportMailing;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;
using DocMageFramework.DataManipulation;


namespace WebAccounting
{
    public partial class MailingSettings : System.Web.UI.Page
    {
        private SettingsMasterPage settingsMasterPage;

        private MailingDAO mailingDAO;


        protected void Page_Load(object sender, EventArgs e)
        {
            settingsMasterPage = (SettingsMasterPage)Page.Master;
            settingsMasterPage.InitializeMasterPageComponents();
            DataAccess dataAccess = settingsMasterPage.dataAccess;

            int mailingId;
            Boolean isNumeric = int.TryParse(Request.QueryString["mailingId"], out mailingId);
            if (!isNumeric)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os parâmetros passados para a página não estão em um formato válido.", true);
                return;
            }

            Tenant tenant = (Tenant)Session["tenant"];
            mailingDAO = new MailingDAO(dataAccess.GetConnection());
            Mailing mailing = mailingDAO.GetMailing(tenant.id, mailingId);
            if (mailing == null)
            {
                mailing = new Mailing();
                mailing.tenantId = tenant.id;
            }

            SettingsInput settingsInput = new SettingsInput(settingsArea, null);
            settingsInput.AddHidden("txtId", mailing.id.ToString());
            settingsInput.AddHidden("txtTenantId", mailing.tenantId.ToString());
            ListItem[] items = DropDownScaffold.RetrieveStrict("pr_retrieveSmtpServer", dataAccess.GetConnection(), typeof(SmtpServer));
            settingsInput.AddDropDownList("cmbSmtpServer", "Servidor SMTP", mailing.smtpServer, items);
            settingsInput.AddDropDownList("cmbFrequency", "Frequencia de Envio", mailing.frequency, typeof(ReportFrequencyEnum));
            settingsInput.AddDropDownList("cmbReportType", "Relatório", mailing.reportType, typeof(ReportTypeEnum));
            settingsInput.Add("txtRecipients", "Enviar para", mailing.recipients);
            settingsInput.AddHidden("txtLastSend", mailing.lastSend.ToShortDateString());
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Mailing mailing = new Mailing();
            try
            {
                foreach (String fieldName in Request.Form)
                {
                    if (fieldName.Contains("txtId"))
                        mailing.id = int.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("txtTenantId"))
                        mailing.tenantId = int.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("cmbSmtpServer"))
                        mailing.smtpServer = int.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("cmbFrequency"))
                        mailing.frequency = int.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("cmbReportType"))
                        mailing.reportType = int.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("txtRecipients"))
                    {
                        mailing.recipients = Request.Form[fieldName];

                        // Verifica o formato da string de destinatários, caso não seja um
                        // formato válido gera uma exceção (formatException)
                        if (String.IsNullOrEmpty(mailing.recipients))
                            throw new FormatException();
                        MailAddressCollection recipients = new MailAddressCollection();
                        recipients.Add(mailing.recipients); // gera formatException caso não estejam separados por vírgula
                    }
                    if (fieldName.Contains("txtLastSend"))
                        mailing.lastSend = DateTime.Parse(Request.Form[fieldName]);
                }
            }
            catch (System.FormatException)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os valores informados não estão em um formato válido!");
                return;
            }
            
            mailingDAO.SetMailing(mailing);
            EmbedClientScript.CloseWindow(this);
        }
    }

}
