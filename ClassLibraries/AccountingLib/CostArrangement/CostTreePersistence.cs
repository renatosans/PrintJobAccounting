using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;


namespace AccountingLib.CostArrangement
{
    public class CostTreePersistence
    {
        private SqlConnection sqlConnection;


        public CostTreePersistence(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        /// <summary>
        /// Recupera uma árvore de custo do banco
        /// </summary>
        public CostTree GetCostTree(int tenantId)
        {
            CostCenterDAO costCenterDAO = new CostCenterDAO(sqlConnection);
            List<Object> costCenterList = costCenterDAO.GetAllCostCenters(tenantId);

            CostCenterAssociateDAO associateDAO = new CostCenterAssociateDAO(sqlConnection);
            List<Object> associateList = associateDAO.GetAllAssociates(tenantId);

            CostTreeBuilder costTreeBuilder = new CostTreeBuilder(costCenterList, associateList);
            CostTree tree = costTreeBuilder.BuildTree();

            return tree;
        }

        /// <summary>
        /// Armazena uma arvore de custo no banco
        /// </summary>
        public void SetCostTree(CostTree tree)
        {
            // not implemented yet
        }

        // Remove um centro de custo do banco e toda a sua hierarquia (ramo de árvore), os registros
        // são excluídos definitivamente das tabelas
        public void RemoveBranch(CostBranch costBranch)
        {
            CostCenterDAO costCenterDAO = new CostCenterDAO(sqlConnection);
            CostCenterAssociateDAO associateDAO = new CostCenterAssociateDAO(sqlConnection);

            // Remove recursivamente todos os objetos dependentes
            Remove(costBranch, costCenterDAO, associateDAO);
        }

        private void Remove(CostBranch costBranch, CostCenterDAO costCenterDAO, CostCenterAssociateDAO associateDAO)
        {
            foreach (CostBranch child in costBranch.Children)
            {
                Remove(child, costCenterDAO, associateDAO);
            }

            associateDAO.RemoveAllAssociates(costBranch.Id);
            costCenterDAO.RemoveCostCenter(costBranch.Id);
        }
    }

}
