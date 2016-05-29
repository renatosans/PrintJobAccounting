using System;
using System.Collections.Generic;
using AccountingLib.Management;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.Reporting;


namespace AccountingLib.ReportMailing
{
    public class PrintedDocumentsReport: AbstractReport
    {
        private int tenantId;

        private DateTime startDate;

        private DateTime endDate;

        private int? userId;

        private int? printerId;


        public PrintedDocumentsReport(int tenantId, DateTime startDate, DateTime endDate, int? userId, int? printerId)
        {
            // não recebe objeto do tipo "Tenant" pois o filtro do relatório não dá suporte a tipos complexos
            this.tenantId = tenantId;
            this.startDate = startDate;
            this.endDate = endDate;
            this.userId = userId;
            this.printerId = printerId;
        }

        public override void BuildReport()
        {
            TenantDAO tenantDAO = new TenantDAO(sqlConnection);
            Tenant tenant = tenantDAO.GetTenant(tenantId);

            // Obtem a lista de documentos considerando o filtro (faixa de datas, usuário, impressora)
            PrintedDocumentDAO printedDocumentDAO = new PrintedDocumentDAO(sqlConnection);
            List<Object> printedDocuments = printedDocumentDAO.GetPrintedDocuments(tenantId, startDate, endDate, userId, printerId);

            reportBuilder.OpenMedia(reportMedia); // Abre a mídia para o output do relatório

            Dictionary<String, Object> reportFilter = new Dictionary<String, Object>();
            reportFilter.Add("tenantId", tenantId);
            reportFilter.Add("startDate", startDate);
            reportFilter.Add("endDate", endDate);
            reportFilter.Add("userId", userId);
            reportFilter.Add("printerId", printerId);
            reportBuilder.SetReportHeadings("Relatório de Impressões", tenant.alias, reportFilter);

            String[] columnNames = new String[] { "Data/Hora", "Usuário", "Impressora", "Páginas", "Nome do documento" };
            int[] columnWidths = new int[] { 25, 25, 25, 15, 45 };
            int rowCount = printedDocuments.Count;
            reportBuilder.CreateDataTable(columnNames, columnWidths, rowCount);
            if (reportBuilder.IsNavigable())
            {
                Dictionary<String, Object> exportOptions = ExportFormatContext.GetExportOptions(tenantId, sqlConnection);
                reportBuilder.SetNavigationData(this.GetType().Name, rowCount, exportOptions); // neste caso recordCount = rowCount
                reportBuilder.SetReportPage(action, currentPage);
            }
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                PrintedDocument printedDocument = (PrintedDocument) printedDocuments[rowIndex];
                ReportCell[] cells = new ReportCell[]
                {
                    new ReportCell(printedDocument.jobTime.ToString()),
                    new ReportCell(printedDocument.userName),
                    new ReportCell(printedDocument.printerName),
                    new ReportCell(printedDocument.pageCount * printedDocument.copyCount),
                    new ReportCell(printedDocument.name)
                };
                reportBuilder.InsertRow(rowIndex, cells);
            }

            reportBuilder.CloseMedia();
        }
    }

}
