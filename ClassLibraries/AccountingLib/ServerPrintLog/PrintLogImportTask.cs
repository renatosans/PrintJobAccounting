using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.Reporting;
using DocMageFramework.JobExecution;
using DocMageFramework.DataManipulation;


namespace AccountingLib.ServerPrintLog
{
    public class PrintLogImportTask : IPeriodicTask, IListener
    {
        private NameValueCollection taskParams;

        private DataAccess dataAccess;

        private List<Object> notifications;

        private FileLogger fileLogger;

        private Boolean firstExecution = true;


        public void InitializeTaskState(NameValueCollection taskParams, DataAccess dataAccess)
        {
            this.taskParams = taskParams;
            this.dataAccess = dataAccess;

            notifications = new List<Object>();

            fileLogger = new FileLogger(FileResource.MapDesktopResource("PrintLogImport.log"));
            fileLogger.FileHeader = new String[3];
            fileLogger.FileHeader[0] = "╔".PadRight(80, '═') + "╗";
            fileLogger.FileHeader[1] = "║".PadRight(23, ' ') + " Log de aplicativo (Log Importer)  " + "║".PadLeft(23);
            fileLogger.FileHeader[2] = "╚".PadRight(80, '═') + "╝";
            fileLogger.LogRawData(Environment.NewLine);
        }

        /// <summary>
        /// Importa o arquivo de log (insere os registros no banco de dados)
        /// </summary>
        private Boolean ImportFile(String fileName, Boolean createDigest)
        {
            Boolean result = false;
            try
            {
                // Caso os registros do arquivo não estejam no banco realiza a importação
                PrintLogPersistence logPersistence = new PrintLogPersistence(1, dataAccess.GetConnection(), this, createDigest);
                DateRange dateRange = new DateRange(true);
                DateTime? fileDate = PrintLogFile.GetDate(fileName);
                if (fileDate != null)
                {
                    // Verifica se existe alguma impressão na mesma data, considera o intervalo
                    // de um dia ( 23 horas e 59 minutos )
                    TimeSpan timeSpan = new TimeSpan(23, 59, 00);
                    dateRange.SetRange(fileDate.Value.Date, fileDate.Value.Date.Add(timeSpan));
                }
                if (!logPersistence.FileImported(dateRange))
                    result = logPersistence.ImportFile(fileName);
            }
            catch (Exception exc)
            {
                fileLogger.LogInfo("Exceção encontrada.");
                fileLogger.LogError(exc.Message);
                return false;
            }

            return result;
        }

        /// <summary>
        /// Importa a massa de dados existente no diretório de logs
        /// </summary>
        private void ImportPreviousLogs(String logDirectory)
        {
            PrintLogSorter logSorter = new PrintLogSorter(logDirectory);
            String[] logFiles = logSorter.GetOrderedFiles();
            DateTime? lastFileDate = null;

            // Importa os arquivos encontrados no diretório de log
            fileLogger.LogInfo("Importando logs pré-existentes no diretório " + logDirectory);
            foreach (String file in logFiles)
            {
                DateTime? fileDate = PrintLogFile.GetDate(file); // considera hora como sendo 00:00:00
                DateTime? today = DateTime.Now.Date; // considera hora como sendo 00:00:00

                // Importa todos com exceção do arquivo com as impressões de hoje, faz isso no final do dia
                if ((fileDate != null) && (fileDate != today))
                {
                    String fileName = PathFormat.Adjust(logDirectory) + file;
                    // Loga informações sem profundidade, para não sobrecarregar/poluir o arquivo de logs. Apenas
                    // em caso de falha acrescenta detalhes (processa as notificações)
                    fileLogger.LogInfo("Importando arquivo " + Path.GetFileName(fileName));
                    notifications.Clear();

                    Boolean imported = ImportFile(fileName, false);
                    if (!imported) // Falha ao importar o log
                    {
                        // São gravados logs detalhados para que se possa determinar a causa da falha
                        ProcessNotifications();
                        return;
                    }
                    lastFileDate = fileDate;
                }
            }

            // Grava a data do último acesso (baseada na data do último arquivo)
            if (lastFileDate != null)
            {
                PrintLogAccess printLogAccess = new PrintLogAccess(dataAccess.GetConnection());
                printLogAccess.SetLastAccess(lastFileDate.Value.AddDays(1));
            }
        }

        public void Execute()
        {
            // Verifica se as dependências foram instanciadas (se o método InitializeTaskState foi chamado)
            if (taskParams == null) return;
            if (dataAccess == null) return;
            if (notifications == null) return;
            if (fileLogger == null) return;

            // Verifica os parâmetros recebidos
            String logDirectory = taskParams["logDirectory"];
            Boolean importPreviousLogs = Boolean.Parse(taskParams["importPreviousLogs"]);

            dataAccess.OpenConnection();

            // "lastAccess" é gravado e recuperado do banco com frequência, logo não pode ser obtido de "taskParams"
            PrintLogAccess printLogAccess = new PrintLogAccess(dataAccess.GetConnection());
            DateTime lastAccess = printLogAccess.GetLastAccess();

            // Importa a massa de dados existente
            if ((importPreviousLogs) && (firstExecution))
            {
                ImportPreviousLogs(logDirectory);
                firstExecution = false;
                dataAccess.CloseConnection();
                return;
            }

            // Procura pelo arquivo do último dia trabalhado
            String fileName = PrintLogFile.GetLastFile(logDirectory);
            if (fileName == null)
            {
                // Não processa (sai do método) se não achou o arquivo
                dataAccess.CloseConnection();
                return;
            }

            // Compara a data do arquivo com a data do último acesso
            DateTime? fileDate = PrintLogFile.GetDate(fileName);
            if ((fileDate == null) || (fileDate.Value.CompareTo(lastAccess) < 0))
            {
                // Não processa (sai do método) se o arquivo já foi processado
                dataAccess.CloseConnection();
                return;
            }

            // Inicia o append no arquivo de log (acrescentando o "startingDelimiter")
            fileLogger.LogInfo("Importação de log - Iniciando execução...", true);
            // Informa dados do arquivo
            fileLogger.LogInfo("Arquivo - fileName = " + fileName);
            fileLogger.LogInfo("Data - fileDate = " + fileDate.Value.ToShortDateString());
            notifications.Clear();
            // Persiste os registros do arquivo de log
            Boolean imported = ImportFile(fileName, true);
            ProcessNotifications();
            fileLogger.LogInfo("Execução concluída.");

            // Grava a data do último acesso no banco
            if (imported) printLogAccess.SetLastAccess(DateTime.Now);

            dataAccess.CloseConnection();
        }

        private void ProcessNotifications()
        {
            foreach (Object notification in notifications)
            {
                if (notification is String)
                {
                    fileLogger.LogInfo((String)notification);
                }
                if (notification is Exception)
                {
                    Exception exc = (Exception)notification;
                    fileLogger.LogError(exc.Message + Environment.NewLine + exc.StackTrace);
                }
            }
        }

        public void NotifyObject(Object obj)
        {
            if (notifications != null) notifications.Add(obj);
        }
    }

}
