using System;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Collections.Specialized;
using AccountingLib.Spool;
using AccountingLib.Spool.EMF;
using AccountingLib.Printers;
using AccountingLib.PrintInspect;
using AccountingLib.ReportMailing;
using AccountingLib.ServerPrintLog;
using AccountingLib.DataAccessObjects;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.DataManipulation;


namespace UnitLibraryTestApp
{
    public partial class Form4 : Form, IListener
    {
        public Form4()
        {
            this.notifications = new List<Object>();
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

        private void Form4_Shown(object sender, EventArgs e)
        {
            StartDBAccess();
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            FinishDBAccess();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            lblTime.Text = currentTime.ToShortTimeString() + "   " + currentTime.Second.ToString() + "s";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = DeviceHandler.GetSpoolDirectory();
            openFileDialog.Filter = "Spool Shadow File (*.shd)|*.shd|Todos os arquivos (*.*)|*.*";
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.Cancel) return;
            
            SpooledJob spooledJob = new SpooledJob(openFileDialog.FileName, this);
            Dictionary<String, Object> jobSummary = PrintJobContext.GetJobSummary(spooledJob);
            infoBox.Clear();
            infoBox.Text += "Documento: " + jobSummary["documentName"] + Environment.NewLine;
            infoBox.Text += "Hora: " + jobSummary["submitted"] + Environment.NewLine;
            infoBox.Text += "UserName: " + jobSummary["userName"] + Environment.NewLine;
            infoBox.Text += "PrinterName: " + jobSummary["printerName"] + Environment.NewLine;
            infoBox.Text += "Número de cópias: " + jobSummary["copyCount"] + Environment.NewLine;
            infoBox.Text += "Número de páginas: " + jobSummary["pageCount"] + Environment.NewLine;


            /**************************************************************************************/
            /***********************            Extra job info              ***********************/
            /**************************************************************************************/
            infoBox.Text += Environment.NewLine;
            infoBox.Text += "Driver Name: " + spooledJob.ShadowFile.DriverName + Environment.NewLine;
            infoBox.Text += "Notify Name: " + spooledJob.ShadowFile.NotifyName + Environment.NewLine;
            infoBox.Text += "Print Processor: " + spooledJob.ShadowFile.PrintProcessor + Environment.NewLine;
            infoBox.Text += "Port: " + spooledJob.ShadowFile.Port + Environment.NewLine;
            infoBox.Text += "JobId: " + spooledJob.ShadowFile.JobId.ToString() + Environment.NewLine;
            infoBox.Text += "Data type: " + jobSummary["dataType"] + Environment.NewLine;
            infoBox.Text += "Spool fileSize: " + jobSummary["spoolFileSize"] + "(" + GetPrivateFieldValue(spooledJob.ShadowFile, "spoolFileSize") + " bytes)" + Environment.NewLine;

            /**************************************************************************************/
            /***********************             DevMode info               ***********************/
            /**************************************************************************************/
            infoBox.Text += Environment.NewLine;
            infoBox.Text += "Device Name: " + spooledJob.ShadowFile.DevMode.DeviceName + Environment.NewLine;
            infoBox.Text += "Form Name: " + spooledJob.ShadowFile.DevMode.FormName + Environment.NewLine;

            infoBox.Text += Environment.NewLine;
            infoBox.Text += "Número de páginas = " + spooledJob.SpoolFile.Pages.Count.ToString() + Environment.NewLine;

            // Encerra o método caso não seja um EMF
            if (!spooledJob.ShadowFile.DataType.ToUpper().Contains("EMF")) return;

            imgPageView.Image = ((EMFPage)spooledJob.SpoolFile.Pages[0]).Thumbnail;

            int index = 0;
            foreach (EMFPage page in spooledJob.SpoolFile.Pages)
            {
                index++;
                EMFReader.Save(page.PageImage, @"C:\page" + index.ToString() + ".emf");
            }
        }

        private object GetPrivateFieldValue(Object obj, String fieldName)
        {
            Type type = obj.GetType();
            BindingFlags privateBindings = BindingFlags.NonPublic | BindingFlags.Instance;
            FieldInfo[] fieldInfos = type.GetFields(privateBindings);
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                // Retorna o valor encontrado
                if (fieldInfo.Name.ToUpper() == fieldName.ToUpper())
                    return fieldInfo.GetValue(obj);
            }
            // Caso não tenha encontrado o retorno é nulo
            return null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Abre a conexão com o banco
            DataAccess dataAccess = DataAccess.Instance;
            dataAccess.MountConnection(FileResource.MapDesktopResource("DataAccess.xml"), DatabaseEnum.PrintAccounting);
            dataAccess.OpenConnection();

            // Busca os parâmetros de execução no banco
            ApplicationParamDAO applicationParamDAO = new ApplicationParamDAO(dataAccess.GetConnection());
            Dictionary<String, NameValueCollection> appParams = applicationParamDAO.GetParamsGroupByTask();
            double interval = Double.Parse(appParams["reportMailing"]["interval"]);

            NameValueCollection taskParams = new NameValueCollection();

            // Executa a tarefa
            ReportMailingTask task = new ReportMailingTask();
            task.InitializeTaskState(taskParams, dataAccess);
            task.Execute();

            // Fecha a conexão com o banco
            dataAccess.CloseConnection();

            // ReportMailingController controller = new ReportMailingController();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Busca os parâmetros de execução
            NameValueCollection taskParams = PrintLogContext.GetTaskParams();

            // Executa a tarefa
            PrintLogRoutingTask task = new PrintLogRoutingTask();
            task.InitializeTaskState(taskParams, null);
            task.Execute();

            // PrintLogRoutingController printLogRouter = new PrintLogRoutingController(this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EMFReader emfReader = new EMFReader(@"C:\sample.emf");
            emfReader.RenderEMFRecords();
        }

        public void NotifyObject(Object obj)
        {
            notifications.Add(obj);
        }
    }

}
