using System;
using System.Timers;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocMageFramework.DataManipulation;


namespace DocMageFramework.JobExecution
{
    /// <summary>
    /// Classe utilizada para o controle de execução de uma tarefa periódica, a tarefa controlada precisa
    /// implementar a interface IPeriodicTask
    /// </summary>
    public class JobController
    {
        private List<IPeriodicTask> taskList;

        private NameValueCollection taskParams;

        private DataAccess dataAccess;

        private Timer jobTrigger;


        /// <summary>
        /// Construtor da classe, que recebe a tarefa a ser controlada e seus parâmetros de execução.
        /// Adicionalmente recebe o intervalo de execução da tarefa e o mecanismo de acesso ao banco.
        /// </summary>
        public JobController(IPeriodicTask task, NameValueCollection taskParams, DataAccess dataAccess, double interval)
        {
            this.taskList = new List<IPeriodicTask>();
            this.taskList.Add(task);
            this.taskParams = taskParams;
            this.dataAccess = dataAccess;

            jobTrigger = new Timer(interval);
            jobTrigger.Elapsed += new ElapsedEventHandler(OnTimerEvent);
            jobTrigger.Enabled = false;
        }

        public JobController(List<IPeriodicTask> taskList, NameValueCollection taskParams, DataAccess dataAccess, double interval)
        {
            this.taskList = taskList;
            this.taskParams = taskParams;
            this.dataAccess = dataAccess;

            jobTrigger = new Timer(interval);
            jobTrigger.Elapsed += new ElapsedEventHandler(OnTimerEvent);
            jobTrigger.Enabled = false;
        }

        public void Start()
        {
            foreach(IPeriodicTask task in this.taskList)
                task.InitializeTaskState(taskParams, dataAccess);

            jobTrigger.Start();
        }

        public void Stop()
        {
            jobTrigger.Stop();
        }

        public void SetInterval(double interval)
        {
            jobTrigger.Stop();
            jobTrigger.Interval = interval;
            jobTrigger.Start();
        }

        private void OnTimerEvent(object sender, ElapsedEventArgs e)
        {
            foreach (IPeriodicTask task in this.taskList)
                task.Execute();
        }
    }

}
