using System;
using System.Drawing;
using System.Collections.Generic;
using AccountingLib.Management;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.Reporting;


namespace AccountingLib.ReportMailing
{
    public class DuplexPrintingCostsReport : AbstractReport
    {
        private int tenantId;

        private DateTime startDate;

        private DateTime endDate;


        public DuplexPrintingCostsReport(int tenantId, DateTime startDate, DateTime endDate)
        {
            // não recebe objeto do tipo "Tenant" pois o filtro do relatório não dá suporte a tipos complexos
            this.tenantId = tenantId;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public override void BuildReport()
        {
            TenantDAO tenantDAO = new TenantDAO(sqlConnection);
            Tenant tenant = tenantDAO.GetTenant(tenantId);

            DuplexPrintingCostDAO duplexPrintingCostDAO = new DuplexPrintingCostDAO(sqlConnection);
            List<Object> duplexPrintingCosts = duplexPrintingCostDAO.GetDuplexPrintingCosts(tenantId, startDate, endDate);

            reportBuilder.OpenMedia(reportMedia); // Abre a mídia para o output do relatório

            Dictionary<String, Object> reportFilter = new Dictionary<String, Object>();
            reportFilter.Add("tenantId", tenantId);
            reportFilter.Add("startDate", startDate);
            reportFilter.Add("endDate", endDate);
            reportBuilder.SetReportHeadings("Relatório de custos de impressão Simplex/Duplex", tenant.alias, reportFilter);

            String[] columnNames = new String[] { "Usuário", "Páginas Simplex", "Páginas Duplex", "Total Páginas", "Custo Simplex", "Custo Duplex", "Total Custo" };
            int[] columnWidths = new int[] { 50, 15, 15, 15, 15, 15, 15 };
            int rowCount = duplexPrintingCosts.Count;
            reportBuilder.CreateDataTable(columnNames, columnWidths, rowCount);
            if (reportBuilder.IsNavigable())
            {
                Dictionary<String, Object> exportOptions = ExportFormatContext.GetExportOptions(tenantId, sqlConnection);
                reportBuilder.SetNavigationData(this.GetType().Name, rowCount, exportOptions); // neste caso recordCount = rowCount
                reportBuilder.SetReportPage(action, currentPage);
            }
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                DuplexPrintingCost duplexPrintingCost = (DuplexPrintingCost)duplexPrintingCosts[rowIndex];
                ReportCell[] cells = new ReportCell[]
                {
                    new ReportCell(duplexPrintingCost.userName),
                    new ReportCell(duplexPrintingCost.simplexPageCount),
                    new ReportCell(duplexPrintingCost.duplexPageCount),
                    new ReportCell(duplexPrintingCost.totalPageCount),
                    new ReportCell(duplexPrintingCost.simplexCost),
                    new ReportCell(duplexPrintingCost.duplexCost),
                    new ReportCell(duplexPrintingCost.totalCost)
                };
                reportBuilder.InsertRow(rowIndex, cells);
            }
            ReportCell[] footerCells = new ReportCell[]
            {
                new ReportCell("TOTAL", Color.Red),
                new ReportCell("paginasSimplex", ReportCellType.Number),
                new ReportCell("paginasDuplex", ReportCellType.Number),
                new ReportCell("totalPaginas", ReportCellType.Number),
                new ReportCell("custoSimplex", ReportCellType.Money),
                new ReportCell("custoDuplex", ReportCellType.Money),
                new ReportCell("totalCusto", ReportCellType.Money)
            };
            reportBuilder.InsertFooter(footerCells);

            reportBuilder.CloseMedia();
        }
    }

}
