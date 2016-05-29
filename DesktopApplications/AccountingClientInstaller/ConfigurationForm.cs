using System;
using System.IO;
using System.Windows.Forms;
using AccountingClientInstaller.Util;


namespace AccountingClientInstaller
{
    public partial class ConfigurationForm : Form
    {
        private InstallationInfo installationInfo;

        private IListener listener;

        private String copyLogDirectory;

        private String lastError;


        public ConfigurationForm(String targetDirectory, IListener listener)
        {
            InitializeComponent();
            this.installationInfo = null;
            this.listener = listener;
            String programFilesDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            txtTargetDirectory.Text = PathFormat.Adjust(programFilesDir) + "DataCount";
            if (!String.IsNullOrEmpty(targetDirectory)) txtTargetDirectory.Text = targetDirectory;
        }

        private void btnAdd_Click(Object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            DialogResult dialogResult = folderDialog.ShowDialog();
            
            // Verifica se o usuário escolheu uma pasta
            if (dialogResult == DialogResult.Cancel) return;
            
            if (!String.IsNullOrEmpty(txtLogDirectories.Text)) txtLogDirectories.Text += ";";
            txtLogDirectories.Text += folderDialog.SelectedPath;
        }

        private Boolean CheckLogDirectories()
        {
            String programFilesDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            String loggerPath = Path.Combine(programFilesDir, "PrintLogger");
            String paperCutLogDirectory = Path.Combine(loggerPath, @"logs\csv\daily");
            Boolean paperCutLogDirIncluded = false;

            String[] logDirectories = txtLogDirectories.Text.Split(new Char[] { ';' });
            foreach(String directory in logDirectories)
            {
                if ((!String.IsNullOrEmpty(directory)) && (!Directory.Exists(directory)))
                {
                    lastError = "O diretório " + directory + " é inválido.";
                    return false;
                }

                if (directory.ToUpper() == paperCutLogDirectory.ToUpper()) paperCutLogDirIncluded = true;
            }

            // Adiciona o diretório de logs do papercut caso o usuário não tenha adicionado
            if (!paperCutLogDirIncluded)
            {
                if (!String.IsNullOrEmpty(txtLogDirectories.Text)) txtLogDirectories.Text += ";";
                txtLogDirectories.Text += paperCutLogDirectory;
                this.Refresh(); Application.DoEvents();
            }

            return true;
        }

        private Boolean CheckSubmitedInfo()
        {
            txtProcessInfo.Text += Environment.NewLine + "Verificando dados fornecidos...";
            const String submitFail = "Dados incorretos. ";

            if (String.IsNullOrEmpty(txtTargetDirectory.Text))
            {
                txtProcessInfo.Text += Environment.NewLine + submitFail + Environment.NewLine + "É necessário informar um diretório de instalação. ";
                return false;
            }

            if (!CheckLogDirectories())
            {
                txtProcessInfo.Text += Environment.NewLine + submitFail + Environment.NewLine + lastError;
                return false;
            }

            return true;
        }

        private Boolean PrepareDirectory()
        {
            txtProcessInfo.Text += Environment.NewLine + "Preparando diretório de instalação...";
            const String prepareFail = "O diretório de instalação apresentou alguns problemas. ";

            // Ajusta o formato do caminho de instalação
            txtTargetDirectory.Text = PathFormat.Adjust(txtTargetDirectory.Text);

            // Prepara o diretório de destino ( faz algumas verificações / cria o diretório )
            // Caso existam arquivos no diretório aborta e exibe mensagem de erro
            TargetDirectory targetDir = new TargetDirectory(txtTargetDirectory.Text);
            if (!targetDir.Mount())
            {
                txtProcessInfo.Text += Environment.NewLine + prepareFail + Environment.NewLine + targetDir.GetLastError();
                return false;
            }

            // Cria o subdiretório para gravação de log de cópias
            copyLogDirectory = @"C:\CopyLogs";
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(copyLogDirectory);
                dirInfo.Create();
            }
            catch (Exception exc)
            {
                txtProcessInfo.Text += Environment.NewLine + prepareFail + Environment.NewLine + exc.Message;
                return false;
            }

            return true;
        }

        private void btnSubmit_Click(Object sender, EventArgs e)
        {
            // Verifica as informações fornecidas
            if (!CheckSubmitedInfo()) return;

            // Prepara o diretório de instalação
            if (!PrepareDirectory()) return;

            // Repassa as informações para o form principal
            installationInfo = new InstallationInfo(txtTargetDirectory.Text, txtLogDirectories.Text, copyLogDirectory);
            if (listener != null)
                listener.NotifyObject(installationInfo);

            // Fecha a janela
            this.Close();
        }

        private void btnCancel_Click(Object sender, EventArgs e)
        {
            // Cancela (fecha a janela)
            this.Close();
        }
    }

}
