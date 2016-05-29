using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using AccountingLib.Spool;
using AccountingLib.Spool.EMF;
using AccountingLib.Printers;
using DocMageFramework.AppUtils;


namespace AccountingLib.PrintInspect
{
    public static class PrintJobContext
    {
        /// <summary>
        /// Obtem informações do job em uma String
        /// </summary>
        public static String GetJobInfo(SpooledJob spooledJob)
        {
            if (spooledJob.ShadowFile == null) // Verifica se o arquivo de Shadow está disponível
                return "?,?,?,?,?,?,?,?,?,?"; // Caso não esteja retorna uma indicação de que sua leitura falhou

            Dictionary<String, Object> jobSummary = GetJobSummary(spooledJob);
            String jobTime = DateFormat.Adjust((DateTime)jobSummary["jobTime"], true);
            String dataType = (String)jobSummary["dataType"];
            String jobSize = (String)jobSummary["spoolFileSize"];

            String jobInfo = jobTime + "," + jobSummary["userName"] + "," + jobSummary["printerName"] + "," +
                             jobSummary["documentName"] + "," + jobSummary["pageCount"] + "," + jobSummary["copyCount"] + "," +
                             jobSummary["duplex"] + "," + "false" + "," + dataType + "," + jobSize;

            return jobInfo;
        }

        /// <summary>
        /// Obtem informações do job resumidas em um Dictionary
        /// </summary>
        public static Dictionary<String, Object> GetJobSummary(SpooledJob spooledJob)
        {
            if (spooledJob.ShadowFile == null) // Verifica se o arquivo de Shadow está disponível
                return null; // Caso não esteja retorna "null"

            JobShadowFile shdw = spooledJob.ShadowFile;

            DateTime jobTime = spooledJob.FileDate;
            int pageCount = shdw.PageCount;
            int copyCount = shdw.DevMode.Copies;
            Boolean duplex = shdw.DevMode.Duplex;
            Boolean color = shdw.DevMode.Color;
            int spoolFileSize = shdw.SpoolFileSize;

            EMFSpoolFile spoolFile = null;
            Boolean isEMF = false;
            if (shdw.DataType.ToUpper().Contains("EMF"))
            {
                spoolFile = (EMFSpoolFile)spooledJob.SpoolFile;
                if (!spoolFile.MalformedFile) isEMF = true; // Verifica se o formato está OK
            }

            if (isEMF)
            {   // Substitui as informações do SHD por outras mais precisas encontradas arquivo de Spool
                if (spoolFile.Pages != null)
                {
                    pageCount = spoolFile.Pages.Count;
                }
                if (spoolFile.DevModeRecord != null)
                {
                    copyCount = spoolFile.DevModeRecord.Copies;
                    duplex = spoolFile.DevModeRecord.Duplex;
                    color = spoolFile.DevModeRecord.Color;
                }
            }

            // Busca o tamanho exato do arquivo se ele já foi aberto para leitura
            if ((spoolFile != null) && (spoolFileSize != spoolFile.FileSize))
            {
                spoolFileSize = spoolFile.FileSize;
            }

            Dictionary<String, Object> jobSummary = new Dictionary<String, Object>();
            jobSummary.Add("jobTime", jobTime);
            jobSummary.Add("submitted", shdw.Submitted.ToString());
            jobSummary.Add("userName", shdw.UserName);
            jobSummary.Add("printerName", shdw.PrinterName);
            jobSummary.Add("documentName", shdw.DocumentName);
            jobSummary.Add("pageCount", pageCount);
            jobSummary.Add("copyCount", copyCount);
            jobSummary.Add("duplex", duplex);
            jobSummary.Add("color", color);
            jobSummary.Add("dataType", isEMF ? "EMF" : "RAW");
            jobSummary.Add("spoolFileSize", FormatFileSize(spoolFileSize));

            return jobSummary;
        }

        private static String FormatFileSize(int fileSize)
        {
            int kilobytes = fileSize / 1024;
            return kilobytes.ToString() + "kb";
        }
    }

}
