using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Specialized;
using AccountingClientInstaller.Util;


// Obs.: O arquivo de recursos (*.resources) anexado ao projeto foi pré-compilado para ser utilizado pelo build do sistema
// via linha de comando, no "Visual Studio" apenas o .resx é necessário. O "Sharp Develop" pode abrir arquivos *.resources
namespace AccountingClientInstaller
{
    public partial class MainForm : Form, IListener
    {
        private String licenseKey;

        private String targetDirectory;

        private Boolean startInspector;

        private Boolean skipPapercutInstall;

        private RegistrationInfo registrationInfo;

        private InstallationInfo installationInfo;

        private String content = "";

        private Boolean doNotRegister = false;


        public MainForm(String licenseKey, String targetDirectory, Boolean startInspector, Boolean skipPapercutInstall)
        {
            InitializeComponent();
            this.licenseKey = licenseKey;
            this.targetDirectory = targetDirectory;
            this.startInspector = startInspector;
            this.skipPapercutInstall = skipPapercutInstall;

            if (!EventLog.SourceExists("Accounting Client Installer"))
                EventLog.CreateEventSource("Accounting Client Installer", null);

            // Verifica se o produto já está instalado, caso esteja permitirá sua remoção
            ServiceInfo printLogRouter = ServiceLocator.LocateWindowsService("Print Log Router");
            if (printLogRouter != null)
            {
                lblGeneralInfo.Text = "O produto já foi instalado neste computador ( clique Remover para desinstalar )";
                btnInstall.Text = "Remover";
                btnInstall.Click -= new EventHandler(btnInstall_Click);
                btnInstall.Click += new EventHandler(btnUninstall_Click);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.Refresh();
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveHandler);
        }

        // Tenta carregar a dll embarcada(dentro do executável) caso não encontre ela no diretório da aplicação
        private Assembly AssemblyResolveHandler(Object sender, ResolveEventArgs args)
        {
            String[] assemblyDetails = args.Name.Split(new Char[] { ',' });
            String assemblyFilename = assemblyDetails[0] + ".dll";

            Assembly thisExe = Assembly.GetExecutingAssembly();
            Stream dllStream = thisExe.GetManifestResourceStream(assemblyFilename);
            if (dllStream != null)
            {
                Byte[] rawAssembly = new Byte[dllStream.Length];
                dllStream.Read(rawAssembly, 0, (int)dllStream.Length);
                return Assembly.Load(rawAssembly);
            }

            return null; // falha no carregamento
        }

        private void btnInstall_Click(Object sender, EventArgs e)
        {
            this.btnInstall.Enabled = false;

            String installationFilesDirectory = Path.Combine(Path.GetTempPath(), "AccountingClientFiles");
            String loggerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "PrintLogger");
            String version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            registrationInfo = null;
            EnrollmentForm enrollmentForm = new EnrollmentForm(licenseKey, this);
            enrollmentForm.ShowDialog();
            this.Refresh();
            // Se o usuário optou por não ativar o produto termina a instalação
            if (doNotRegister)
            {
                MessageBox.Show("Execução concluída. Encerrando instalador...");
                // Inicia o instalador do papercut e encerra este instalador
                Process.Start(Path.Combine(installationFilesDirectory, "papercut-print-logger.exe"));
                this.Close();
                return;
            }

            installationInfo = null;
            ConfigurationForm configurationForm = new ConfigurationForm(targetDirectory, this);
            configurationForm.ShowDialog();
            this.Refresh();
            if (installationInfo == null)
            {
                MessageBox.Show("Operação cancelada pelo usuário!");
                return;
            }

            InstallationHandler handler = new InstallationHandler(this);

            // Extrai os arquivos de instalação do pacote (zip)
            if (!handler.ExtractInstallationFiles(installationFilesDirectory)) return;

            // Copia os arquivos extraídos para o diretório de destino
            if (!handler.CopyInstallationFiles(installationFilesDirectory, installationInfo.TargetDirectory)) return;

            // Instala o Papercut Print Logger
            if (!handler.InstallPapercut(installationFilesDirectory, loggerPath, skipPapercutInstall)) return;

            // Configura os parâmetros de execução no XML
            if (!handler.CreateConfigurationXML(registrationInfo, installationInfo, this.startInspector)) return;

            // Inicia os serviços do windows
            if (!handler.StartWindowsServices(installationInfo.TargetDirectory, this.startInspector)) return;

            // Adiciona dados do Addon ao registro do windows
            if (!handler.AddToWindowsRegistry(content, version, installationInfo.PrintLogDirectories, installationInfo.CopyLogDirectory)) return;

            // Informa ao servidor que a licença está em uso por esta estação de trabalho
            if (!handler.SetLicense(registrationInfo.ServiceUrl, registrationInfo.ConvertToLicense())) return;

            MessageBox.Show("Execução concluída. Encerrando instalador...");
            this.Close();
        }

        private void btnUninstall_Click(Object sender, EventArgs e)
        {
            this.btnInstall.Enabled = false;

            // Para os serviços em execução e realiza a remoção dos arquivos
            InstallationHandler handler = new InstallationHandler(this);
            if (!handler.Uninstall()) return;

            MessageBox.Show("Execução concluída. Encerrando instalador...");
            this.Close();
        }

        private void LogException(Exception exception)
        {
            String customExceptionData = "";
            if (exception.Data.Contains("customExceptionData"))
                customExceptionData = (String)exception.Data["customExceptionData"];
            String logMessage = "Falha na instalação... " + exception.Message + Environment.NewLine + customExceptionData;

            if (EventLog.SourceExists("Accounting Client Installer"))
                EventLog.WriteEntry("Accounting Client Installer", logMessage);

            MessageBox.Show(logMessage, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void NotifyObject(Object obj)
        {
            if (obj is RegistrationInfo)
                registrationInfo = (RegistrationInfo)obj;
            if (obj is InstallationInfo)
                installationInfo = (InstallationInfo)obj;
            if (obj is ContentNotification)
                content = ((ContentNotification)obj).content;
            if (obj is DoNotRegisterNotification)
                doNotRegister = true; // usuário optou por não ativar o produto (apenas instalar o papercut)
            if (obj is Exception)
                LogException((Exception)obj);
        }
    }

}
