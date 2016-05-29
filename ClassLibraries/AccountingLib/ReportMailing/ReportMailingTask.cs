using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using AccountingLib.Management;
using AccountingLib.ServerPrintLog;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.Reporting;
using DocMageFramework.Reflection;
using DocMageFramework.JobExecution;
using DocMageFramework.DataManipulation;


namespace AccountingLib.ReportMailing
{
    public class ReportMailingTask: IPeriodicTask, IListener
    {
        private NameValueCollection taskParams;

        private DataAccess dataAccess;

        private List<Object> notifications;

        private FileLogger fileLogger;

        private int currentTenant; // tenant cujos mailings estão sendo processados

        private String currentSysSender;

        private String currentFormatExtension;

        private int currentPeriodEndDate;

        private IReportBuilder currentReportBuilder;


        public void InitializeTaskState(NameValueCollection taskParams, DataAccess dataAccess)
        {
            this.taskParams = taskParams;
            this.dataAccess = dataAccess;

            notifications = new List<Object>();

            fileLogger = new FileLogger(FileResource.MapDesktopResource("ReportMailing.log"));
            fileLogger.FileHeader = new String[3];
            fileLogger.FileHeader[0] = "╔".PadRight(80, '═') + "╗";
            fileLogger.FileHeader[1] = "║".PadRight(23, ' ') + " Log de aplicativo (Report Mailer) " + "║".PadLeft(23);
            fileLogger.FileHeader[2] = "╚".PadRight(80, '═') + "╝";
            fileLogger.LogRawData(Environment.NewLine);
        }

        /// <summary>
        /// Encapsula a chamada ao gerador de relatórios, decide qual classe de relatório utilizar
        /// </summary>
        private void BuildReport(String reportFilename, ReportTypeEnum reportType, ReportFrequencyEnum reportFrequency)
        {
            FileInfo reportFile = new FileInfo(reportFilename);
            DateRange dateRange = ReportContext.GetDateRange(reportFrequency);

            // Usa a classe base dos relatórios para obter o nome completo da classe incluindo dll/assembly
            String qualifiedName = typeof(AbstractReport).AssemblyQualifiedName;
            qualifiedName = qualifiedName.Replace("AbstractReport", reportType.ToString());
            Type reportClass = Type.GetType(qualifiedName);

            // Monta os parâmetros do relatório e cria uma instância da classe de relatório
            ArgumentBuilder argumentBuilder = new ArgumentBuilder();
            argumentBuilder.Add("tenantId", currentTenant.ToString());
            argumentBuilder.Add("startDate", dateRange.GetFirstDay().ToString());
            argumentBuilder.Add("endDate", dateRange.GetLastDay().ToString());
            AbstractReport report = (AbstractReport)Activator.CreateInstance(reportClass, argumentBuilder.GetArguments(reportClass));

            // Caso não seja nenhum dos relatórios implementados aborta
            if ((reportClass == null) || (report == null)) return;

            // Gera o relatório
            report.InitializeComponents(reportFile, currentReportBuilder, dataAccess.GetConnection());
            report.BuildReport();
        }


        private void ProcessMailing(MailingDAO mailingDAO, Mailing mailing)
        {
            SmtpServerDAO smtpServerDAO = new SmtpServerDAO(dataAccess.GetConnection());
            SmtpServer smtpServer = smtpServerDAO.GetSmtpServer(currentTenant, mailing.smtpServer);
            ReportFrequencyEnum reportFrequency = (ReportFrequencyEnum)mailing.frequency;
            ReportTypeEnum reportType = (ReportTypeEnum)mailing.reportType;
            String recipients = mailing.recipients;
            DateTime lastSend = mailing.lastSend;

            // Verifica se está na data de envio, aborta caso não esteja
            if (!ReportContext.IsScheduledTime(reportFrequency, currentPeriodEndDate)) return;
            
            // Verifica se o log foi importado
            PrintLogPersistence logPersistence = new PrintLogPersistence(currentTenant, dataAccess.GetConnection(), null, false);
            Boolean logImported = logPersistence.FileImported(ReportContext.GetDateRange(reportFrequency));

            // Verifica se o relatório já foi enviado hoje
            Boolean alreadySent = lastSend.Date == DateTime.Now.Date;

            // Caso o log tenha sido importado e se ainda não enviou, gera o relatório e envia
            if ((logImported) && (!alreadySent))
            {
                // Inicia o append no arquivo de log (acrescentando o "startingDelimiter")
                fileLogger.LogInfo("Envio de relatório - Iniciando execução...", true);
                // Informa dados do mailing
                fileLogger.LogInfo("Frequência de envio - reportFrequency = " + reportFrequency.ToString());
                fileLogger.LogInfo("Relatório - reportType = " + reportType.ToString());
                fileLogger.LogInfo("Destinatários - recipients = " + recipients);
                notifications.Clear();

                String reportStamp = DateTime.Now.Ticks.ToString();
                String reportFilename = FileResource.MapDesktopResource("Report" + reportStamp + currentFormatExtension);
                BuildReport(reportFilename, reportType, reportFrequency);

                String mailSubject = "Relatório " + ReportContext.GetFrequencyCaption(reportFrequency);
                List<String> attachmentFiles = new List<String>();
                attachmentFiles.Add(reportFilename);
                MailSender mailSender = new MailSender(smtpServer.CreateSysObject(), this);
                mailSender.SetContents("Email gerado automaticamente, não responder.", attachmentFiles);
                Boolean success = mailSender.SendMail(mailSubject, currentSysSender, recipients);

                ProcessNotifications();
                if (success) // Grava a data de envio de envio no banco
                {
                    mailing.lastSend = DateTime.Now.Date;
                    mailingDAO.SetMailing(mailing);
                    fileLogger.LogInfo("Execução concluída.");
                }
            }

            // Tenta remover arquivos temporários, ignora caso os arquivos estejam em uso
            // tentará novamente nas próximas execuções
            ReportContext.TryRemoveTempFiles();            
        }

        private String GetFormatExtension(int exportFormat)
        {
            ExportFormatEnum currentFormat = (ExportFormatEnum)exportFormat;
            return "." + currentFormat.ToString();
        }

        // Obtem o mecanismo de renderização de relatórios ( necessário para gerar o arquivo de
        // relatório que será enviado por e-mail )
        private IReportBuilder GetReportBuilder(int exportFormat)
        {
            IReportBuilder reportBuilder = null;

            ExportFormatEnum currentFormat = (ExportFormatEnum)exportFormat;
            Dictionary<String, Object> exportOptions = ExportFormatContext.GetExportOptions(currentFormat);

            if (exportOptions.ContainsKey("ReportBuilder"))
                reportBuilder = (IReportBuilder)exportOptions["ReportBuilder"];

            return reportBuilder;
        }

        public void Execute()
        {
            // Verifica se as dependências foram instanciadas (se o método InitializeTaskState foi chamado)
            if (taskParams == null) return;
            if (dataAccess == null) return;
            if (notifications == null) return;
            if (fileLogger == null) return;
            
            dataAccess.OpenConnection();

            TenantDAO tenantDAO = new TenantDAO(dataAccess.GetConnection());
            PreferenceDAO preferenceDAO = new PreferenceDAO(dataAccess.GetConnection());
            MailingDAO mailingDAO = new MailingDAO(dataAccess.GetConnection());

            List<Object> tenantList = tenantDAO.GetAllTenants();
            foreach (Tenant tenant in tenantList)
            {
                currentTenant = tenant.id;
                Preference senderPreference = preferenceDAO.GetTenantPreference(currentTenant, "sysSender");
                currentSysSender = senderPreference.value;
                Preference exportPreference = preferenceDAO.GetTenantPreference(currentTenant, "exportFormat");
                int exportFormat = 0; // o default é eportar para PDF
                if (exportPreference != null) exportFormat = int.Parse(exportPreference.value);
                currentFormatExtension = GetFormatExtension(exportFormat);
                Preference endDatePreference = preferenceDAO.GetTenantPreference(currentTenant, "periodEndDate");
                currentPeriodEndDate = 1; // o default é o periodo entre o dia 1 deste mês e o dia 1 do mês passado
                if (endDatePreference != null) currentPeriodEndDate = int.Parse(endDatePreference.value);
                currentReportBuilder = GetReportBuilder(exportFormat);

                List<Object> mailingList = mailingDAO.GetAllMailings(currentTenant);
                foreach (Mailing mailing in mailingList)
                {
                    ProcessMailing(mailingDAO, mailing);
                }
            }

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
