namespace AccountingClientInstaller
{
    partial class EnrollmentForm
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
            this.lblLicenseKey = new System.Windows.Forms.Label();
            this.txtLicenseKey = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.chkDoNotRegister = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblLicenseKey
            // 
            this.lblLicenseKey.AutoSize = true;
            this.lblLicenseKey.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLicenseKey.Location = new System.Drawing.Point(34, 24);
            this.lblLicenseKey.Name = "lblLicenseKey";
            this.lblLicenseKey.Size = new System.Drawing.Size(131, 17);
            this.lblLicenseKey.TabIndex = 0;
            this.lblLicenseKey.Text = "Chave de Ativação";
            // 
            // txtLicenseKey
            // 
            this.txtLicenseKey.Location = new System.Drawing.Point(37, 44);
            this.txtLicenseKey.Name = "txtLicenseKey";
            this.txtLicenseKey.Size = new System.Drawing.Size(401, 20);
            this.txtLicenseKey.TabIndex = 1;
            this.txtLicenseKey.Text = "C:\\ProductKey.bin";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(202, 118);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(100, 33);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "Submeter";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFile.Location = new System.Drawing.Point(444, 42);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(27, 23);
            this.btnOpenFile.TabIndex = 3;
            this.btnOpenFile.Text = "...";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // chkDoNotRegister
            // 
            this.chkDoNotRegister.AutoSize = true;
            this.chkDoNotRegister.Location = new System.Drawing.Point(37, 85);
            this.chkDoNotRegister.Name = "chkDoNotRegister";
            this.chkDoNotRegister.Size = new System.Drawing.Size(422, 17);
            this.chkDoNotRegister.TabIndex = 4;
            this.chkDoNotRegister.Text = "Não ativar o produto neste computador. (Instala o printLogger para ativação remota)";
            this.chkDoNotRegister.UseVisualStyleBackColor = true;
            this.chkDoNotRegister.CheckStateChanged += new System.EventHandler(this.chkDoNotRegister_CheckStateChanged);
            //
            // EnrollmentForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;			
            this.ClientSize = new System.Drawing.Size(505, 172);
            this.ControlBox = false;
            this.Controls.Add(this.chkDoNotRegister);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtLicenseKey);
            this.Controls.Add(this.lblLicenseKey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EnrollmentForm";
            this.Text = "Ativação do Produto";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblLicenseKey;
        private System.Windows.Forms.TextBox txtLicenseKey;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.CheckBox chkDoNotRegister;
    }
}
