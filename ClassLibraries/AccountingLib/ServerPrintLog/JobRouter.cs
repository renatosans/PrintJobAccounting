using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.ServerCopyLog;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;


namespace AccountingLib.ServerPrintLog
{
    public class JobRouter
    {
        private String serviceUrl;

        private int tenantId;

        private IListener listener;


        public JobRouter(String serviceUrl, int tenantId, IListener listener)
        {
            this.serviceUrl = serviceUrl;
            this.tenantId = tenantId;
            this.listener = listener;
        }

        private Boolean ProcessCopyLogFile(String fileName, CopyLogDevice sourceDevice, CopyLogSender copyLogSender)
        {
            if (sourceDevice == null)
            {
                NotifyListener("O arquivo de log precisa estar associado a um dispositivo.");
                return false;
            }

            // Informações de trace são enviadas ao listener através de NotifyListener()
            // O listener grava essas informações em log de arquivo
            CSVReader reader = new CSVReader(fileName, listener);
            NotifyListener("Fazendo a leitura do CSV.");
            DataTable fullTable = reader.Read(0);
            int rowCount = fullTable.Rows.Count;

            // Verifica se existem registros no CSV
            if (rowCount < 1)
            {
                NotifyListener("CSV inválido. Nenhum registro encontrado.");
                return false;
            }
            // Informa a quantidade de registros no CSV
            NotifyListener("Quantidade de registros no CSV - " + rowCount);

            CopyLogDeviceHandler deviceHandler = new CopyLogDeviceHandler(sourceDevice);
            return deviceHandler.ProcessCopyLog(fullTable, copyLogSender, tenantId);
        }

        private Boolean ProcessPrintLogFile(String fileName, PrintLogSender printLogSender)
        {
            // Informações de trace são enviadas ao listener através de NotifyListener()
            // O listener grava essas informações em log de arquivo
            CSVReader reader = new CSVReader(fileName, listener);
            NotifyListener("Fazendo a leitura do CSV.");
            DataTable fullTable = reader.Read();
            int rowCount = fullTable.Rows.Count;

            // Verifica se existem registros no CSV
            if (rowCount < 1)
            {
                NotifyListener("CSV inválido. Nenhum registro encontrado.");
                return false;
            }

            // Informa a quantidade de registros no CSV e uma amostra de seu conteúdo
            NotifyListener("Quantidade de registros no CSV - " + rowCount);
            String sampleData = fullTable.Rows[0]["Time"].ToString() + " - " +
                                fullTable.Rows[0]["Document Name"].ToString();
            NotifyListener("Amostra dos dados - " + sampleData);

            // Gera uma view do log com uma faixa de horário
            String startHour = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            String endHour = DateTime.Now.AddHours(+1).ToString("yyyy-MM-dd HH:mm:ss");
            String rowFilter = "Time > '" + startHour + "' AND Time < '" + endHour + "'";
            DataView view = new DataView(fullTable, rowFilter, null, DataViewRowState.Added);
            DataTable printedDocumentTable = view.ToTable();

            // Cria arquivo que armazenará resumo dos trabalhos de impressão
            DateTime? fileDate = PrintLogFile.GetDate(fileName);
            PrintLogDigest digest = new PrintLogDigest();
            digest.Create(fileDate);

            PrintedDocument printedDocument;
            foreach (DataRow row in printedDocumentTable.Rows)
            {
                printedDocument = new PrintedDocument();
                printedDocument.tenantId = tenantId;
                printedDocument.jobTime = DateTime.Parse(row["Time"].ToString());
                printedDocument.userName = row["User"].ToString();
                printedDocument.printerName = row["Printer"].ToString();
                printedDocument.name = row["Document Name"].ToString();
                printedDocument.pageCount = int.Parse(row["Pages"].ToString());
                printedDocument.copyCount = int.Parse(row["Copies"].ToString());
                printedDocument.duplex = ConvertToBool(row["Duplex"].ToString());
                printedDocument.color = !ConvertToBool(row["Grayscale"].ToString());

                printLogSender.AddPrintedDocument(printedDocument);
                digest.AddToDigest(printedDocument, row["Language"].ToString(), row["Size"].ToString());
            }

            return true;
        }

        private List<CopyLogDevice> GetDevices(String devicesXml)
        {
            /*
            
            <?xml version="1.0" encoding="utf-8" ?>
            <!--  Tipos de log:   1-Brother MFC 8890DW  2-Ricoh SP C232DN -->
            <devices>
              <device>
                <printername>Printer 0001</printername>
                <logfile>printer0001.csv</logfile>
                <logtype>1</logtype>
              </device>
              <device>
                <printername>Printer 0002</printername>
                <logfile>printer0002.csv</logfile>
                <logtype>2</logtype>
              </device>
            </devices>
            
            */
            List<CopyLogDevice> devices = new List<CopyLogDevice>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(devicesXml);
            XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;
            foreach (XmlNode node in xmlNodeList)
            {
                String printerName = node["printername"].InnerText;
                String logFile = node["logfile"].InnerText;
                String logType = node["logtype"].InnerText;
                devices.Add(new CopyLogDevice(printerName, logFile, logType));
            }

            return devices;
        }

        /// <summary>
        /// Procura a origem do arquivo de log (equipamento que gerou o arquivo) nos dispositivos cadastrados
        /// </summary>
        private CopyLogDevice SearchLogFileSource(String logFile, List<CopyLogDevice> devices)
        {
            foreach(CopyLogDevice device in devices)
            {
                if (device.logFile.ToUpper() == logFile.ToUpper())
                    return device;
            }

            // retorna "null" caso não tenha encontrado o dispositivo
            return null;
        }

        public Boolean SendCopyJobs(String logDirectory)
        {
            Boolean success = true;
            CopyLogSender copyLogSender = new CopyLogSender(serviceUrl, listener);

            DirectoryInfo logDir = new DirectoryInfo(logDirectory);
            if (!logDir.Exists)
            {
                NotifyListener("Falha ao enviar logs. O diretório " + logDirectory + " não existe.");
                return false;
            }

            String devicesXml = PathFormat.Adjust(logDirectory) + "Devices.xml";
            if (!File.Exists(devicesXml))
            {
                NotifyListener("Falha ao enviar logs. O arquivo " + devicesXml + " não foi encontrado.");
                return false;
            }
            List<CopyLogDevice> devices = GetDevices(devicesXml);

            // Procura os arquivos .CSV no diretório de logs, faz a importação e armazena um backup
            // do arquivo com a extensão .OLD
            NotifyListener("Fazendo varredura de arquivos .CSV no diretório de logs: " + logDirectory);
            FileInfo[] files = logDir.GetFiles();
            int filesParsed = 0;
            foreach (FileInfo file in files)
            {
                if (Path.GetExtension(file.Name).ToUpper() == ".CSV")
                {
                    NotifyListener("Processando arquivo: " + file.Name);
                    String filePath = PathFormat.Adjust(logDirectory);
                    String fileName = Path.GetFileNameWithoutExtension(file.Name);

                    // Procura o dispositivo que gerou o log (quando não encontra retorna null)
                    CopyLogDevice sourceDevice = SearchLogFileSource(file.Name, devices);

                    // Processa o arquivo de log, possui tratamento caso "sourceDevice" seja null
                    if (!ProcessCopyLogFile(filePath + fileName + ".csv", sourceDevice, copyLogSender))
                        success = false;
                    
                    // Armazena um backup do arquivo com a extensão .OLD caso o arquivo tenha mais de um dia
                    DateTime lastWrite = CopyLogFile.GetTimeStamp(filePath + fileName + ".csv");
                    if (lastWrite < DateTime.Now.AddDays(-1))
                        CopyLogFile.StoreOldFile(filePath, fileName, ".csv");

                    // Se processou o arquivo com sucesso incrementa a contagem
                    if (success) filesParsed++;
                }
            }
            if (filesParsed == 0) NotifyListener("Nenhum arquivo processado( Não existem .CSVs no diretório ou houve falha no processamento).");

            if (!copyLogSender.FinishSending())
                success = false; // Falha ao enviar algum pacote de logs

            return success;
        }

        public Boolean SendPrintJobs(String logDirectories)
        {
            Boolean success = true;
            DateTime today = DateTime.Now.Date;
            PrintLogSender printLogSender = new PrintLogSender(serviceUrl, listener);

            String[] directoryArray = logDirectories.Split(new Char[] { ';' });
            foreach (String directory in directoryArray)
            {
                DirectoryInfo logDir = new DirectoryInfo(directory);
                String fileName = null;
                if (logDir.Exists)
                {
                    fileName = PrintLogFile.MountName(directory, today.Day, today.Month, today.Year);
                    FileInfo logFile = new FileInfo(fileName);
                    if (!logFile.Exists) fileName = null;
                }

                // Se o arquivo existe, processa seu conteúdo
                if (!String.IsNullOrEmpty(fileName))
                {
                    // Informa dados do arquivo
                    NotifyListener("Arquivo - fileName = " + fileName);

                    if (!ProcessPrintLogFile(fileName, printLogSender))
                        success = false; // Falha ao processar o arquivo .CSV
                }
            }

            if (!printLogSender.FinishSending())
                success = false; // Falha ao enviar algum pacote de logs

            return success;
        }

        private Boolean ConvertToBool(String flag)
        {
            Boolean result = true;

            if (flag.Contains("NOT"))
                result = false;

            return result;
        }

        private void NotifyListener(Object obj)
        {
            if (listener != null) listener.NotifyObject(obj);
        }
    }

}
