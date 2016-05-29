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
    public class GroupCostDetailsReport: AbstractReport
    {
        private int tenantId;

        private int costCenterId;

        private DateTime startDate;

        private DateTime endDate;


        public GroupCostDetailsReport(int tenantId, int costCenterId, DateTime startDate, DateTime endDate)
        {
            // não recebe objeto do tipo "Tenant" pois o filtro do relatório não dá suporte a tipos complexos
            this.tenantId = tenantId;
            this.costCenterId = costCenterId;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        // Retorna os custos de impressão relacionados aos usuários de um centro de custo
        private List<Object> GetDetailedCosts(CostBranch branch)
        {
            List<Object> detailedCosts = new List<Object>();

            UserPrintingCostDAO userPrintingCostDAO = new UserPrintingCostDAO(sqlConnection);
            List<Object> userPrintingCosts = userPrintingCostDAO.GetUserPrintingCosts(tenantId, startDate, endDate);

            PrintingCostsAssembler costAssembler = new PrintingCostsAssembler(userPrintingCosts);
            detailedCosts = costAssembler.GetCostsOfAssociates(branch);

            return detailedCosts;
        }

        public override void BuildReport()
        {
            TenantDAO tenantDAO = new TenantDAO(sqlConnection);
            Tenant tenant = tenantDAO.GetTenant(tenantId);

            CostCenterDAO costCenterDAO = new CostCenterDAO(sqlConnection);
            CostCenter costCenter = costCenterDAO.GetCostCenter(tenantId, costCenterId);

            CostTreePersistence persistence = new CostTreePersistence(sqlConnection);
            CostTree tree = persistence.GetCostTree(tenantId);

            CostBranch branch = tree.GetBranchById(costCenterId);
            List<Object> detailedCosts = GetDetailedCosts(branch);

            reportBuilder.OpenMedia(reportMedia); // Abre a mídia para o output do relatório

            Dictionary<String, Object> reportFilter = new Dictionary<String, Object>();
            reportFilter.Add("tenantId", tenantId);
            reportFilter.Add("costCenterId", costCenterId);
            reportFilter.Add("startDate", startDate);
            reportFilter.Add("endDate", endDate);
            reportBuilder.SetReportHeadings("Relatório de Custos de Impressão" + ". " + "Centro de Custo:  " + costCenter.name, tenant.alias, reportFilter);

            String[] columnNames = new String[] { "Usuário", "Páginas Pb", "Páginas Cor", "Total Páginas", "Custo Pb", "Custo Cor", "Total Custo" };
            int[] columnWidths = new int[] { 50, 15, 15, 15, 15, 15, 15 };
            int rowCount = detailedCosts.Count;
            reportBuilder.CreateDataTable(columnNames, columnWidths, rowCount);
            if (reportBuilder.IsNavigable())
            {
                Dictionary<String, Object> exportOptions = ExportFormatContext.GetExportOptions(tenantId, sqlConnection);
                reportBuilder.SetNavigationData(this.GetType().Name, rowCount, exportOptions); // neste caso recordCount = rowCount
                reportBuilder.SetReportPage(action, currentPage);
            }
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                UserPrintingCost userPrintingCost = (UserPrintingCost) detailedCosts[rowIndex];
                ReportCell[] cells = new ReportCell[]
                {
                    new ReportCell(userPrintingCost.userName),
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
