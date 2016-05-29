using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class DevicePrintingCostDAO
    {
        private SqlConnection sqlConnection;


        public DevicePrintingCostDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }


        public List<Object> GetDevicePrintingCosts(int tenantId, DateTime startDate, DateTime endDate)
        {
            List<Object> devicePrintingCosts;

            ProcedureCall retrieveDevicePrintingCosts = new ProcedureCall("pr_retrieveDevicePrintingCosts", sqlConnection);
            retrieveDevicePrintingCosts.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveDevicePrintingCosts.parameters.Add(new ProcedureParam("@startDate", SqlDbType.DateTime, 8, startDate));
            retrieveDevicePrintingCosts.parameters.Add(new ProcedureParam("@endDate", SqlDbType.DateTime, 8, endDate));
            retrieveDevicePrintingCosts.Execute(true);
            devicePrintingCosts = retrieveDevicePrintingCosts.ExtractFromResultset(typeof(DevicePrintingCost));

            return devicePrintingCosts;
        }
    }

}
