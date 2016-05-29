using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using AccountingLib.Entities;
using AccountingLib.ReportMailing;
using DocMageFramework.WebUtils;
using DocMageFramework.Reporting;


namespace WebAccounting
{
    public partial class UserPrintingCosts : System.Web.UI.Page
    {
        private AccountingMasterPage accountingMasterPage;

        // Exibe apenas a caixa de busca/filtro do relatório caso o parâmetro "action" não seja recebido
        private String action = "";

        private int currentPage = 1;
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            accountingMasterPage = (AccountingMasterPage)Page.Master;
            accountingMasterPage.InitializeMasterPageComponents();

            if (!String.IsNullOrEmpty(Request["action"]))
                action = Request["action"];

            if (!String.IsNullOrEmpty(Request["currPage"]))
                currentPage = int.Parse(Request["currPage"]);
            
            
            if (!Page.IsPostBack) // Ajusta os valores iniciais do filtro
            {
                SqlConnection sqlConnection = accountingMasterPage.dataAccess.GetConnection();

                ListItem[] userList = DropDownScaffold.Retrieve("pr_retrieveUser", sqlConnection, typeof(User));
                userList[0].Text = "Todos os usuários";
                cmbUser.Items.AddRange(userList);

                chkLastMonth.Checked = true;
            }
            
            // Configura os valores para a faixa de datas (considerando o periodo do último mês)
            DateRange dateRange = new DateRange(false);
            hiddenStartDate.Value = dateRange.GetFirstDay().ToString("yyyy-MM-dd");
            hiddenStartHour.Value = "08:00";
            hiddenEndDate.Value = dateRange.GetLastDay().ToString("yyyy-MM-dd");
            hiddenEndHour.Value = "18:00";

            if (chkLastMonth.Checked)
            {
                // caso o checkbox esteja marcado configura o último mês, senão recupera o viewstate
                txtStartDate.Value = hiddenStartDate.Value;
                txtStartHour.Value = hiddenStartHour.Value;
                txtEndDate.Value = hiddenEndDate.Value;
                txtEndHour.Value = hiddenEndHour.Value;
            }
            txtStartDate.Disabled = chkLastMonth.Checked;
            btnOpenCalendar1.Disabled = chkLastMonth.Checked;
            txtStartHour.Disabled = chkLastMonth.Checked;
            txtEndDate.Disabled = chkLastMonth.Checked;
            btnOpenCalendar2.Disabled = chkLastMonth.Checked;
            txtEndHour.Disabled = chkLastMonth.Checked;
            
            EmbedClientScript.AddButtonClickHandler(this.Page, "GenerateReport");
            lblErrorMessages.Text = "";

            // caso "action" não exista encerra por aqui
            if (action == "") return;

            int? userId = null;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            try
            {
                userId = DropDownScaffold.GetSelectedItemId(cmbUser);
                startDate = DateTime.Parse(txtStartDate.Value + " " + txtStartHour.Value);
                endDate = DateTime.Parse(txtEndDate.Value + " " + txtEndHour.Value);
            }
            catch (System.FormatException)
            {
                lblErrorMessages.Text = "As datas informadas não estão em um formato válido.";
                return;
            }

            if (userId != null)
            {
                String queryString = "?userId=" + userId.ToString() + "&" +
                                     "startDate=" + startDate.ToString() + "&" +
                                     "endDate=" + endDate.ToString() + "&" +
                                     "detailType=PrintingCosts";

                Response.Redirect("UserCostDetails.aspx" + queryString);
                return;
            }

            GenerateReport(startDate, endDate);
        }


        private void GenerateReport(DateTime startDate, DateTime endDate)
        {
            Tenant tenant = (Tenant)Session["tenant"];
            UserPrintingCostsReport report = new UserPrintingCostsReport(tenant.id, startDate, endDate);
            report.InitializeComponents(reportSurface, new WebReportBuilder(), accountingMasterPage.dataAccess.GetConnection());
            report.SetPage(action, currentPage);
            report.BuildReport();
        }
    }

}
