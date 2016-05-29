using System;
using System.Web.UI;
using System.Data.SqlClient;
using AccountingLib.Security;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.AppUtils;
using DocMageFramework.WebUtils;


namespace WebAccounting
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        private SettingsMasterPage settingsMasterPage;


        protected void Page_Load(object sender, EventArgs e)
        {
            // Inicializa a master page sem autenticação, o usuário ainda não entrou no sistema
            settingsMasterPage = (SettingsMasterPage)Page.Master;
            settingsMasterPage.InitializeMasterPageComponents(false);

            SettingsInput settingsInput = new SettingsInput(settingsArea, null);
            settingsInput.Add("txtLoginName", "Usuário", "");
            settingsInput.Add("txtOldPassword", "Senha antiga", "", true, null);
            settingsInput.Add("txtNewPassword", "Nova senha", "", true, null);
            settingsInput.Add("txtConfirmation", "Nova senha", "", true, null);
        }

        private Boolean ConfirmNewPassword(String newPassword, String confirmationPassword)
        {
            if (String.IsNullOrEmpty(newPassword))
                return false;

            if (String.IsNullOrEmpty(confirmationPassword))
                return false;

            if (newPassword != confirmationPassword)
                return false;

            return true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConnection = settingsMasterPage.dataAccess.GetConnection();

            String loginName = "";
            String password = "";
            String newPassword = "";
            String confirmationPassword = "";

            foreach (String fieldName in Request.Form)
            {
                if (fieldName.Contains("txtLoginName"))
                    loginName = Request.Form[fieldName];
                if (fieldName.Contains("txtOldPassword"))
                    password = Request.Form[fieldName];
                if (fieldName.Contains("txtNewPassword"))
                    newPassword = Request.Form[fieldName];
                if (fieldName.Contains("txtConfirmation"))
                    confirmationPassword = Request.Form[fieldName];
            }

            CredentialManager credentialManager = new CredentialManager(loginName, password, sqlConnection);
            if (!credentialManager.ValidateCredentials())
            {
                EmbedClientScript.ShowErrorMessage(this.Page, credentialManager.GetLastError());
                return;
            }

            // Verifica se a nova senha foi digitada duas vezes
            if (!ConfirmNewPassword(newPassword, confirmationPassword))
            {
                EmbedClientScript.ShowErrorMessage(this.Page, "Digite a nova senha nas duas caixas de texto.");
                return;
            }

            // Atualiza o senha no banco
            Login login = credentialManager.GetLogin();
            login.password = Cipher.GenerateHash(newPassword);
            LoginDAO loginDAO = new LoginDAO(sqlConnection);
            loginDAO.SetLogin(login);

            EmbedClientScript.CloseWindow(this);
        }
    }

}
