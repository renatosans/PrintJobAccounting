using System;
using System.Timers;
using System.Collections.Generic;
using System.Collections.Specialized;
using AccountingLib.DataAccessObjects;
using DocMageFramework.FileUtils;
using DocMageFramework.JobExecution;
using DocMageFramework.DataManipulation;


namespace AccountingLib.ReportMailing
{
    public class ReportMailingController
    {
        private JobController jobController;

        private Timer startupTrigger;


        public ReportMailingController()
        {
            jobController = null;

            // É necessário evitar acesso ao banco na inicialização do serviço, pois quando a máquina for
            // ligada e o serviço estiver subindo o SQL Server também pode estar subindo. Acessos ao banco
            // realizados nesta situação irão provocar a falha de inicialização do serviço.

            // Esta classe está implementada de maneira a realizar o primeiro acesso ao BD 10 minutos
            // após o serviço ser inicializado
            startupTrigger = new Timer(599000);
            startupTrigger.Elapsed += new ElapsedEventHandler(InitializeJob);
            startupTrigger.Start();
        }

        private void InitializeJob(Object sender, ElapsedEventArgs e)
        {
            // Verifica se o job já foi iniciado
            if (jobController != null)
            {
                startupTrigger.Stop();
                return;
            }

            // Abre a conexão com o banco
            DataAccess dataAccess = DataAccess.Instance;
            dataAccess.MountConnection(FileResource.MapDesktopResource("DataAccess.xml"), DatabaseEnum.PrintAccounting);
            dataAccess.OpenConnection();

            // Busca os parâmetros de execução no banco
            ApplicationParamDAO applicationParamDAO = new ApplicationParamDAO(dataAccess.GetConnection());
            Dictionary<String, NameValueCollection> appParams = applicationParamDAO.GetParamsGroupByTask();
            double interval = Double.Parse(appParams["reportMailing"]["interval"]);

            // Fecha a conexão com o banco
            dataAccess.CloseConnection();

            // Cria a lista sem nenhum parâmetro ( a classe reportMailingTask não necessita parâmetros por enquanto)
            NameValueCollection taskParams = new NameValueCollection();

            IPeriodicTask reportMailingTask = new ReportMailingTask();
            jobController = new JobController(reportMailingTask, taskParams, dataAccess, interval);
            jobController.Start();
        }

        public void SuspendJob()
        {
            if (jobController != null) jobController.Stop();
        }
    }

}
