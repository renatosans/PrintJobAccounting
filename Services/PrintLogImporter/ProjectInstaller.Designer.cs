namespace PrintLogImporter
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PrintLogImportProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.PrintLogImportInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // PrintLogImportProcessInstaller
            // 
            this.PrintLogImportProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.PrintLogImportProcessInstaller.Password = null;
            this.PrintLogImportProcessInstaller.Username = null;
            // 
            // PrintLogImportInstaller
            // 
            this.PrintLogImportInstaller.Description = "Importa os logs de impressão gerados pelo Papercut";
            this.PrintLogImportInstaller.ServiceName = "Print Log Importer";
            this.PrintLogImportInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.PrintLogImportProcessInstaller,
            this.PrintLogImportInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller PrintLogImportProcessInstaller;
        private System.ServiceProcess.ServiceInstaller PrintLogImportInstaller;
    }
}