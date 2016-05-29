using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using AccountingLib.ReportMailing;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;


namespace ReportMailer
{
    public partial class ReportMailing : ServiceBase
    {
        public ReportMailing()
        {
            InitializeComponent();
        }

        private ReportMailingController controller;


        protected override void OnStart(String[] args)
        {
            // Inicia a execução
            controller = new ReportMailingController();
            ExtractAppResources();
        }

        protected override void OnStop()
        {
            // Interrompe a execução
            controller.SuspendJob();
        }

        private void ExtractAppResources()
        {
            // Obtem o caminho do arquivo ( onde ele será extraido)
            String logoFile = FileResource.MapDesktopResource("Logo.png");
            if (File.Exists(logoFile)) return; // aborta pois já foi extraido previamente

            // Extrai o arquivo para o diretório da aplicação
            Stream embeddedResource = IOHandler.GetEmbeddedResource("Logo.png");
            FileStream fileStream = new FileStream(logoFile, FileMode.Create);
            IOHandler.CopyStream(embeddedResource, fileStream);
            fileStream.Flush();
            fileStream.Close();
            embeddedResource.Close();
        }
    }

}
