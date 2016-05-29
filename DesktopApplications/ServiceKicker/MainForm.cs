using System;
using System.Windows.Forms;
using System.ServiceProcess;
using DocMageFramework.JobExecution;


namespace ServiceKicker
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ServiceController[] services = ServiceController.GetServices();
            foreach(ServiceController service in services)
            {
                cmbServiceName.Items.Add(service.ServiceName);
            }
        }

        private Boolean ServiceSelected()
        {
            if (cmbServiceName.SelectedItem == null)
            {
                MessageBox.Show("Escolha um serviço.");
                return false;
            }

            return true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!ServiceSelected()) return;
            ServiceHandler.StopService((String)cmbServiceName.SelectedItem, 33000);
            MessageBox.Show("Serviço parado com exito.");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!ServiceSelected()) return;
            ServiceHandler.StartService((String)cmbServiceName.SelectedItem, 33000);
            MessageBox.Show("Serviço iniciado com exito.");
        }
    }

}
