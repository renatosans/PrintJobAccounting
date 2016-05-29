using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class CopiedDocumentDAO
    {
        private SqlConnection sqlConnection;

        public CopiedDocumentDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public List<Object> GetCopiedDocuments(int tenantId, DateTime startDate, DateTime endDate, int? userId, int? printerId)
        {
            List<Object> copiedDocuments;

            ProcedureCall retrieveCopiedDocuments = new ProcedureCall("pr_retrieveCopiedDocuments", sqlConnection);
            retrieveCopiedDocuments.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveCopiedDocuments.parameters.Add(new ProcedureParam("@startDate", SqlDbType.DateTime, 8, startDate));
            retrieveCopiedDocuments.parameters.Add(new ProcedureParam("@endDate", SqlDbType.DateTime, 8, endDate));
            retrieveCopiedDocuments.parameters.Add(new ProcedureParam("@userId", SqlDbType.Int, 4, userId));
            retrieveCopiedDocuments.parameters.Add(new ProcedureParam("@printerId", SqlDbType.Int, 4, printerId));
            retrieveCopiedDocuments.Execute(true);
            copiedDocuments = retrieveCopiedDocuments.ExtractFromResultset(typeof(CopiedDocument));

            return copiedDocuments;
        }

        public void InsertCopiedDocument(CopiedDocument copiedDocument)
        {
            ProcedureCall storeCopiedDocument = new ProcedureCall("pr_storeCopiedDocument", sqlConnection);
            storeCopiedDocument.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, copiedDocument.tenantId));
            storeCopiedDocument.parameters.Add(new ProcedureParam("@jobTime", SqlDbType.DateTime, 8, copiedDocument.jobTime));
            storeCopiedDocument.parameters.Add(new ProcedureParam("@userName", SqlDbType.VarChar, 100, copiedDocument.userName));
            storeCopiedDocument.parameters.Add(new ProcedureParam("@printerName", SqlDbType.VarChar, 100, copiedDocument.printerName));
            storeCopiedDocument.parameters.Add(new ProcedureParam("@pageCount", SqlDbType.Int, 4, copiedDocument.pageCount));
            storeCopiedDocument.parameters.Add(new ProcedureParam("@duplex", SqlDbType.Bit, 1, copiedDocument.duplex));
            storeCopiedDocument.parameters.Add(new ProcedureParam("@color", SqlDbType.Bit, 1, copiedDocument.color));
            storeCopiedDocument.Execute(false);
        }
    }

}
