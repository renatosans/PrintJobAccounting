using System;
using System.Windows.Forms;
using AccountingInstaller.Util;
using AccountingInstaller.DataManipulation;


namespace AccountingInstaller
{
    public partial class CreateLoginForm : Form
    {
        private IListener listener;

        public CreateLoginForm(IListener listener)
        {
            InitializeComponent();
            this.listener = listener;
        }

        private void ShowWarning(String warningMessage)
        {
            MessageBox.Show(warningMessage, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUsername.Text))
            {
                ShowWarning("É necessário definir o username!");
                return;
            }

            if (String.IsNullOrEmpty(txtPassword.Text))
            {
                ShowWarning("É necessário definir o password!");
                return;
            }

            DBLogin newDBLogin = new DBLogin(txtUsername.Text, txtPassword.Text);
            if (listener != null)
                listener.NotifyObject(newDBLogin);

            this.Close();
        }
    }

}
