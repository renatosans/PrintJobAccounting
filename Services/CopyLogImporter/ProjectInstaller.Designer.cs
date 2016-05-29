namespace CopyLogImporter
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
            this.CopyLogImportProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.CopyLogImportInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // CopyLogImportProcessInstaller
            // 
            this.CopyLogImportProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.CopyLogImportProcessInstaller.Password = null;
            this.CopyLogImportProcessInstaller.Username = null;
            // 
            // CopyLogImportInstaller
            // 
            this.CopyLogImportInstaller.Description = "Importa os logs de cópia gerados pelos multifuncionais Brother";
            this.CopyLogImportInstaller.ServiceName = "Copy Log Importer";
            this.CopyLogImportInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.CopyLogImportProcessInstaller,
            this.CopyLogImportInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller CopyLogImportProcessInstaller;
        private System.ServiceProcess.ServiceInstaller CopyLogImportInstaller;
    }
}