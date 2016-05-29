using System;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.Security;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;
using DocMageFramework.Reflection;
using DocMageFramework.DataManipulation;


namespace WebAccounting
{
    public partial class ConfigSmtpServers : System.Web.UI.Page
    {
        private AccountingMasterPage accountingMasterPage;

        private DataAccess dataAccess;

        private SmtpServerDAO smtpServerDAO;


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

            int smtpServerId;
            Boolean paramExists = !String.IsNullOrEmpty(Request.QueryString["smtpServerId"]);
            Boolean isNumeric = int.TryParse(Request.QueryString["smtpServerId"], out smtpServerId);
            if ((paramExists) && (!isNumeric))
            {
                // Mostra aviso de inconsistência nos parâmetros
                ShowWarning(ArgumentBuilder.GetWarning());
                return;
            }

            smtpServerDAO = new SmtpServerDAO(dataAccess.GetConnection());
            if (paramExists) // Se o parametro existe é uma exclusão
            {
                smtpServerDAO.RemoveSmtpServer(smtpServerId);
                Response.Redirect("ConfigSmtpServers.aspx"); // Limpa a QueryString para evitar erros
            }

            Tenant tenant = (Tenant)Session["tenant"];
            List<Object> serverList = smtpServerDAO.GetAllSmtpServers(tenant.id);
            int defaultItemId = 0;
            if (serverList.Count > 0)
            {
                // Considera como sendo item default o primeiro smtp server criado para o tenant
                SmtpServer defaultItem = (SmtpServer)serverList[0];
                defaultItemId = defaultItem.id;
            }

            String[] columnNames = new String[] { "Nome Servidor", "Endereço", "Porta" };
            String alterScript = "window.open('SmtpServerSettings.aspx?smtpServerId=' + {0}, 'Settings', 'width=540,height=600');";
            String removeScript = "var confirmed = confirm('Deseja realmente excluir este item?'); if (confirmed) window.location='ConfigSmtpServers.aspx?smtpServerId=' + {0};";
            EditableListButton[] buttons = new EditableListButton[]
            {
                // Botões que devem aparecer para os items da lista
                new EditableListButton("Editar", alterScript, ButtonTypeEnum.Edit),
                new EditableListButton("Excluir", removeScript, ButtonTypeEnum.Remove)
            };
            EditableList editableList = new EditableList(configurationArea, columnNames, buttons);
            editableList.PreserveDefaultItem();
            foreach (SmtpServer server in serverList)
            {
                String[] serverProperties = new String[]
                {
                    server.name,
                    server.address,
                    server.port.ToString()
                };
                Boolean isDefaultItem = server.id == defaultItemId;
                editableList.InsertItem(server.id, isDefaultItem, serverProperties);
            }
            editableList.DrawList();

            // O clique do botão chama o script de alteração passando "id = 0", a tela de alteração
            // interpreta "id = 0" como "criar um novo", o id é então gerado no banco de dados.
            btnNovo.Attributes.Add("onClick", String.Format(alterScript, 0));
        }
    }

}
