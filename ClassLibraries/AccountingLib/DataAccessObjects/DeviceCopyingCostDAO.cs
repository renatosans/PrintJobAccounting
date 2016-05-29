using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class DeviceCopyingCostDAO
    {
        private SqlConnection sqlConnection;


        public DeviceCopyingCostDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }


        public List<Object> GetDeviceCopyingCosts(int tenantId, DateTime startDate, DateTime endDate)
        {
            List<Object> deviceCopyingCosts;

            ProcedureCall retrieveDeviceCopyingCosts = new ProcedureCall("pr_retrieveDeviceCopyingCosts", sqlConnection);
            retrieveDeviceCopyingCosts.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveDeviceCopyingCosts.parameters.Add(new ProcedureParam("@startDate", SqlDbType.DateTime, 8, startDate));
            retrieveDeviceCopyingCosts.parameters.Add(new ProcedureParam("@endDate", SqlDbType.DateTime, 8, endDate));
            retrieveDeviceCopyingCosts.Execute(true);
            deviceCopyingCosts = retrieveDeviceCopyingCosts.ExtractFromResultset(typeof(DeviceCopyingCost));

            return deviceCopyingCosts;
        }
    }

}
