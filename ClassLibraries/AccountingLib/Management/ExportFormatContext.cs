using System;
using System.Web;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.ReportMailing;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.FileUtils;
using DocMageFramework.DataManipulation;


namespace AccountingLib.Management
{
    public static class ExportFormatContext
    {
        private static ExportFormatEnum GetCurrentFormat(HttpSessionState Session)
        {
            // Obtem os dados da sessão e servidor web
            Tenant tenant = (Tenant)Session["tenant"];
            HttpServerUtility server = HttpContext.Current.Server;

            // Abre a conexão com o banco
            DataAccess dataAccess = DataAccess.Instance;
            dataAccess.MountConnection(FileResource.MapWebResource(server, "DataAccess.xml"), DatabaseEnum.PrintAccounting);
            dataAccess.OpenConnection();

            // Busca no banco o formato configurado para exportação
            PreferenceDAO preferenceDAO = new PreferenceDAO(dataAccess.GetConnection());
            Preference exportFormat = preferenceDAO.GetTenantPreference(tenant.id, "exportFormat");
            if (exportFormat == null)
                return ExportFormatEnum.PDF; // não esta configurado, retorna default

            // Fecha a conexão com o banco
            dataAccess.CloseConnection();
            dataAccess = null;

            // Verifica se é um inteiro
            int storedFormat;
            Boolean retrieved = int.TryParse(exportFormat.value, out storedFormat);
            if (!retrieved)
                return ExportFormatEnum.PDF; // dado incorreto configurado no BD, retorna default

            // Verifica se está na faixa de valores aceitos
            if ((storedFormat < 0) || (storedFormat > 2))   // PDF = 0, XLS = 1, CSV = 2
                return ExportFormatEnum.PDF; // fora de faixa, retorna default

            return (ExportFormatEnum)storedFormat;
        }

        public static Dictionary<String, Object> GetExportOptions(ExportFormatEnum currentFormat)
        {
            Dictionary<String, Object> exportOptions = new Dictionary<String, Object>();

            if (currentFormat == ExportFormatEnum.PDF)
            {
                exportOptions.Add("ContentType", "application/pdf");
                exportOptions.Add("Disposition", "inline; filename=report.pdf");
                exportOptions.Add("ReportBuilder", new PdfReportBuilder());
            }
            if (currentFormat == ExportFormatEnum.XLS)
            {
                exportOptions.Add("ContentType", "application/vnd.ms-excel");
                exportOptions.Add("Disposition", "attachment; filename=report.xls");
                exportOptions.Add("ReportBuilder", new XlsReportBuilder());
            }
            if (currentFormat == ExportFormatEnum.CSV)
            {
                exportOptions.Add("ContentType", "application/octet-stream");
                exportOptions.Add("Disposition", "attachment; filename=report.csv");
                exportOptions.Add("ReportBuilder", new CsvReportBuilder());
            }

            return exportOptions;
        }

        public static Dictionary<String, Object> GetExportOptions(HttpSessionState Session)
        {
            ExportFormatEnum currentFormat = ExportFormatContext.GetCurrentFormat(Session);
            return GetExportOptions(currentFormat);
        }

        public static Dictionary<String, Object> GetExportOptions(int tenantId, SqlConnection sqlConnection)
        {
            ExportFormatEnum currentFormat = ExportFormatEnum.PDF;

            PreferenceDAO preferenceDAO = new PreferenceDAO(sqlConnection);
            Preference exportFormat = preferenceDAO.GetTenantPreference(tenantId, "exportFormat");

            // A conversão abaixo seria propensa a estouros de exceção visto que não esão sendo feitas
            // verificações de tipo, faixa de inteiro, etc. Estas verificações são um pouco redundantes
            // pois os valores gravados no banco tem garantia de corretude
            if (exportFormat != null)
                currentFormat = (ExportFormatEnum)int.Parse(exportFormat.value);

            return GetExportOptions(currentFormat);
        }
    }

}
