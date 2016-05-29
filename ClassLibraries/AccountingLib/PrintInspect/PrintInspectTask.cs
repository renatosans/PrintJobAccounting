using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using AccountingLib.Spool;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;


namespace AccountingLib.PrintInspect
{
    /// <summary>
    /// Classe que executa o a tarefa de monitorar o spool. Não implementa IPeriodicTask pois
    /// verificar periodicamente o diretório de spool apresentaria lacunas na captura das im-
    /// pressões. Ao invés disso é utilizado um FileSystemWatcher para monitorar os arquivos
    /// de spool, disparando eventos na criação/alteração/exclusão dos arquivos.
    /// </summary>
    public class PrintInspectTask: IListener
    {
        private List<Object> notifications;

        private SpoolMonitor spoolMonitor;

        private FileLogger fileLogger;

        private String logFolder;


        public PrintInspectTask()
        {
            notifications = new List<Object>();

            spoolMonitor = new SpoolMonitor(this);

            fileLogger = new FileLogger(""); // não especifica o arquivo, usa o Relocate() posteriormente
            fileLogger.FileHeader = new String[2];
            fileLogger.FileHeader[0] = "Print Inspector - Version 1.0.1";
            fileLogger.FileHeader[1] = "Time,User,Printer,Document Name,Pages,Copies,Duplex,Color,DataType,Size";

            String applicationFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            logFolder = PathFormat.Adjust(applicationFolder) + @"PrintLogs\";
            Directory.CreateDirectory(logFolder);

            if (!EventLog.SourceExists("Print Inspector"))
                EventLog.CreateEventSource("Print Inspector", null);
        }

        public void NotifyObject(object obj)
        {
            if (obj is JobNotification)
            {
                // Processa a notificação de job em tempo real (o processamento não pode ser adiado)
                JobNotification jobNotification = (JobNotification)obj;
                ProcessJobNotification(jobNotification);
                return;
            }

            if (notifications.Count >= 10)
            {
                // Recolhe informações de trace acumuladas em notifications e limpa a lista
                String notificationItems = "";
                foreach (Object item in notifications)
                    if (item is String) notificationItems += item + Environment.NewLine;
                notifications.Clear();

                // Armazena informações de trace como evento para visualização no Event Viewer
                if (EventLog.SourceExists("Print Inspector"))
                    EventLog.WriteEntry("Print Inspector", notificationItems);
            }

            // Armazena outros tipos de notificação para processamento posterior
            notifications.Add(obj);
        }

        private void ProcessJobNotification(JobNotification jobNotification)
        {
            JobNotificationTypeEnum notificationType = jobNotification.NotificationType;
            String jobName = jobNotification.JobName;

            SpooledJob spooledJob = spoolMonitor.FindSpooledJob(jobName);
            ManagedPrintJob managedJob = null;
            if (spooledJob != null) managedJob = new ManagedPrintJob(jobName);

            if (notificationType == JobNotificationTypeEnum.JobCreated)
            {
                //if (!managedJob.IsPaused())
                //    managedJob.Pause();
            }

            if (notificationType == JobNotificationTypeEnum.JobChanged)
            {
                // Verifica se terminou o spooling para então processar os dados do job
                if (!managedJob.IsSpooling() && !spooledJob.Processed)
                {
                    // spooledJob.CopyFiles(@"C:\tempSpool\" + jobName.Split(new char[] { ',' })[0]);
                    String jobInfo = PrintJobContext.GetJobInfo(spooledJob);
                    WriteToLog(jobInfo);
                    spooledJob.Processed = true;
                }
            }
        }
        
        private void WriteToLog(String text)
        {
            String logName = "PrintLog-" + DateFormat.Adjust(DateTime.Now, false) + "_.csv";
            fileLogger.Relocate(logFolder + logName);
            fileLogger.LogRawData(text);
        }

        public void Start()
        {
            // not implemented yet
        }

        public void Stop()
        {
            // not implemented yet
        }
    }

}
