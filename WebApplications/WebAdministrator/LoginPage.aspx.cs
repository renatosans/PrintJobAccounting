using System;
using AccountingLib.Security;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.AppUtils;
using DocMageFramework.WebUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.DataManipulation;


namespace WebAdministrator
{
    public partial class LoginPage : System.Web.UI.Page
    {
        private DataAccess dataAccess;


        private void InitializeComponent()
        {
            this.LoadComplete += new System.EventHandler(this.Page_LoadComplete);
        }

        protected void Page_Load(Object sender, EventArgs e)
        {
            // Abre a conexão com o banco
            dataAccess = DataAccess.Instance;
            dataAccess.MountConnection(FileResource.MapWebResource(Server, "DataAccess.xml"), DatabaseEnum.PrintAccounting);
            dataAccess.OpenConnection();

            // Limpa as mensagens de erro
            lblErrorMessages.Text = "";

            // action:
            //    null -  Sem ação, apenas abre a página de login
            //    0    -  Efetua o Logout, removendo a autenticação previa do usuário
            int action;
            Boolean paramExists = !String.IsNullOrEmpty(Request.QueryString["action"]);
            Boolean isNumeric = int.TryParse(Request.QueryString["action"], out action);
            if ((paramExists) && (!isNumeric))
            {
                // Remove todos os controles da página
                controlArea.Controls.Clear();

                // Mostra aviso de inconsistência nos parâmetros
                WarningMessage.Show(controlArea, "Os parâmetros passados para a página não estão em um formato válido.");
                return;
            }

            if ((paramExists) && (action == 0))
            {
                Authentication.Disauthenticate(Session);
                Response.Redirect("LoginPage.aspx"); // Limpa a QueryString para evitar erros
            }
        }

        private void Page_LoadComplete(Object sender, EventArgs e)
        {
            // Fecha a conexão com o banco
            dataAccess.CloseConnection();
            dataAccess = null;
        }

        protected void btnLogin_Click(Object sender, EventArgs e)
        {
            // A busca do login no BD não é case sensitive, posteriormente faz verificação case sensitive
            // através do LoginValidator
            AdministratorLoginDAO loginDAO = new AdministratorLoginDAO(dataAccess.GetConnection());
            AdministratorLogin login = loginDAO.GetLogin(txtLoginName.Text);

            LoginValidator loginValidator = new LoginValidator(login);
            String username = txtLoginName.Text;
            String password = Cipher.GenerateHash(txtPassword.Text);
            Boolean validLogin = loginValidator.CheckCredentials(username, password);
            if (!validLogin)
            {
                lblErrorMessages.Text = loginValidator.GetLastError();
                return;
            }
            Authentication.Authenticate(login, null, Session);
            Response.Redirect("ConfigTenants.aspx");
        }
    }

}
