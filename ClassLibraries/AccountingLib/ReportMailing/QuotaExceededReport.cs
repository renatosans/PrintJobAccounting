using System;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using AccountingLib.Management;
using DocMageFramework.Reporting;


namespace AccountingLib.ReportMailing
{
    public class QuotaExceededReport: AbstractReport
    {
        private int tenantId;

        private DateTime startDate;

        private DateTime endDate;


        public QuotaExceededReport(int tenantId, DateTime startDate, DateTime endDate)
        {
            // não recebe objeto do tipo "Tenant" pois o filtro do relatório não dá suporte a tipos complexos
            this.tenantId = tenantId;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        private List<Object> GetQuotaExceededUsers()
        {
            List<Object> quotaExceededUsers = new List<Object>();

            UserPrintingCostDAO userPrintingCostDAO = new UserPrintingCostDAO(sqlConnection);
            List<Object> userPrintingCosts = userPrintingCostDAO.GetUserPrintingCosts(tenantId, startDate, endDate);

            User user = null;
            UserDAO userDAO = new UserDAO(sqlConnection);
            foreach(UserPrintingCost userPrintingCost in userPrintingCosts)
            {
                Decimal userQuota = Decimal.MaxValue;
                user = userDAO.GetUser(tenantId, userPrintingCost.userId);
                if (user.quota != null) userQuota = user.quota.Value;

                if (userPrintingCost.totalCost > userQuota)
                {
                    String[] rowValues = new String[] {
                        userPrintingCost.userName,
                        String.Format("{0:0.000}", userQuota),
                        String.Format("{0:0.000}", userPrintingCost.totalCost),
                        String.Format("{0:0.000}", userPrintingCost.totalCost - userQuota),
                    };
                    quotaExceededUsers.Add(rowValues);
                }
            }

            return quotaExceededUsers;
        }

        public override void BuildReport()
        {
            TenantDAO tenantDAO = new TenantDAO(sqlConnection);
            Tenant tenant = tenantDAO.GetTenant(tenantId);

            List<Object> quotaExceededUsers = GetQuotaExceededUsers();

            reportBuilder.OpenMedia(reportMedia); // Abre a mídia para o output do relatório

            Dictionary<String, Object> reportFilter = new Dictionary<String, Object>();
            reportFilter.Add("tenantId", tenantId);
            reportFilter.Add("startDate", startDate);
            reportFilter.Add("endDate", endDate);
            reportBuilder.SetReportHeadings("Relatório de Cotas Excedidas", tenant.alias, reportFilter);

            String[] columnNames = new String[] { "Usuário", "Cota Definida", "Valor Impressões", "Valor Excedido" };
            int[] columnWidths = new int[] { 50, 30, 30, 30 };
            int rowCount = quotaExceededUsers.Count;
            reportBuilder.CreateDataTable(columnNames, columnWidths, rowCount);
            if (reportBuilder.IsNavigable())
            {
                Dictionary<String, Object> exportOptions = ExportFormatContext.GetExportOptions(tenantId, sqlConnection);
                reportBuilder.SetNavigationData(this.GetType().Name, rowCount, exportOptions); // neste caso recordCount = rowCount
                reportBuilder.SetReportPage(action, currentPage);
            }
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                String[] rowValues = (String[])quotaExceededUsers[rowIndex];
                ReportCell[] cells = new ReportCell[]
                {
                    new ReportCell(rowValues[0]),
                    new ReportCell(rowValues[1]),
                    new ReportCell(rowValues[2]),
                    new ReportCell(rowValues[3])
                };
                reportBuilder.InsertRow(rowIndex, cells);
            }

            reportBuilder.CloseMedia();
        }
    }

}
