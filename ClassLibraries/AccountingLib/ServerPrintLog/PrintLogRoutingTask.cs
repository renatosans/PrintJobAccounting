using System;
using System.IO;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Specialized;
using AccountingLib.Entities;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.JobExecution;
using DocMageFramework.DataManipulation;


namespace AccountingLib.ServerPrintLog
{
    public class PrintLogRoutingTask : IPeriodicTask, IListener
    {
        private NameValueCollection taskParams;

        private List<Object> notifications;

        private FileLogger fileLogger;

        private Boolean hasErrors;


        public void InitializeTaskState(NameValueCollection taskParams, DataAccess dataAccess)
        {
            this.taskParams = taskParams;

            notifications = new List<Object>();

            fileLogger = new FileLogger(FileResource.MapDesktopResource("PrintLogRouting.log"));
            fileLogger.FileHeader = new String[3];
            fileLogger.FileHeader[0] = "╔".PadRight(80, '═') + "╗";
            fileLogger.FileHeader[1] = "║".PadRight(23, ' ') + "   Log de aplicativo (Log Router)  " + "║".PadLeft(23);
            fileLogger.FileHeader[2] = "╚".PadRight(80, '═') + "╝";
            fileLogger.LogRawData(Environment.NewLine);
        }

        public void Execute()
        {
            // Verifica se as dependências foram instanciadas ( se o método InitializeTaskState foi chamado )
            if (taskParams == null) return;
            if (notifications == null) return;
            if (fileLogger == null) return;

            // Verifica os parâmetros recebidos
            String serviceUrl = taskParams["url"];
            int tenantId = int.Parse(taskParams["tenantId"]);
            String logDirectories = taskParams["logDirectories"];
            String copyLogDir = taskParams["copyLogDir"];

            // Inicia o append no arquivo de trace (acrescentando o "startingDelimiter")
            fileLogger.LogInfo("Envio de logs - Iniciando execução...", true);
            JobRouter jobRouter = new JobRouter(serviceUrl, tenantId, this);

            // Envia os logs de impressão
            notifications.Clear();
            if (!jobRouter.SendPrintJobs(logDirectories))
            {
                // São gravados logs detalhados para que se possa determinar a causa da falha
                ProcessNotifications();
                return;
            }

            // Envia os logs de cópia
            notifications.Clear();
            if (!jobRouter.SendCopyJobs(copyLogDir))
            {
                // São gravados logs detalhados para que se possa determinar a causa da falha
                ProcessNotifications();
                return;
            }

            fileLogger.LogInfo("Execução concluída.");
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
