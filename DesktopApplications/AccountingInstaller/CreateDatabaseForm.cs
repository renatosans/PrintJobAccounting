using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingInstaller.Util;
using AccountingInstaller.DataManipulation;


namespace AccountingInstaller
{
    public partial class CreateDatabaseForm : Form, IListener, IProgressListener
    {
        private IListener listener;

        private String installationFilesDirectory;

        private int currentProgress;


        public CreateDatabaseForm(IListener listener)
        {
            InitializeComponent();
            this.listener = listener;
            String tempFolder = PathFormat.Adjust(Path.GetTempPath());
            installationFilesDirectory = PathFormat.Adjust(tempFolder + "AccountingServerFiles");
        }


        private SqlConnection sqlConnection;
        private String server;
        private DBLogin saLogin;
        private DBLogin sysLogin;
        private int sqlVersion;


        private Boolean OpenConnection()
        {
            const String connectionFail = "Falha ao abrir conexão com o banco. ";

            if (String.IsNullOrEmpty(server))
            {
                txtProcessInfo.Text += Environment.NewLine + connectionFail + "Favor informar a instancia do SQL Server.";
                return false;
            }

            if (saLogin == null)
            {
                txtProcessInfo.Text += Environment.NewLine + connectionFail + "Favor informar um login com permissões administrativas.";
                return false;
            }

            try
            {
                sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = @"Data Source=" + server + ";User=" + saLogin.username + "; password=" + saLogin.password;
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

        private int GetSQLServerVersion()
        {
            String query = "SELECT CONVERT(INTEGER, CONVERT(FLOAT, CONVERT(VARCHAR(3), SERVERPROPERTY('ProductVersion')))) majorVersion";
            DBQuery dbQuery = new DBQuery(query, sqlConnection);
            dbQuery.Execute(true);
            List<Object> resultSet = dbQuery.ExtractFromResultset(typeof(SQLServerVersion));
            SQLServerVersion productVersion = (SQLServerVersion) resultSet[0];

            return productVersion.majorVersion;
        }

        private Boolean CreateSysUser()
        {
            const String sysUser = "FrameworkUser";
            const String sysPass = "Abcd1234.";
            txtProcessInfo.Text += Environment.NewLine + "Criando usuário para uso do sistema...";
            const String creationFail = "Falha ao criar usuário para o sistema. ";

            try
            {
                // Cria o usuário que será utilizado pelo sistema para execução de procedures
                String creationQuery;
                if (sqlVersion > 8) // SQL Server 2005 ou superior
                    creationQuery = "CREATE LOGIN " + sysUser + " WITH PASSWORD = '" + sysPass + "'";
                else
                    creationQuery = "sp_addlogin '" + sysUser + "', '" + sysPass + "'";

                DBQuery dbQuery = new DBQuery(creationQuery, sqlConnection);
                dbQuery.Execute(false);
            }
            catch (Exception exc)
            {
                txtProcessInfo.Text += Environment.NewLine + creationFail + Environment.NewLine + exc.Message;
                return false;
            }

            // Se não houve nenhuma falha armazena o usuário e retorna status de sucesso
            sysLogin = new DBLogin(sysUser, sysPass);
            return true;
        }


        private Boolean CreateDataAccess()
        {
            txtProcessInfo.Text += Environment.NewLine + "Criando arquivo de configuração...";
            const String creationFail = "Falha ao criar arquivo de configuração. ";

            if (sysLogin == null)
            {
                txtProcessInfo.Text += Environment.NewLine + creationFail + "O login de sistema não está disponível.";
                return false;
            }

            try
            {
                DBAccess.BuildDataAccess(server, sysLogin.username, sysLogin.password, installationFilesDirectory);
            }
            catch (Exception exc)
            {
                txtProcessInfo.Text += Environment.NewLine + creationFail + Environment.NewLine + exc.Message;
                return false;
            }

            // Se não houve nenhuma falha retorna status de sucesso
            return true;
        }


        private Boolean RunDBScripts()
        {
            txtProcessInfo.Text += Environment.NewLine + "Executando scripts no DB...";
            const String executionFail = "Falha ao executar scripts no servidor banco de dados. ";

            ScriptRunner scriptRunner = new ScriptRunner(new String[] { "Accounting" , "AppCommon" }, sqlConnection, this);
            try
            {
                scriptRunner.RunAll(installationFilesDirectory + "DatabaseScripts", this);
                txtProcessInfo.Text += Environment.NewLine + scriptRunner.GetFileCount() + " scripts executados.";
            }
            catch (Exception exc)
            {
                txtProcessInfo.Text += Environment.NewLine + executionFail + Environment.NewLine + exc.Message;
                txtProcessInfo.Text += Environment.NewLine + "Script em execução: " + scriptRunner.GetCurrentScript();
                return false;
            }

            // Se não houve nenhuma falha retorna status de sucesso
            return true;
        }


        private void btnSALogin_Click(object sender, EventArgs e)
        {
            CreateLoginForm createLoginForm = new CreateLoginForm(this);
            createLoginForm.ShowDialog();
        }


        public void NotifyObject(Object obj)
        {
            if (obj is DBLogin)
            {
                txtSALogin.Text = "Login(username:" + ((DBLogin)obj).username + ", password: *****)";
                txtSALogin.Tag = obj;
            }
            if (obj is String)
            {
                txtProcessInfo.Text += Environment.NewLine + obj;
            }
        }

        public void ProgressInitialized()
        {
            // Define um valor inicial no gráfico
            this.currentProgress = 2;
            progressBar.Value = 2;

            pnlProgressMeter.Visible = true;
            this.Refresh();
        }

        public void ProgressChanged(int currentProgress)
        {
            // Ignora eventos sem mudança no percentual de progresso
            if (currentProgress <= this.currentProgress)
            {
                Application.DoEvents();
                return;
            }

            this.currentProgress = currentProgress;
            progressBar.Value = currentProgress;
            progressBar.Refresh();
            this.Refresh();
        }

        public void ProgressConcluded()
        {
            pnlProgressMeter.Visible = false;
            this.Refresh();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // Evita duplos cliques
            // btnCreate.Enabled = false;

            // Obtem os valores preenchidos no form
            server = txtServer.Text;
            saLogin = (DBLogin) txtSALogin.Tag;

            // Tenta abrir a conexão com o banco
            if (!OpenConnection()) return;

            // Obtem a versão do SQL Server
            sqlVersion = GetSQLServerVersion();

            // Tenta criar o usuário do sistema no banco
            if (!CreateSysUser()) return;

            // Tenta criar o arquivo de configuração para acesso do sistema
            if (!CreateDataAccess()) return;

            // Executa os scripts a partir do container
            if (!RunDBScripts()) return;

            // Exibe mensagem de sucesso nas operações
            // btnCreate.Enabled = true;
            MessageBox.Show("Todas as operações foram executadas com sucesso!");

            // Repassa as informações de login para o form principal
            DBAccess saAccess = new DBAccess(server, saLogin);
            if (listener != null)
                listener.NotifyObject(saAccess);

            // Encerra a conexão com o banco e fecha a janela
            CloseConnection();
            this.Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Cancela (fecha a janela)
            this.Close();
        }
    }

}
