using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Util;
using DataManipulation;


namespace BackupUtility
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            btnIniciar.Enabled = false;

            String server = txtServer.Text;
            DBLogin saLogin = new DBLogin(txtUsername.Text, txtPassword.Text);

            // Cria o diretório onde para onde os dados serão exportados
            String baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.ToString());
            String dataDirectory = PathFormat.Adjust(baseDir) + "Data";
            Directory.CreateDirectory(dataDirectory);

            // Executa a exportação dos databases
            Recovery recovery = new Recovery(new DBAccess(server, saLogin), dataDirectory);
            recovery.DBExport("AppCommon");
            recovery.DBExport("Accounting");

            btnIniciar.Enabled = true;
            MessageBox.Show("Backup dos dados concluído");
        }
    }

}
