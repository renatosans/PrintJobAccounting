using System;
using System.IO;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;


namespace AccountingLib.ServerPrintLog
{
    public class PrintLogDigest
    {
        private FileLogger fileLogger;

        private List<Object> alreadyInserted;


        // Cria o arquivo onde será guardado um resumo do csv original
        public void Create(DateTime? fileDate)
        {
            if (fileDate == null) return;

            String logName = "PrintLog-" + DateFormat.Adjust(fileDate) + ".csv";

            String applicationFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String logFolder = PathFormat.Adjust(applicationFolder) + @"PrintLogs\";
            Directory.CreateDirectory(logFolder);

            fileLogger = new FileLogger(logFolder + logName);
            fileLogger.FileHeader = new String[2];
            fileLogger.FileHeader[0] = "Digest File - Source: Papercut Print Logger";
            fileLogger.FileHeader[1] = "Time,User,Printer,Document Name,Pages,Copies,Duplex,Color,DataType,Size";

            CSVReader csvReader = new CSVReader(logFolder + logName, null);
            DataTable alreadyInsertedTable = csvReader.Read();
            alreadyInserted = new List<Object>();
            foreach (DataRow row in alreadyInsertedTable.Rows)
            {
                PrintedDocument printedDocument = new PrintedDocument();
                printedDocument.jobTime = DateTime.Parse(row["Time"].ToString());
                printedDocument.name = row["Document Name"].ToString();

                alreadyInserted.Add(printedDocument);
            }
        }

        // Adiciona algumas informações da impressão ao resumo
        public void AddToDigest(PrintedDocument printedDocument, String language, String jobSize)
        {
            if (fileLogger == null) return;
            
            currentJob = printedDocument;
            if (alreadyInserted.Find(CheckPrintedDocument) == null) // Verifica se o registro já existe
            {
                // Caso não exista insere no resumo
                String jobTime = DateFormat.Adjust(printedDocument.jobTime, true);
                String dataType = language.ToUpper().Contains("EMF") ? "EMF" : "RAW";
                String newRow = jobTime + "," + printedDocument.userName + "," + printedDocument.printerName + "," +
                                printedDocument.name + "," + printedDocument.pageCount + "," + printedDocument.copyCount + "," +
                                printedDocument.duplex + "," + "false" + "," + dataType + "," + jobSize;

                fileLogger.LogRawData(newRow);
            }
        }

        private Object currentJob;

        private Boolean CheckPrintedDocument(Object obj)
        {
            PrintedDocument current = (PrintedDocument)currentJob;
            PrintedDocument match = (PrintedDocument)obj;

            if (current.jobTime != match.jobTime)
                return false;

            if (current.name != match.name)
                return false;

            return true;
        }
    }

}
