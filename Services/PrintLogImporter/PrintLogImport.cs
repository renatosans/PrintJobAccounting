using System;
using System.ServiceProcess;
using AccountingLib.ServerPrintLog;


namespace PrintLogImporter
{
    public partial class PrintLogImport : ServiceBase
    {
        public PrintLogImport()
        {
            InitializeComponent();
        }

        private PrintLogImportController controller;


        protected override void OnStart(String[] args)
        {
            // Inicia a execução
            controller = new PrintLogImportController();
        }

        protected override void OnStop()
        {
            // Interrompe a execução
            controller.SuspendJob();
        }
    }

}
