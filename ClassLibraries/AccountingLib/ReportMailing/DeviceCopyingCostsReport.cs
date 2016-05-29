using System;
using System.Drawing;
using System.Collections.Generic;
using AccountingLib.Management;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.Reporting;


namespace AccountingLib.ReportMailing
{
    public class DeviceCopyingCostsReport: AbstractReport
    {
        private int tenantId;

        private DateTime startDate;

        private DateTime endDate;


        public DeviceCopyingCostsReport(int tenantId, DateTime startDate, DateTime endDate)
        {
            // não recebe objeto do tipo "Tenant" pois o filtro do relatório não dá suporte a tipos complexos
            this.tenantId = tenantId;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        private ReportCell GetDeviceCell(DeviceCopyingCost deviceCopyingCost, Boolean navigateToDeviceDetails)
        {
            // Se o relatório não é navegável apenas retorna a célula com o nome da copiadora
            if (!navigateToDeviceDetails)
                return new ReportCell(deviceCopyingCost.printerName);

            // Se o relatório é navegável cria o link que permite acessar os detalhes da copiadora
            String queryString = "?printerId=" + deviceCopyingCost.printerId.ToString() + "&" +
                                 "startDate=" + startDate.ToString() + "&" +
                                 "endDate=" + endDate.ToString() + "&" +
                                 "detailType=CopyingCosts";
            return new ReportCell(deviceCopyingCost.printerName, "DeviceCostDetails.aspx" + queryString);
        }

        public override void BuildReport()
        {
            TenantDAO tenantDAO = new TenantDAO(sqlConnection);
            Tenant tenant = tenantDAO.GetTenant(tenantId);

            DeviceCopyingCostDAO deviceCopyingCostDAO = new DeviceCopyingCostDAO(sqlConnection);
            List<Object> deviceCopyingCosts = deviceCopyingCostDAO.GetDeviceCopyingCosts(tenantId, startDate, endDate);

            reportBuilder.OpenMedia(reportMedia); // Abre a mídia para o output do relatório

            Dictionary<String, Object> reportFilter = new Dictionary<String, Object>();
            reportFilter.Add("tenantId", tenantId);
            reportFilter.Add("startDate", startDate);
            reportFilter.Add("endDate", endDate);
            reportBuilder.SetReportHeadings("Custos de Cópia por Equipamento", tenant.alias, reportFilter);

            String[] columnNames = new String[] { "Copiadora", "Páginas", "Percentual de páginas", "Custo", "Percentual de custo" };
            int[] columnWidths = new int[] { 25, 15, 30, 15, 30 };
            int rowCount = deviceCopyingCosts.Count;
            reportBuilder.CreateDataTable(columnNames, columnWidths, rowCount);
            if (reportBuilder.IsNavigable())
            {
                Dictionary<String, Object> exportOptions = ExportFormatContext.GetExportOptions(tenantId, sqlConnection);
                reportBuilder.SetNavigationData(this.GetType().Name, rowCount, exportOptions); // neste caso recordCount = rowCount
                reportBuilder.SetReportPage(action, currentPage);
            }
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                DeviceCopyingCost deviceCopyingCost = (DeviceCopyingCost) deviceCopyingCosts[rowIndex];
                ReportCell[] cells = new ReportCell[]
                {
                    GetDeviceCell(deviceCopyingCost, reportBuilder.IsNavigable()),
                    new ReportCell(deviceCopyingCost.pageAmount),
                    new ReportCell(deviceCopyingCost.pagePercentage),
                    new ReportCell(deviceCopyingCost.cost),
                    new ReportCell(deviceCopyingCost.costPercentage)
                };
                reportBuilder.InsertRow(rowIndex, cells);
            }
            ReportCell[] footerCells = new ReportCell[]
            {
                new ReportCell("TOTAL", Color.Red),
                new ReportCell("totalPaginas", ReportCellType.Number),
                new ReportCell("totalPercPag", ReportCellType.Percentage),
                new ReportCell("totalCusto", ReportCellType.Money),
                new ReportCell("totalPercCusto", ReportCellType.Percentage)
            };
            reportBuilder.InsertFooter(footerCells);

            reportBuilder.CloseMedia();
        }
    }

}
