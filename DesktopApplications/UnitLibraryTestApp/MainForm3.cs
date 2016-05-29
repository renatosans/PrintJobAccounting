using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using AccountingLib.Entities;
using AccountingLib.Management;
using DocMageFramework.DataManipulation;


namespace UnitLibraryTestApp
{
    public partial class MainForm3 : Form
    {
        public MainForm3()
        {
            InitializeComponent();
        }

        private void btnGetTenants_Click(object sender, EventArgs e)
        {
            if (!OpenConnection()) return;

            ProcedureCall retrieveTenants = new ProcedureCall("pr_retrieveTenant", sqlConnection);
            retrieveTenants.Execute(true);
            DataTable tenantTable = retrieveTenants.ExtractFromResultset(typeof(Tenant), "tb_tenant");
            dataGridView1.DataSource = tenantTable.DefaultView;
            
            CloseConnection();
        }

        private SqlConnection sqlConnection;

        private Boolean OpenConnection()
        {
            try
            {
                sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = @"Data Source=" + txtDBServer.Text + "; Initial Catalog=Accounting; User=" + txtDBUser.Text + "; Password=" + txtDBPass.Text;
                sqlConnection.Open();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return false;
            }

            // Se não houve nenhuma falha retorna status de sucesso
            return true;
        }

        private void CloseConnection()
        {
            sqlConnection.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            int randomId = 5; // utiliza um id qualquer
            DateTime selectedDate = expirationDatePicker.Value;
            DateTime expirationDate = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 0, 0, 0);
            infoBox.Text = LicenseKeyMaker.GenerateKey(txtServiceUrl.Text, int.Parse(txtTenantId.Text), randomId, expirationDate);
        }
    }

}
