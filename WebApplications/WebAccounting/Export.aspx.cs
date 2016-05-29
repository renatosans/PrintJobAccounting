using System;
using System.Collections.Generic;
using AccountingLib.Management;
using AccountingLib.ReportMailing;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.Reflection;
using DocMageFramework.DataManipulation;


namespace WebAccounting
{
    public partial class Export : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Organiza os argumentos recebidos na querystring
            ArgumentBuilder argumentBuilder = new ArgumentBuilder();
            foreach (String argumentName in Request.QueryString)
            {
                argumentBuilder.Add(argumentName, Request.QueryString[argumentName]);
            }

            // Cria uma instância da classe de relatório ( através de Reflection )
            Type reportClass = null;
            AbstractReport report = null;
            if (!String.IsNullOrEmpty(Request["report"]))
            {
                // Usa a classe base dos relatórios para obter o nome completo da classe incluindo dll/assembly
                String qualifiedName = typeof(AbstractReport).AssemblyQualifiedName;
                qualifiedName = qualifiedName.Replace("AbstractReport", Request["report"]);

                reportClass = Type.GetType(qualifiedName);
                report = (AbstractReport) Activator.CreateInstance(reportClass, argumentBuilder.GetArguments(reportClass));
            }

            // Aborta a operação caso o relatório solicitado não exista
            if ((reportClass == null) || (report == null)) return;

            Dictionary<String, Object> exportOptions = ExportFormatContext.GetExportOptions(Session);
            
            this.Response.Clear();
            this.Response.ContentType = (String)exportOptions["ContentType"];
            this.Response.AddHeader("content-disposition", (String)exportOptions["Disposition"]);

            // Abre a conexão com o banco
            DataAccess dataAccess = DataAccess.Instance;
            dataAccess.MountConnection(FileResource.MapWebResource(this.Page.Server, "DataAccess.xml"), DatabaseEnum.PrintAccounting);
            dataAccess.OpenConnection();

            // Executa inicializações e chama o método "BuildReport" na instância da classe de relatório
            report.InitializeComponents(this.Page, (IReportBuilder)exportOptions["ReportBuilder"], dataAccess.GetConnection());
            report.BuildReport();

            // Fecha a conexão com o banco
            dataAccess.CloseConnection();
            dataAccess = null;

            this.Response.End();
        }
    }

}
