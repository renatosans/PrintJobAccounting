using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class DuplexPrintingCostDAO
    {
        private SqlConnection sqlConnection;


        public DuplexPrintingCostDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public List<Object> GetDuplexPrintingCosts(int tenantId, DateTime startDate, DateTime endDate)
        {
            List<Object> duplexPrintingCosts;
            
            ProcedureCall retrieveDuplexPrintingCosts = new ProcedureCall("pr_retrieveDuplexPrintingCosts", sqlConnection);
            retrieveDuplexPrintingCosts.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveDuplexPrintingCosts.parameters.Add(new ProcedureParam("@startDate", SqlDbType.DateTime, 8, startDate));
            retrieveDuplexPrintingCosts.parameters.Add(new ProcedureParam("@endDate", SqlDbType.DateTime, 8, endDate));
            retrieveDuplexPrintingCosts.Execute(true);
            duplexPrintingCosts = retrieveDuplexPrintingCosts.ExtractFromResultset(typeof(DuplexPrintingCost));
            
            return duplexPrintingCosts;
        }
    }

}
