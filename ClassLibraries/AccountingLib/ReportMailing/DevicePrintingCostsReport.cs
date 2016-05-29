using System;
using System.Drawing;
using System.Collections.Generic;
using AccountingLib.Management;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.Reporting;


namespace AccountingLib.ReportMailing
{
    public class DevicePrintingCostsReport: AbstractReport
    {
        private int tenantId;

        private DateTime startDate;

        private DateTime endDate;


        public DevicePrintingCostsReport(int tenantId, DateTime startDate, DateTime endDate)
        {
            // não recebe objeto do tipo "Tenant" pois o filtro do relatório não dá suporte a tipos complexos
            this.tenantId = tenantId;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        private ReportCell GetDeviceCell(DevicePrintingCost devicePrintingCost, Boolean navigateToDeviceDetails)
        {
            // Se o relatório não é navegável apenas retorna a célula com o nome da impressora
            if (!navigateToDeviceDetails)
                return new ReportCell(devicePrintingCost.printerName);

            // Se o relatório é navegável cria o link que permite acessar os detalhes da impressora
            String queryString = "?printerId=" + devicePrintingCost.printerId.ToString() + "&" +
                                 "startDate=" + startDate.ToString() + "&" +
                                 "endDate=" + endDate.ToString() + "&" +
                                 "detailType=PrintingCosts";
            return new ReportCell(devicePrintingCost.printerName, "DeviceCostDetails.aspx" + queryString);
        }

        public override void BuildReport()
        {
            TenantDAO tenantDAO = new TenantDAO(sqlConnection);
            Tenant tenant = tenantDAO.GetTenant(tenantId);

            DevicePrintingCostDAO devicePrintingCostDAO = new DevicePrintingCostDAO(sqlConnection);
            List<Object> devicePrintingCosts = devicePrintingCostDAO.GetDevicePrintingCosts(tenantId, startDate, endDate);

            reportBuilder.OpenMedia(reportMedia); // Abre a mídia para o output do relatório

            Dictionary<String, Object> reportFilter = new Dictionary<String, Object>();
            reportFilter.Add("tenantId", tenantId);
            reportFilter.Add("startDate", startDate);
            reportFilter.Add("endDate", endDate);
            reportBuilder.SetReportHeadings("Custos de Impressão por equipamento", tenant.alias, reportFilter);

            String[] columnNames = new String[] { "Impressora", "Páginas Pb", "Páginas Cor", "Total Páginas", "Custo Pb", "Custo Cor", "Total Custo" };
            int[] columnWidths = new int[] { 50, 15, 15, 15, 15, 15, 15 };
            int rowCount = devicePrintingCosts.Count;
            reportBuilder.CreateDataTable(columnNames, columnWidths, rowCount);
            if (reportBuilder.IsNavigable())
            {
                Dictionary<String, Object> exportOptions = ExportFormatContext.GetExportOptions(tenantId, sqlConnection);
                reportBuilder.SetNavigationData(this.GetType().Name, rowCount, exportOptions); // neste caso recordCount = rowCount
                reportBuilder.SetReportPage(action, currentPage);
            }
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                DevicePrintingCost devicePrintingCost = (DevicePrintingCost) devicePrintingCosts[rowIndex];
                ReportCell[] cells = new ReportCell[]
                {
                    GetDeviceCell(devicePrintingCost, reportBuilder.IsNavigable()),
                    new ReportCell(devicePrintingCost.bwPageCount),
                    new ReportCell(devicePrintingCost.colorPageCount),
                    new ReportCell(devicePrintingCost.totalPageCount),
                    new ReportCell(devicePrintingCost.bwCost),
                    new ReportCell(devicePrintingCost.colorCost),
                    new ReportCell(devicePrintingCost.totalCost)
                };
                reportBuilder.InsertRow(rowIndex, cells);
            }
            ReportCell[] footerCells = new ReportCell[]
            {
                new ReportCell("TOTAL", Color.Red),
                new ReportCell("paginasPb", ReportCellType.Number),
                new ReportCell("paginasCor", ReportCellType.Number),
                new ReportCell("totalPaginas", ReportCellType.Number),
                new ReportCell("custoPb", ReportCellType.Money),
                new ReportCell("custoCor", ReportCellType.Money),
                new ReportCell("totalCusto", ReportCellType.Money)
            };
            reportBuilder.InsertFooter(footerCells);

            reportBuilder.CloseMedia();
        }
    }

}
