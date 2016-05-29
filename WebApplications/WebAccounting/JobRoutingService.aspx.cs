using System;
using System.IO;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Reflection;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.DataManipulation;


namespace WebAccounting
{
    /// <summary>
    /// Web Service utilizado para o roteamento de jobs de impressão/cópia e registro do client que realizará este roteamento
    /// </summary>
    public partial class JobRoutingService: Page
    {
        private String action;           // indica o método a ser chamado

        private String aditionalData;    // indica os parâmetros a serem passados ao método

        private String serializedObject; // parâmetros após desserialização

        private DataAccess dataAccess;


        private void WriteResponse(String response)
        {
            this.Page.Response.Write("<response>" + response + "</response>");
        }

        protected void Page_Load(Object sender, EventArgs e)
        {
            // Verifica se o método e seus parâmetros foram informados
            if (String.IsNullOrEmpty(Request["action"]))
            {
                WriteResponse("Missing Argument.");
                return;
            }
            action = Request["action"];

            if (action != "DownloadCurrentVersion")
            {
                if (String.IsNullOrEmpty(Request["txtPostData"]))
                {
                    WriteResponse("Missing Argument.");
                    return;
                }
                aditionalData = Request["txtPostData"];
                serializedObject = HttpUtility.UrlDecode(Convert.FromBase64String(aditionalData), Encoding.UTF8);
            }


            // Loga no banco de dados os trabalhos de impressão enviados pelo client/workstation
            if (action == "LogPrintedDocuments")
            {
                Type objectType = typeof(List<PrintedDocument>);
                List<PrintedDocument> printedDocumentList;
                printedDocumentList = (List<PrintedDocument>)ObjectSerializer.DeserializeObject(serializedObject, objectType);

                int rowsAffected = LogPrintedDocuments(printedDocumentList);
                WriteResponse(rowsAffected + "rows affected.");
                return;
            }

            // Loga no banco de dados os trabalhos de cópia enviados pelo client/workstation
            if (action == "LogCopiedDocuments")
            {
                Type objectType = typeof(List<CopiedDocument>);
                List<CopiedDocument> copiedDocumentList;
                copiedDocumentList = (List<CopiedDocument>)ObjectSerializer.DeserializeObject(serializedObject, objectType);

                int rowsAffected = LogCopiedDocuments(copiedDocumentList);
                WriteResponse(rowsAffected + "rows affected.");
                return;
            }

            // Checa a situação da licença. O client verifica se a licença está disponível para uso e posteriormente se registra
            // para que possa fazer o roteamento de jobs de impressão/cópia
            if (action == "LicenseIsAvailable")
            {
                Type objectType = typeof(License);
                License license = (License)ObjectSerializer.DeserializeObject(serializedObject, objectType);

                Boolean isAvailable = LicenseIsAvailable(license);
                WriteResponse(isAvailable.ToString());
                return;
            }

            // Registra o client que realizará o roteamento de jobs de impressão/cópia
            if (action == "SetLicense")
            {
                Type objectType = typeof(License);
                License license = (License)ObjectSerializer.DeserializeObject(serializedObject, objectType);

                SetLicense(license);
                WriteResponse("License set.");
                return;
            }

            // Verifica a versão do instalador
            if (action == "GetVersionNumber")
            {
                String filePath = Path.Combine(Server.MapPath("Client"), "ClientSetup.exe");
                Assembly exeAssembly = Assembly.LoadFile(filePath);
                AssemblyName info = exeAssembly.GetName();
                String exeName = info.Name;
                String exeVersion = info.Version.ToString();

                WriteResponse(exeVersion);
                return;
            }

            // Realiza o download da versão corrente
            if (action == "DownloadCurrentVersion")
            {
                String filePath = Path.Combine(Server.MapPath("Client"), "ClientSetup.exe");

                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=ClientSetup.exe");
                Response.WriteFile(filePath);
                Response.Flush();
                Response.Close();
                return;
            }

            // Registra no banco de dados os dispositivos SNMP com contador e número de série (atualiza periodicamente)
            if (action == "RegisterDevices")
            {
                Type objectType = typeof(List<PrintingDevice>);
                List<PrintingDevice> deviceList = (List<PrintingDevice>)ObjectSerializer.DeserializeObject(serializedObject, objectType);

                int rowsAffected = RegisterDevices(deviceList);
                WriteResponse(rowsAffected + "rows affected.");
                return;
            }

            WriteResponse("Method not implemented.");
        }

        private void StartDBAccess()
        {
            // Abre a conexão com o banco
            dataAccess = DataAccess.Instance;
            dataAccess.MountConnection(FileResource.MapWebResource(Server, "DataAccess.xml"), DatabaseEnum.PrintAccounting);
            dataAccess.OpenConnection();
        }

        private void FinishDBAccess()
        {
            // Fecha a conexão com o banco
            dataAccess.CloseConnection();
            dataAccess = null;
        }

        private Object currentJob;

        private Boolean CheckPrintedDocument(Object obj)
        {
            PrintedDocument current = (PrintedDocument)currentJob;
            PrintedDocument match = (PrintedDocument)obj;

            if (current.jobTime != match.jobTime)
                return false;

            String currentName = current.name;
            // Limita o tamanho do nome em 100 caracteres para fazer a comparação
            // o nome vindo do BD já esta truncado pois é um varchar(100)
            if (currentName.Length > 100) currentName = current.name.Substring(0, 100);

            if (currentName != match.name)
                return false;

            return true;
        }

        private Boolean CheckCopiedDocument(Object obj)
        {
            CopiedDocument current = (CopiedDocument)currentJob;
            CopiedDocument match = (CopiedDocument)obj;

            if (current.jobTime != match.jobTime)
                return false;

            if (current.userName.ToUpper() != match.userName.ToUpper())
                return false;

            if (current.printerName.ToUpper() != match.printerName.ToUpper())
                return false;

            return true;
        }

        public int LogPrintedDocuments(List<PrintedDocument> printedDocuments)
        {
            if (printedDocuments == null) return 0;
            if (printedDocuments.Count == 0) return 0;
            StartDBAccess();

            int tenantId = printedDocuments[0].tenantId;
            DateTime startDate = DateTime.Now.Date;
            DateTime endDate = startDate.Add(new TimeSpan(23, 59, 59));

            JobDataDependency jobDataDependency = new JobDataDependency(printedDocuments, dataAccess.GetConnection(), tenantId);
            jobDataDependency.CreateDataDependency();

            PrintedDocumentDAO printedDocumentDAO = new PrintedDocumentDAO(dataAccess.GetConnection());
            List<Object> alreadyInserted = printedDocumentDAO.GetPrintedDocuments(tenantId, startDate, endDate, null, null);

            int rowCount = 0;
            foreach (PrintedDocument printedDocument in printedDocuments)
            {
                currentJob = printedDocument;
                if (alreadyInserted.Find(CheckPrintedDocument) == null) // Verifica se o registro já existe
                {
                    // Caso não exista insere no banco
                    printedDocumentDAO.InsertPrintedDocument(printedDocument);
                    rowCount++;
                }
            }

            FinishDBAccess();
            return rowCount;
        }

        public int LogCopiedDocuments(List<CopiedDocument> copiedDocuments)
        {
            if (copiedDocuments == null) return 0;
            if (copiedDocuments.Count == 0) return 0;
            StartDBAccess();

            int tenantId = copiedDocuments[0].tenantId;
            DateTime startDate = DateTime.Now.Date;
            DateTime endDate = startDate.Add(new TimeSpan(23, 59, 59));

            JobDataDependency jobDataDependency = new JobDataDependency(copiedDocuments, dataAccess.GetConnection(), tenantId);
            jobDataDependency.CreateDataDependency();

            CopiedDocumentDAO copiedDocumentDAO = new CopiedDocumentDAO(dataAccess.GetConnection());
            List<Object> alreadyInserted = copiedDocumentDAO.GetCopiedDocuments(tenantId, startDate, endDate, null, null);

            int rowCount = 0;
            foreach (CopiedDocument copiedDocument in copiedDocuments)
            {
                currentJob = copiedDocument;
                if (alreadyInserted.Find(CheckCopiedDocument) == null) // Verifica se o registro já existe
                {
                    // Caso não exista insere no banco
                    copiedDocumentDAO.InsertCopiedDocument(copiedDocument);
                    rowCount++;
                }
            }

            FinishDBAccess();
            return rowCount;
        }

        public Boolean LicenseIsAvailable(License license)
        {
            StartDBAccess();
            LicenseDAO licenseDAO = new LicenseDAO(dataAccess.GetConnection());
            License storedLicense = licenseDAO.GetLicense(license.tenantId, license.id);
            FinishDBAccess();

            // A licença não existe
            if (storedLicense == null)
                return false;

            // A licença está disponível
            if (String.IsNullOrEmpty(storedLicense.installationKey))
                return true;

            // Verifica se o produto esta sendo reinstalado, neste caso "installationKey" permanecerá o mesmo,
            // a licença é reaproveitada
            if (license.installationKey.ToUpper() == storedLicense.installationKey.ToUpper())
                return true;

            // Caso não se enquadre em nenhum dos anteriores a licença está em uso por outra workstation
            return false;
        }

        public void SetLicense(License license)
        {
            StartDBAccess();

            LicenseDAO licenseDAO = new LicenseDAO(dataAccess.GetConnection());
            licenseDAO.SetLicense(license);

            FinishDBAccess();
        }

        public int RegisterDevices(List<PrintingDevice> deviceList)
        {
            if (deviceList == null) return 0;
            if (deviceList.Count == 0) return 0;
            StartDBAccess();

            int rowCount = 0;
            PrintingDeviceDAO printingDeviceDAO = new PrintingDeviceDAO(dataAccess.GetConnection());
            foreach (PrintingDevice printingDevice in deviceList)
            {
                printingDeviceDAO.SetPrintingDevice(printingDevice);
                rowCount++;
            }

            FinishDBAccess();
            return rowCount;
        }
    }

}
