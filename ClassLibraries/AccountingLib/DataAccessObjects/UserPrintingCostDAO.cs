using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class UserPrintingCostDAO
    {
        private SqlConnection sqlConnection;


        public UserPrintingCostDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }


        public List<Object> GetUserPrintingCosts(int tenantId, DateTime startDate, DateTime endDate)
        {
            List<Object> userPrintingCosts;

            ProcedureCall retrieveUserPrintingCosts = new ProcedureCall("pr_retrieveUserPrintingCosts", sqlConnection);
            retrieveUserPrintingCosts.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveUserPrintingCosts.parameters.Add(new ProcedureParam("@startDate", SqlDbType.DateTime, 8, startDate));
            retrieveUserPrintingCosts.parameters.Add(new ProcedureParam("@endDate", SqlDbType.DateTime, 8, endDate));
            retrieveUserPrintingCosts.Execute(true);
            userPrintingCosts = retrieveUserPrintingCosts.ExtractFromResultset(typeof(UserPrintingCost));

            return userPrintingCosts;
        }
    }

}
