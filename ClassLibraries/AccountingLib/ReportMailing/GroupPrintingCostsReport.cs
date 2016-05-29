using System;
using System.Drawing;
using System.Collections.Generic;
using AccountingLib.Management;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using AccountingLib.CostArrangement;
using DocMageFramework.Reporting;


namespace AccountingLib.ReportMailing
{
    public class GroupPrintingCostsReport: AbstractReport
    {
        private int tenantId;

        private DateTime startDate;

        private DateTime endDate;


        public GroupPrintingCostsReport(int tenantId, DateTime startDate, DateTime endDate)
        {
            // não recebe objeto do tipo "Tenant" pois o filtro do relatório não dá suporte a tipos complexos
            this.tenantId = tenantId;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        private ReportCell GetGroupCell(GroupPrintingCost groupPrintingCost, Boolean navigateToGroupDetails)
        {
            ReportCell groupCell;

            // Se o relatório não é navegável apenas retorna a célula com o nome do
            // grupo de usuários (centro de custo)
            if (!navigateToGroupDetails)
            {
                groupCell = new ReportCell(groupPrintingCost.costCenterName);
                groupCell.align = ReportCellAlign.Left;
                return groupCell;
            }

            // Se o relatório é navegável cria o link que permite acessar os detalhes sobre o grupo
            String queryString = "?costCenterId=" + groupPrintingCost.costCenterId.ToString() + "&" +
                                 "startDate=" + startDate.ToString() + "&" +
                                 "endDate=" + endDate.ToString() + "&" +
                                 "detailType=PrintingCosts";

            groupCell = new ReportCell(groupPrintingCost.costCenterName, "GroupCostDetails.aspx" + queryString);
            groupCell.align = ReportCellAlign.Left;
            return groupCell;
        }

        public override void BuildReport()
        {
            TenantDAO tenantDAO = new TenantDAO(sqlConnection);
            Tenant tenant = tenantDAO.GetTenant(tenantId);

            CostTreePersistence persistence = new CostTreePersistence(sqlConnection);
            CostTree tree = persistence.GetCostTree(tenantId);

            UserPrintingCostDAO userPrintingCostDAO = new UserPrintingCostDAO(sqlConnection);
            List<Object> userPrintingCosts = userPrintingCostDAO.GetUserPrintingCosts(tenantId, startDate, endDate);

            PrintingCostsAssembler costAssembler = new PrintingCostsAssembler(userPrintingCosts);
            List<Object> groupPrintingCosts = costAssembler.GetCostsOfBranches(tree);

            reportBuilder.OpenMedia(reportMedia); // Abre a mídia para o output do relatório

            Dictionary<String, Object> reportFilter = new Dictionary<String, Object>();
            reportFilter.Add("tenantId", tenantId);
            reportFilter.Add("startDate", startDate);
            reportFilter.Add("endDate", endDate);
            reportBuilder.SetReportHeadings("Custos de impressão por Grupo (CC)", tenant.alias, reportFilter);

            String[] columnNames = new String[] { "Centro de custo", "Páginas Pb", "Páginas Cor", "Total Páginas", "Custo Pb", "Custo Cor", "Total Custo" };
            int[] columnWidths = new int[] { 50, 15, 15, 15, 15, 15, 15 };
            int rowCount = groupPrintingCosts.Count;
            reportBuilder.CreateDataTable(columnNames, columnWidths, rowCount);
            if (reportBuilder.IsNavigable())
            {
                Dictionary<String, Object> exportOptions = ExportFormatContext.GetExportOptions(tenantId, sqlConnection);
                reportBuilder.SetNavigationData(this.GetType().Name, rowCount, exportOptions); // neste caso recordCount = rowCount
                reportBuilder.SetReportPage(action, currentPage);
            }
            for(int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                GroupPrintingCost groupPrintingCost = (GroupPrintingCost) groupPrintingCosts[rowIndex];
                ReportCell[] cells = new ReportCell[]
                {
                    GetGroupCell(groupPrintingCost, reportBuilder.IsNavigable()),
                    new ReportCell(groupPrintingCost.bwPageCount),
                    new ReportCell(groupPrintingCost.colorPageCount),
                    new ReportCell(groupPrintingCost.totalPageCount),
                    new ReportCell(groupPrintingCost.bwCost),
                    new ReportCell(groupPrintingCost.colorCost),
                    new ReportCell(groupPrintingCost.totalCost)
                };
                reportBuilder.InsertRow(rowIndex, cells);
            }

            reportBuilder.CloseMedia();
        }
    }

}
