using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AccountingLib.Security;
using DocMageFramework.WebUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.DataManipulation;


namespace WebAdministrator
{
    public partial class AdministratorMasterPage : System.Web.UI.MasterPage
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

        private void Page_LoadComplete(Object sender, EventArgs e)
        {
            if (Initialized)
            {
                // Fecha a conexão com o banco
                dataAccess.CloseConnection();
            }
        }

        public void AddMenuItems()
        {
            Object[][] menuItems = new Object[4][];
            menuItems[0] = new String[] { "Empresas", "ConfigTenants.aspx" };
            menuItems[1] = new String[] { "Licenças de Uso", "ConfigLicenses.aspx" };
            menuItems[2] = new String[] { "Logins de Acesso", "ConfigLogins.aspx" };
            menuItems[3] = new String[] { "Logout", "LoginPage.aspx?action=0" };

            for (int index = menuItems.Length - 1; index >= 0; index--)
            {
                Panel itemContainer = new Panel();
                itemContainer.Width = Unit.Pixel(150);
                itemContainer.Style.Add("float", "right");
                itemContainer.Style.Add("text-align", "left");
                HtmlAnchor menuItem = new HtmlAnchor();
                menuItem.InnerText = (String)menuItems[index][0];
                menuItem.HRef = (String)menuItems[index][1];
                menuItem.Style.Add("color", "yellow");
                itemContainer.Controls.Add(menuItem);
                menuContainer.Controls.Add(itemContainer);
            }
        }

        public void InitializeMasterPageComponents()
        {
            // Verifica se o usuário está autenticado, redireciona para o login caso não esteja (evita o acesso a página)
            if (!Authentication.IsAuthenticated(Session))
                Response.Redirect("LoginPage.aspx");

            // Abre a conexão com o banco
            dataAccess = DataAccess.Instance;
            dataAccess.MountConnection(FileResource.MapWebResource(Server, "DataAccess.xml"), DatabaseEnum.PrintAccounting);
            dataAccess.OpenConnection();

            Initialized = true;
        }
    }

}
