using System;
using AccountingLib.Security;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.Reflection;
using DocMageFramework.DataManipulation;


namespace WebAccounting
{
    public partial class LoginPage : System.Web.UI.Page
    {
        private DataAccess dataAccess;


        private void InitializeComponent()
        {
            this.LoadComplete += new System.EventHandler(this.Page_LoadComplete);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Abre a conexão com o banco
            dataAccess = DataAccess.Instance;
            dataAccess.MountConnection(FileResource.MapWebResource(Server, "DataAccess.xml"), DatabaseEnum.PrintAccounting);
            dataAccess.OpenConnection();

            String buttonScript = "window.open('ChangePassword.aspx', 'PasswordSettings', 'width=540,height=450');";
            EmbedClientScript.AddElementClickHandler(this.Page, "btnChangePassword", buttonScript);
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
                WarningMessage.Show(controlArea, ArgumentBuilder.GetWarning());
                return;
            }
            if ((paramExists) && (action == 0))
            {
                Authentication.Disauthenticate(Session);
                Response.Redirect("LoginPage.aspx"); // Limpa a QueryString para evitar erros
            }
        }

        private void Page_LoadComplete(object sender, EventArgs e)
        {
            // Fecha a conexão com o banco
            dataAccess.CloseConnection();
            dataAccess = null;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            CredentialManager credentialManager = new CredentialManager(txtLoginName.Text, txtPassword.Text, dataAccess.GetConnection());
            if (!credentialManager.ValidateCredentials())
            {
                lblErrorMessages.Text = credentialManager.GetLastError();
                return;
            }
            Authentication.Authenticate(credentialManager.GetLogin(), credentialManager.GetTenant(), Session);
            Response.Redirect("PrintedDocuments.aspx");
        }
    }

}
