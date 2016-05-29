using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class CostCenterDAO
    {
        private SqlConnection sqlConnection;


        public CostCenterDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        private Boolean CheckCostCenterMethod(Object costCenter)
        {
            CostCenter objToCheck = (CostCenter)costCenter;
            if (objToCheck.parentId != null)
                return false;

            return true;
        }

        public void RemoveCostCenterTree(int tenantId)
        {
            ProcedureCall removeCostCenters = new ProcedureCall("pr_removeCostCenter", sqlConnection);
            removeCostCenters.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            removeCostCenters.Execute(false);
        }

        public void RemoveCostCenter(int costCenterId)
        {
            ProcedureCall removeCostCenter = new ProcedureCall("pr_removeCostCenter", sqlConnection);
            removeCostCenter.parameters.Add(new ProcedureParam("@costCenterId", SqlDbType.Int, 4, costCenterId));
            removeCostCenter.Execute(false);
        }

        public CostCenter GetCostCenter(int tenantId, int costCenterId)
        {
            ProcedureCall retrieveCostCenter = new ProcedureCall("pr_retrieveCostCenter", sqlConnection);
            retrieveCostCenter.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveCostCenter.parameters.Add(new ProcedureParam("@costCenterId", SqlDbType.Int, 4, costCenterId));
            retrieveCostCenter.Execute(true);
            List<Object> returnList = retrieveCostCenter.ExtractFromResultset(typeof(CostCenter));

            // Verifica se retornou apenas um item, mais de um indica falha, nenhum também
            if (returnList.Count != 1) return null;

            return (CostCenter)returnList[0];
        }

        public List<Object> GetAllCostCenters(int tenantId)
        {
            List<Object> costCenterList;

            ProcedureCall retrieveCostCenters = new ProcedureCall("pr_retrieveCostCenter", sqlConnection);
            retrieveCostCenters.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveCostCenters.Execute(true);
            costCenterList = retrieveCostCenters.ExtractFromResultset(typeof(CostCenter));

            return costCenterList;
        }

        public CostCenter GetMainCostCenter(int tenantId)
        {
            List<Object> costCenterList = GetAllCostCenters(tenantId);
            return (CostCenter)costCenterList.Find(CheckCostCenterMethod);
        }

        public void SetCostCenter(CostCenter costCenter)
        {
            ProcedureCall storeCostCenter = new ProcedureCall("pr_storeCostCenter", sqlConnection);
            storeCostCenter.parameters.Add(new ProcedureParam("costCenterId", SqlDbType.Int, 4, costCenter.id));
            storeCostCenter.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, costCenter.tenantId));
            storeCostCenter.parameters.Add(new ProcedureParam("@name", SqlDbType.VarChar, 100, costCenter.name));
            storeCostCenter.parameters.Add(new ProcedureParam("@parentId", SqlDbType.Int, 4, costCenter.parentId));
            storeCostCenter.Execute(false);
        }
    }

}
