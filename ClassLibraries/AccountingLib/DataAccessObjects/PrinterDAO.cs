using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class PrinterDAO
    {
        private SqlConnection sqlConnection;


        public PrinterDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public Printer GetPrinter(int tenantId, int printerId)
        {
            ProcedureCall retrievePrinter = new ProcedureCall("pr_retrievePrinter", sqlConnection);
            retrievePrinter.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrievePrinter.parameters.Add(new ProcedureParam("@printerId", SqlDbType.Int, 4, printerId));
            retrievePrinter.Execute(true);
            List<Object> retrievedPrinter = retrievePrinter.ExtractFromResultset(typeof(Printer));
            if (retrievedPrinter.Count == 1)
            {
                return (Printer) retrievedPrinter[0];
            }
            else
            {
                return null;
            }
        }

        public List<Object> GetAllPrinters(int tenantId)
        {
            List<Object> printerList;

            ProcedureCall retrievePrinters = new ProcedureCall("pr_retrievePrinter", sqlConnection);
            retrievePrinters.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrievePrinters.Execute(true);
            printerList = retrievePrinters.ExtractFromResultset(typeof(Printer));

            return printerList;
        }

        public void SetPrinter(Printer printer)
        {
            ProcedureCall storePrinter = new ProcedureCall("pr_storePrinter", sqlConnection);
            storePrinter.parameters.Add(new ProcedureParam("@printerId", SqlDbType.Int, 4, printer.id));
            storePrinter.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, printer.tenantId));
            storePrinter.parameters.Add(new ProcedureParam("@name", SqlDbType.VarChar, 100, printer.name));
            storePrinter.parameters.Add(new ProcedureParam("@alias", SqlDbType.VarChar, 100, printer.alias));
            storePrinter.parameters.Add(new ProcedureParam("@pageCost", SqlDbType.Money, 8, printer.pageCost));
            storePrinter.parameters.Add(new ProcedureParam("@colorCostDiff", SqlDbType.Money, 8, printer.colorCostDiff));
            storePrinter.parameters.Add(new ProcedureParam("@duplexCostDiff", SqlDbType.Money, 8, printer.duplexCostDiff));
            storePrinter.parameters.Add(new ProcedureParam("@bwPrinter", SqlDbType.Bit, 1, printer.bwPrinter));
            storePrinter.Execute(false);
        }

        public void RemovePrinter(int printerId)
        {
            ProcedureCall removePrinter = new ProcedureCall("pr_removePrinter", sqlConnection);
            removePrinter.parameters.Add(new ProcedureParam("@printerId", SqlDbType.Int, 4, printerId));
            removePrinter.Execute(false);
        }
    }

}
