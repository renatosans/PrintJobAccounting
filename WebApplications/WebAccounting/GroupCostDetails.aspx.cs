using System;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using AccountingLib.ReportMailing;
using DocMageFramework.WebUtils;
using DocMageFramework.Reflection;


namespace WebAccounting
{
    public partial class GroupCostDetails : System.Web.UI.Page
    {
        private AccountingMasterPage accountingMasterPage;

        private Tenant tenant;

        // A primeira página do relatório é exibida caso o parâmetro "action" não seja recebido
        private String action = "";

        private int currentPage = 1;


        protected void Page_Load(Object sender, EventArgs e)
        {
            accountingMasterPage = (AccountingMasterPage)Page.Master;
            accountingMasterPage.InitializeMasterPageComponents();
            tenant = (Tenant)Session["tenant"];

            if (!String.IsNullOrEmpty(Request["action"]))
                action = Request["action"];

            if (!String.IsNullOrEmpty(Request["currPage"]))
                currentPage = int.Parse(Request["currPage"]);


            int costCenterId = 0;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            try
            {
                costCenterId = int.Parse(Request.QueryString["costCenterId"]);
                startDate = DateTime.Parse(Request.QueryString["startDate"]);
                endDate = DateTime.Parse(Request.QueryString["endDate"]);
            }
            catch (System.FormatException)
            {
                // Remove todos os controles da página
                reportSurface.Controls.Clear();

                // Mostra aviso de inconsistência nos parâmetros
                WarningMessage.Show(reportSurface, ArgumentBuilder.GetWarning());
                return;
            }

            lblTitle.Text = "Relatório de Custos de Impressão";

            CostCenterDAO costCenterDAO = new CostCenterDAO(accountingMasterPage.dataAccess.GetConnection());
            CostCenter costCenter = costCenterDAO.GetCostCenter(tenant.id, costCenterId);
            lblCostCenter.Text = "Centro de Custo: " + costCenter.name;

            GenerateReport(costCenterId, startDate, endDate);
        }

        private void GenerateReport(int costCenterId, DateTime startDate, DateTime endDate)
        {
            GroupCostDetailsReport report = new GroupCostDetailsReport(tenant.id, costCenterId, startDate, endDate);
            report.InitializeComponents(reportSurface, new WebReportBuilder(), accountingMasterPage.dataAccess.GetConnection());
            report.SetPage(action, currentPage);
            report.BuildReport();
        }
    }

}
