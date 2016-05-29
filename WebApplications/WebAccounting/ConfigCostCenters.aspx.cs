using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using AccountingLib.Security;
using AccountingLib.Entities;
using AccountingLib.CostArrangement;
using DocMageFramework.WebUtils;
using DocMageFramework.Reflection;


namespace WebAccounting
{
    public partial class ConfigCostCenters : System.Web.UI.Page
    {
        private AccountingMasterPage accountingMasterPage;

        private Tenant tenant;


        private void RemoveBranch(int costBranchId)
        {
            CostTreePersistence persistence = new CostTreePersistence(accountingMasterPage.dataAccess.GetConnection());
            CostTree tree = persistence.GetCostTree(tenant.id);
            
            CostBranch costBranch = tree.GetBranchById(costBranchId);

            // Existe um alert avisando quando o usuário tenta excluir a raiz, mesmo assim
            // aqui é verificado se o nó é raiz ( assim os dados ficam protegidos )
            if (costBranch.IsRoot()) return;

            persistence.RemoveBranch(costBranch);
        }

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
            tenant = (Tenant)Session["tenant"];

            if (!Authorization.AuthorizedAsAdministrator(Session))
            {
                // Mostra aviso de falta de autorização
                ShowWarning(Authorization.GetWarning());
                return;
            }

            // action:
            //    null -  Sem ação, apenas lista os centros de custo
            //    0    -  Excluir centro de custo (e depententes), lista os restantes
            int? action = null;
            int? costCenterId = null;
            try
            {
                if (!String.IsNullOrEmpty(Request.QueryString["action"]))
                    action = int.Parse(Request.QueryString["action"]);

                if (!String.IsNullOrEmpty(Request.QueryString["costCenterId"]))
                    costCenterId = int.Parse(Request.QueryString["costCenterId"]);
            }
            catch (System.FormatException)
            {
                // Mostra aviso de inconsistência nos parâmetros
                ShowWarning(ArgumentBuilder.GetWarning());
                return;
            }

            if ((action == 0) && (costCenterId != null))
            {
                RemoveBranch(costCenterId.Value);
                Response.Redirect("ConfigCostCenters.aspx"); // Limpa a QueryString para evitar erros
            }
            
            CostTreePersistence persistence = new CostTreePersistence(accountingMasterPage.dataAccess.GetConnection());
            CostTree tree = persistence.GetCostTree(tenant.id);
            
            CostTreeRenderer renderer = new CostTreeRenderer(tree, pnlCostCenters);
            renderer.SetNavigationData(pnlAssociates, txtSelectedNode, txtRootNode);
            renderer.RenderTree();

            // Define botão para criar um novo centro de custo, ele é criado como filho do
            // centro de custo selecionado, Create() é definida na classe "CostTreeRenderer"
            btnCreate.Attributes.Add("onClick", "Create();");

            // Define botão para remover um centro de custo, os objetos dependentes são
            // excluídos recursivamente, Remove() é definida na classe "CostTreeRenderer"
            btnRemove.Attributes.Add("onClick", "Remove();");

            // Define botão para associar usuários ao centro de custo selecionado
            btnAssociate.Attributes.Add("onClick", "Associate();");

            // Define botão para remover associações (entre usuário e centro de custo)
            btnDisassociate.Attributes.Add("onClick", "Disassociate();");
        }
    }

}
