using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.JobExecution;
using DocMageFramework.DataManipulation;


namespace AccountingLib.ServerCopyLog
{
    public class CopyLogImportTask : IPeriodicTask, IListener
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

            fileLogger = new FileLogger(FileResource.MapDesktopResource("CopyLogImport.log"));
            fileLogger.FileHeader = new String[3];
            fileLogger.FileHeader[0] = "╔".PadRight(80, '═') + "╗";
            fileLogger.FileHeader[1] = "║".PadRight(23, ' ') + " Log de aplicativo (Log Importer)  " + "║".PadLeft(23);
            fileLogger.FileHeader[2] = "╚".PadRight(80, '═') + "╝";
            fileLogger.LogRawData(Environment.NewLine);
        }

        /// <summary>
        /// Importa o arquivo de log (insere os registros no banco de dados). Como a importação ocorre no final
        /// do dia (pouco depois da meia noite), a data dos registros deve ser (dia -1) se o dia teve expediente
        /// normal ou (dia-n) em domingos/feriados/greves
        /// </summary>
        private void ImportFile(String fileName)
        {
            notifications.Clear();

            /// Examina o arquivo e obtem o último dia trabalhado, considera como sendo
            /// a data da inserção do último registro
            DateTime businessDay = CopyLogFile.GetTimeStamp(fileName);

            // Não verificar se a data já possui registros pois .CSVs de diferentes equipamentos
            // são importados na mesma data  Ex.: if (!logPersistence.FileImported(dateRange))
            CopyLogPersistence logPersistence = new CopyLogPersistence(1, dataAccess.GetConnection(), this);
            logPersistence.ImportFile(fileName, businessDay);

            ProcessNotifications();
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
            DirectoryInfo dirInfo = new DirectoryInfo(logDirectory);

            dataAccess.OpenConnection();

            // "lastAccess" é gravado e recuperado do banco com frequência, logo não pode ser obtido de "taskParams"
            CopyLogAccess copyLogAccess = new CopyLogAccess(dataAccess.GetConnection());
            DateTime lastAccess = copyLogAccess.GetLastAccess();

            // Inicializa o parâmetro "lastAccess" na primeira execução ou quando estiver desatualizado
            if (firstExecution || copyLogAccess.IsOutOfDate(lastAccess))
            {
                copyLogAccess.SetLastAccess(DateTime.Now);
                firstExecution = false;
                dataAccess.CloseConnection();
                return;
            }

            // Não processa (sai do método) se a tarefa já foi executada hoje
            if (DateTime.Now.Date.CompareTo(lastAccess) <= 0)
            {
                dataAccess.CloseConnection();
                return;
            }

            // Inicia o append no arquivo de log (acrescentando o "startingDelimiter")
            fileLogger.LogInfo("Importação de log - Iniciando execução...", true);
            // Procura os arquivos .CSV no diretório de logs, faz a importação e armazena um backup
            // do arquivo com a extensão .OLD
            fileLogger.LogInfo("Fazendo varredura de arquivos .CSV no diretório de logs: " + logDirectory);
            FileInfo[] files = dirInfo.GetFiles();
            int filesParsed = 0;
            foreach (FileInfo file in files)
            {
                if (Path.GetExtension(file.Name).ToUpper() == ".CSV")
                {
                    fileLogger.LogInfo("Processando arquivo: " + file.Name);
                    String filePath = PathFormat.Adjust(logDirectory);
                    String fileName = Path.GetFileNameWithoutExtension(file.Name);
                    ImportFile(filePath + fileName + ".csv");
                    CopyLogFile.StoreOldFile(filePath, fileName, ".csv");
                    filesParsed++;
                }
            }
            if (filesParsed == 0) fileLogger.LogInfo("Nenhum arquivo processado( Não existem .CSVs no diretório).");
            fileLogger.LogInfo("Execução concluída.");

            // Grava a data do último acesso no banco
            copyLogAccess.SetLastAccess(DateTime.Now);

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
