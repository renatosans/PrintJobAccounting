using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class CostCenterAssociateDAO
    {
        private SqlConnection sqlConnection;


        public CostCenterAssociateDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public void RemoveAllAssociates(int costCenterId)
        {
            ProcedureCall removeAssociates = new ProcedureCall("pr_removeAssociate", sqlConnection);
            removeAssociates.parameters.Add(new ProcedureParam("@costCenterId", SqlDbType.Int, 4, costCenterId));
            removeAssociates.Execute(false);
        }

        public void RemoveAssociate(int associateId)
        {
            ProcedureCall removeAssociate = new ProcedureCall("pr_removeAssociate", sqlConnection);
            removeAssociate.parameters.Add(new ProcedureParam("@associateId", SqlDbType.Int, 4, associateId));
            removeAssociate.Execute(false);
        }

        public List<Object> GetAssociates(int tenantId, int costCenterId)
        {
            List<Object> associateList;

            ProcedureCall retrieveAssociates = new ProcedureCall("pr_retrieveAssociates", sqlConnection);
            retrieveAssociates.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveAssociates.parameters.Add(new ProcedureParam("@costCenterId", SqlDbType.Int, 4, costCenterId));
            retrieveAssociates.Execute(true);
            associateList = retrieveAssociates.ExtractFromResultset(typeof(CostCenterAssociate));

            return associateList;
        }

        public List<Object> GetAllAssociates(int tenantId)
        {
            List<Object> associateList;

            ProcedureCall retrieveAssociates = new ProcedureCall("pr_retrieveAssociates", sqlConnection);
            retrieveAssociates.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveAssociates.Execute(true);
            associateList = retrieveAssociates.ExtractFromResultset(typeof(CostCenterAssociate));

            return associateList;
        }

        public void SetAssociate(CostCenterAssociate associate)
        {
            ProcedureCall storeAssociate = new ProcedureCall("pr_storeAssociate", sqlConnection);
            storeAssociate.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, associate.tenantId));
            storeAssociate.parameters.Add(new ProcedureParam("@costCenterId", SqlDbType.Int, 4, associate.costCenterId));
            storeAssociate.parameters.Add(new ProcedureParam("@userId", SqlDbType.Int, 4, associate.userId));
            storeAssociate.Execute(false);
        }
    }

}
