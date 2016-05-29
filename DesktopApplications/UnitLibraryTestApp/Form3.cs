using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.Specialized;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.FileUtils;
using DocMageFramework.DataManipulation;


namespace UnitLibraryTestApp
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private DataAccess dataAccess;

        private ApplicationParamDAO applicationParamDAO;


        private void StartDBAccess()
        {
            // Abre a conexão com o banco
            dataAccess = DataAccess.Instance;
            dataAccess.MountConnection(FileResource.MapDesktopResource("DataAccess.xml"), DatabaseEnum.PrintAccounting);
            dataAccess.OpenConnection();

            // Inicia o objeto de acesso ao banco
            applicationParamDAO = new ApplicationParamDAO(dataAccess.GetConnection());
        }

        private void FinishDBAccess()
        {
            // Fecha a conexão com o banco
            dataAccess.CloseConnection();
            dataAccess = null;

            // Finaliza o objeto de acesso ao banco
            applicationParamDAO = null;
        }

        private void Form3_Shown(object sender, EventArgs e)
        {
            StartDBAccess();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            FinishDBAccess();
        }

        private void btnGetParam_Click(object sender, EventArgs e)
        {
            infoBox.Text = "";
            ApplicationParam lastAccess = applicationParamDAO.GetParam("lastAccess", "copyLogImport");
            infoBox.Text = infoBox.Text + lastAccess.name + "    "  + lastAccess.value + Environment.NewLine;
            infoBox.Select(0, 1);
        }

        private void btnGetAllParams_Click(object sender, EventArgs e)
        {
            infoBox.Text = "";
            List<Object> accountingParams = applicationParamDAO.GetAllParams();
            foreach (ApplicationParam applicationParam in accountingParams)
            {                
                infoBox.Text = infoBox.Text + applicationParam.ownerTask + "." + applicationParam.name +
                               "    " + applicationParam.value + Environment.NewLine;
            }
            infoBox.Select(0, 1);
        }

        private void btnGetTaskParams_Click(object sender, EventArgs e)
        {
            infoBox.Text = "";
            NameValueCollection taskParams = applicationParamDAO.GetTaskParams("copyLogImport");
            foreach (String param in taskParams)
            {
                infoBox.Text = infoBox.Text + param + "    " + taskParams[param] + Environment.NewLine;
            }
            infoBox.Select(0, 1);
        }

        private void btnUpdateParam_Click(object sender, EventArgs e)
        {
            ApplicationParam lastAccessParam = applicationParamDAO.GetParam("lastAccess", "copyLogImport");
            lastAccessParam.value = "13/03/2010";
            applicationParamDAO.SetParam(lastAccessParam);
        }
    }

}
