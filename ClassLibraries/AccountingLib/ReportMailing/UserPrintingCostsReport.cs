using System;
using System.Drawing;
using System.Collections.Generic;
using AccountingLib.Management;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.Reporting;


namespace AccountingLib.ReportMailing
{
    public class UserPrintingCostsReport: AbstractReport
    {
        private int tenantId;

        private DateTime startDate;

        private DateTime endDate;


        public UserPrintingCostsReport(int tenantId, DateTime startDate, DateTime endDate)
        {
            // não recebe objeto do tipo "Tenant" pois o filtro do relatório não dá suporte a tipos complexos
            this.tenantId = tenantId;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        private ReportCell GetUserCell(UserPrintingCost userPrintingCost, Boolean navigateToUserDetails)
        {
            // Se o relatório não é navegável apenas retorna a célula com o nome do usuário
            if (!navigateToUserDetails)
                return new ReportCell(userPrintingCost.userName);

            // Se o relatório é navegável cria o link que permite acessar os detalhes sobre o usuário
            String queryString = "?userId=" + userPrintingCost.userId.ToString() + "&" +
                                 "startDate=" + startDate.ToString() + "&" +
                                 "endDate=" + endDate.ToString() + "&" +
                                 "detailType=PrintingCosts";
            return new ReportCell(userPrintingCost.userName, "UserCostDetails.aspx" + queryString);
        }

        public override void BuildReport()
        {
            TenantDAO tenantDAO = new TenantDAO(sqlConnection);
            Tenant tenant = tenantDAO.GetTenant(tenantId);

            UserPrintingCostDAO userPrintingCostDAO = new UserPrintingCostDAO(sqlConnection);
            List<Object> userPrintingCosts = userPrintingCostDAO.GetUserPrintingCosts(tenantId, startDate, endDate);

            reportBuilder.OpenMedia(reportMedia); // Abre a mídia para o output do relatório

            Dictionary<String, Object> reportFilter = new Dictionary<String, Object>();
            reportFilter.Add("tenantId", tenantId);
            reportFilter.Add("startDate", startDate);
            reportFilter.Add("endDate", endDate);
            reportBuilder.SetReportHeadings("Custos de Impressão por usuário", tenant.alias, reportFilter);

            String[] columnNames = new String[] { "Usuário", "Páginas Pb", "Páginas Cor", "Total Páginas", "Custo Pb", "Custo Cor", "Total Custo" };
            int[] columnWidths = new int[] { 50, 15, 15, 15, 15, 15, 15 };
            int rowCount = userPrintingCosts.Count;
            reportBuilder.CreateDataTable(columnNames, columnWidths, rowCount);
            if (reportBuilder.IsNavigable())
            {
                Dictionary<String, Object> exportOptions = ExportFormatContext.GetExportOptions(tenantId, sqlConnection);
                reportBuilder.SetNavigationData(this.GetType().Name, rowCount, exportOptions); // neste caso recordCount = rowCount
                reportBuilder.SetReportPage(action, currentPage);
            }
            for(int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                UserPrintingCost userPrintingCost = (UserPrintingCost) userPrintingCosts[rowIndex];
                ReportCell[] cells = new ReportCell[]
                {
                    GetUserCell(userPrintingCost, reportBuilder.IsNavigable()),
                    new ReportCell(userPrintingCost.bwPageCount),
                    new ReportCell(userPrintingCost.colorPageCount),
                    new ReportCell(userPrintingCost.totalPageCount),
                    new ReportCell(userPrintingCost.bwCost),
                    new ReportCell(userPrintingCost.colorCost),
                    new ReportCell(userPrintingCost.totalCost)
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
