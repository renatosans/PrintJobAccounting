namespace AccountingClientInstaller
{
    partial class ConfigurationForm
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
            this.btnSubmit = new System.Windows.Forms.Button();
            this.lblTargetDirectory = new System.Windows.Forms.Label();
            this.txtTargetDirectory = new System.Windows.Forms.TextBox();
            this.lblDetails = new System.Windows.Forms.Label();
            this.lblGeneralInfo = new System.Windows.Forms.Label();
            this.lblLogDirectories = new System.Windows.Forms.Label();
            this.txtLogDirectories = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.Location = new System.Drawing.Point(315, 286);
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
            this.txtProcessInfo.Location = new System.Drawing.Point(20, 337);
            this.txtProcessInfo.Multiline = true;
            this.txtProcessInfo.Name = "txtProcessInfo";
            this.txtProcessInfo.ReadOnly = true;
            this.txtProcessInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProcessInfo.Size = new System.Drawing.Size(547, 115);
            this.txtProcessInfo.TabIndex = 19;
            this.txtProcessInfo.Text = "Aguardando preenchimento dos dados...";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSubmit.Location = new System.Drawing.Point(163, 286);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(117, 33);
            this.btnSubmit.TabIndex = 18;
            this.btnSubmit.Text = "Submeter";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // lblTargetDirectory
            // 
            this.lblTargetDirectory.AutoSize = true;
            this.lblTargetDirectory.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTargetDirectory.Location = new System.Drawing.Point(30, 95);
            this.lblTargetDirectory.Name = "lblTargetDirectory";
            this.lblTargetDirectory.Size = new System.Drawing.Size(152, 16);
            this.lblTargetDirectory.TabIndex = 17;
            this.lblTargetDirectory.Text = "Diretório de instalação";
            // 
            // txtTargetDirectory
            // 
            this.txtTargetDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTargetDirectory.Location = new System.Drawing.Point(188, 94);
            this.txtTargetDirectory.Name = "txtTargetDirectory";
            this.txtTargetDirectory.Size = new System.Drawing.Size(370, 20);
            this.txtTargetDirectory.TabIndex = 16;
            // 
            // lblDetails
            // 
            this.lblDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDetails.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetails.Location = new System.Drawing.Point(20, 134);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(547, 93);
            this.lblDetails.TabIndex = 15;
            this.lblDetails.Text = "Observações: Os diretórios que deverão ser monitorados são separados por ponto e vírgula. É permitido a " +
                                   "inclusão de compartilhamentos de rede, assim caso mais de uma máquina precise ser monitorada compartilhe os " +
                                   "diretórios de logs destas máquinas e inclua eles aqui.";
            //
            // lblGeneralInfo
            // 
            this.lblGeneralInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGeneralInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblGeneralInfo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGeneralInfo.Location = new System.Drawing.Point(20, 10);
            this.lblGeneralInfo.Name = "lblGeneralInfo";
            this.lblGeneralInfo.Size = new System.Drawing.Size(547, 66);
            this.lblGeneralInfo.TabIndex = 14;
            this.lblGeneralInfo.Text = "Forneça as informações abaixo para configurar o programa na estação de trabalho. " +
                "Informe o diretório de instalação do programa e os diretórios que devem ser moni" +
                "torados.";
            // 
            // lblLogDirectories
            // 
            this.lblLogDirectories.AutoSize = true;
            this.lblLogDirectories.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogDirectories.Location = new System.Drawing.Point(32, 246);
            this.lblLogDirectories.Name = "lblLogDirectories";
            this.lblLogDirectories.Size = new System.Drawing.Size(146, 16);
            this.lblLogDirectories.TabIndex = 22;
            this.lblLogDirectories.Text = "Diretório monitorados";
            // 
            // txtLogDirectories
            // 
            this.txtLogDirectories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogDirectories.Location = new System.Drawing.Point(184, 245);
            this.txtLogDirectories.Name = "txtLogDirectories";
            this.txtLogDirectories.Size = new System.Drawing.Size(338, 20);
            this.txtLogDirectories.TabIndex = 21;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(529, 242);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(29, 24);
            this.btnAdd.TabIndex = 23;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 464);
            this.ControlBox = false;
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblLogDirectories);
            this.Controls.Add(this.txtLogDirectories);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtProcessInfo);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.lblTargetDirectory);
            this.Controls.Add(this.txtTargetDirectory);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.lblGeneralInfo);
            this.MinimumSize = new System.Drawing.Size(596, 434);
            this.Name = "ConfigurationForm";
            this.Text = "Configuração do programa";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtProcessInfo;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label lblTargetDirectory;
        private System.Windows.Forms.TextBox txtTargetDirectory;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.Label lblGeneralInfo;
        private System.Windows.Forms.Label lblLogDirectories;
        private System.Windows.Forms.TextBox txtLogDirectories;
        private System.Windows.Forms.Button btnAdd;
    }
}
