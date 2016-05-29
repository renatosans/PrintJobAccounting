using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class DeviceCostDetailDAO
    {
        private SqlConnection sqlConnection;


        public DeviceCostDetailDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }


        public List<Object> GetDeviceCostDetails(int tenantId, int printerId, DateTime startDate, DateTime endDate, String detailType)
        {
            List<Object> deviceCostDetails;

            ProcedureCall retrieveDeviceCostDetails = new ProcedureCall("pr_retrieveDeviceCostDetails", sqlConnection);
            retrieveDeviceCostDetails.parameters.Add(new ProcedureParam("tenantId", SqlDbType.Int, 4, tenantId));
            retrieveDeviceCostDetails.parameters.Add(new ProcedureParam("@printerId", SqlDbType.Int, 4, printerId));
            retrieveDeviceCostDetails.parameters.Add(new ProcedureParam("@startDate", SqlDbType.DateTime, 8, startDate));
            retrieveDeviceCostDetails.parameters.Add(new ProcedureParam("@endDate", SqlDbType.DateTime, 8, endDate));
            retrieveDeviceCostDetails.parameters.Add(new ProcedureParam("@detailType", SqlDbType.VarChar, 50, detailType));
            retrieveDeviceCostDetails.Execute(true);
            deviceCostDetails = retrieveDeviceCostDetails.ExtractFromResultset(typeof(DeviceCostDetail));

            return deviceCostDetails;
        }
    }

}
