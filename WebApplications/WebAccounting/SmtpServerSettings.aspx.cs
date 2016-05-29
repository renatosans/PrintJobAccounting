using System;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.AppUtils;
using DocMageFramework.WebUtils;


namespace WebAccounting
{
    public partial class SmtpServerSettings : System.Web.UI.Page
    {
        private SettingsMasterPage settingsMasterPage;

        private SmtpServerDAO smtpServerDAO;


        protected void Page_Load(object sender, EventArgs e)
        {
            settingsMasterPage = (SettingsMasterPage)Page.Master;
            settingsMasterPage.InitializeMasterPageComponents();

            int smtpServerId;
            Boolean isNumeric = int.TryParse(Request.QueryString["smtpServerId"], out smtpServerId);
            if (!isNumeric)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os parâmetros passados para a página não estão em um formato válido.", true);
                return;
            }

            Tenant tenant = (Tenant)Session["tenant"];
            smtpServerDAO = new SmtpServerDAO(settingsMasterPage.dataAccess.GetConnection());
            SmtpServer smtpServer = smtpServerDAO.GetSmtpServer(tenant.id, smtpServerId);
            if (smtpServer == null)
            {
                smtpServer = new SmtpServer();
                smtpServer.tenantId = tenant.id;
            }

            SettingsInput settingsInput = new SettingsInput(settingsArea, null);
            settingsInput.AddHidden("txtId", smtpServer.id.ToString());
            settingsInput.AddHidden("txtTenantId", smtpServer.tenantId.ToString());
            settingsInput.Add("txtName", "Nome", smtpServer.name);
            settingsInput.Add("txtAddress", "Endereço", smtpServer.address);
            settingsInput.Add("txtPort", "Porta", smtpServer.port.ToString());
            settingsInput.Add("txtUsername", "Usuário", smtpServer.username);
            settingsInput.Add("txtPassword", "Senha", smtpServer.password, true, null);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Tenant tenant = (Tenant)Session["tenant"];
            SmtpServer smtpServer = new SmtpServer();
            try
            {
                foreach (String fieldName in Request.Form)
                {
                    if (fieldName.Contains("txtId"))
                        smtpServer.id = int.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("txtTenantId"))
                        smtpServer.tenantId = int.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("txtName"))
                    {
                        smtpServer.name = Request.Form[fieldName];
                        if (String.IsNullOrEmpty(smtpServer.name))
                            throw new FormatException();
                    }
                    if (fieldName.Contains("txtAddress"))
                    {
                        smtpServer.address = Request.Form[fieldName];
                        if (String.IsNullOrEmpty(smtpServer.address))
                            throw new FormatException();
                    }
                    if (fieldName.Contains("txtPort"))
                        smtpServer.port = int.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("txtUsername"))
                        smtpServer.username = Request.Form[fieldName];
                    if (fieldName.Contains("txtPassword"))
                        smtpServer.password = Request.Form[fieldName];
                }
            }
            catch (System.FormatException)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os valores informados não estão em um formato válido!");
                return;
            }
            smtpServer.hash = Cipher.GenerateHash(tenant.name + smtpServer.name);

            smtpServerDAO.SetSmtpServer(smtpServer);
            EmbedClientScript.CloseWindow(this);
        }
    }

}
