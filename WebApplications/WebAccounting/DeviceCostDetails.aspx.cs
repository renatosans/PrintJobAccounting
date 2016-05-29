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
    public partial class DeviceCostDetails : System.Web.UI.Page
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


            int printerId = 0;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            try
            {
                printerId = int.Parse(Request.QueryString["printerId"]);
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

            Dictionary<String, Object> reportData = DeviceCostDetailsReport.GetReportData(detailType);
            lblTitle.Text = (String)reportData["title"];

            PrinterDAO printerDAO = new PrinterDAO(accountingMasterPage.dataAccess.GetConnection());
            Printer printer = printerDAO.GetPrinter(tenant.id, printerId);
            lblDeviceName.Text = "Impressora: " + printer.alias;

            GenerateReport(printerId, startDate, endDate, detailType);
        }


        private void GenerateReport(int printerId, DateTime startDate, DateTime endDate, String detailType)
        {
            DeviceCostDetailsReport report = new DeviceCostDetailsReport(tenant.id, printerId, startDate, endDate, detailType);
            report.InitializeComponents(reportSurface, new WebReportBuilder(), accountingMasterPage.dataAccess.GetConnection());
            report.SetPage(action, currentPage);
            report.BuildReport();
        }
    }

}
