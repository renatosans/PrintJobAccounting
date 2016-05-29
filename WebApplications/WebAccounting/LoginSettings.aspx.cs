using System;
using AccountingLib.Security;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;


namespace WebAccounting
{
    public partial class LoginSettings : System.Web.UI.Page
    {
        private SettingsMasterPage settingsMasterPage;

        private LoginDAO loginDAO;


        protected void Page_Load(object sender, EventArgs e)
        {
            settingsMasterPage = (SettingsMasterPage)Page.Master;
            settingsMasterPage.InitializeMasterPageComponents();

            int loginId;
            Boolean isNumeric = int.TryParse(Request.QueryString["loginId"], out loginId);
            if (!isNumeric)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os parâmetros passados para a página não estão em um formato válido.", true);
                return;
            }

            Tenant tenant = (Tenant)Session["tenant"];
            loginDAO = new LoginDAO(settingsMasterPage.dataAccess.GetConnection());
            AccountingLib.Entities.Login login = loginDAO.GetLogin(tenant.id, loginId);
            if (login == null)
            {
                login = new AccountingLib.Entities.Login();
                login.tenantId = tenant.id;
            }

            SettingsInput settingsInput = new SettingsInput(settingsArea, null);
            settingsInput.AddHidden("txtId", login.id.ToString());
            settingsInput.AddHidden("txtTenantId", login.tenantId.ToString());
            settingsInput.AddHidden("txtPassword", login.password);
            settingsInput.Add("txtUsername", "Login", login.username);
            settingsInput.AddDropDownList("cmbUserGroup", "Grupo/Permissão", login.userGroup, typeof(UserGroupEnum));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            AccountingLib.Entities.Login login = new AccountingLib.Entities.Login();
            try
            {
                foreach (String fieldName in Request.Form)
                {
                    if (fieldName.Contains("txtId"))
                        login.id = int.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("txtTenantId"))
                        login.tenantId = int.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("txtPassword"))
                        login.password = Request.Form[fieldName];
                    if (fieldName.Contains("txtUsername"))
                    {
                        login.username = Request.Form[fieldName];
                        if (String.IsNullOrEmpty(login.username))
                            throw new FormatException();
                    }
                    if (fieldName.Contains("cmbUserGroup"))
                        login.userGroup = int.Parse(Request.Form[fieldName]);
                }
            }
            catch (System.FormatException)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os valores informados não estão em um formato válido!");
                return;
            }

            try
            {
                ResourceProtector.RectifyPassword(login); // caso o login seja novo gera a senha padrão
                loginDAO.SetLogin(login);
                EmbedClientScript.CloseWindow(this);
            }
            catch (Exception genericException)
            {
                if (genericException.Message.Contains("Violation of UNIQUE KEY"))
                {
                    EmbedClientScript.ShowErrorMessage(this, "Este login já existe!");
                    return;
                }

                EmbedClientScript.ShowErrorMessage(this, genericException.Message);
            }
        }
    }

}
