using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using AccountingClientInstaller.Util;


namespace AccountingClientInstaller
{
    public partial class EnrollmentForm : Form
    {
        private RegistrationInfo registrationInfo;

        private IListener listener;


        public EnrollmentForm(String licenseKey, IListener listener)
        {
            InitializeComponent();
            if (!String.IsNullOrEmpty(licenseKey)) txtLicenseKey.Text = licenseKey;
            this.listener = listener;
        }

        private void ShowWarning(String warningMessage)
        {
            MessageBox.Show(warningMessage, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            DialogResult dialogResult = fileDialog.ShowDialog();
            if (dialogResult == DialogResult.Cancel) return;

            txtLicenseKey.Text = fileDialog.FileName;
        }

        private void chkDoNotRegister_CheckStateChanged(object sender, EventArgs e)
        {
            lblLicenseKey.Enabled = !chkDoNotRegister.Checked;
            txtLicenseKey.Enabled = !chkDoNotRegister.Checked;
            btnOpenFile.Enabled = !chkDoNotRegister.Checked;
        }

        private void btnSubmit_Click(Object sender, EventArgs e)
        {
            if (chkDoNotRegister.Checked)
            {
                // Envia notificação para o form principal
                if (listener != null)
                    listener.NotifyObject(new DoNotRegisterNotification());
                this.Close();
                return;
            }

            if (String.IsNullOrEmpty(txtLicenseKey.Text))
            {
                ShowWarning("É necessário fornecer a chave de ativação!");
                return;
            }

            if (!File.Exists(txtLicenseKey.Text))
            {
                ShowWarning("O arquivo " + txtLicenseKey.Text + " não existe.");
                return;
            }

            TextReader textReader = new StreamReader(txtLicenseKey.Text);
            String fileContent = textReader.ReadToEnd();
            registrationInfo = LicenseKeyMaker.ReadKey(fileContent, listener);
            if (registrationInfo == null)
            {
                ShowWarning("Licença inválida! Obtenha uma licença válida para o produto.");
                return;
            }
            
            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
            Version clientVersion = assemblyName.Version;
            Version serverVersion = new Version(registrationInfo.Version);
            if (clientVersion != serverVersion)
            {
                ShowWarning("Versão incompatível! Obtenha um executável atualizado para instalar o produto." +
                            Environment.NewLine + "Versão do Servidor: " + serverVersion.ToString());
                return;
            }

            if (DateTime.Now > registrationInfo.ExpirationDate)
            {
                ShowWarning("Chave de produto expirada! Obtenha outra chave.");
                return;
            }

            // Checa a licença junto ao servidor, para verificar se ela está disponível para uso
            RequestHandler requestHandler = new RequestHandler(registrationInfo.ServiceUrl, 16000, listener);
            Boolean requestSucceded = requestHandler.StartRequest("LicenseIsAvailable", registrationInfo.ConvertToLicense());
            if (!requestSucceded)
            {
                ShowWarning("Não foi possível verificar a licença junto ao servidor!");
                return;
            }

            Boolean isAvailable = false;
            try
            {
                isAvailable = (Boolean)requestHandler.ParseResponse(typeof(Boolean));
            }
            catch
            {
                ShowWarning("Não foi possível verificar a licença junto ao servidor!");
                ShowWarning("Resultado da requisição: " + Environment.NewLine + requestHandler.GetRawResponse());
                return;
            }

            if (!isAvailable)
            {
                ShowWarning("Esta licença já está em uso por outra estação de trabalho! Obtenha outra licença.");
                return;
            }

            if (listener != null)
                listener.NotifyObject(new ContentNotification(fileContent));

            // Repassa as informações para o form principal
            if (listener != null)
                listener.NotifyObject(registrationInfo);

            // Fecha a janela
            this.Close();
        }
    }

}
