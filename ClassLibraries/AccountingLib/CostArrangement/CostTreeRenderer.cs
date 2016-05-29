using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;


namespace AccountingLib.CostArrangement
{
    /// <summary>
    /// Classe utilizada para renderizar árvores de centros de custo, recebe ao ser instanciada
    /// um objeto "tree" com a árvore a ser reproduzida em TreeView e recebe também um objeto
    /// "output" que é a superfície onde a classe renderiza o TreeView
    /// </summary>
    public class CostTreeRenderer
    {
        private CostTree tree;

        private Object output;

        private Page currentPage;

        private HtmlInputHidden selectedNode;

        private HtmlInputHidden rootNode;

        private Panel associationPanel;

        private SortedList<String, CostCenterAssociate> orderedAssociates;


        public CostTreeRenderer(CostTree tree, Object output)
        {
            this.tree = tree;
            this.output = output;
        }

        /// <summary>
        /// Seta referências para os controles envolvidos na navegação da página, recebe o painel
        /// onde devem ser renderizados os associados ao centro de custo, recebe também um objeto
        /// "selectedNode" onde a classe informa o nó selecionado no TreeView e o objeto "rootNode"
        /// onde a classe informa qual é o nó raiz
        /// </summary>
        public void SetNavigationData(Panel associationPanel, HtmlInputHidden selectedNode, HtmlInputHidden rootNode)
        {
            this.associationPanel = associationPanel;
            this.orderedAssociates = new SortedList<String, CostCenterAssociate>();

            this.selectedNode = selectedNode;
            this.rootNode = rootNode;
        }

        private void RenderBranch(Object output, CostBranch branch)
        {
            if (branch == null) return;

            TreeNode child = new TreeNode(branch.Name);
            child.Value = branch.Id.ToString();

            if (output is TreeNode)
            {
                TreeNode node = (TreeNode)output;
                node.ChildNodes.Add(child);
            }

            if (output is TreeView)
            {
                TreeView tree = (TreeView)output;
                tree.Nodes.Add(child);

                child.Selected = true;
                selectedNode.Value = child.Value;
                RenderAssociations();
            }

            SortedList<String, CostBranch> orderedBranches = new SortedList<String, CostBranch>();
            foreach (CostBranch costBranch in branch.Children)
            {
                orderedBranches.Add(costBranch.Name, costBranch);
            }
            
            foreach (CostBranch costBranch in orderedBranches.Values)
            {
                RenderBranch(child, costBranch);
            }
        }

        private void RenderAssociations()
        {
            if ((associationPanel == null) || (orderedAssociates == null)) return;

            associationPanel.Controls.Clear();
            orderedAssociates.Clear();

            int costBranchId = int.Parse(selectedNode.Value);
            CostBranch costBranch = tree.GetBranchById(costBranchId);
            FindAssociations(costBranch);

            foreach (CostCenterAssociate associate in orderedAssociates.Values)
            {
                Label associateName = new Label();
                associateName.Text = associate.userName;
                associateName.CssClass = "itemStyle";

                associationPanel.Controls.Add(associateName);
                associationPanel.Controls.Add(new LiteralControl("<br/>"));
            }
        }

        private void FindAssociations(CostBranch costBranch)
        {
            foreach (CostCenterAssociate associate in costBranch.Associates)
            {
                orderedAssociates.Add(associate.userName, associate);
            }

            foreach (CostBranch child in costBranch.Children)
            {
                FindAssociations(child);
            }
        }

        private void CreateActions(TreeView costCenterTree)
        {
            costCenterTree.CssClass = "dropDownMenuStyle";
            costCenterTree.Attributes.Add("rel", "costCenterActions");

            // Define scripts para manipular a árvore
            String getSelected = "var selectedNodeInput = document.getElementById('" + selectedNode.ClientID + "'); var selected = selectedNodeInput.value; ";
            String getRootNode = "var rootNodeInput = document.getElementById('" + rootNode.ClientID + "'); var rootNode = rootNodeInput.value; ";

            // Define scripts para manipular os centros de custo
            String createScript = "function Create() { " + getSelected + "window.open('CostCenterSettings.aspx?parentId=' + selected, 'Settings', 'width=480,height=500'); } ";

            String removeScript = "function Remove() { " + getSelected + getRootNode + "if (selected == rootNode) { alert('Não é possivel excluir a raiz.');  return; }" +
                                  "var confirmed = confirm('Deseja realmente excluir este item?'); if (confirmed) { " +
                                  "window.location='ConfigCostCenters.aspx?action=0&costCenterId=' + selected; } } ";

            String renameScript = "function Rename() { " + getSelected + "window.open('CostCenterSettings.aspx?costCenterId=' + selected, 'Settings', 'width=480,height=500'); } ";

            String associateScript = "function Associate() { " + getSelected + "window.open('AssociateSettings.aspx?action=Associate&costCenterId=' + selected, 'Settings', 'width=750,height=650'); } ";

            String disassociateScript = "function Disassociate() { " + getSelected + "window.open('AssociateSettings.aspx?action=Disassociate&costCenterId=' + selected, 'Settings', 'width=750,height=650'); } ";

            // Define as ações do menu de popup que aparece ao passar o mouse por cima da árvore
            LiteralControl costCenterActions = new LiteralControl();
            costCenterActions.Text = "<script type='text/javascript'>" + Environment.NewLine +
                                     "    " + createScript + Environment.NewLine +
                                     "    " + removeScript + Environment.NewLine +
                                     "    " + renameScript + Environment.NewLine +
                                     "    " + associateScript + Environment.NewLine +
                                     "    " + disassociateScript + Environment.NewLine +
                                     "    var costCenterActions = { divclass: 'anylinkmenu', inlinestyle: '', linktarget: '' }" + Environment.NewLine +
                                     "    costCenterActions.items = [" + Environment.NewLine +
                                     "    	['Criar um novo', \"javascript:Create();\" ]," + Environment.NewLine +
                                     "    	['Remover', \"javascript:Remove();\" ]," + Environment.NewLine +
                                     "    	['Renomear', \"javascript:Rename();\" ]," + Environment.NewLine +
                                     "      ['Associar Usuários', \"javascript:Associate();\" ]," + Environment.NewLine +
                                     "      ['Desassociar Usuários', \"javascript:Disassociate();\" ]" + Environment.NewLine +
                                     "    ]" + Environment.NewLine +
                                     "</script>";
            currentPage.Header.Controls.Add(costCenterActions);
        }

        /// <summary>
        /// Renderiza a árvore na superfície (Panel) definido no construtor
        /// </summary>
        public void RenderTree()
        {
            currentPage = null;
            Panel renderingSurface = null;
            if (output is Panel)
            {
                renderingSurface = (Panel)output;
                currentPage = renderingSurface.Page;
            }

            // Verifica os controles utilizados na renderização
            if ((currentPage == null) || (renderingSurface == null)) return;

            // Verifica os controles usados durante a navegação
            if ((selectedNode == null) || (rootNode == null) ) return;

            TreeView costCenterTree = new TreeView();
            costCenterTree.SelectedNodeChanged += new EventHandler(SelectedNodeChanged);
            costCenterTree.TreeNodeCollapsed += new TreeNodeEventHandler(TreeNodeCollapsed);
            costCenterTree.TreeNodeExpanded += new TreeNodeEventHandler(TreeNodeExpanded);
            costCenterTree.NodeStyle.CssClass = "itemStyle";
            costCenterTree.SelectedNodeStyle.CssClass = "selectedItemStyle";

            rootNode.Value = tree.Root.Id.ToString();
            CreateActions(costCenterTree);
            RenderBranch(costCenterTree, tree.Root);
            costCenterTree.ExpandAll();

            renderingSurface.Controls.Add(costCenterTree);
        }

        private void SelectedNodeChanged(Object sender, EventArgs e)
        {
            TreeView costCenterTree = (TreeView)sender;
            // Utiliza um campo oculto para armazenar o id do centro de custo selecionado
            selectedNode.Value = costCenterTree.SelectedNode.Value;

            RenderAssociations();
        }


        private void TreeNodeCollapsed(Object sender, TreeNodeEventArgs e)
        {
            e.Node.Selected = true;
            // Utiliza um campo oculto para armazenar o id do centro de custo selecionado
            selectedNode.Value = e.Node.Value;

            RenderAssociations();
        }


        private void TreeNodeExpanded(Object sender, TreeNodeEventArgs e)
        {
            e.Node.Selected = true;
            // Utiliza um campo oculto para armazenar o id do centro de custo selecionado
            selectedNode.Value = e.Node.Value;

            RenderAssociations();
        }
    }

}
