using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class UserCopyingCostDAO
    {
        private SqlConnection sqlConnection;


        public UserCopyingCostDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public List<Object> GetUserCopyingCosts(int tenantId, DateTime startDate, DateTime endDate)
        {
            List<Object> userCopyingCosts;

            ProcedureCall retrieveUserCopyingCosts = new ProcedureCall("pr_retrieveUserCopyingCosts", sqlConnection);
            retrieveUserCopyingCosts.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveUserCopyingCosts.parameters.Add(new ProcedureParam("@startDate", SqlDbType.DateTime, 8, startDate));
            retrieveUserCopyingCosts.parameters.Add(new ProcedureParam("@endDate", SqlDbType.DateTime, 8, endDate));
            retrieveUserCopyingCosts.Execute(true);
            userCopyingCosts = retrieveUserCopyingCosts.ExtractFromResultset(typeof(UserCopyingCost));

            return userCopyingCosts;
        }
    }

}
