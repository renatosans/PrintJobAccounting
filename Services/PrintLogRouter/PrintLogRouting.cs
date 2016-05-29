using System;
using System.IO;
using System.Timers;
using System.Reflection;
using System.Diagnostics;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Collections.Specialized;
using AccountingLib.ServerPrintLog;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.JobExecution;


namespace PrintLogRouter
{
    public partial class PrintLogRouting : ServiceBase
    {
        private JobController jobController;


        public PrintLogRouting()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Busca os parâmetros de execução
            NameValueCollection taskParams = PrintLogContext.GetTaskParams();

            // Caso não consiga recuperar os parâmetros de execução, avisa que o XML
            // é inválido e para o serviço
            if (taskParams == null)
            {
                String stopReason = "XML inválido. Verifique se a instalação foi executada corretamente.";

                if (EventLog.SourceExists("Print Log Router"))
                    EventLog.WriteEntry("Print Log Router", "Parando o serviço... " + stopReason);

                this.Stop();
                return;
            }

            // Inicia a execução
            double interval = double.Parse(taskParams["interval"]);
            List<IPeriodicTask> taskList = new List<IPeriodicTask>();
            taskList.Add(new DeviceDiscoverer());
            taskList.Add(new PrintLogRoutingTask());
            jobController = new JobController(taskList, taskParams, null, interval);
            jobController.Start();

            // Agenda o mecanismo de reinicio do serviço para daqui a 5 horas
            Timer timeToLive = new Timer(17999000);
            timeToLive.Elapsed += new ElapsedEventHandler(KickService);
            timeToLive.Start();
        }

        void KickService(Object sender, ElapsedEventArgs e)
        {
            // Verifica se o aplicativo ServiceKicker está disponível
            String serviceLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String serviceKicker = PathFormat.Adjust(serviceLocation) + "ServiceKicker.exe";
            if (!File.Exists(serviceKicker)) return;

            // Derruba o serviço e reinicia
            String serviceName = '"' + this.ServiceName + '"';
            Process.Start(serviceKicker, "/Service:" + serviceName + " /WaitBeforeStop:8000 /WaitBeforeStart:59000");
        }

        protected override void OnStop()
        {
            // Interrompe a execução
            jobController.Stop();
        }
    }

}
