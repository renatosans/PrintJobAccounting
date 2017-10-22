using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using AccountingInstaller.Util;
using AccountingInstaller.DataManipulation;
using ICSharpCode.SharpZipLib.Zip;


// Obs.: O arquivo de recursos (*.resources) anexado ao projeto foi pré-compilado para ser utilizado pelo build do sistema
// via linha de comando, no "Visual Studio" apenas o .resx é necessário. O "Sharp Develop" pode abrir arquivos *.resources
namespace AccountingInstaller
{
    public partial class MainForm : Form, IListener
    {
        private DBAccess saAccess;

        private ImportInfo importInfo;

        private ServicesInfo servicesInfo;

        private FrontendInfo frontendInfo;

        private int beginAt = -1;


        public MainForm(DBAccess saAccess, int beginAt)
        {
            InitializeComponent();

            // Define o acesso ao banco caso seja recebido na linha de comando
            this.saAccess = saAccess;

            // Define o form inicial caso seja recebido na linha de comando
            if ((beginAt >= 1) && (beginAt <= 4)) this.beginAt = beginAt;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.Refresh();
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveHandler);

            // Utiliza os parâmetros da linha de comando, inicia a instalação a partir do form definido por "beginAt"
            if ((this.beginAt != -1) && (saAccess != null))
            {
                this.btnInstall.Enabled = false;
                BeginInstallationProcess();
            }
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

        private Boolean ExtractInstallationFiles()
        {
            // Para gerar o arquivo de instalação execute o build do sistema (build.bat), que vai montar
            // a pasta DebugData com os arquivos necessários.
            Assembly installerAssembly = Assembly.GetExecutingAssembly();
            Stream zipStream = installerAssembly.GetManifestResourceStream("ServerFiles.zip");
            if (!File.Exists("ServerFiles.zip") && (zipStream == null))
            {
                MessageBox.Show("Não foi possivel encontrar o arquivo de instalação.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            String installationFilesDirectory = Path.Combine(Path.GetTempPath(), "AccountingServerFiles");
            if (Directory.Exists(installationFilesDirectory))
                Directory.Delete(installationFilesDirectory, true);

            // Verifica se o arquivo de instalação está embarcado(dentro do executável), escolhendo entre
            // descompactar a partir do arquivo em disco ou o arquivo embarcado
            FastZip zipManager = new FastZip();
            if (zipStream == null)
                zipManager.ExtractZip("ServerFiles.zip", installationFilesDirectory, null);
            else
                zipManager.ExtractZip(zipStream, installationFilesDirectory, FastZip.Overwrite.Always, null, null, null, false, true);

            return true;
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            this.btnInstall.Enabled = false;
            this.beginAt = 1;
            BeginInstallationProcess();
        }

        private void BeginInstallationProcess()
        {
            // Extrai os arquivos de instalação do pacote (zip)
            if (!ExtractInstallationFiles()) return;

            if ((this.beginAt != -1) && (saAccess != null))
            {
                // Cria o XML de configuração no diretório temporário
                String installationFilesDirectory = Path.Combine(Path.GetTempPath(), "AccountingServerFiles");
                DBAccess.BuildDataAccess(saAccess.server, "FrameworkUser", "Abcd1234.", installationFilesDirectory);
            }

            switch (beginAt)
            {
                case 1:
                    saAccess = null;
                    CreateDatabaseForm createDatabase = new CreateDatabaseForm(this);
                    createDatabase.ShowDialog();
                    if (saAccess == null)
                    {
                        MessageBox.Show("Operação cancelada pelo usuário!");
                        return;
                    }
                    goto case 2; // continua para a próxima tela
                case 2:
                    importInfo = null;
                    ImportForm import = new ImportForm(saAccess, this);
                    import.ShowDialog();
                    if (importInfo == null)
                    {
                        MessageBox.Show("Operação cancelada pelo usuário!");
                        return;
                    }
                    if (beginAt == 2) break;
                    goto case 3; // continua para a próxima tela
                case 3:
                    servicesInfo = null;
                    InstallServicesForm installServices = new InstallServicesForm(this);
                    installServices.ShowDialog();
                    if (servicesInfo == null)
                    {
                        MessageBox.Show("Operação cancelada pelo usuário!");
                        return;
                    }
                    if (beginAt == 3) break;
                    goto case 4; // continua para a próxima tela
                case 4:
                    frontendInfo = null;
                    InstallWebFrontendForm installWebFrontend = new InstallWebFrontendForm(saAccess, this);
                    installWebFrontend.ShowDialog();
                    if (frontendInfo == null)
                    {
                        MessageBox.Show("Operação cancelada pelo usuário!");
                        return;
                    }
                    if (chkOpenFrontend.Checked)
                        Process.Start("http://localhost/" + frontendInfo.siteName);
                    break;
            }
            
            MessageBox.Show("Execução concluída. Encerrando instalador...");
            this.Close();
        }

        public void NotifyObject(Object obj)
        {
            if (obj is DBAccess)
                saAccess = (DBAccess)obj;
            if (obj is ImportInfo)
                importInfo = (ImportInfo)obj;
            if (obj is ServicesInfo)
                servicesInfo = (ServicesInfo)obj;
            if (obj is FrontendInfo)
                frontendInfo = (FrontendInfo)obj;
        }
    }

}
