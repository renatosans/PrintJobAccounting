using System;
using System.Collections.Generic;


namespace AccountingLib.CostArrangement
{
    public class CostTree
    {
        private CostBranch root;

        public CostBranch Root
        {
            get { return root; }
        }

        private Dictionary<int, CostBranch> branchDictionary;


        public CostTree(CostBranch root, List<CostBranch> branchList)
        {
            this.root = root;
            branchDictionary = new Dictionary<int, CostBranch>();
            foreach (CostBranch branch in branchList)
            {
                branchDictionary.Add(branch.Id, branch);
            }
        }

        public CostBranch GetBranchById(int costBranchId)
        {
            // Verifica se o branch está presente no dicionário
            if (!branchDictionary.ContainsKey(costBranchId)) return null;

            return branchDictionary[costBranchId];
        }

        public String GetBranchQualifiedName(int costBranchId)
        {
            // Verifica se o branch está presente no dicionário
            if (!branchDictionary.ContainsKey(costBranchId)) return null;

            CostBranch branch = branchDictionary[costBranchId];
            String qualifiedName = branch.Name;
            while (branch.Parent != null)
            {
                branch = branch.Parent;
                qualifiedName = branch.Name + "/" + qualifiedName;
            }

            return qualifiedName;
        }
    }

}
