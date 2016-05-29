namespace PrintInspector
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
            this.PrintInspectorProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.PrintInspectorInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // PrintInspectorProcessInstaller
            // 
            this.PrintInspectorProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.PrintInspectorProcessInstaller.Password = null;
            this.PrintInspectorProcessInstaller.Username = null;
            // 
            // PrintInspectorInstaller
            // 
            this.PrintInspectorInstaller.Description = "Obtem dados do spool de impressão para o sistema de Accounting";
            this.PrintInspectorInstaller.ServiceName = "Print Inspector";
            this.PrintInspectorInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.PrintInspectorProcessInstaller,
            this.PrintInspectorInstaller});
        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller PrintInspectorProcessInstaller;
        private System.ServiceProcess.ServiceInstaller PrintInspectorInstaller;
    }
}