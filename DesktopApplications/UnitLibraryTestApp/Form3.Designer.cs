namespace UnitLibraryTestApp
{
    partial class Form3
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
            this.btnGetParam = new System.Windows.Forms.Button();
            this.btnGetAllParams = new System.Windows.Forms.Button();
            this.btnUpdateParam = new System.Windows.Forms.Button();
            this.btnGetTaskParams = new System.Windows.Forms.Button();
            this.infoBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnGetParam
            // 
            this.btnGetParam.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnGetParam.Location = new System.Drawing.Point(221, 22);
            this.btnGetParam.Name = "btnGetParam";
            this.btnGetParam.Size = new System.Drawing.Size(117, 36);
            this.btnGetParam.TabIndex = 0;
            this.btnGetParam.Text = "Get Param";
            this.btnGetParam.UseVisualStyleBackColor = true;
            this.btnGetParam.Click += new System.EventHandler(this.btnGetParam_Click);
            // 
            // btnGetAllParams
            // 
            this.btnGetAllParams.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnGetAllParams.Location = new System.Drawing.Point(221, 64);
            this.btnGetAllParams.Name = "btnGetAllParams";
            this.btnGetAllParams.Size = new System.Drawing.Size(117, 36);
            this.btnGetAllParams.TabIndex = 1;
            this.btnGetAllParams.Text = "Get All Params";
            this.btnGetAllParams.UseVisualStyleBackColor = true;
            this.btnGetAllParams.Click += new System.EventHandler(this.btnGetAllParams_Click);
            // 
            // btnUpdateParam
            // 
            this.btnUpdateParam.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnUpdateParam.Location = new System.Drawing.Point(221, 148);
            this.btnUpdateParam.Name = "btnUpdateParam";
            this.btnUpdateParam.Size = new System.Drawing.Size(117, 36);
            this.btnUpdateParam.TabIndex = 2;
            this.btnUpdateParam.Text = "Update Param";
            this.btnUpdateParam.UseVisualStyleBackColor = true;
            this.btnUpdateParam.Click += new System.EventHandler(this.btnUpdateParam_Click);
            // 
            // btnGetTaskParams
            // 
            this.btnGetTaskParams.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnGetTaskParams.Location = new System.Drawing.Point(221, 106);
            this.btnGetTaskParams.Name = "btnGetTaskParams";
            this.btnGetTaskParams.Size = new System.Drawing.Size(117, 36);
            this.btnGetTaskParams.TabIndex = 3;
            this.btnGetTaskParams.Text = "Get Task Params";
            this.btnGetTaskParams.UseVisualStyleBackColor = true;
            this.btnGetTaskParams.Click += new System.EventHandler(this.btnGetTaskParams_Click);
            // 
            // infoBox
            // 
            this.infoBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.infoBox.Location = new System.Drawing.Point(12, 202);
            this.infoBox.Name = "infoBox";
            this.infoBox.Size = new System.Drawing.Size(535, 175);
            this.infoBox.TabIndex = 4;
            this.infoBox.Text = "";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 389);
            this.Controls.Add(this.infoBox);
            this.Controls.Add(this.btnGetTaskParams);
            this.Controls.Add(this.btnUpdateParam);
            this.Controls.Add(this.btnGetAllParams);
            this.Controls.Add(this.btnGetParam);
            this.Name = "Form3";
            this.Text = "Form3";
            this.Shown += new System.EventHandler(this.Form3_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form3_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGetParam;
        private System.Windows.Forms.Button btnGetAllParams;
        private System.Windows.Forms.Button btnUpdateParam;
        private System.Windows.Forms.Button btnGetTaskParams;
        private System.Windows.Forms.RichTextBox infoBox;
    }
}