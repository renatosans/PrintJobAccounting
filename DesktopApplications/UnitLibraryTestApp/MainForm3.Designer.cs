namespace UnitLibraryTestApp
{
    partial class MainForm3
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
            this.label3 = new System.Windows.Forms.Label();
            this.txtDBPass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDBUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDBServer = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnGetTenants = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.expirationDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTenantId = new System.Windows.Forms.TextBox();
            this.infoBox = new System.Windows.Forms.RichTextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtServiceUrl = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(31, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "DBPass";
            // 
            // txtDBPass
            // 
            this.txtDBPass.Location = new System.Drawing.Point(98, 117);
            this.txtDBPass.Name = "txtDBPass";
            this.txtDBPass.PasswordChar = '*';
            this.txtDBPass.Size = new System.Drawing.Size(166, 20);
            this.txtDBPass.TabIndex = 14;
            this.txtDBPass.Text = "dt";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = "DBServer";
            // 
            // txtDBUser
            // 
            this.txtDBUser.Location = new System.Drawing.Point(98, 91);
            this.txtDBUser.Name = "txtDBUser";
            this.txtDBUser.Size = new System.Drawing.Size(166, 20);
            this.txtDBUser.TabIndex = 12;
            this.txtDBUser.Text = "sa";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(31, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "DBUser";
            // 
            // txtDBServer
            // 
            this.txtDBServer.Location = new System.Drawing.Point(98, 65);
            this.txtDBServer.Name = "txtDBServer";
            this.txtDBServer.Size = new System.Drawing.Size(166, 20);
            this.txtDBServer.TabIndex = 10;
            this.txtDBServer.Text = "SLREUNIAO\\SQLEXPRESS";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(412, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(359, 197);
            this.dataGridView1.TabIndex = 9;
            // 
            // btnGetTenants
            // 
            this.btnGetTenants.Location = new System.Drawing.Point(279, 84);
            this.btnGetTenants.Name = "btnGetTenants";
            this.btnGetTenants.Size = new System.Drawing.Size(118, 33);
            this.btnGetTenants.TabIndex = 8;
            this.btnGetTenants.Text = "Verificar Empresas";
            this.btnGetTenants.UseVisualStyleBackColor = true;
            this.btnGetTenants.Click += new System.EventHandler(this.btnGetTenants_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.expirationDatePicker);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtTenantId);
            this.groupBox1.Controls.Add(this.infoBox);
            this.groupBox1.Controls.Add(this.btnGenerate);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtServiceUrl);
            this.groupBox1.Location = new System.Drawing.Point(27, 229);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(744, 340);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Geração de Chaves";
            // 
            // expirationDatePicker
            // 
            this.expirationDatePicker.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.expirationDatePicker.Location = new System.Drawing.Point(291, 85);
            this.expirationDatePicker.Name = "expirationDatePicker";
            this.expirationDatePicker.Size = new System.Drawing.Size(351, 20);
            this.expirationDatePicker.TabIndex = 24;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(124, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 16);
            this.label6.TabIndex = 23;
            this.label6.Text = "Data de Expiração";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(137, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 16);
            this.label5.TabIndex = 21;
            this.label5.Text = "ID da Empresa";
            // 
            // txtTenantId
            // 
            this.txtTenantId.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtTenantId.Location = new System.Drawing.Point(291, 58);
            this.txtTenantId.Name = "txtTenantId";
            this.txtTenantId.Size = new System.Drawing.Size(351, 20);
            this.txtTenantId.TabIndex = 20;
            this.txtTenantId.Text = "2";
            // 
            // infoBox
            // 
            this.infoBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.infoBox.Location = new System.Drawing.Point(20, 193);
            this.infoBox.Name = "infoBox";
            this.infoBox.Size = new System.Drawing.Size(703, 132);
            this.infoBox.TabIndex = 19;
            this.infoBox.Text = "";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnGenerate.Location = new System.Drawing.Point(307, 145);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(130, 33);
            this.btnGenerate.TabIndex = 18;
            this.btnGenerate.Text = "Gerar Chave";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(101, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(172, 16);
            this.label4.TabIndex = 17;
            this.label4.Text = "Url do Servidor Datacount";
            // 
            // txtServiceUrl
            // 
            this.txtServiceUrl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtServiceUrl.Location = new System.Drawing.Point(291, 32);
            this.txtServiceUrl.Name = "txtServiceUrl";
            this.txtServiceUrl.Size = new System.Drawing.Size(351, 20);
            this.txtServiceUrl.TabIndex = 16;
            this.txtServiceUrl.Text = "http://187.45.232.244/Datacount/JobRoutingService.aspx";
            // 
            // MainForm3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 581);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDBPass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDBUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDBServer);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnGetTenants);
            this.Name = "MainForm3";
            this.Text = "Gerador de Chave de Ativação";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDBPass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDBUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDBServer;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnGetTenants;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtServiceUrl;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.RichTextBox infoBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTenantId;
        private System.Windows.Forms.DateTimePicker expirationDatePicker;
        private System.Windows.Forms.Label label6;
    }
}