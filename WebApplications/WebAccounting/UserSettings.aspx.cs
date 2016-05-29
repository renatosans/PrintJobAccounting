using System;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;


namespace WebAccounting
{
    public partial class UserSettings : System.Web.UI.Page
    {
        private SettingsMasterPage settingsMasterPage;

        private UserDAO userDAO;


        protected void Page_Load(Object sender, EventArgs e)
        {
            settingsMasterPage = (SettingsMasterPage)Page.Master;
            settingsMasterPage.InitializeMasterPageComponents();

            int userId;
            Boolean isNumeric = int.TryParse(Request.QueryString["userId"], out userId);
            if (!isNumeric)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os parâmetros passados para a página não estão em um formato válido.", true);
                return;
            }

            Tenant tenant = (Tenant)Session["tenant"];
            userDAO = new UserDAO(settingsMasterPage.dataAccess.GetConnection());
            AccountingLib.Entities.User user = userDAO.GetUser(tenant.id, userId);
            if (user == null)
            {
                EmbedClientScript.ShowErrorMessage(this, "Falha ao obter os dados do usuário.", true);
                return;
            }

            lblTitle.Text = "Dados do usuário " + user.name;
            SettingsInput settingsInput = new SettingsInput(settingsArea, null);
            settingsInput.AddHidden("txtId", user.id.ToString());
            settingsInput.AddHidden("txtTenantId", user.tenantId.ToString());
            settingsInput.AddHidden("txtName", user.name);
            settingsInput.Add("txtAlias", "Nome amigável", user.alias);
            settingsInput.Add("txtQuota", "Cota Mensal", String.Format("{0:0.000}", user.quota));
        }

        protected void btnSubmit_Click(Object sender, EventArgs e)
        {
            AccountingLib.Entities.User user = new AccountingLib.Entities.User();
            try
            {
                foreach (String fieldName in Request.Form)
                {
                    if (fieldName.Contains("txtId"))
                        user.id = int.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("txtTenantId"))
                        user.tenantId = int.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("txtName"))
                        user.name = Request.Form[fieldName];
                    if (fieldName.Contains("txtAlias"))
                        user.alias = Request.Form[fieldName];
                    if (fieldName.Contains("txtQuota"))
                        user.quota = Decimal.Parse(Request.Form[fieldName]);
                }
            }
            catch (System.FormatException)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os valores informados não estão em um formato válido!");
                return;
            }

            if (String.IsNullOrEmpty(user.alias))
            {
                EmbedClientScript.ShowErrorMessage(this, "Os valores informados não estão em um formato válido!");
                return;
            }
       
            userDAO.SetUser(user);
            EmbedClientScript.CloseWindow(this);
        }
    }

}
