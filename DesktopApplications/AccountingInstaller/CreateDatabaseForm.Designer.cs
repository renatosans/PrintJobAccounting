namespace AccountingInstaller
{
    partial class CreateDatabaseForm
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
            this.lblGeneralInfo = new System.Windows.Forms.Label();
            this.lblDetails = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblSALogin = new System.Windows.Forms.Label();
            this.txtSALogin = new System.Windows.Forms.TextBox();
            this.btnSALogin = new System.Windows.Forms.Button();
            this.txtProcessInfo = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlProgressMeter = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.pnlProgressMeter.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblGeneralInfo
            // 
            this.lblGeneralInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGeneralInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblGeneralInfo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGeneralInfo.Location = new System.Drawing.Point(14, 18);
            this.lblGeneralInfo.Name = "lblGeneralInfo";
            this.lblGeneralInfo.Size = new System.Drawing.Size(478, 62);
            this.lblGeneralInfo.TabIndex = 0;
            this.lblGeneralInfo.Text = "Preencha as informações de acesso ao servidor de banco de dados para que o instal" +
                "ador crie as estruturas (tabelas e procedures) necessárias. ";
            // 
            // lblDetails
            // 
            this.lblDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDetails.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetails.Location = new System.Drawing.Point(14, 127);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(478, 27);
            this.lblDetails.TabIndex = 1;
            this.lblDetails.Text = " Forneça um usuário com permissões administrativas no banco de dados.";
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServer.Location = new System.Drawing.Point(24, 97);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(162, 16);
            this.lblServer.TabIndex = 4;
            this.lblServer.Text = "Instancia do SQL Server";
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(194, 93);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(291, 20);
            this.txtServer.TabIndex = 3;
            this.txtServer.Text = "SERVER01\\SQL2008";
            // 
            // lblSALogin
            // 
            this.lblSALogin.AutoSize = true;
            this.lblSALogin.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSALogin.Location = new System.Drawing.Point(30, 168);
            this.lblSALogin.Name = "lblSALogin";
            this.lblSALogin.Size = new System.Drawing.Size(66, 16);
            this.lblSALogin.TabIndex = 6;
            this.lblSALogin.Text = "SA Login";
            // 
            // txtSALogin
            // 
            this.txtSALogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSALogin.BackColor = System.Drawing.SystemColors.Window;
            this.txtSALogin.Location = new System.Drawing.Point(102, 167);
            this.txtSALogin.Name = "txtSALogin";
            this.txtSALogin.ReadOnly = true;
            this.txtSALogin.Size = new System.Drawing.Size(343, 20);
            this.txtSALogin.TabIndex = 5;
            // 
            // btnSALogin
            // 
            this.btnSALogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSALogin.Location = new System.Drawing.Point(457, 166);
            this.btnSALogin.Name = "btnSALogin";
            this.btnSALogin.Size = new System.Drawing.Size(28, 22);
            this.btnSALogin.TabIndex = 7;
            this.btnSALogin.Text = "...";
            this.btnSALogin.UseVisualStyleBackColor = true;
            this.btnSALogin.Click += new System.EventHandler(this.btnSALogin_Click);
            // 
            // txtProcessInfo
            // 
            this.txtProcessInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProcessInfo.BackColor = System.Drawing.SystemColors.Window;
            this.txtProcessInfo.Location = new System.Drawing.Point(14, 289);
            this.txtProcessInfo.Multiline = true;
            this.txtProcessInfo.Name = "txtProcessInfo";
            this.txtProcessInfo.ReadOnly = true;
            this.txtProcessInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProcessInfo.Size = new System.Drawing.Size(478, 111);
            this.txtProcessInfo.TabIndex = 8;
            this.txtProcessInfo.Text = "Aguardando preenchimento dos dados...";
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCreate.Location = new System.Drawing.Point(110, 240);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(117, 34);
            this.btnCreate.TabIndex = 10;
            this.btnCreate.Text = "Criar";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.Location = new System.Drawing.Point(287, 240);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(117, 34);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlProgressMeter
            // 
            this.pnlProgressMeter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProgressMeter.Controls.Add(this.progressBar);
            this.pnlProgressMeter.Location = new System.Drawing.Point(14, 198);
            this.pnlProgressMeter.Name = "pnlProgressMeter";
            this.pnlProgressMeter.Size = new System.Drawing.Size(478, 33);
            this.pnlProgressMeter.TabIndex = 12;
            this.pnlProgressMeter.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(30, 8);
            this.progressBar.Minimum = 1;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(419, 17);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 0;
            this.progressBar.Value = 50;
            // 
            // CreateDatabaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 412);
            this.ControlBox = false;
            this.Controls.Add(this.pnlProgressMeter);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.txtProcessInfo);
            this.Controls.Add(this.btnSALogin);
            this.Controls.Add(this.lblSALogin);
            this.Controls.Add(this.txtSALogin);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.lblGeneralInfo);
            this.MinimumSize = new System.Drawing.Size(520, 450);
            this.Name = "CreateDatabaseForm";
            this.Text = "Criação de estruturas no banco de dados";
            this.pnlProgressMeter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGeneralInfo;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label lblSALogin;
        private System.Windows.Forms.TextBox txtSALogin;
        private System.Windows.Forms.Button btnSALogin;
        private System.Windows.Forms.TextBox txtProcessInfo;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlProgressMeter;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}
