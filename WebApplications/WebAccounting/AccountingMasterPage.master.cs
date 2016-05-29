using System;
using System.Web;
using AccountingLib.Entities;
using AccountingLib.Security;
using DocMageFramework.FileUtils;
using DocMageFramework.DataManipulation;


namespace WebAccounting
{
    public partial class AccountingMasterPage : System.Web.UI.MasterPage
    {
        public DataAccess dataAccess;

        private Boolean Initialized = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            // Evita que o browser faça cache das páginas, pois o cache provoca erros da aplicação
            Response.Cache.SetNoStore();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            this.Page.LoadComplete += new System.EventHandler(this.Page_LoadComplete);
        }


        private String Welcome()
        {
            // Dia da semana (palavra começando em maísculo)
            String currentDate = DateTime.Now.ToString("dddd");
            currentDate = char.ToUpper(currentDate[0]) + currentDate.Substring(1);
            // Adiciona informações sobre dia, mês e ano
            currentDate = currentDate + DateTime.Now.ToString(", dd {0} MMMM {0} yyyy");
            currentDate = String.Format(currentDate, "de");
            // Mensagem de boas vindas
            AccountingLib.Entities.Login login = (AccountingLib.Entities.Login)Session["login"];
            String welcomeMessage = String.Format("Bem vindo(a), {0}", login.username);

            return welcomeMessage + "<br>" + currentDate;
        }


        public void InitializeMasterPageComponents()
        {
            // Verifica se o usuário está autenticado, redireciona para o login caso não esteja (evita o acesso a página)
            if (!Authentication.IsAuthenticated(Session))
                Response.Redirect("LoginPage.aspx");

            // Apresenta as boas vindas ao usuário
            lblWelcome.Text = Welcome();

            // Abre a conexão com o banco
            dataAccess = DataAccess.Instance;
            dataAccess.MountConnection(FileResource.MapWebResource(Server, "DataAccess.xml"), DatabaseEnum.PrintAccounting);
            dataAccess.OpenConnection();

            Initialized = true;
        }

        private void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Initialized)
            {
                // Fecha a conexão com o banco
                dataAccess.CloseConnection();
            }
        }
    }

}
