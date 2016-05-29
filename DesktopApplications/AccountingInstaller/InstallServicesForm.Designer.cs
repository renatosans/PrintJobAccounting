namespace AccountingInstaller
{
    partial class InstallServicesForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnInstall = new System.Windows.Forms.Button();
            this.lblInstallDirectory = new System.Windows.Forms.Label();
            this.txtInstallDirectory = new System.Windows.Forms.TextBox();
            this.lblGeneralInfo = new System.Windows.Forms.Label();
            this.txtProcessInfo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.Location = new System.Drawing.Point(284, 144);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(117, 34);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnInstall
            // 
            this.btnInstall.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnInstall.Location = new System.Drawing.Point(132, 144);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(117, 34);
            this.btnInstall.TabIndex = 1;
            this.btnInstall.Text = "Instalar";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // lblInstallDirectory
            // 
            this.lblInstallDirectory.AutoSize = true;
            this.lblInstallDirectory.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstallDirectory.Location = new System.Drawing.Point(22, 90);
            this.lblInstallDirectory.Name = "lblInstallDirectory";
            this.lblInstallDirectory.Size = new System.Drawing.Size(152, 16);
            this.lblInstallDirectory.TabIndex = 21;
            this.lblInstallDirectory.Text = "Diretório de instalação";
            // 
            // txtInstallDirectory
            // 
            this.txtInstallDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInstallDirectory.Location = new System.Drawing.Point(180, 89);
            this.txtInstallDirectory.Name = "txtInstallDirectory";
            this.txtInstallDirectory.Size = new System.Drawing.Size(328, 20);
            this.txtInstallDirectory.TabIndex = 20;
            this.txtInstallDirectory.Text = "C:\\Services";
            // 
            // lblGeneralInfo
            // 
            this.lblGeneralInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGeneralInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblGeneralInfo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGeneralInfo.Location = new System.Drawing.Point(12, 9);
            this.lblGeneralInfo.Name = "lblGeneralInfo";
            this.lblGeneralInfo.Size = new System.Drawing.Size(508, 65);
            this.lblGeneralInfo.TabIndex = 18;
            this.lblGeneralInfo.Text = "Informe o diretório para instalação dos serviços. O instalador irá registrar os s" +
                "erviços no sistema operacional e inicia-los.";
            // 
            // txtProcessInfo
            // 
            this.txtProcessInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProcessInfo.BackColor = System.Drawing.SystemColors.Window;
            this.txtProcessInfo.Location = new System.Drawing.Point(12, 210);
            this.txtProcessInfo.Multiline = true;
            this.txtProcessInfo.Name = "txtProcessInfo";
            this.txtProcessInfo.ReadOnly = true;
            this.txtProcessInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProcessInfo.Size = new System.Drawing.Size(508, 144);
            this.txtProcessInfo.TabIndex = 22;
            this.txtProcessInfo.Text = "Aguardando preenchimento dos dados...";
            // 
            // InstallServicesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 392);
            this.ControlBox = false;
            this.Controls.Add(this.txtProcessInfo);
            this.Controls.Add(this.lblInstallDirectory);
            this.Controls.Add(this.txtInstallDirectory);
            this.Controls.Add(this.lblGeneralInfo);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.btnCancel);
            this.MinimumSize = new System.Drawing.Size(540, 400);
            this.Name = "InstallServicesForm";
            this.Text = "Instalação de serviços";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Label lblInstallDirectory;
        private System.Windows.Forms.TextBox txtInstallDirectory;
        private System.Windows.Forms.Label lblGeneralInfo;
        private System.Windows.Forms.TextBox txtProcessInfo;
    }
}
