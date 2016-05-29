using System;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Specialized;
using Microsoft.Win32;
using ICSharpCode.SharpZipLib.Zip;
using AccountingClientInstaller.Util;


namespace AccountingClientInstaller
{
    public class InstallationHandler
    {
        private IListener listener;

        private Boolean processRunning = false;


        public InstallationHandler(IListener listener)
        {
            this.listener = listener;
        }

        public Boolean ExtractInstallationFiles(String installationFilesDirectory)
        {
            // Para gerar o arquivo de instalação execute o build do sistema (build.bat), que vai montar
            // a pasta DebugData com os arquivos necessários.
            Assembly installerAssembly = Assembly.GetExecutingAssembly();
            Stream zipStream = installerAssembly.GetManifestResourceStream("ClientFiles.zip");
            if (!File.Exists("ClientFiles.zip") && (zipStream == null))
            {
                if (listener != null)
                    listener.NotifyObject(new Exception("Não foi possivel encontrar o arquivo de instalação."));
                return false;
            }

            if (Directory.Exists(installationFilesDirectory))
            {
                String[] contents = Directory.GetFiles(installationFilesDirectory, "*", SearchOption.AllDirectories);
                foreach (String file in contents) File.Delete(file);
                Directory.Delete(installationFilesDirectory, true); // comportamento bizarro do Framework, as vezes deleta recursivamente as vezes não
            }

            // Verifica se o arquivo de instalação está embarcado(dentro do executável), escolhendo entre
            // descompactar a partir do arquivo em disco ou o arquivo embarcado
            FastZip zipManager = new FastZip();
            if (zipStream == null)
                zipManager.ExtractZip("ClientFiles.zip", installationFilesDirectory, null);
            else
                zipManager.ExtractZip(zipStream, installationFilesDirectory, FastZip.Overwrite.Always, null, null, null, false, true);

            return true;
        }

        public Boolean CopyInstallationFiles(String sourceFolder, String destinationFolder)
        {
            FileInfo[] sourceFiles = null;
            try
            {
                DirectoryInfo sourceDirectory = new DirectoryInfo(Path.Combine(sourceFolder, "Binaries"));
                sourceFiles = sourceDirectory.GetFiles("*.*", SearchOption.AllDirectories);
            }
            catch (Exception exc)
            {
                if (listener != null)
                    listener.NotifyObject(new Exception("Falha ao copiar arquivos! " + exc.Message));
                return false;
            }

            TargetDirectory targetDirectory = new TargetDirectory(destinationFolder);
            if (!targetDirectory.CopyFilesFrom(sourceFiles))
            {
                if (listener != null)
                    listener.NotifyObject(new Exception("Falha ao copiar arquivos! " + targetDirectory.GetLastError()));
                return false;
            }

            return true;
        }

        public Boolean InstallPapercut(String installationFilesDirectory, String targetDirectory, Boolean skipPapercutInstall)
        {
            if (skipPapercutInstall) return true;

            try
            {
                Process.Start(Path.Combine(installationFilesDirectory, "papercut-print-logger.exe"), "/VERYSILENT /DIR=\"" + targetDirectory + "\"");
            }
            catch (Exception exc)
            {
                if (listener != null)
                    listener.NotifyObject(new Exception("Falha ao instalar pré-requisitos do sistema. (Print Logger)" + exc.Message));
                return false;
            }

            // Se não houve nenhuma falha retorna status de sucesso
            return true;
        }

        public Boolean CreateConfigurationXML(RegistrationInfo registrationInfo, InstallationInfo installationInfo, Boolean startPrintInspector)
        {
            // Verifica se as informações de instalação estão disponíveis
            if (registrationInfo == null) return false;
            if (installationInfo == null) return false;

            String interval = startPrintInspector ? "59000" : "599000";  // caso utilize o "Print Inspector" aumenta a frequência
            String xmlHash = ResourceProtector.GenerateHash(registrationInfo.ServiceUrl + registrationInfo.TenantId + interval + installationInfo.PrintLogDirectories);

            NameValueCollection taskParams = new NameValueCollection();
            taskParams.Add("url", registrationInfo.ServiceUrl);
            taskParams.Add("tenantId", registrationInfo.TenantId.ToString());
            taskParams.Add("interval", interval);
            taskParams.Add("logDirectories", installationInfo.PrintLogDirectories);
            taskParams.Add("copyLogDir", installationInfo.CopyLogDirectory);
            taskParams.Add("installationKey", ResourceProtector.GenerateHash(ResourceProtector.GetHardwareId()));
            taskParams.Add("xmlHash", xmlHash);

            // Configura os parâmetros de execução no XML
            FileStream fileStream = new FileStream(Path.Combine(installationInfo.TargetDirectory, "JobRouting.xml"), FileMode.Create);
            PrintLogContext.SetTaskParams(taskParams, fileStream);
            fileStream.Close();

            return true;
        }

        public Boolean StartWindowsServices(String servicesDirectory, Boolean startPrintInspector)
        {
            // Tenta registrar os serviços no sistema operacional e inicia-los
            try 
            {
                ServiceHandler.InstallService(Path.Combine(servicesDirectory, "PrintLogRouter.EXE"));
                ServiceHandler.StartService("Print Log Router", 33000);

                if (startPrintInspector)
                {
                    ServiceHandler.InstallService(Path.Combine(servicesDirectory, "PrintInspector.EXE"));
                    ServiceHandler.StartService("Print Inspector", 33000);
                }
            }
            catch (Exception exc)
            {
                if (listener != null)
                    listener.NotifyObject(new Exception("Falha na instalação. " + exc.Message));
                return false;
            }

            return true;
        }

        public Boolean AddToWindowsRegistry(String downloadedKey, String installerVersion, String logDirectories, String copyLogDir)
        {
            RegistryKey parentKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
            if (parentKey == null)
            {
                if (listener != null)
                    listener.NotifyObject(new Exception("Erro ao registrar addon. Não foi encontrada a chave SOFTWARE."));
                return false;
            }

            RegistryKey softwareKey = parentKey.OpenSubKey("Accounting", true);
            try
            {
                if (softwareKey == null) softwareKey = parentKey.CreateSubKey("Accounting");
                softwareKey.SetValue("DownloadedKey", downloadedKey);
                softwareKey.SetValue("InstallerVersion", installerVersion);
                softwareKey.SetValue("LogDirectories", logDirectories);
                softwareKey.SetValue("CopyLogDir", copyLogDir);
            }
            catch (Exception exc)
            {
                if (listener != null)
                    listener.NotifyObject(new Exception("Erro ao registrar chave do produto." + exc.Message));
                return false;
            }

            softwareKey.Close();
            parentKey.Close();

            return true;
        }

        public Boolean SetLicense(String serviceUrl, License license)
        {
            RequestHandler requestHandler = new RequestHandler(serviceUrl, 16000, this.listener);
            Boolean requestSucceded = requestHandler.StartRequest("SetLicense", license);
            if (!requestSucceded)
            {
                if (listener != null)
                    listener.NotifyObject(new Exception("Falha ao configurar a licença de uso no servidor."));
                return false;
            }

            String response = (String)requestHandler.ParseResponse(typeof(String));
            if (!response.Contains("License set"))
            {
                if (listener != null)
                    listener.NotifyObject(new Exception("O servidor recusou a licença de uso."));
                return false;
            }

            return true;
        }

        public Boolean Uninstall()
        {
            // A presença do serviço do windows "Print Log Router" indica que ele deve ser removido,
            // este é o principal serviço em execução no diretório mas existem outros que também são
            // removidos do registro (executa o /UNINSTALL de cada serviço)
            ServiceInfo printLogRouter = ServiceLocator.LocateWindowsService("Print Log Router");
            if (printLogRouter != null)
            {
                // Como não existe aplicativo de remoção, executa o /uninstall de cada serviço e apaga o diretório
                String servicesDirectory = Path.GetDirectoryName(printLogRouter.PathName);
                String[] serviceList = new String[] { "PrintLogRouter.exe", "PrintInspector.exe" };
                foreach (String serviceName in serviceList)
                {
                    String filename = PathFormat.Adjust(servicesDirectory) + serviceName;
                    if (!File.Exists(filename)) continue; // caso não ache o arquivo passa para o próximo da lista

                    Process serviceUninstall = new Process();
                    serviceUninstall.StartInfo.FileName = filename;
                    serviceUninstall.StartInfo.Arguments = "/UNINSTALL";
                    serviceUninstall.EnableRaisingEvents = true;
                    serviceUninstall.Exited += new EventHandler(UninstallCompleted);
                    processRunning = true;
                    serviceUninstall.Start();
                    while (processRunning) Thread.Sleep(500); // Aguarda até a finalização do processo
                }
                Directory.Delete(servicesDirectory, true);
            }

            /* devido a problemas que tem ocorrido na desinstalação do produto,  a desinstalação automática do papercut logger foi retirada
            ServiceInfo paperCutPrintLogger = ServiceLocator.LocateWindowsService("PCPrintLogger");
            if (paperCutPrintLogger != null)
            {
                // Executa o aplicativo de remoção
                //String[] arguments = ArgumentParser.ParseCommandLine(paperCutPrintLogger.PathName);
                //String printLoggerDirectory = Path.GetDirectoryName(arguments[0]);
                //String printLoggerUninstaller = Path.Combine(printLoggerDirectory, "unins000.exe");
                //if (File.Exists(printLoggerUninstaller))
                //{
                //    Process uninstaller = new Process();
                //    uninstaller.StartInfo.FileName = printLoggerUninstaller;
                //    uninstaller.StartInfo.Arguments = "/VERYSILENT";
                //    uninstaller.EnableRaisingEvents = true;
                //    uninstaller.Exited += new EventHandler(UninstallCompleted);
                //    processRunning = true;
                //    uninstaller.Start();
                //    while (processRunning) Thread.Sleep(500); // Aguarda até a finalização do processo
                //}
            }
            */

            return true;
        }

        private void UninstallCompleted(Object sender, EventArgs e)
        {
            processRunning = false;
        }
    }

}
