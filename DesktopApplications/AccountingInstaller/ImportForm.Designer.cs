namespace AccountingInstaller
{
    partial class ImportForm
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
            this.txtProcessInfo = new System.Windows.Forms.TextBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.lblDetails = new System.Windows.Forms.Label();
            this.lblGeneralInfo = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnDeselect = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.selectedListBox = new System.Windows.Forms.ListBox();
            this.tenantListBox = new System.Windows.Forms.ListBox();
            this.pnlProgressMeter = new System.Windows.Forms.Panel();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panel1.SuspendLayout();
            this.pnlProgressMeter.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.Location = new System.Drawing.Point(369, 325);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(117, 34);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtProcessInfo
            // 
            this.txtProcessInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProcessInfo.BackColor = System.Drawing.SystemColors.Window;
            this.txtProcessInfo.Location = new System.Drawing.Point(20, 375);
            this.txtProcessInfo.Multiline = true;
            this.txtProcessInfo.Name = "txtProcessInfo";
            this.txtProcessInfo.ReadOnly = true;
            this.txtProcessInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProcessInfo.Size = new System.Drawing.Size(543, 75);
            this.txtProcessInfo.TabIndex = 19;
            this.txtProcessInfo.Text = "Aguardando ação do usuário...";
            // 
            // btnImport
            // 
            this.btnImport.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnImport.Location = new System.Drawing.Point(101, 326);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(117, 33);
            this.btnImport.TabIndex = 18;
            this.btnImport.Text = "Importar Dados";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // lblDetails
            // 
            this.lblDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDetails.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetails.Location = new System.Drawing.Point(20, 237);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(543, 71);
            this.lblDetails.TabIndex = 15;
            this.lblDetails.Text = "Obs.: O botão \"Criar dados\" cria apenas a massa de dados inicial. Dados das empre" +
                "sas deverão ser cadastrados manualmente.";
            // 
            // lblGeneralInfo
            // 
            this.lblGeneralInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGeneralInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblGeneralInfo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGeneralInfo.Location = new System.Drawing.Point(20, 10);
            this.lblGeneralInfo.Name = "lblGeneralInfo";
            this.lblGeneralInfo.Size = new System.Drawing.Size(543, 90);
            this.lblGeneralInfo.TabIndex = 14;
            this.lblGeneralInfo.Text = "Escolha entre as opções abaixo qual atende melhor a implantação. Para importar os dados de um servidor " +
                                       "já existente exporte os dados do servidor para o subdiretório \"Data\" e clique em \"Importar Dados\". Para " +
                                       "fazer uma instalação sem importação de massa de dados preexistentes clique em \"Criar dados\".";
            //
            // btnCreate
            // 
            this.btnCreate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCreate.Location = new System.Drawing.Point(236, 325);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(117, 34);
            this.btnCreate.TabIndex = 23;
            this.btnCreate.Text = "Criar Dados";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 14);
            this.label1.TabIndex = 26;
            this.label1.Text = "Empresas cadastradas:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(290, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 14);
            this.label2.TabIndex = 27;
            this.label2.Text = "Importar:";
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSelect.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.Location = new System.Drawing.Point(252, 29);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(30, 27);
            this.btnSelect.TabIndex = 28;
            this.btnSelect.Text = ">>";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnDeselect
            // 
            this.btnDeselect.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDeselect.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeselect.Location = new System.Drawing.Point(252, 62);
            this.btnDeselect.Name = "btnDeselect";
            this.btnDeselect.Size = new System.Drawing.Size(30, 27);
            this.btnDeselect.TabIndex = 29;
            this.btnDeselect.Text = "<<";
            this.btnDeselect.UseVisualStyleBackColor = true;
            this.btnDeselect.Click += new System.EventHandler(this.btnDeselect_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.selectedListBox);
            this.panel1.Controls.Add(this.tenantListBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnSelect);
            this.panel1.Controls.Add(this.btnDeselect);
            this.panel1.Location = new System.Drawing.Point(20, 113);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(543, 112);
            this.panel1.TabIndex = 30;
            // 
            // selectedListBox
            // 
            this.selectedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedListBox.FormattingEnabled = true;
            this.selectedListBox.Location = new System.Drawing.Point(288, 20);
            this.selectedListBox.Name = "selectedListBox";
            this.selectedListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.selectedListBox.Size = new System.Drawing.Size(252, 82);
            this.selectedListBox.TabIndex = 31;
            // 
            // tenantListBox
            // 
            this.tenantListBox.FormattingEnabled = true;
            this.tenantListBox.Location = new System.Drawing.Point(3, 20);
            this.tenantListBox.Name = "tenantListBox";
            this.tenantListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.tenantListBox.Size = new System.Drawing.Size(243, 82);
            this.tenantListBox.TabIndex = 30;
            // 
            // pnlProgressMeter
            // 
            this.pnlProgressMeter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProgressMeter.Controls.Add(this.lblProgress);
            this.pnlProgressMeter.Controls.Add(this.progressBar);
            this.pnlProgressMeter.Location = new System.Drawing.Point(20, 237);
            this.pnlProgressMeter.Name = "pnlProgressMeter";
            this.pnlProgressMeter.Size = new System.Drawing.Size(543, 71);
            this.pnlProgressMeter.TabIndex = 31;
            this.pnlProgressMeter.Visible = false;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(25, 8);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(71, 16);
            this.lblProgress.TabIndex = 1;
            this.lblProgress.Text = "Progresso";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(25, 27);
            this.progressBar.Minimum = 1;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(495, 23);
            this.progressBar.Step = 25;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 0;
            this.progressBar.Value = 50;
            // 
            // ImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 492);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblGeneralInfo);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.pnlProgressMeter);
            this.Controls.Add(this.txtProcessInfo);
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "ImportForm";
            this.Text = "Importação de massa de dados existente";
            this.Shown += new System.EventHandler(this.ImportForm_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlProgressMeter.ResumeLayout(false);
            this.pnlProgressMeter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtProcessInfo;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.Label lblGeneralInfo;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnDeselect;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox tenantListBox;
        private System.Windows.Forms.ListBox selectedListBox;
        private System.Windows.Forms.Panel pnlProgressMeter;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}
