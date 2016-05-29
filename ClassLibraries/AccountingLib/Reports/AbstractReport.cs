using System;
using System.Data.SqlClient;
using DocMageFramework.AppUtils;


namespace AccountingLib.Reports
{
    public abstract class AbstractReport
    {
        protected Object reportMedia;

        protected IReportBuilder reportBuilder;

        protected SqlConnection sqlConnection;

        protected String action;

        protected int currentPage;


        public void InitializeComponents(Object reportMedia, IReportBuilder reportBuilder, SqlConnection sqlConnection)
        {
            this.reportMedia = reportMedia;
            this.reportBuilder = reportBuilder;
            this.sqlConnection = sqlConnection;
        }

        public void SetPage(String action, int currentPage)
        {
            this.action = action;
            this.currentPage = currentPage;
        }

        public abstract void BuildReport();
    }

}
