using System;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.Security;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;
using DocMageFramework.Reflection;
using DocMageFramework.DataManipulation;
using DocMageFramework.CustomAttributes;


namespace WebAccounting
{
    public partial class ConfigLogins : System.Web.UI.Page
    {
        private AccountingMasterPage accountingMasterPage;

        private DataAccess dataAccess;

        private LoginDAO loginDAO;


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
            dataAccess = accountingMasterPage.dataAccess;

            if (!Authorization.AuthorizedAsAdministrator(Session))
            {
                // Mostra aviso de falta de autorização
                ShowWarning(Authorization.GetWarning());
                return;
            }

            int loginId;
            Boolean paramExists = !String.IsNullOrEmpty(Request.QueryString["loginId"]);
            Boolean isNumeric = int.TryParse(Request.QueryString["loginId"], out loginId);
            if ((paramExists) && (!isNumeric))
            {
                // Mostra aviso de inconsistência nos parâmetros
                ShowWarning(ArgumentBuilder.GetWarning());
                return;
            }

            loginDAO = new LoginDAO(dataAccess.GetConnection());
            if (paramExists) // Se o parametro existe é uma exclusão
            {
                loginDAO.RemoveLogin(loginId);
                Response.Redirect("ConfigLogins.aspx"); // Limpa a QueryString para evitar erros
            }

            Tenant tenant = (Tenant)Session["tenant"];
            List<Object> loginList = loginDAO.GetAllLogins(tenant.id);
            int defaultItemId = 0;
            if (loginList.Count > 0)
            {
                // Considera como sendo item default o primeiro login criado para o tenant
                AccountingLib.Entities.Login defaultItem = (AccountingLib.Entities.Login) loginList[0];
                defaultItemId = defaultItem.id;
            }

            String[] columnNames = new String[] { "Login", "Grupo/Permissão" };
            String alterScript = "window.open('LoginSettings.aspx?loginId=' + {0}, 'Settings', 'width=540,height=600');";
            String removeScript = "var confirmed = confirm('Deseja realmente excluir este login?'); if (confirmed) window.location='ConfigLogins.aspx?loginId=' + {0};";
            EditableListButton[] buttons = new EditableListButton[]
            {
                // Botões que devem aparecer para os items da lista
                new EditableListButton("Editar", alterScript, ButtonTypeEnum.Edit),
                new EditableListButton("Excluir", removeScript, ButtonTypeEnum.Remove)
            };
            EditableList editableList = new EditableList(configurationArea, columnNames, buttons);
            editableList.PreserveDefaultItem();
            foreach (AccountingLib.Entities.Login login in loginList)
            {
                UserGroupEnum userGroup = (UserGroupEnum)login.userGroup;

                String[] loginProperties = new String[]
                {
                    login.username,
                    AssociatedText.GetFieldDescription(typeof(UserGroupEnum), userGroup.ToString())
                };
                Boolean isDefaultItem = login.id == defaultItemId;
                if (!isDefaultItem) editableList.InsertItem(login.id, false, loginProperties);
            }
            editableList.DrawList();

            // O clique do botão chama o script de alteração passando "id = 0", a tela de alteração
            // interpreta "id = 0" como "criar um novo", o id é então gerado no banco de dados.
            btnNovo.Attributes.Add("onClick", String.Format(alterScript, 0));
        }
    }

}
