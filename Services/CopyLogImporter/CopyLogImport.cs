using System;
using System.ServiceProcess;
using AccountingLib.ServerCopyLog;


namespace CopyLogImporter
{
    public partial class CopyLogImport : ServiceBase
    {
        public CopyLogImport()
        {
            InitializeComponent();
        }

        private CopyLogImportController controller;


        protected override void OnStart(String[] args)
        {
            // Inicia a execução
            controller = new CopyLogImportController();
        }

        protected override void OnStop()
        {
            // Interrompe a execução
            controller.SuspendJob();
        }
    }

}
