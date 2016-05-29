using System;
using System.Drawing;
using System.Collections.Generic;
using AccountingLib.Management;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.Reporting;


namespace AccountingLib.ReportMailing
{
    public class DeviceCostDetailsReport: AbstractReport
    {
        private int tenantId;

        private int printerId;

        private DateTime startDate;

        private DateTime endDate;

        private String detailType;


        public DeviceCostDetailsReport(int tenantId, int printerId, DateTime startDate, DateTime endDate, String detailType)
        {
            // não recebe objeto do tipo "Tenant" pois o filtro do relatório não dá suporte a tipos complexos
            this.tenantId = tenantId;
            this.printerId = printerId;
            this.startDate = startDate;
            this.endDate = endDate;
            this.detailType = detailType;
        }

        public static Dictionary<String, Object> GetReportData(String detailType)
        {
            Dictionary<String, Object> reportData = new Dictionary<String, Object>();

            switch (detailType)
            {
                case "PrintingCosts":
                    reportData.Add("title", "Relatório de Custos de Impressão");
                    reportData.Add("columnNames", new String[] { "Data/Hora", "Nome do documento", "Usuário", "Páginas", "Custo" });
                    reportData.Add("columnWidths", new int[] { 25, 60, 15, 15, 15 });
                    break;
                case "CopyingCosts":
                    reportData.Add("title", "Relatório de Custos de Cópia");
                    reportData.Add("columnNames", new String[] { "Data/Hora", "Usuário", "Páginas", "Custo" });
                    reportData.Add("columnWidths", new int[] { 40, 30, 30, 30 });
                    break;
                default:
                    reportData.Add("title", "");
                    reportData.Add("columnNames", new String[] { });
                    reportData.Add("columnWidths", new int[] { });
                    break;
            }

            return reportData;
        }

        public static ReportCell[] GetDetailRow(DeviceCostDetail deviceCostDetail, String detailType)
        {
            ReportCell[] cells;

            switch (detailType)
            {
                case "PrintingCosts":
                    cells = new ReportCell[]
                    {
                        new ReportCell(deviceCostDetail.jobTime.ToString()),
                        new ReportCell(deviceCostDetail.documentName),
                        new ReportCell(deviceCostDetail.userName),
                        new ReportCell(deviceCostDetail.pageAmount),
                        new ReportCell(deviceCostDetail.cost)
                    };
                    break;
                case "CopyingCosts":
                    cells = new ReportCell[]
                    {
                        new ReportCell(deviceCostDetail.jobTime.ToString()),
                        new ReportCell(deviceCostDetail.userName),
                        new ReportCell(deviceCostDetail.pageAmount),
                        new ReportCell(deviceCostDetail.cost)
                    };
                    break;
                default:
                    cells = new ReportCell[] { };
                    break;
            }

            return cells;
        }

        public static ReportCell[] GetFooterCells(String detailType)
        {
            ReportCell[] footerCells;

            switch (detailType)
            {
                case "PrintingCosts":
                    footerCells = new ReportCell[]
                    {
                        new ReportCell("TOTAL", Color.Red),
                        new ReportCell("", Color.Red),
                        new ReportCell("", Color.Red),
                        new ReportCell("totalPaginas", ReportCellType.Number),
                        new ReportCell("totalCusto", ReportCellType.Money)
                    };
                    break;
                case "CopyingCosts":
                    footerCells = new ReportCell[]
                    {
                        new ReportCell("TOTAL", Color.Red),
                        new ReportCell("", Color.Red),
                        new ReportCell("totalPaginas", ReportCellType.Number),
                        new ReportCell("totalCusto", ReportCellType.Money)
                    };
                    break;
                default:
                    footerCells = new ReportCell[] { };
                    break;
            }

            return footerCells;
        }

        public override void BuildReport()
        {
            Dictionary<String, Object> reportData = GetReportData(detailType);

            TenantDAO tenantDAO = new TenantDAO(sqlConnection);
            Tenant tenant = tenantDAO.GetTenant(tenantId);
            
            PrinterDAO printerDAO = new PrinterDAO(sqlConnection);
            Printer printer = printerDAO.GetPrinter(tenantId, printerId);

            DeviceCostDetailDAO deviceCostDetailDAO = new DeviceCostDetailDAO(sqlConnection);
            List<Object> deviceCostDetails = deviceCostDetailDAO.GetDeviceCostDetails(tenantId, printerId, startDate, endDate, detailType);

            reportBuilder.OpenMedia(reportMedia); // Abre a mídia para o output do relatório

            Dictionary<String, Object> reportFilter = new Dictionary<String, Object>();
            reportFilter.Add("tenantId", tenantId);
            reportFilter.Add("printerId", printerId);
            reportFilter.Add("startDate", startDate);
            reportFilter.Add("endDate", endDate);
            reportFilter.Add("detailType", detailType);
            reportBuilder.SetReportHeadings(reportData["title"] + ". " + "Impressora: " + printer.name, tenant.alias, reportFilter);

            String[] columnNames = (String[])reportData["columnNames"];
            int[] columnWidths = (int[])reportData["columnWidths"];
            int rowCount = deviceCostDetails.Count;
            reportBuilder.CreateDataTable(columnNames, columnWidths, rowCount);
            if (reportBuilder.IsNavigable())
            {
                Dictionary<String, Object> exportOptions = ExportFormatContext.GetExportOptions(tenantId, sqlConnection);
                reportBuilder.SetNavigationData(this.GetType().Name, rowCount, exportOptions); // neste caso recordCount = rowCount
                reportBuilder.SetReportPage(action, currentPage);
            }
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++ )
            {
                DeviceCostDetail deviceCostDetail = (DeviceCostDetail) deviceCostDetails[rowIndex];
                reportBuilder.InsertRow(rowIndex, GetDetailRow(deviceCostDetail, detailType));
            }
            reportBuilder.InsertFooter(GetFooterCells(detailType));

            reportBuilder.CloseMedia();
        }
    }

}
