using System;
using System.IO;
using System.Net;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.Win32;
using AccountingLib.Management;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;


namespace ServiceKicker
{
    public class UpdateHandler
    {
        private String updateUrl;

        private String updateExe;

        private String updateData;

        private String[] execParams;


        public void UpdateSoftwareProduct()
        {
            RegistryKey parentKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
            RegistryKey chlidKey = parentKey.OpenSubKey("Accounting", true);
            String productKey = (String)chlidKey.GetValue("DownloadedKey");
            String thisVersion = (String)chlidKey.GetValue("InstallerVersion");
            String printLogDir = (String)chlidKey.GetValue("LogDirectories");
            String copyLogDir = (String)chlidKey.GetValue("CopyLogDir");

            RegistrationInfo registrationInfo = LicenseKeyMaker.ReadKey(productKey, null);

            updateUrl = registrationInfo.ServiceUrl;
            updateExe = FileResource.MapDesktopResource("ClientSetup.EXE");
            updateData = FileResource.MapDesktopResource("ProductKey.BIN");
            execParams = new String[3];
            execParams[0] = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            execParams[1] = printLogDir;
            execParams[2] = copyLogDir;

            RequestHandler requestHandler = new RequestHandler(updateUrl, 90000, null);
            String requestData = Convert.ToBase64String(Encoding.Default.GetBytes("ClientSetup.EXE"));
            requestHandler.StartRequest("GetVersionNumber", requestData);
            String currentVersion = (String)requestHandler.ParseResponse(typeof(String));

            if (thisVersion != currentVersion)
            {
                // Salva em disco a chave
                FileStream fileStream = new FileStream(updateData, FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(productKey);
                streamWriter.Close();

                // Baixa o EXE do servidor
                Uri downloadUrl = new Uri(updateUrl + "?action=DownloadCurrentVersion");
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownloaded);
                webClient.DownloadFileAsync(downloadUrl, updateExe);
            }
        }

        private void FileDownloaded(Object sender, AsyncCompletedEventArgs e)
        {
            Process.Start(updateExe + " /K:" + QuoteString(updateData) + " /D:" + QuoteString(execParams[0]) + " /P:" + QuoteString(execParams[1]) + " /C:" + QuoteString(execParams[2]) + " /J" + " /S");
        }

        private String QuoteString(String text)
        {
            return '"' + text + '"';
        }
    }

}
