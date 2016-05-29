using System;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using AccountingLib.Security;
using DocMageFramework.WebUtils;
using DocMageFramework.Reflection;


namespace WebAccounting
{
    public partial class ConfigUsers : System.Web.UI.Page
    {
        private AccountingMasterPage accountingMasterPage;

        private UserDAO userDAO;


        private void ShowWarning(String warningMessage)
        {
            // Remove todos os controles da página
            configurationArea.Controls.Clear();
            controlArea.Controls.Clear();

            // Renderiza a mensagem na página
            WarningMessage.Show(controlArea, warningMessage);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            accountingMasterPage = (AccountingMasterPage)Page.Master;
            accountingMasterPage.InitializeMasterPageComponents();

            if (!Authorization.AuthorizedAsAdministrator(Session))
            {
                // Mostra aviso de falta de autorização
                ShowWarning(Authorization.GetWarning());
                return;
            }

            // action:
            //    null -  Sem ação, apenas lista os usuários
            //    0    -  Excluir usuário, lista os restantes
            int? action = null;
            int? userId = null;
            try
            {
                if (!String.IsNullOrEmpty(Request.QueryString["action"]))
                    action = int.Parse(Request.QueryString["action"]);

                if (!String.IsNullOrEmpty(Request.QueryString["userId"]))
                    userId = int.Parse(Request.QueryString["userId"]);
            }
            catch (System.FormatException)
            {
                // Remove todos os controles da página
                configurationArea.Controls.Clear();
                controlArea.Controls.Clear();

                // Mostra aviso de inconsistência nos parâmetros
                WarningMessage.Show(controlArea, ArgumentBuilder.GetWarning());
                return;
            }

            Tenant tenant = (Tenant)Session["tenant"];
            userDAO = new UserDAO(accountingMasterPage.dataAccess.GetConnection());

            if (userId != null)
                switch (action)
                {
                    case 0:
                        userDAO.RemoveUser(userId.Value);
                        Response.Redirect("ConfigUsers.aspx"); // Limpa a QueryString para evitar erros
                        break;
                    default:
                        break;
                }

            List<Object> userList = userDAO.GetAllUsers(tenant.id);


            String[] columnNames = new String[] { "Usuário", "Nome Amigável", "Cota Mensal" };
            String alterScript = "window.open('UserSettings.aspx?userId=' + {0}, 'Settings', 'width=540,height=600');";
            String removeScript = "var confirmed = confirm('Deseja realmente excluir este usuário?'); if (confirmed) window.location='ConfigUsers.aspx?action=0&userId=' + {0};";
            EditableListButton[] buttons = new EditableListButton[]
            {
                // Botões que devem aparecer para os items da lista
                new EditableListButton("Editar", alterScript, ButtonTypeEnum.Edit),
                new EditableListButton("Excluir", removeScript, ButtonTypeEnum.Remove)
            };
            EditableList editableList = new EditableList(configurationArea, columnNames, buttons);
            foreach (User user in userList)
            {
                String quota = "-";
                if (user.quota != null) quota = String.Format("{0:0.000}", user.quota);
                String[] userProperties = new String[]
                {
                    user.name,
                    user.alias,
                    quota
                };
                editableList.InsertItem(user.id, false, userProperties);
            }
            editableList.DrawList();
        }
    }

}
