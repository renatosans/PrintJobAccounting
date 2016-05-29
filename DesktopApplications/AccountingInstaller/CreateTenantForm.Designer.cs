namespace AccountingInstaller
{
    partial class CreateTenantForm
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
            this.lblTenantName = new System.Windows.Forms.Label();
            this.txtTenantName = new System.Windows.Forms.TextBox();
            this.lblDetails = new System.Windows.Forms.Label();
            this.lblGeneralInfo = new System.Windows.Forms.Label();
            this.lblTenantAlias = new System.Windows.Forms.Label();
            this.txtTenantAlias = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtProcessInfo = new System.Windows.Forms.TextBox();
            this.tenantGridView = new System.Windows.Forms.DataGridView();
            this.btnFinish = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tenantGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTenantName
            // 
            this.lblTenantName.AutoSize = true;
            this.lblTenantName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenantName.Location = new System.Drawing.Point(22, 85);
            this.lblTenantName.Name = "lblTenantName";
            this.lblTenantName.Size = new System.Drawing.Size(181, 16);
            this.lblTenantName.TabIndex = 8;
            this.lblTenantName.Text = "Identificador (sem espaços)";
            // 
            // txtTenantName
            // 
            this.txtTenantName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTenantName.Location = new System.Drawing.Point(209, 84);
            this.txtTenantName.Name = "txtTenantName";
            this.txtTenantName.Size = new System.Drawing.Size(372, 20);
            this.txtTenantName.TabIndex = 7;
            // 
            // lblDetails
            // 
            this.lblDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDetails.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetails.Location = new System.Drawing.Point(12, 118);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(579, 82);
            this.lblDetails.TabIndex = 6;
            this.lblDetails.Text = "O identificador não deve conter espaços e caracteres especiais (Apenas para uso i" +
                "nterno do sistema).  Para entrar com um nome amigável para a empresa utilize o c" +
                "ampo abaixo.";
            // 
            // lblGeneralInfo
            // 
            this.lblGeneralInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGeneralInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblGeneralInfo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGeneralInfo.Location = new System.Drawing.Point(12, 9);
            this.lblGeneralInfo.Name = "lblGeneralInfo";
            this.lblGeneralInfo.Size = new System.Drawing.Size(579, 62);
            this.lblGeneralInfo.TabIndex = 5;
            this.lblGeneralInfo.Text = "Preencha as informações da empresa para que o instalador cadastre a mesma no sist" +
                "ema.";
            // 
            // lblTenantAlias
            // 
            this.lblTenantAlias.AutoSize = true;
            this.lblTenantAlias.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenantAlias.Location = new System.Drawing.Point(22, 217);
            this.lblTenantAlias.Name = "lblTenantAlias";
            this.lblTenantAlias.Size = new System.Drawing.Size(108, 16);
            this.lblTenantAlias.TabIndex = 10;
            this.lblTenantAlias.Text = "Nome amigável";
            // 
            // txtTenantAlias
            // 
            this.txtTenantAlias.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTenantAlias.Location = new System.Drawing.Point(136, 216);
            this.txtTenantAlias.Name = "txtTenantAlias";
            this.txtTenantAlias.Size = new System.Drawing.Size(445, 20);
            this.txtTenantAlias.TabIndex = 9;
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCreate.Location = new System.Drawing.Point(243, 259);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(117, 33);
            this.btnCreate.TabIndex = 11;
            this.btnCreate.Text = "Criar";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.Location = new System.Drawing.Point(320, 435);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(117, 34);
            this.btnCancel.TabIndex = 13;
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
            this.txtProcessInfo.Location = new System.Drawing.Point(12, 484);
            this.txtProcessInfo.Multiline = true;
            this.txtProcessInfo.Name = "txtProcessInfo";
            this.txtProcessInfo.ReadOnly = true;
            this.txtProcessInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProcessInfo.Size = new System.Drawing.Size(579, 156);
            this.txtProcessInfo.TabIndex = 14;
            this.txtProcessInfo.Text = "Aguardando preenchimento dos dados...";
            // 
            // tenantGridView
            // 
            this.tenantGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tenantGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tenantGridView.Location = new System.Drawing.Point(12, 306);
            this.tenantGridView.Name = "tenantGridView";
            this.tenantGridView.ReadOnly = true;
            this.tenantGridView.Size = new System.Drawing.Size(579, 115);
            this.tenantGridView.TabIndex = 15;
            // 
            // btnFinish
            // 
            this.btnFinish.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnFinish.Location = new System.Drawing.Point(166, 436);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(117, 33);
            this.btnFinish.TabIndex = 16;
            this.btnFinish.Text = "Concluir";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // CreateTenantForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 652);
            this.ControlBox = false;
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.tenantGridView);
            this.Controls.Add(this.txtProcessInfo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.lblTenantAlias);
            this.Controls.Add(this.txtTenantAlias);
            this.Controls.Add(this.lblTenantName);
            this.Controls.Add(this.txtTenantName);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.lblGeneralInfo);
            this.Location = new System.Drawing.Point(10, 10);
            this.MinimumSize = new System.Drawing.Size(460, 480);
            this.Name = "CreateTenantForm";
            this.Text = "Criação de empresas";
            ((System.ComponentModel.ISupportInitialize)(this.tenantGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTenantName;
        private System.Windows.Forms.TextBox txtTenantName;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.Label lblGeneralInfo;
        private System.Windows.Forms.Label lblTenantAlias;
        private System.Windows.Forms.TextBox txtTenantAlias;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtProcessInfo;
        private System.Windows.Forms.DataGridView tenantGridView;
        private System.Windows.Forms.Button btnFinish;
    }
}
