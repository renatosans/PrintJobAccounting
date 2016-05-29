using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using AccountingInstaller.Util;


namespace AccountingInstaller
{
    public partial class InstallServicesForm : Form
    {
        private ServicesInfo servicesInfo;

        private IListener listener;


        public InstallServicesForm(IListener listener)
        {
            InitializeComponent();
            this.listener = listener;
        }


        private Boolean InstallServices()
        {
            txtProcessInfo.Text += Environment.NewLine + "Iniciando instalação...";
            const String installFail = "Falha ao instalar serviços. ";

            if (String.IsNullOrEmpty(txtInstallDirectory.Text))
            {
                txtProcessInfo.Text += Environment.NewLine + installFail + Environment.NewLine + "É necessário informar um diretório. ";
                return false;
            }

            // Ajusta o formato do caminho de instalação
            txtInstallDirectory.Text = PathFormat.Adjust(txtInstallDirectory.Text);
            txtProcessInfo.Text += Environment.NewLine + "Preparando-se para copiar arquivos...";
            TargetDirectory targetDir = new TargetDirectory(txtInstallDirectory.Text);

            // Prepara o diretório de destino ( faz algumas verificações / cria o diretório )
            // Caso existam arquivos no diretório aborta e exibe mensagem de erro
            if (!targetDir.Mount())
            {
                txtProcessInfo.Text += Environment.NewLine + installFail + Environment.NewLine + targetDir.GetLastError();
                return false;
            }
            
            txtProcessInfo.Text += Environment.NewLine + "Copia de arquivos iniciada...";
            FileInfo[] sourceFiles = null;
            try // tenta obter os arquivos de origem (extraídos do instalador)
            {
                String tempFolder = PathFormat.Adjust(Path.GetTempPath());
                String installationFilesDirectory = PathFormat.Adjust(tempFolder + "AccountingServerFiles");
                File.Copy(installationFilesDirectory + "DataAccess.XML", installationFilesDirectory + @"WindowsServices\DataAccess.XML", true);

                DirectoryInfo sourceDirectory = new DirectoryInfo(installationFilesDirectory + "WindowsServices");
                sourceFiles = sourceDirectory.GetFiles("*.*", SearchOption.AllDirectories);
            }
            catch (Exception exc)
            {
                txtProcessInfo.Text += Environment.NewLine + installFail + Environment.NewLine + exc.Message;
                return false;
            }

            // Tenta copiar os arquivos para o diretório de instalação
            if (!targetDir.CopyFilesFrom(sourceFiles))
            {
                txtProcessInfo.Text += Environment.NewLine + installFail + Environment.NewLine + targetDir.GetLastError();
                return false;
            }

            try // Tenta registrar os serviços no sistema operacional e inicia-los
            {
                txtProcessInfo.Text += Environment.NewLine + "Registrando serviços no Windows...";
                ServiceHandler.InstallService(txtInstallDirectory.Text + "PrintLogImporter.EXE");
                ServiceHandler.InstallService(txtInstallDirectory.Text + "CopyLogImporter.EXE");
                ServiceHandler.InstallService(txtInstallDirectory.Text + "ReportMailer.EXE");

                txtProcessInfo.Text += Environment.NewLine + "Iniciando serviços...";
                ServiceHandler.StartService("Print Log Importer", 33000);
                ServiceHandler.StartService("Copy Log Importer", 33000);
                ServiceHandler.StartService("Report Mailer", 33000);
            }
            catch (Exception exc)
            {
                txtProcessInfo.Text += Environment.NewLine + installFail + Environment.NewLine + exc.Message;
                return false;
            }

            // Se não houve nenhuma falha retorna informações da instalação e notifica sucesso
            servicesInfo = GetServicesInfo();
            return true;
        }


        private ServicesInfo GetServicesInfo()
        {
            ServicesInfo servicesInfo = new ServicesInfo(txtInstallDirectory.Text);
            TextReader reader;

            reader = new StreamReader(txtInstallDirectory.Text + "PrintLogImporter_install.log");
            servicesInfo.printLogImporterStatus = reader.ReadToEnd();
            reader.Close();

            reader = new StreamReader(txtInstallDirectory.Text + "CopyLogImporter_install.log");
            servicesInfo.copyLogImporterStatus = reader.ReadToEnd();
            reader.Close();

            reader = new StreamReader(txtInstallDirectory.Text + "ReportMailer_install.log");
            servicesInfo.reportMailerStatus = reader.ReadToEnd();
            reader.Close();

            return servicesInfo;
        }


        private void btnInstall_Click(object sender, EventArgs e)
        {
            // Tenta instalar e iniciar os serviços no servidor
            if (!InstallServices()) return;

            // Exibe mensagem de sucesso nas operações
            MessageBox.Show("Todas as operações foram executadas com sucesso!");

            // Repassa informações dos serviços para o form principal
            if (listener != null)
                listener.NotifyObject(servicesInfo);

            // Fecha a janela
            this.Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Cancela (fecha a janela)
            this.Close();
        }
    }

}
