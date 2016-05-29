using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using AccountingLib.Printers;
using AccountingLib.Security;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using AccountingLib.ReportMailing;
using AccountingLib.ServerPrintLog;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.DataManipulation;


namespace UnitLibraryTestApp
{
    public partial class Form5 : Form, IListener
    {
        public Form5()
        {
            InitializeComponent();
        }

        private List<Object> notifications;

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

        private void Form5_Shown(object sender, EventArgs e)
        {
            StartDBAccess();

            Object[] listaUsuarios = ComboBoxScaffold.Retrieve("pr_retrieveUser", dataAccess.GetConnection(), typeof(User));
            ComboUsuario.Items.AddRange(listaUsuarios);

            Object[] listaImpressoras = ComboBoxScaffold.Retrieve("pr_retrievePrinter", dataAccess.GetConnection(), typeof(Printer));
            ComboImpressora.Items.AddRange(listaImpressoras);
        }

        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            FinishDBAccess();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User user = (User)ComboUsuario.SelectedItem;
            Printer printer = (Printer)ComboImpressora.SelectedItem;

            if ((user == null) || (printer == null))
            {
                MessageBox.Show("Selecione usuário e impressora.");
                return;
            }

            MessageBox.Show("ID Usuário= " + user.id.ToString() + "\r\n" + "ID Impressora= " + printer.id.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime startDate = new DateTime(2011, 05, 01);
            DateTime endDate = new DateTime(2011, 05, 31);
            FileInfo reportFile = new FileInfo(@"C:\quickTest.pdf");
            PrintedDocumentsReport report = new PrintedDocumentsReport(1, startDate, endDate, null, null);
            report.InitializeComponents(reportFile, new PdfReportBuilder(), dataAccess.GetConnection());
            report.BuildReport();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // not implemented yet
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // not implemented yet
        }

        public void NotifyObject(Object obj)
        {
            if (obj is Exception)
            {
                Exception exc = (Exception)obj;
                MessageBox.Show(exc.Message);
                return;
            }

            notifications.Add(obj);
        }
    }

}
