using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class PrintedDocumentDAO
    {
        private SqlConnection sqlConnection;


        public PrintedDocumentDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }


        public List<Object> GetPrintedDocuments(int tenantId, DateTime startDate, DateTime endDate, int? userId, int? printerId)
        {
            List<Object> printedDocuments;

            ProcedureCall retrievePrintedDocuments = new ProcedureCall("pr_retrievePrintedDocuments", sqlConnection);
            retrievePrintedDocuments.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrievePrintedDocuments.parameters.Add(new ProcedureParam("@startDate", SqlDbType.DateTime, 8, startDate));
            retrievePrintedDocuments.parameters.Add(new ProcedureParam("@endDate", SqlDbType.DateTime, 8, endDate));
            retrievePrintedDocuments.parameters.Add(new ProcedureParam("@userId", SqlDbType.Int, 4, userId));
            retrievePrintedDocuments.parameters.Add(new ProcedureParam("@printerId", SqlDbType.Int, 4, printerId));
            retrievePrintedDocuments.Execute(true);
            printedDocuments = retrievePrintedDocuments.ExtractFromResultset(typeof(PrintedDocument));

            return printedDocuments;
        }


        public void InsertPrintedDocument(PrintedDocument printedDocument)
        {
            ProcedureCall storePrintedDocument = new ProcedureCall("pr_storePrintedDocument", sqlConnection);
            storePrintedDocument.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, printedDocument.tenantId));
            storePrintedDocument.parameters.Add(new ProcedureParam("@jobTime", SqlDbType.DateTime, 8, printedDocument.jobTime));
            storePrintedDocument.parameters.Add(new ProcedureParam("@userName", SqlDbType.VarChar, 100, printedDocument.userName));
            storePrintedDocument.parameters.Add(new ProcedureParam("@printerName", SqlDbType.VarChar, 100, printedDocument.printerName));
            storePrintedDocument.parameters.Add(new ProcedureParam("@name", SqlDbType.VarChar, 100, printedDocument.name));
            storePrintedDocument.parameters.Add(new ProcedureParam("@pageCount", SqlDbType.Int, 4, printedDocument.pageCount));
            storePrintedDocument.parameters.Add(new ProcedureParam("@copyCount", SqlDbType.Int, 4, printedDocument.copyCount));
            storePrintedDocument.parameters.Add(new ProcedureParam("@duplex", SqlDbType.Bit, 1, printedDocument.duplex));
            storePrintedDocument.parameters.Add(new ProcedureParam("@color", SqlDbType.Bit, 1, printedDocument.color));
            storePrintedDocument.Execute(false);
        }
    }

}
