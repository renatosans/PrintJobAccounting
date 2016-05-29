using System;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;


namespace WebAdministrator
{
    public partial class ConfigTenants : System.Web.UI.Page
    {
        private AdministratorMasterPage administratorMasterPage;


        protected void Page_Load(Object sender, EventArgs e)
        {
            administratorMasterPage = (AdministratorMasterPage)Page.Master;
            administratorMasterPage.AddMenuItems();
            administratorMasterPage.InitializeMasterPageComponents();

            // action:
            //    null -  Sem ação, apenas lista os inquilinos(clientes)
            //    0    -  Excluir cliente, lista as restantes
            int? action = null;
            int? tenantId = null;
            try
            {
                if (!String.IsNullOrEmpty(Request.QueryString["action"]))
                    action = int.Parse(Request.QueryString["action"]);

                if (!String.IsNullOrEmpty(Request.QueryString["tenantId"]))
                    tenantId = int.Parse(Request.QueryString["tenantId"]);
            }
            catch (System.FormatException)
            {
                // Remove todos os controles da página
                configurationArea.Controls.Clear();
                controlArea.Controls.Clear();

                // Mostra aviso de inconsistência nos parâmetros
                WarningMessage.Show(controlArea, "Erro nos parâmetros passados para a página.");
                return;
            }

            TenantDAO tenantDAO = new TenantDAO(administratorMasterPage.dataAccess.GetConnection());
            if (tenantId != null)
                switch (action)
                {
                    case 0:
                        tenantDAO.RemoveTenant(tenantId.Value);
                        Response.Redirect("ConfigTenants.aspx"); // Limpa a QueryString para evitar erros
                        break;
                    default:
                        break;
                }
            List<Object> tenantList = tenantDAO.GetAllTenants();

            String[] columnNames = new String[] { "Empresa", "Nome amigável" };
            String alterScript = "window.open('TenantSettings.aspx?tenantId=' + {0}, 'Settings', 'width=540,height=600');";
            String removeScript = "var confirmed = confirm('Deseja realmente excluir este cliente?'); if (confirmed) window.location='ConfigTenants.aspx?action=0&tenantId=' + {0};";
            EditableListButton[] buttons = new EditableListButton[]
            {
                // Botões que devem aparecer para os items da lista
                new EditableListButton("Editar", alterScript, ButtonTypeEnum.Edit),
                new EditableListButton("Excluir", removeScript, ButtonTypeEnum.Remove)
            };

            EditableList editableList = new EditableList(configurationArea, columnNames, buttons);
            foreach(Tenant tenant in tenantList)
            {
                String[] tenantProperties = new String[]
                {
                    tenant.name,
                    tenant.alias
                };
                editableList.InsertItem(tenant.id, false, tenantProperties);
            }
            editableList.DrawList();

            // O clique do botão chama o script de alteração passando "id = 0", a tela de alteração
            // interpreta "id = 0" como "criar um novo", o id é então gerado no banco de dados.
            btnNovo.Attributes.Add("onClick", String.Format(alterScript, 0));
        }
    }

}
