using System;
using System.Collections.Generic;
using AccountingLib.Management;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.Reporting;


namespace AccountingLib.ReportMailing
{
    public class CopiedDocumentsReport: AbstractReport
    {
        private int tenantId;

        private DateTime startDate;

        private DateTime endDate;

        private int? userId;

        private int? printerId;


        public CopiedDocumentsReport(int tenantId, DateTime startDate, DateTime endDate, int? userId, int? printerId)
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
            CopiedDocumentDAO copiedDocumentDAO = new CopiedDocumentDAO(sqlConnection);
            List<Object> copiedDocuments = copiedDocumentDAO.GetCopiedDocuments(tenantId, startDate, endDate, userId, printerId);

            reportBuilder.OpenMedia(reportMedia); // Abre a mídia para o output do relatório

            Dictionary<String, Object> reportFilter = new Dictionary<String, Object>();
            reportFilter.Add("tenantId", tenantId);
            reportFilter.Add("startDate", startDate);
            reportFilter.Add("endDate", endDate);
            reportFilter.Add("userId", userId);
            reportFilter.Add("printerId", printerId);
            reportBuilder.SetReportHeadings("Relatório de Cópias", tenant.alias, reportFilter);

            String[] columnNames = new String[] { "Data/Hora", "Usuário", "Copiadora", "Páginas" };
            int[] columnWidths = new int[] { 45, 35, 40, 15 };
            int rowCount = copiedDocuments.Count;
            reportBuilder.CreateDataTable(columnNames, columnWidths, rowCount);
            if (reportBuilder.IsNavigable())
            {
                Dictionary<String, Object> exportOptions = ExportFormatContext.GetExportOptions(tenantId, sqlConnection);
                reportBuilder.SetNavigationData(this.GetType().Name, rowCount, exportOptions); // neste caso recordCount = rowCount
                reportBuilder.SetReportPage(action, currentPage);
            }
            for(int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                CopiedDocument copiedDocument = (CopiedDocument) copiedDocuments[rowIndex];
                ReportCell[] cells = new ReportCell[]
                {
                    new ReportCell(copiedDocument.jobTime.ToString()),
                    new ReportCell(copiedDocument.userName),
                    new ReportCell(copiedDocument.printerName),
                    new ReportCell(copiedDocument.pageCount)
                };
                reportBuilder.InsertRow(rowIndex, cells);
            }

            reportBuilder.CloseMedia();
        }
    }

}
