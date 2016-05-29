using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingInstaller.Util;
using AccountingInstaller.DataManipulation;


namespace AccountingInstaller
{
    public partial class CreateTenantForm : Form
    {
        private DBAccess saAccess;

        private IListener listener;

        private List<TenantInfo> tenantList;


        public CreateTenantForm(DBAccess saAccess, IListener listener)
        {
            InitializeComponent();
            this.saAccess = saAccess;
            this.listener = listener;
            this.tenantList = null; // Só cria a instancia da lista ao criar o primeiro item a ser inserido

            txtProcessInfo.Clear();

            // Tenta abrir a conexão com o banco
            if (!OpenConnection())
            {
                // Desabilita as ações que precisam de conexão em caso de falha
                btnCreate.Enabled = false;
                btnFinish.Enabled = false;
            }

            txtProcessInfo.Text += "Aguardando preenchimento dos dados..." + Environment.NewLine;

            // Exibe a lista de empresas cadastradas no sistema
            DisplayAvailableTenants();
            this.Refresh();
        }

        private SqlConnection sqlConnection;

        private Boolean OpenConnection()
        {
            // Os dados da conexão já haviam sido validados previamente logo
            // a conexão com o DB é quase que uma certeza
            txtProcessInfo.Text += "Abrindo conexão com o banco..." + Environment.NewLine;
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


        private Boolean CreateTenant()
        {
            txtProcessInfo.Text += Environment.NewLine + "Criando empresa no sistema...";
            const String creationFail = "Falha ao criar a empresa no sistema. ";

            if (String.IsNullOrEmpty(txtTenantName.Text))
            {
                txtProcessInfo.Text += Environment.NewLine + creationFail + "Favor informar um identificador para a empresa. ";
                return false;
            }

            if (String.IsNullOrEmpty(txtTenantAlias.Text))
            {
                txtProcessInfo.Text += Environment.NewLine + creationFail + "Favor informar um nome amigável para a empresa. ";
                return false;
            }

            TenantInfo tenantInfo = null;
            try
            {
                DBQuery dbQuery = new DBQuery(sqlConnection);

                // Altera o database para "AppCommon"
                dbQuery.Query = "USE AppCommon";
                dbQuery.Execute(false);

                // Insere a empresa no banco de dados (guarda o id da empresa inserida no banco)
                txtProcessInfo.Text += Environment.NewLine + "Inserindo a empresa no BD...";
                dbQuery.Query = "INSERT INTO tb_tenant VALUES ('" + txtTenantName.Text + "', '" + txtTenantAlias.Text + "')" + Environment.NewLine +
                                "SELECT SCOPE_IDENTITY() tenantId";
                dbQuery.Execute(true);
                int? tenantId = dbQuery.ExtractFromResultset();

                // Cria o tenantInfo, o tenantId foi o id atribuido pelo BD na operação INSERT
                // e obtido através do SCOPE_IDENTITY()
                tenantInfo = new TenantInfo(tenantId.Value, txtTenantName.Text, txtTenantAlias.Text);

                // Insere as preferências para empresa no banco de dados
                txtProcessInfo.Text += Environment.NewLine + "Inserindo preferências para a empresa no BD...";
                dbQuery.Query = "INSERT INTO tb_tenantPreference VALUES (" + tenantInfo.id + ", 'sysSender', 'datacount@datacopy.com.br', 'System.String')";
                dbQuery.Execute(false);

                // Insere os logins de acesso para a empresa no banco de dados
                txtProcessInfo.Text += Environment.NewLine + "Inserindo logins de acesso para a empresa no BD...";
                dbQuery.Query = "INSERT INTO tb_login VALUES (" + tenantInfo.id + ", 'admin', '1E588BE3A984524C7F2C278686F44E72', 0, 0)" + Environment.NewLine +
                                "INSERT INTO tb_login VALUES (" + tenantInfo.id + ", 'guest', '1E588BE3A984524C7F2C278686F44E72', 1, 0)";
                dbQuery.Execute(false);

                // Insere o servidor de smtp default para a empresa no banco de dados
                txtProcessInfo.Text += Environment.NewLine + "Inserindo servidor de smtp(default) para a empresa no BD...";
                dbQuery.Query = "INSERT INTO tb_smtpServer VALUES (" + tenantInfo.id + ", 'Servidor Default', 'smtp.gmail.com', 587, 'datacount@datacopy.com.br', 'datacopy123', 0)";
                dbQuery.Execute(false);

                // Altera o database para "Accounting"
                dbQuery.Query = "USE Accounting";
                dbQuery.Execute(false);

                // Insere o centro de custo raiz para a empresa no banco de dados
                txtProcessInfo.Text += Environment.NewLine + "Inserindo centro de custo (raiz) para a empresa no BD...";
                dbQuery.Query = "INSERT INTO tb_costCenter VALUES (" + tenantInfo.id + ", '" + tenantInfo.alias + "', NULL)";
                dbQuery.Execute(false);
            }
            catch (Exception exc)
            {
                txtProcessInfo.Text += Environment.NewLine + creationFail + Environment.NewLine + exc.Message;
                return false;
            }

            // Se não houve nenhuma falha insere a empresa na lista retorna status de sucesso
            if (tenantList == null) tenantList = new List<TenantInfo>();
            tenantList.Add(tenantInfo);
            return true;
        }

        private void DisplayAvailableTenants()
        {
            DBQuery dbQuery = new DBQuery(sqlConnection);

            // Altera o database para "AppCommon"
            dbQuery.Query = "USE AppCommon";
            dbQuery.Execute(false);

            // Busca todos os tenants cadastrados no banco
            dbQuery.Query = "SELECT * FROM tb_tenant";
            dbQuery.Execute(true);

            DataTable tenantTable = dbQuery.ExtractFromResultset(typeof(TenantInfo), "tenantTable");
            tenantGridView.DataSource = tenantTable;
            tenantGridView.Columns[2].MinimumWidth = tenantGridView.Width;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // Evita duplos cliques
            // btnCreate.Enabled = false;

            // Tenta criar a empresa no banco de dados
            if (!CreateTenant()) return;

            // Exibe mensagem de sucesso nas operações
            // btnCreate.Enabled = true;
            MessageBox.Show("Todas as operações foram executadas com sucesso!");

            txtTenantName.Clear();
            txtTenantAlias.Clear();
            txtProcessInfo.Clear();
            txtProcessInfo.Text += "Aguardando preenchimento dos dados..." + Environment.NewLine;

            DisplayAvailableTenants();
            this.Refresh();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            // Verifica se pelo menos uma empresa foi criada
            if (tenantList == null)
            {
                MessageBox.Show("Você deve criar pelo menos uma empresa!");
                return;
            }

            // Repassa informações sobre os tenants para o form principal
            if (listener != null)
                listener.NotifyObject(tenantList);

            // Encerra a conexão com o banco e fecha a janela
            CloseConnection();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Cancela (fecha a janela)
            CloseConnection();
            this.Close();
        }
    }

}
