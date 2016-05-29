namespace ReportMailer
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
            this.ReportMailingProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ReportMailingInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ReportMailingProcessInstaller
            // 
            this.ReportMailingProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ReportMailingProcessInstaller.Password = null;
            this.ReportMailingProcessInstaller.Username = null;
            // 
            // ReportMailingInstaller
            // 
            this.ReportMailingInstaller.Description = "Envia relatórios períodicos por e-mail (Cont. Impressões)";
            this.ReportMailingInstaller.ServiceName = "Report Mailer";
            this.ReportMailingInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ReportMailingProcessInstaller,
            this.ReportMailingInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ReportMailingProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ReportMailingInstaller;
    }
}