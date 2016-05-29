using System;
using System.Collections.Generic;
using AccountingLib.Entities;


namespace AccountingLib.CostArrangement
{
    public class CostTreeBuilder
    {
        private List<Object> costCenters;

        private List<Object> associates;

        private int? comparisonValue;

        private List<CostBranch> branchList;


        public CostTreeBuilder(List<Object> costCenters, List<Object> associates)
        {
            this.costCenters = costCenters;
            this.associates = associates;
            branchList = new List<CostBranch>();
        }

        private Boolean CheckCostCenterMethod(Object costCenter)
        {
            CostCenter objToCheck = (CostCenter)costCenter;
            if (objToCheck.parentId != comparisonValue)
                return false;

            return true;
        }

        private Boolean CheckAssociateMethod(Object associate)
        {
            CostCenterAssociate objToCheck = (CostCenterAssociate)associate;
            if (objToCheck.costCenterId != comparisonValue)
                return false;

            return true;
        }

        private CostBranch GetRoot()
        {
            // Seta o valor de comparação, para verificar se o centro de custo faz parte
            // da raiz do organograma (parentId = null)
            comparisonValue = null;

            // Busca a raiz do organograma (centro de custo com parentId = null)
            CostCenter rootCC = (CostCenter) costCenters.Find(CheckCostCenterMethod);

            // Não foi possível localizar a raiz do organograma
            if (rootCC == null) return null;

            CostBranch rootBranch = new CostBranch(rootCC);
            comparisonValue = rootBranch.Id;
            rootBranch.Associates = associates.FindAll(CheckAssociateMethod);
            branchList.Add(rootBranch);

            return rootBranch;
        }

        /// <summary>
        /// Obtem a filiação recursivamente
        /// </summary>
        private List<CostBranch> GetChildren(CostBranch parent)
        {
            List<CostBranch> returnList = new List<CostBranch>();

            comparisonValue = parent.Id;
            List<Object> children = costCenters.FindAll(CheckCostCenterMethod);
            foreach (CostCenter costCenter in children)
            {
                CostBranch branch = new CostBranch(costCenter);
                branch.Parent = parent;
                branch.Children = GetChildren(branch);
                comparisonValue = branch.Id;
                branch.Associates = associates.FindAll(CheckAssociateMethod);

                returnList.Add(branch);
                branchList.Add(branch);
            }

            return returnList;
        }

        public CostTree BuildTree()
        {
            CostBranch root = GetRoot();

            // Não foi possível localizar a raiz do organograma
            if (root == null) return null;

            // Obtem a estrutura de árvore (centros de custo), monta os nós recursivamente
            root.Children = GetChildren(root);

            return new CostTree(root, branchList);
        }
    }

}
