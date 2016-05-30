using System;
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using AccountingLib.ServerPrintLog;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.DataManipulation;


namespace UnitLibraryTestApp
{
    public partial class Form1 : Form, IListener
    {
        public Form1()
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

            // Inicia o componente de acesso ao banco
            applicationParamDAO = new ApplicationParamDAO(dataAccess.GetConnection());
        }

        private void FinishDBAccess()
        {
            // Fecha a conexão com o banco
            dataAccess.CloseConnection();
            dataAccess = null;

            // Finaliza o componente de acesso ao banco
            applicationParamDAO = null;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            // StartDBAccess();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // FinishDBAccess();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Assembly exeAssembly = Assembly.GetEntryAssembly();
            AssemblyName info = exeAssembly.GetName();
            String exeName = info.Name;
            String exeVersion = info.Version.ToString();

            /************************************************************************************/

            Byte[] serializedObject = ObjectSerializer.SerializeObjectToArray(exeName + " " + exeVersion);
            String encodedData = HttpUtility.UrlEncode(Convert.ToBase64String(serializedObject));
            Byte[] postData = Encoding.UTF8.GetBytes("txtPostData=" + encodedData);

            String serviceUrl = "http://localhost:2086/JobRoutingService.aspx";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceUrl + "?" + "action=GetVersionNumber");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postData, 0, postData.Length);
            requestStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            MessageBox.Show(response.StatusCode + Environment.NewLine);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Registra os dispositivos SNMP com número de série e contador
            PrintingDevice printingDevice1 = new PrintingDevice();
            printingDevice1.tenantId = 5;
            printingDevice1.ipAddress = "192.168.0.101";
            printingDevice1.description = "Brother MFC-8890DW";
            printingDevice1.serialNumber = "E0J405989";
            printingDevice1.counter = 11111;
            printingDevice1.lastUpdated = DateTime.Now;

            PrintingDevice printingDevice2 = new PrintingDevice();
            printingDevice2.tenantId = 5;
            printingDevice2.ipAddress = "192.168.0.136";
            printingDevice2.description = "Lexmark X466de 35P620G LR.BS.P649";
            printingDevice2.serialNumber = "35P620G";
            printingDevice2.counter = 22222;
            printingDevice2.lastUpdated = DateTime.Now;

            List<PrintingDevice> deviceList = new List<PrintingDevice>();
            deviceList.Add(printingDevice1);
            deviceList.Add(printingDevice2);

            /************************************************************************************/

            Byte[] serializedObject = ObjectSerializer.SerializeObjectToArray(deviceList);
            String encodedData = HttpUtility.UrlEncode(Convert.ToBase64String(serializedObject));
            Byte[] postData = Encoding.UTF8.GetBytes("txtPostData=" + encodedData);

            String serviceUrl = "http://localhost:2086/JobRoutingService.aspx";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceUrl + "?" + "action=RegisterDevices");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postData, 0, postData.Length);
            requestStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            MessageBox.Show(response.StatusCode + Environment.NewLine);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String filename = PrintLogFile.MountName(@"C:\PaperCut Print Logger\logs\csv\daily", 10, 04, 2013);
            CSVReader csvReader = new CSVReader(filename, null);
            dataGridView1.DataSource = csvReader.Read();

            /*
            dataGridView1.Columns.Add("hora", "Hora");
            dataGridView1.Columns.Add("usuario", "Usuário");
            dataGridView1.Columns.Add("impressora", "Impressora");
            dataGridView1.Columns.Add("quantidade_paginas", "Quant. Páginas");

            CopiedDocumentDAO copiedDocumentDAO = new CopiedDocumentDAO(dataAccess.GetConnection());
            List<Object> copiedDocuments = copiedDocumentDAO.GetCopiedDocuments(10, new DateTime(2010, 11, 17), new DateTime(2010, 11, 20), null, null);
            foreach (CopiedDocument copy in copiedDocuments)
            {
                Object[] values = new Object[] { copy.jobTime, copy.userName, copy.printerName, copy.pageCount };
                dataGridView1.Rows.Add(values);
            }
            */
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String serviceUrl = "http://www.datacount.com.br/Datacount/JobRoutingService.aspx";
            int tenantId = 10;

            JobRouter jobRouter = new JobRouter(serviceUrl, tenantId, this);

            // Envia os logs de cópia
            //notifications.Clear();
            String baseDir = @"C:\temp\Datacount";
            String copyLogDir = Path.Combine(baseDir, "CopyLogs");
            if (!jobRouter.SendCopyJobs(copyLogDir))
            {
                // São gravados logs detalhados para que se possa determinar a causa da falha
                //ProcessNotifications();
                return;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            String filename;
            Boolean fileExists;
            for (int day = 1; day <= 31; day++)
            {
                filename = PrintLogFile.MountName(@"C:\Work\PrintLogs", day, 12, 2010);
                fileExists = File.Exists(filename);
                if (fileExists)
                {
                    MessageBox.Show("Importando arquivo: " + filename);
                    PrintLogPersistence persistence = new PrintLogPersistence(4, dataAccess.GetConnection(), this, false);
                    persistence.ImportFile(filename);
                }
            }
        }

        public void NotifyObject(Object obj)
        {
            MessageBox.Show((String)obj);
        }
    }

}

//  http://www.infoq.com/presentations/10-Ways-to-Better-Code-Neal-Ford
//  http://msdn.microsoft.com/en-us/library/cc168615.aspx
//  https://code.visualstudio.com/docs/editor/tasks

