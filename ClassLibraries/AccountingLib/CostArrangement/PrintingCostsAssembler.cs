using System;
using System.Collections.Generic;
using AccountingLib.Entities;


namespace AccountingLib.CostArrangement
{
    public class PrintingCostsAssembler
    {
        private List<Object> userPrintingCosts;

        private SortedList<String, CostBranch> orderedBranches; // ramos em ordem alfabética

        private List<int> associates; // ids dos usuários associados a um ramo (de centros de custo)

        private int comparisonValue;


        public PrintingCostsAssembler(List<Object> userPrintingCosts)
        {
            this.userPrintingCosts = userPrintingCosts;
            this.orderedBranches = new SortedList<String, CostBranch>();
            this.associates = new List<int>();
        }

        private Boolean CheckCostMethod(Object userPrintingCost)
        {
            UserPrintingCost objToCheck = (UserPrintingCost)userPrintingCost;
            if (objToCheck.userId != comparisonValue)
                return false;

            return true;
        }

        // Busca os ramos derivados de "branch", lista eles em ordem alfabética através do nome
        // completo desde a raiz
        private void SearchBranches(CostTree tree, CostBranch branch)
        {
            orderedBranches.Add(tree.GetBranchQualifiedName(branch.Id), branch);

            foreach (CostBranch child in branch.Children)
            {
                SearchBranches(tree, child);
            }
        }

        // Busca os usuários associados a um ramo recursivamente
        private void SearchAssociates(CostBranch costBranch, Boolean clearList)
        {
            if (clearList) associates.Clear();

            foreach (CostCenterAssociate associate in costBranch.Associates)
            {
                associates.Add(associate.userId);
            }

            foreach (CostBranch child in costBranch.Children)
            {
                SearchAssociates(child, false);
            }
        }

        // Agrupa os custos(impressões) dos usuários existentes no "branch" (ramo de centros de custo), todos os centros
        // de custo derivados são contabilizados e o próprio ramo é um centro de custo com seus usuários associados
        private GroupPrintingCost AssembleCosts(CostBranch costBranch, String branchQualifiedName)
        {
            GroupPrintingCost groupPrintingCost = new GroupPrintingCost();
            groupPrintingCost.costCenterId = costBranch.Id;
            groupPrintingCost.costCenterName = branchQualifiedName;

            SearchAssociates(costBranch, true);
            foreach (int userId in associates)
            {
                comparisonValue = userId;
                UserPrintingCost userPrintingCost = (UserPrintingCost)userPrintingCosts.Find(CheckCostMethod);
                if (userPrintingCost != null)
                {
                    groupPrintingCost.bwPageCount    += userPrintingCost.bwPageCount;
                    groupPrintingCost.colorPageCount += userPrintingCost.colorPageCount;
                    groupPrintingCost.totalPageCount += userPrintingCost.totalPageCount;
                    groupPrintingCost.bwCost    += userPrintingCost.bwCost;
                    groupPrintingCost.colorCost += userPrintingCost.colorCost;
                    groupPrintingCost.totalCost += userPrintingCost.totalCost;
                }
            }

            return groupPrintingCost;
        }

        /// <summary>
        /// Retorna uma lista com os custos de impressão por centro de custo
        /// </summary>
        public List<Object> GetCostsOfBranches(CostTree tree)
        {
            List<Object> costsOfBranches = new List<Object>();

            // Caso não tenham sido passados os custos por usuário retorna a lista em branco
            if (userPrintingCosts == null)
                return costsOfBranches;

            // Não modificar o método "SearchBranches" para receber um parâmetro apenas, pois "tree.root"
            // é apenas um dos casos, nos outros são branchs variados
            SearchBranches(tree, tree.Root);

            foreach (KeyValuePair<String, CostBranch> pair in orderedBranches)
            {
                GroupPrintingCost groupPrintingCost = AssembleCosts(pair.Value, pair.Key);
                costsOfBranches.Add(groupPrintingCost);
            }

            return costsOfBranches;
        }

        /// <summary>
        /// Retorna uma lista com os custos de impressão por usuário associado
        /// </summary>
        public List<Object> GetCostsOfAssociates(CostBranch branch)
        {
            List<Object> costsOfAssociates = new List<Object>();

            // Caso não tenham sido passados os custos por usuário retorna a lista em branco
            if ((userPrintingCosts == null) || (userPrintingCosts.Count < 1))
                return costsOfAssociates;

            SearchAssociates(branch, true);

            foreach (int userId in associates)
            {
                comparisonValue = userId;
                UserPrintingCost userPrintingCost = (UserPrintingCost)userPrintingCosts.Find(CheckCostMethod);
                if (userPrintingCost != null) costsOfAssociates.Add(userPrintingCost);
            }
            
            return costsOfAssociates;
        }
    }

}
