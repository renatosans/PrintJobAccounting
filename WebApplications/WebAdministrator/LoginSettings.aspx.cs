using System;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.AppUtils;
using DocMageFramework.WebUtils;


namespace WebAdministrator
{
    public partial class LoginSettings : System.Web.UI.Page
    {
        private AdministratorMasterPage administratorMasterPage;

        private AdministratorLoginDAO administratorLoginDAO;


        protected void Page_Load(Object sender, EventArgs e)
        {
            administratorMasterPage = (AdministratorMasterPage)Page.Master;
            administratorMasterPage.InitializeMasterPageComponents();

            int loginId;
            Boolean isNumeric = int.TryParse(Request.QueryString["loginId"], out loginId);
            if (!isNumeric)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os parâmetros passados para a página não estão em um formato válido.", true);
                return;
            }

            administratorLoginDAO = new AdministratorLoginDAO(administratorMasterPage.dataAccess.GetConnection());
            AdministratorLogin login = administratorLoginDAO.GetLogin(loginId);
            // Caso o login não exista cria um novo (não é uma edição e sim uma inclusão)
            if (login == null) login = new AdministratorLogin();

            SettingsInput settingsInput = new SettingsInput(settingsArea, null);
            settingsInput.AddHidden("txtId", login.id.ToString());
            settingsInput.Add("txtUsername", "Username", login.username);
            settingsInput.Add("txtPassword", "Password", "", true, null); // deixa o password vazio, não recupera
        }

        protected void btnSubmit_Click(Object sender, EventArgs e)
        {
            AdministratorLogin login = new AdministratorLogin();
            foreach (String fieldName in Request.Form)
            {
                if (fieldName.Contains("txtId"))
                    login.id = int.Parse(Request.Form[fieldName]);
                if (fieldName.Contains("txtUsername"))
                    login.username = Request.Form[fieldName];
                if (fieldName.Contains("txtPassword"))
                    login.password = Request.Form[fieldName];
            }
            
            if ((String.IsNullOrEmpty(login.username)) || (String.IsNullOrEmpty(login.password)))
            {
                EmbedClientScript.ShowErrorMessage(this, "Os valores informados não estão em um formato válido!");
                return;
            }

            login.password = Cipher.GenerateHash(login.password);

            try
            {
                administratorLoginDAO.SetLogin(login);
            }
            catch (Exception exc)
            {
                EmbedClientScript.ShowErrorMessage(this, exc.Message);
                return;
            }

            EmbedClientScript.CloseWindow(this);
        }
    }

}
