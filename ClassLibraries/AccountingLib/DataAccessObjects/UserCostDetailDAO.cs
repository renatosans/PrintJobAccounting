using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class UserCostDetailDAO
    {
        private SqlConnection sqlConnection;


        public UserCostDetailDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }


        public List<Object> GetUserCostDetails(int tenantId, int userId, DateTime startDate, DateTime endDate, String detailType)
        {
            List<Object> userCostDetails;

            ProcedureCall retrieveUserCostDetails = new ProcedureCall("pr_retrieveUserCostDetails", sqlConnection);
            retrieveUserCostDetails.parameters.Add(new ProcedureParam("tenantId", SqlDbType.Int, 4, tenantId));
            retrieveUserCostDetails.parameters.Add(new ProcedureParam("@userId", SqlDbType.Int, 4, userId));
            retrieveUserCostDetails.parameters.Add(new ProcedureParam("@startDate", SqlDbType.DateTime, 8, startDate));
            retrieveUserCostDetails.parameters.Add(new ProcedureParam("@endDate", SqlDbType.DateTime, 8, endDate));
            retrieveUserCostDetails.parameters.Add(new ProcedureParam("@detailType", SqlDbType.VarChar, 50, detailType));
            retrieveUserCostDetails.Execute(true);
            userCostDetails = retrieveUserCostDetails.ExtractFromResultset(typeof(UserCostDetail));

            return userCostDetails;
        }
    }

}
