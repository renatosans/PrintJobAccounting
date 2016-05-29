using System;
using System.Timers;
using System.Collections.Specialized;
using AccountingLib.DataAccessObjects;
using DocMageFramework.FileUtils;
using DocMageFramework.JobExecution;
using DocMageFramework.DataManipulation;


namespace AccountingLib.ServerCopyLog
{
    public class CopyLogImportController
    {
        private JobController jobController;

        private Timer startupTrigger;


        public CopyLogImportController()
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
            NameValueCollection taskParams = applicationParamDAO.GetTaskParams("copyLogImport");
            double interval = double.Parse(taskParams["interval"]);

            // Fecha a conexão com o banco
            dataAccess.CloseConnection();

            IPeriodicTask copyLogImportTask = new CopyLogImportTask();
            jobController = new JobController(copyLogImportTask, taskParams, dataAccess, interval);
            jobController.Start();
        }

        public void SuspendJob()
        {
            if (jobController != null) jobController.Stop();
        }
    }

}
