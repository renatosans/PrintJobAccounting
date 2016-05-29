using System;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;
using AccountingInstaller.Util;
using AccountingInstaller.DataManipulation;


namespace AccountingInstaller
{
    public partial class InstallWebFrontendForm : Form
    {
        private FrontendInfo frontendInfo;

        private SqlConnection sqlConnection;

        private DBAccess saAccess;

        private IListener listener;


        public InstallWebFrontendForm(DBAccess saAccess, IListener listener)
        {
            InitializeComponent();
            this.saAccess = saAccess;
            this.listener = listener;
        }


        private Boolean GetFrontendInfo()
        {
            txtProcessInfo.Text += Environment.NewLine + "Coletando dados de instalação...";
            const String retrieveFail = "Falha ao coletar dados de instalação. ";

            if (String.IsNullOrEmpty(txtSiteName.Text))
            {
                txtProcessInfo.Text += Environment.NewLine + retrieveFail + Environment.NewLine + "É necessário informar o nome do site. ";
                return false;
            }

            String iisDirectory = IISHandler.GetWebRootDirectory();
            if (iisDirectory == null)
            {
                txtProcessInfo.Text += Environment.NewLine + retrieveFail + Environment.NewLine + "Não foi possivel localizar o diretório base do IIS (wwwRoot). ";
                txtProcessInfo.Text += Environment.NewLine + "Verifique se o Internet Information Services está instalado.";
                return false;
            }

            // Ajusta o formato do caminho de instalação
            String installDirectory = PathFormat.Adjust(iisDirectory) + (txtSiteName.Text + "WebDir");

            // Se não houve nenhuma falha retorna informações do frontend e notifica sucesso
            frontendInfo = new FrontendInfo(txtSiteName.Text, installDirectory);
            return true;
        }


        private Boolean CopyInstallationFiles()
        {
            txtProcessInfo.Text += Environment.NewLine + "Copiando arquivos de instalação...";
            const String copyFail = "Falha ao copiar arquivos de instalação. ";

            TargetDirectory targetDir = new TargetDirectory(frontendInfo.installDirectory);

            // Prepara o diretório de destino ( faz algumas verificações / cria o diretório )
            // Caso existam arquivos no diretório aborta e exibe mensagem de erro
            if (!targetDir.Mount())
            {
                txtProcessInfo.Text += Environment.NewLine + copyFail + Environment.NewLine + targetDir.GetLastError();
                return false;
            }

            FileInfo[] sourceFiles = null;
            try // tenta obter os arquivos de origem (extraídos do instalador)
            {
                String tempFolder = PathFormat.Adjust(Path.GetTempPath());
                String installationFilesDirectory = PathFormat.Adjust(tempFolder + "AccountingServerFiles");
                File.Copy(installationFilesDirectory + "DataAccess.XML", installationFilesDirectory + @"WebFrontend\WebAccounting\App_Data\DataAccess.XML", true);
                File.Copy(installationFilesDirectory + "DataAccess.XML", installationFilesDirectory + @"WebFrontend\WebAdministrator\App_Data\DataAccess.XML", true);

                DirectoryInfo sourceDirectory = new DirectoryInfo(installationFilesDirectory + "WebFrontend");
                sourceFiles = sourceDirectory.GetFiles("*.*", SearchOption.AllDirectories);
            }
            catch (Exception exc)
            {
                txtProcessInfo.Text += Environment.NewLine + copyFail + Environment.NewLine + exc.Message;
                return false;
            }

            // Tenta copiar os arquivos para o diretório de instalação
            if (!targetDir.CopyFilesFrom(sourceFiles))
            {
                txtProcessInfo.Text += Environment.NewLine + copyFail + Environment.NewLine + targetDir.GetLastError();
                return false;
            }

            // Se não houve nenhuma falha notifica sucesso
            return true;
        }


        private Boolean InstallWebFrontend()
        {
            txtProcessInfo.Text += Environment.NewLine + "Iniciando instalação...";
            const String installFail = "Falha ao instalar frontend web. ";

            // Verifica se o ASP.NET está registrado no IIS
            Boolean aspNetRegistered = IISHandler.IsAspNetRegistered();
            if (!aspNetRegistered)
            {
                txtProcessInfo.Text += Environment.NewLine + installFail + Environment.NewLine + "O ASP.NET 2.0 não está registrado/habilitado no IIS.";
                return false;
            }

            // Tenta criar os diretórios virtuais no IIS
            IISHandler iisHandler = new IISHandler();
            Boolean dirCreated = false;
            String webAccountingDir = PathFormat.Adjust(frontendInfo.installDirectory) + "WebAccounting";
            String webAdministratorDir = PathFormat.Adjust(frontendInfo.installDirectory) + "WebAdministrator";
            dirCreated = iisHandler.CreateVirtualDirectory(txtSiteName.Text, webAccountingDir, "LoginPage.aspx");
            if (!dirCreated)
            {
                txtProcessInfo.Text += Environment.NewLine + installFail + Environment.NewLine + iisHandler.GetLastError();
                return false;
            }
            dirCreated = iisHandler.CreateVirtualDirectory(txtSiteName.Text + "Admin", webAdministratorDir, "LoginPage.aspx");
            if (!dirCreated)
            {
                txtProcessInfo.Text += Environment.NewLine + installFail + Environment.NewLine + iisHandler.GetLastError();
                return false;
            }

            // Pergunta ao usuário se deseja reiniciar o IIS
            String dialogText = "O instalador precisa reiniciar o serviço de publicação na internet (IIS)." + Environment.NewLine +
                                "Escolha 'sim' para reiniciar agora ou 'não' para reiniciar mais tarde.";
            DialogResult dialogResult = MessageBox.Show(dialogText, "Reiniciar IIS agora?", MessageBoxButtons.YesNo);
            if (dialogResult != DialogResult.Yes) return true; // recusou IISreset, sai do instalador (instalação OK)

            // Tenta resetar o IIS
            Boolean iisReset = iisHandler.ResetIIS();
            if (!iisReset)
            {
                txtProcessInfo.Text += Environment.NewLine + installFail + Environment.NewLine + iisHandler.GetLastError();
                return false;
            }

            // Se não houve nenhuma falha notifica sucesso
            return true;
        }


        private Boolean OpenConnection()
        {
            // Os dados da conexão já haviam sido validados previamente logo
            // a conexão com o DB é quase que uma certeza
            txtProcessInfo.Text += Environment.NewLine + "Abrindo conexão com o banco...";
            const String connectionFail = "Falha ao abrir conexão com o banco. ";

            try
            {
                sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = @"Data Source=" + saAccess.server + ";User=" + saAccess.saLogin.username + "; password=" + saAccess.saLogin.password;
                sqlConnection.Open();
            }
            catch (Exception exc)
            {
                txtProcessInfo.Text += Environment.NewLine + connectionFail + Environment.NewLine + exc.Message;
                return false;
            }

            // Se não houve nenhuma falha retorna status de sucesso
            return true;
        }


        private void CloseConnection()
        {
            sqlConnection.Close();
        }


        private Boolean SetUrl()
        {
            txtProcessInfo.Text += Environment.NewLine + "Configurando url do sistema no banco de dados...";
            const String executionFail = "Falha ao configurar url do sistema no BD. ";

            if (String.IsNullOrEmpty(txtUrl.Text))
            {
                txtProcessInfo.Text += Environment.NewLine + executionFail + "Favor informar a url de acesso externo. ";
                return false;
            }

            try
            {
                DBQuery dbQuery = new DBQuery(sqlConnection);
                // Altera o database para "AppCommon"
                dbQuery.Query = "USE AppCommon";
                dbQuery.Execute(false);
                // Atualiza a url de acesso ao sistema no banco
                dbQuery.Query = "UPDATE tb_applicationParam SET value = '" + txtUrl.Text + "' WHERE name = 'url' AND ownerTask='webAccounting'";
                dbQuery.Execute(false);
            }
            catch (Exception exc)
            {
                txtProcessInfo.Text += Environment.NewLine + executionFail + Environment.NewLine + exc.Message;
                return false;
            }

            // Se não houve nenhuma falha retorna status de sucesso
            return true;
        }


        private void btnCreate_Click(Object sender, EventArgs e)
        {
            // Obtem informações da instalação
            if (!GetFrontendInfo()) return;

            // Faz a cópia dos arquivos de instalação
            if (!CopyInstallationFiles()) return;

            // Tenta instalar o frontend web no servidor
            if (!InstallWebFrontend()) return;

            // Tenta abrir a conexão com o banco
            if (!OpenConnection()) return;

            // Tenta configurar a url de acesso ao sistema no banco
            if (!SetUrl()) return;

            // Encerra a conexão com o banco
            CloseConnection();

            // Exibe mensagem de sucesso nas operações
            MessageBox.Show("Todas as operações foram executadas com sucesso!");

            // Repassa informações do frontend para o form principal
            if (listener != null)
                listener.NotifyObject(frontendInfo);

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
