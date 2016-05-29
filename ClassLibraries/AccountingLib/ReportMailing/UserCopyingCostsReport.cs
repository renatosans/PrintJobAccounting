using System;
using System.Drawing;
using System.Collections.Generic;
using AccountingLib.Management;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.Reporting;


namespace AccountingLib.ReportMailing
{
    public class UserCopyingCostsReport: AbstractReport
    {
        private int tenantId;

        private DateTime startDate;

        private DateTime endDate;


        public UserCopyingCostsReport(int tenantId, DateTime startDate, DateTime endDate)
        {
            // não recebe objeto do tipo "Tenant" pois o filtro do relatório não dá suporte a tipos complexos
            this.tenantId = tenantId;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        private ReportCell GetUserCell(UserCopyingCost userCopyingCost, Boolean navigateToUserDetails)
        {
            // Se o relatório não é navegável apenas retorna a célula com o nome do usuário
            if (!navigateToUserDetails)
                return new ReportCell(userCopyingCost.userName);

            // Se o relatório é navegável cria o link que permite acessar os detalhes sobre o usuário
            String queryString = "?userId=" + userCopyingCost.userId.ToString() + "&" +
                                 "startDate=" + startDate.ToString() + "&" +
                                 "endDate=" + endDate.ToString() + "&" +
                                 "detailType=CopyingCosts";
            return new ReportCell(userCopyingCost.userName, "UserCostDetails.aspx" + queryString);
        }

        public override void BuildReport()
        {
            TenantDAO tenantDAO = new TenantDAO(sqlConnection);
            Tenant tenant = tenantDAO.GetTenant(tenantId);

            UserCopyingCostDAO userCopyingCostDAO = new UserCopyingCostDAO(sqlConnection);
            List<Object> userCopyingCosts = userCopyingCostDAO.GetUserCopyingCosts(tenantId, startDate, endDate);

            reportBuilder.OpenMedia(reportMedia); // Abre a mídia para o output do relatório

            Dictionary<String, Object> reportFilter = new Dictionary<String, Object>();
            reportFilter.Add("tenantId", tenantId);
            reportFilter.Add("startDate", startDate);
            reportFilter.Add("endDate", endDate);
            reportBuilder.SetReportHeadings("Custos de Cópia por Usuário", tenant.alias, reportFilter);

            String[] columnNames = new String[] { "Usuário", "Páginas", "Percentual de páginas", "Custo", "Percentual de custo" };
            int[] columnWidths = new int[] { 25, 15, 30, 15, 30 };
            int rowCount = userCopyingCosts.Count;
            reportBuilder.CreateDataTable(columnNames, columnWidths, rowCount);
            if (reportBuilder.IsNavigable())
            {
                Dictionary<String, Object> exportOptions = ExportFormatContext.GetExportOptions(tenantId, sqlConnection);
                reportBuilder.SetNavigationData(this.GetType().Name, rowCount, exportOptions); // neste caso recordCount = rowCount
                reportBuilder.SetReportPage(action, currentPage);
            }
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                UserCopyingCost userCopyingCost = (UserCopyingCost) userCopyingCosts[rowIndex];
                ReportCell[] cells = new ReportCell[]
                {
                    GetUserCell(userCopyingCost, reportBuilder.IsNavigable()),
                    new ReportCell(userCopyingCost.pageAmount),
                    new ReportCell(userCopyingCost.pagePercentage),
                    new ReportCell(userCopyingCost.cost),
                    new ReportCell(userCopyingCost.costPercentage)
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
