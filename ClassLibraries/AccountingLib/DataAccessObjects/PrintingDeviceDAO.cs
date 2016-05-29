using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class PrintingDeviceDAO
    {
        private SqlConnection sqlConnection;

        public PrintingDeviceDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public PrintingDevice GetPrintingDevice(int tenantId, String serialNumber)
        {
            ProcedureCall retrievePrintingDevices = new ProcedureCall("pr_retrievePrintingDevice", sqlConnection);
            retrievePrintingDevices.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrievePrintingDevices.parameters.Add(new ProcedureParam("@serialNumber", SqlDbType.VarChar, 100, serialNumber));
            retrievePrintingDevices.Execute(true);
            List<Object> deviceList = retrievePrintingDevices.ExtractFromResultset(typeof(PrintingDevice));
            if (deviceList.Count == 1)
            {
                return (PrintingDevice) deviceList[0];
            }
            else
            {
                return null;
            }
        }

        public List<Object> GetAllPrintingDevices(int tenantId)
        {
            List<Object> deviceList;

            ProcedureCall retrievePrintingDevices = new ProcedureCall("pr_retrievePrintingDevice", sqlConnection);
            retrievePrintingDevices.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrievePrintingDevices.Execute(true);
            deviceList = retrievePrintingDevices.ExtractFromResultset(typeof(PrintingDevice));

            return deviceList;
        }

        public List<Object> GetCounterHistory(int deviceId)
        {
            List<Object> counterHistory;

            ProcedureCall retrievePageCounters = new ProcedureCall("pr_retrievePageCounter", sqlConnection);
            retrievePageCounters.parameters.Add(new ProcedureParam("@deviceId", SqlDbType.Int, 4, deviceId));
            retrievePageCounters.Execute(true);
            counterHistory = retrievePageCounters.ExtractFromResultset(typeof(PageCounter));

            return counterHistory;
        }

        public void SetPrintingDevice(PrintingDevice printingDevice)
        {
            ProcedureCall storePrintingDevice = new ProcedureCall("pr_storePrintingDevice", sqlConnection);
            storePrintingDevice.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, printingDevice.tenantId));
            storePrintingDevice.parameters.Add(new ProcedureParam("@ipAddress", SqlDbType.VarChar, 100, printingDevice.ipAddress));
            storePrintingDevice.parameters.Add(new ProcedureParam("@description", SqlDbType.VarChar, 100, printingDevice.description));
            storePrintingDevice.parameters.Add(new ProcedureParam("@serialNumber", SqlDbType.VarChar, 100, printingDevice.serialNumber));
            storePrintingDevice.parameters.Add(new ProcedureParam("@counter", SqlDbType.Int, 4, printingDevice.counter));
            storePrintingDevice.Execute(true);

            int? deviceId = storePrintingDevice.ExtractFromResultset();
            if (deviceId != null)
            {
                List<Object> counterHistory = GetCounterHistory(deviceId.Value);
                if (counterHistory.Count > 0)
                {
                    // Verifica se o contador é repetido, considera uma diferença de 50 páginas ao comparar
                    DateTime today = DateTime.Now;
                    PageCounter lastCounter = (PageCounter)counterHistory[0];
                    Decimal diff = Math.Abs(printingDevice.counter - lastCounter.counter);
                    if ((lastCounter.date.Day != today.Day) || (diff > 50))
                    {
                        SetPageCounter(deviceId.Value, printingDevice.counter);
                    }
                }
                else
                {
                    // Nenhum contador prévio, insere primeira ocorrência
                    SetPageCounter(deviceId.Value, printingDevice.counter);
                }
            }
        }

        public void SetPageCounter(int deviceId, int counter)
        {
            ProcedureCall storePageCounter = new ProcedureCall("pr_storePageCounter", sqlConnection);
            storePageCounter.parameters.Add(new ProcedureParam("@deviceId", SqlDbType.Int, 4, deviceId));
            storePageCounter.parameters.Add(new ProcedureParam("@counter", SqlDbType.Int, 4, counter));
            storePageCounter.Execute(false);
        }

        public void RemovePrintingDevice(int id)
        {
            ProcedureCall removePrintingDevice = new ProcedureCall("pr_removePrintingDevice", sqlConnection);
            removePrintingDevice.parameters.Add(new ProcedureParam("@id", SqlDbType.Int, 4, id));
            removePrintingDevice.Execute(false);
        }
    }

}
