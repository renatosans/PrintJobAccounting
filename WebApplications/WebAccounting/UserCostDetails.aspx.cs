using System;
using System.Drawing;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using AccountingLib.ReportMailing;
using DocMageFramework.WebUtils;
using DocMageFramework.Reflection;
using DocMageFramework.Reporting;


namespace WebAccounting
{
    public partial class UserCostDetails : System.Web.UI.Page
    {
        private AccountingMasterPage accountingMasterPage;

        private Tenant tenant;

        // A primeira página do relatório é exibida caso o parâmetro "action" não seja recebido
        private String action = "";

        private int currentPage = 1;

        private String detailType = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            accountingMasterPage = (AccountingMasterPage)Page.Master;
            accountingMasterPage.InitializeMasterPageComponents();
            tenant = (Tenant)Session["tenant"];

            if (!String.IsNullOrEmpty(Request["action"]))
                action = Request["action"];

            if (!String.IsNullOrEmpty(Request["currPage"]))
                currentPage = int.Parse(Request["currPage"]);

            if (!String.IsNullOrEmpty(Request["detailType"]))
                detailType = Request["detailType"];


            int userId = 0;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            try
            {
                userId = int.Parse(Request.QueryString["userId"]);
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

            Dictionary<String, Object> reportData = UserCostDetailsReport.GetReportData(detailType);
            lblTitle.Text = (String)reportData["title"];

            UserDAO userDAO = new UserDAO(accountingMasterPage.dataAccess.GetConnection());
            User user = userDAO.GetUser(tenant.id, userId);
            lblUsername.Text = "Usuário:  " + user.alias;

            GenerateReport(userId, startDate, endDate, detailType);
        }


        private void GenerateReport(int userId, DateTime startDate, DateTime endDate, String detailType)
        {
            UserCostDetailsReport report = new UserCostDetailsReport(tenant.id, userId, startDate, endDate, detailType);
            report.InitializeComponents(reportSurface, new WebReportBuilder(), accountingMasterPage.dataAccess.GetConnection());
            report.SetPage(action, currentPage);
            report.BuildReport();
        }
    }

}
