using System;
using System.ServiceProcess;
using AccountingLib.PrintInspect;


namespace PrintInspector
{
    public partial class PrintInspect : ServiceBase
    {
        private PrintInspectTask printInspectTask;

        public PrintInspect()
        {
            InitializeComponent();
        }

        protected override void OnStart(String[] args)
        {
            printInspectTask = new PrintInspectTask();
            printInspectTask.Start();
        }

        protected override void OnStop()
        {
            printInspectTask.Stop();
        }
    }

}
