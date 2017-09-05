using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using AccountingInstaller.Util;
using AccountingInstaller.DataManipulation;


namespace AccountingInstaller
{
    public partial class ImportForm: Form, IProgressListener
    {
        private DBAccess saAccess;

        private IListener listener;

        private String dataDirectory;

        private int fileCount;

        private int currentProgress;


        public ImportForm(DBAccess saAccess, IListener listener)
        {
            InitializeComponent();
            this.saAccess = saAccess;
            this.listener = listener;
        }

        private void DisableTenantSelection()
        {
            tenantListBox.Enabled = false;
            tenantListBox.BackColor = Color.LightGray;
            selectedListBox.Enabled = false;
            selectedListBox.BackColor = Color.LightGray;
            btnSelect.Enabled = false;
            btnDeselect.Enabled = false;
        }

        private void ImportForm_Shown(Object sender, EventArgs e)
        {
            // Obtem a lista de empresas a partir da massa de dados
            String baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.ToString());
            dataDirectory = PathFormat.Adjust(baseDir) + "Data";
            List<DBObject> tenantList = ImportContext.GetTenantsFromImportData(dataDirectory);

            // Desabilita a importação pois o não foi possivel carregar a lista de empresas
            if (tenantList.Count < 1)
            {
                DisableTenantSelection();
                btnImport.Enabled = false;
                txtProcessInfo.Clear();
                txtProcessInfo.Text = "Massa de dados não encontrada. Aguardando ação do usuário...";
                return;
            }

            // Monta a view com as empresas existentes
            foreach(DBObject tenant in tenantList)
            {
                tenantListBox.Items.Add(tenant);
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            List<Object> removeList = new List<Object>();
            foreach (Object item in tenantListBox.SelectedItems)
            {
                selectedListBox.Items.Add(item);
                removeList.Add(item);
            }

            foreach (Object item in removeList)
            {
                tenantListBox.Items.Remove(item);
            }
        }

        private void btnDeselect_Click(object sender, EventArgs e)
        {
            List<Object> removeList = new List<Object>();
            foreach(Object item in selectedListBox.SelectedItems)
            {
                tenantListBox.Items.Add(item);
                removeList.Add(item);
            }

            foreach (Object item in removeList)
            {
                selectedListBox.Items.Remove(item);
            }
        }

        // Importa para o banco a massa de dados previamente armazenada em arquivos XML (dados das tabelas)
        private Boolean ImportData(DBAccess saAccess)
        {
            txtProcessInfo.Text += Environment.NewLine + "Importando massa de dados para o BD...";
            const String executionFail = "Falha ao importar massa de dados. ";

            // Verifica o acesso ao banco
            if (saAccess == null)
            {
                txtProcessInfo.Text += Environment.NewLine + executionFail + "O acesso ao DB não está disponível. ";
                return false;
            }

            // Verifica se o diretório com a massa de dados existe
            if (!Directory.Exists(dataDirectory))
            {
                txtProcessInfo.Text += Environment.NewLine + executionFail + "O diretório com a massa de dados não foi encontrado. ";
                return false;
            }

            // Verifica se os arquivos foram exportados para o diretório
            DirectoryInfo dirInfo = new DirectoryInfo(dataDirectory);
            FileInfo[] dataFiles = dirInfo.GetFiles("*.xml", SearchOption.TopDirectoryOnly);
            fileCount = dataFiles.Length;
            if (fileCount < 1)
            {
                txtProcessInfo.Text += Environment.NewLine + executionFail + "Nenhum arquivo XML foi encontrado. ";
                return false;
            }

            // Verifica se o usuário escolheu as empresas que deseja importar
            List<int> selectedTenants = new List<int>();
            foreach (Object item in selectedListBox.Items) selectedTenants.Add(((DBObject)item).id);
            if (selectedTenants.Count < 1)
            {
                txtProcessInfo.Text += Environment.NewLine + executionFail + "Nenhuma empresa foi escolhida para importação. ";
                return false;
            }

            // Executa a importação/recuperação da massa de dados
            Recovery recovery = new Recovery(saAccess, dataDirectory);
            Boolean imported;
            // ************  Tabelas migradas para Accounting para evitar erros no Azure  ************
            // imported = recovery.DBImport("AppCommon", selectedTenants, true, this);
            // if (!imported)
            // {
            //    txtProcessInfo.Text += Environment.NewLine + executionFail + recovery.GetLastError();
            //    return false;
            // }
            imported = recovery.DBImport("Accounting", selectedTenants, true, this);
            if (!imported)
            {
                txtProcessInfo.Text += Environment.NewLine + executionFail + recovery.GetLastError();
                return false;
            }

            txtProcessInfo.Text += Environment.NewLine + recovery.GetWarnings();

            // Se não houve nenhuma falha retorna status de sucesso
            return true;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            // Tenta realizar a importação
            if (!ImportData(saAccess)) return;

            // Exibe mensagem de sucesso nas operações
            MessageBox.Show("Todas as operações foram executadas com sucesso!");

            // Repassa informações para o form principal
            ImportInfo importInfo = new ImportInfo(false, dataDirectory, fileCount);
            if (listener != null)
                listener.NotifyObject(importInfo);

            // Execução concluída, fecha a janela
            this.Close();
        }

        public void ProgressInitialized()
        {
            // Define um valor inicial no gráfico
            this.currentProgress = 2;
            progressBar.Value = 2;

            lblDetails.Visible = false;
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
            lblDetails.Visible = true;
            pnlProgressMeter.Visible = false;
            this.Refresh();
        }

        // Cria a massa de dados inicial para uso do sistema caso seja uma instalação sem importação de dados
        private Boolean CreateStartupData(DBAccess saAccess)
        {
            txtProcessInfo.Text += Environment.NewLine + "Criando massa de dados inicial no BD...";
            const String executionFail = "Falha ao criar massa de dados. ";

            // Verifica o acesso ao banco
            if (saAccess == null)
            {
                txtProcessInfo.Text += Environment.NewLine + executionFail + "O acesso ao DB não está disponível. ";
                return false;
            }

            StartupData startupData = new StartupData(saAccess);
            Boolean created = startupData.Create();
            if (!created)
            {
                txtProcessInfo.Text += Environment.NewLine + executionFail + startupData.GetLastError();
                return false;
            }

            // Se não houve nenhuma falha retorna status de sucesso
            return true;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // Tenta criar a massa inicial de dados
            if (!CreateStartupData(saAccess)) return;

            // Exibe mensagem de sucesso nas operações
            MessageBox.Show("Todas as operações foram executadas com sucesso!");

            // Repassa informações para o form principal
            ImportInfo importInfo = new ImportInfo(true, null, null);
            if (listener != null)
                listener.NotifyObject(importInfo);

            // Execução concluída, fecha a janela
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Cancela (fecha a janela)
            this.Close();
        }
    }

}
