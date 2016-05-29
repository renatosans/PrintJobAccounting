using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Security.Principal;
using Microsoft.Win32;
using AccountingClientInstaller.Util;


namespace AccountingClientInstaller
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            String licenseKey = null;
            String targetDir = null;
            String printLogDir = null;
            String copyLogDir = null;
            Boolean startInspector = false;
            Boolean skipPapercutInstall = false;
            Boolean silentMode = false;
            foreach (String argument in args)
            {
                if (argument.ToUpper().Contains("/K:")) licenseKey = ArgumentParser.GetValue(argument); // informa a chave de produto
                if (argument.ToUpper().Contains("/D:")) targetDir = ArgumentParser.GetValue(argument); // informa o diretório de instalação
                if (argument.ToUpper().Contains("/P"))  printLogDir = ArgumentParser.GetValue(argument); // informa o diretório de logs de impressão
                if (argument.ToUpper().Contains("/C"))  copyLogDir = ArgumentParser.GetValue(argument); // informa o diretório de logs de cópia
                if (argument.ToUpper().Contains("/I"))  startInspector = true; // inicia serviço de inspeção de impressoras
                if (argument.ToUpper().Contains("/J"))  skipPapercutInstall = true; // pula o passo de instalação do papercut
                if (argument.ToUpper().Contains("/S"))  silentMode = true; // executa em modo silencioso
            }

            // Verifica se o instalador está sendo executado com permissões administrativas
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(windowsIdentity);
            Boolean executingAsAdmin = windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);

            // Para debugar este programa execute o Visual Studio como administrador, caso contrário
            // o programa não vai parar nos "breakpoints" (isso se deve ao código de controle do UAC)
            Process process = Process.GetCurrentProcess();
            if ((process.ProcessName.ToUpper().Contains("VSHOST")) && (!executingAsAdmin))
            {
                String errorMessage = "Execute o Visual Studio com permissões administrativas para debugar!";
                MessageBox.Show(errorMessage, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Verifica se a caixa de dialogo do UAC (User Account Control) é necessária
            if (!executingAsAdmin)
            {
                // Pede elevação de privilégios (executa como administrador se o usuário concordar), o programa
                // atual é encerrado e uma nova instancia é executada com os privilégios concedidos
                ProcessStartInfo processInfo = new ProcessStartInfo();
                processInfo.Verb = "runas";
                processInfo.FileName = Application.ExecutablePath;
                try { Process.Start(processInfo); } catch { }
                return;
            }

            // Executa a instalação em background, sem intervenção do usuário
            if ((silentMode) && (licenseKey != null) && (targetDir != null))
            {
                String installationFilesDirectory = Path.Combine(Path.GetTempPath(), "AccountingClientFiles");
                String loggerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "PrintLogger");
                String version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                TextReader textReader = new StreamReader(licenseKey);
                String fileContent = textReader.ReadToEnd();
                RegistrationInfo registrationInfo = LicenseKeyMaker.ReadKey(fileContent, null);
                InstallationInfo installationInfo = new InstallationInfo(targetDir, printLogDir, copyLogDir);

                InstallationHandler handler = new InstallationHandler(null);
                if (!handler.Uninstall()) return;
                if (!handler.ExtractInstallationFiles(installationFilesDirectory)) return;
                if (!handler.CopyInstallationFiles(installationFilesDirectory, targetDir)) return;
                if (!handler.InstallPapercut(installationFilesDirectory, loggerPath, skipPapercutInstall)) return;
                if (!handler.CreateConfigurationXML(registrationInfo, installationInfo, false)) return;
                if (!handler.StartWindowsServices(targetDir, false)) return;
                if (!handler.AddToWindowsRegistry(fileContent, version, installationInfo.PrintLogDirectories, installationInfo.CopyLogDirectory)) return;
                if (!handler.SetLicense(registrationInfo.ServiceUrl, registrationInfo.ConvertToLicense())) return;

                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(licenseKey, targetDir, startInspector, skipPapercutInstall));
        }
    }

}
