using System;
using System.Web;
using AccountingLib.Security;
using DocMageFramework.FileUtils;
using DocMageFramework.DataManipulation;


namespace WebAccounting
{
    public partial class SettingsMasterPage : System.Web.UI.MasterPage
    {
        public DataAccess dataAccess;

        private Boolean initialized = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            // Evita que o browser faça cache das páginas, pois o cache provoca erros da aplicação
            Response.Cache.SetNoStore();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            this.Page.LoadComplete += new System.EventHandler(this.Page_LoadComplete);
        }

        public void InitializeMasterPageComponents()
        {
            InitializeMasterPageComponents(true);
        }

        public void InitializeMasterPageComponents(Boolean authenticate)
        {
            if (authenticate)
            {
                // Verifica se o usuário está autenticado, redireciona para o login caso não esteja (evita o acesso a página)
                if (!Authentication.IsAuthenticated(Session))
                    Response.Redirect("LoginPage.aspx");
            }

            // Abre a conexão com o banco
            dataAccess = DataAccess.Instance;
            dataAccess.MountConnection(FileResource.MapWebResource(Server, "DataAccess.xml"), DatabaseEnum.PrintAccounting);
            dataAccess.OpenConnection();

            initialized = true;
        }

        private void Page_LoadComplete(object sender, EventArgs e)
        {
            if (initialized)
            {
                // Fecha a conexão com o banco
                dataAccess.CloseConnection();
            }
        }
    }

}
