using System;
using System.IO;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.Reporting;


namespace AccountingLib.ServerPrintLog
{
    public class PrintLogPersistence
    {
        private int tenantId;

        private SqlConnection sqlConnection;

        private IListener listener;

        private FileLogger fileLogger;

        private Boolean createDigest;


        public PrintLogPersistence(int tenantId, SqlConnection sqlConnection, IListener listener, Boolean createDigest)
        {
            this.tenantId = tenantId;
            this.sqlConnection = sqlConnection;
            this.listener = listener;
            this.fileLogger = null;
            this.createDigest = createDigest;
        }

        /// <summary>
        /// Verifica se o arquivo já foi importado previamente
        /// </summary>
        public Boolean FileImported(DateRange dateRange)
        {
            Boolean imported = false;

            // Verifica se a faixa de datas foi fornecida
            if (dateRange == null) return false;

            // Verifica se existe alguma impressão no intervalo de datas
            PrintedDocumentDAO printedDocumentDAO = new PrintedDocumentDAO(sqlConnection);
            List<Object> printedDocuments = printedDocumentDAO.GetPrintedDocuments(tenantId, dateRange.GetFirstDay(), dateRange.GetLastDay(), null, null);
            if (printedDocuments.Count > 0)
            {
                NotifyListener("Já existiam registros inseridos anteriormente no intervalo de datas.");
                imported = true;
            }

            return imported;
        }

        /// <summary>
        /// Importa os registros do arquivo de log(.CSV) e insere no banco de dados
        /// </summary>
        public Boolean ImportFile(String fileName)
        {
            DateTime? fileDate = PrintLogFile.GetDate(fileName);

            // Informações de trace são enviadas ao listener através de NotifyListener()
            // O listener grava essas informações em log de arquivo
            CSVReader reader = new CSVReader(fileName, listener);
            NotifyListener("Fazendo a leitura do CSV.");
            DataTable printedDocumentTable = reader.Read();
            int rowCount = printedDocumentTable.Rows.Count;

            // Verifica se existem registros no CSV
            if (rowCount < 1)
            {
                NotifyListener("CSV inválido. Nenhum registro encontrado.");
                return false;
            }

            // Informa a quantidade de registros no CSV e uma amostra de seu conteúdo
            NotifyListener("Quantidade de registros no CSV - " + rowCount);
            String sampleData = printedDocumentTable.Rows[0]["Time"].ToString() + " - " +
                                printedDocumentTable.Rows[0]["Document Name"].ToString();
            NotifyListener("Amostra dos dados - " + sampleData);

            PrintedDocumentDAO printedDocumentDAO = new PrintedDocumentDAO(sqlConnection);
            CreateDigest(fileDate);

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

                printedDocumentDAO.InsertPrintedDocument(printedDocument);
                AddToDigest(printedDocument, row["Language"].ToString(), row["Size"].ToString());
            }

            return true;
        }

        private Boolean ConvertToBool(String flag)
        {
            Boolean result = true;

            if (flag.Contains("NOT"))
                result = false;

            return result;
        }

        // Cria o arquivo onde será guardado um resumo do csv original
        private void CreateDigest(DateTime? fileDate)
        {
            if (!createDigest) return;
            if (fileDate == null) return;

            String logName = "PrintLog-" + DateFormat.Adjust(fileDate) + ".csv";

            String applicationFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String logFolder = PathFormat.Adjust(applicationFolder) + @"PrintLogs\";
            Directory.CreateDirectory(logFolder);

            fileLogger = new FileLogger(logFolder + logName);
            fileLogger.FileHeader = new String[2];
            fileLogger.FileHeader[0] = "Print Inspector - Version 1.0.1";
            fileLogger.FileHeader[1] = "Time,User,Printer,Document Name,Pages,Copies,Duplex,Color,DataType,Size";
        }

        // Adiciona algumas informações da impressão ao resumo
        private void AddToDigest(PrintedDocument printedDocument, String language, String jobSize)
        {
            if (!createDigest) return;
            if (fileLogger == null) return;

            String jobTime = DateFormat.Adjust(printedDocument.jobTime, true);
            String dataType = language.ToUpper().Contains("EMF") ? "EMF" : "RAW";
            String newRow = jobTime + "," + printedDocument.userName + "," + printedDocument.printerName + "," +
                            printedDocument.name + "," + printedDocument.pageCount + "," + printedDocument.copyCount + "," +
                            printedDocument.duplex + "," + "false" + "," + "EMF" + "," + jobSize;

            fileLogger.LogRawData(newRow);
        }

        private void NotifyListener(Object obj)
        {
            if (listener != null)
                listener.NotifyObject(obj);
        }
    }

}
