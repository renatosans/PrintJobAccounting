using System;
using System.Drawing;
using System.Collections.Generic;
using AccountingLib.Management;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.Reporting;


namespace AccountingLib.ReportMailing
{
    public class UserCostDetailsReport: AbstractReport
    {
        private int tenantId;

        private int userId;

        private DateTime startDate;

        private DateTime endDate;

        private String detailType;


        public UserCostDetailsReport(int tenantId, int userId, DateTime startDate, DateTime endDate, String detailType)
        {
            // não recebe objeto do tipo "Tenant" pois o filtro do relatório não dá suporte a tipos complexos
            this.tenantId = tenantId;
            this.userId = userId;
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
                    reportData.Add("columnNames", new String[] { "Data/Hora", "Nome do documento", "Impressora", "Páginas", "Custo" });
                    reportData.Add("columnWidths", new int[] { 25, 50, 30, 15, 15 });
                    break;
                case "CopyingCosts":
                    reportData.Add("title", "Relatório de Custos de Cópia");
                    reportData.Add("columnNames", new String[] { "Data/Hora", "Copiadora", "Páginas", "Custo" });
                    reportData.Add("columnWidths", new int[] { 35, 40, 30, 30 });
                    break;
                default:
                    reportData.Add("title", "");
                    reportData.Add("columnNames", new String[] { });
                    reportData.Add("columnWidths", new int[] { });
                    break;
            }

            return reportData;
        }

        public static ReportCell[] GetDetailRow(UserCostDetail userCostDetail, String detailType)
        {
            ReportCell[] cells;

            switch (detailType)
            {
                case "PrintingCosts":
                    cells = new ReportCell[]
                    {
                        new ReportCell(userCostDetail.jobTime.ToString()),
                        new ReportCell(userCostDetail.documentName),
                        new ReportCell(userCostDetail.printerName),
                        new ReportCell(userCostDetail.pageAmount),
                        new ReportCell(userCostDetail.cost)
                    };
                    break;
                case "CopyingCosts":
                    cells = new ReportCell[]
                    {
                        new ReportCell(userCostDetail.jobTime.ToString()),
                        new ReportCell(userCostDetail.printerName),
                        new ReportCell(userCostDetail.pageAmount),
                        new ReportCell(userCostDetail.cost)
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
                    footerCells = new ReportCell[] {};
                    break;
            }

            return footerCells;
        }

        public override void BuildReport()
        {
            Dictionary<String, Object> reportData = GetReportData(detailType);

            TenantDAO tenantDAO = new TenantDAO(sqlConnection);
            Tenant tenant = tenantDAO.GetTenant(tenantId);

            UserDAO userDAO = new UserDAO(sqlConnection);
            User user = userDAO.GetUser(tenantId, userId);

            UserCostDetailDAO userCostDetailDAO = new UserCostDetailDAO(sqlConnection);
            List<Object> userCostDetails = userCostDetailDAO.GetUserCostDetails(tenantId, userId, startDate, endDate, detailType);

            reportBuilder.OpenMedia(reportMedia); // Abre a mídia para o output do relatório

            Dictionary<String, Object> reportFilter = new Dictionary<String, Object>();
            reportFilter.Add("tenantId", tenantId);
            reportFilter.Add("userId", userId);
            reportFilter.Add("startDate", startDate);
            reportFilter.Add("endDate", endDate);
            reportFilter.Add("detailType", detailType);
            reportBuilder.SetReportHeadings(reportData["title"] + ". " + "Usuário:  " + user.name, tenant.alias, reportFilter);

            String[] columnNames = (String[])reportData["columnNames"];
            int[] columnWidths = (int[])reportData["columnWidths"];
            int rowCount = userCostDetails.Count;
            reportBuilder.CreateDataTable(columnNames, columnWidths, rowCount);
            if (reportBuilder.IsNavigable())
            {
                Dictionary<String, Object> exportOptions = ExportFormatContext.GetExportOptions(tenantId, sqlConnection);
                reportBuilder.SetNavigationData(this.GetType().Name, rowCount, exportOptions); // neste caso recordCount = rowCount
                reportBuilder.SetReportPage(action, currentPage);
            }
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                UserCostDetail userCostDetail = (UserCostDetail) userCostDetails[rowIndex];
                reportBuilder.InsertRow(rowIndex, GetDetailRow(userCostDetail, detailType));
            }
            reportBuilder.InsertFooter(GetFooterCells(detailType));

            reportBuilder.CloseMedia();
        }
    }

}
