namespace SPLViewer
{
    partial class MainForm
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
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileAction = new System.Windows.Forms.ToolStripMenuItem();
            this.openAction = new System.Windows.Forms.ToolStripMenuItem();
            this.filePropertiesAction = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitAction = new System.Windows.Forms.ToolStripMenuItem();
            this.pagesAction = new System.Windows.Forms.ToolStripMenuItem();
            this.firstPageAction = new System.Windows.Forms.ToolStripMenuItem();
            this.previousPageAction = new System.Windows.Forms.ToolStripMenuItem();
            this.nextPageAction = new System.Windows.Forms.ToolStripMenuItem();
            this.lastPageAction = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.goToAction = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.pagePropertiesAction = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAction = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomInAction = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomOutAction = new System.Windows.Forms.ToolStripMenuItem();
            this.picturePanel = new System.Windows.Forms.Panel();
            this.pagePicture = new System.Windows.Forms.PictureBox();
            this.MainMenu.SuspendLayout();
            this.picturePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pagePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileAction,
            this.pagesAction,
            this.viewAction});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(537, 24);
            this.MainMenu.TabIndex = 0;
            // 
            // fileAction
            // 
            this.fileAction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openAction,
            this.filePropertiesAction,
            this.menuSeparator1,
            this.exitAction});
            this.fileAction.Name = "fileAction";
            this.fileAction.Size = new System.Drawing.Size(35, 20);
            this.fileAction.Text = "&File";
            // 
            // openAction
            // 
            this.openAction.Name = "openAction";
            this.openAction.Size = new System.Drawing.Size(134, 22);
            this.openAction.Text = "Open";
            this.openAction.Click += new System.EventHandler(this.openAction_Click);
            // 
            // filePropertiesAction
            // 
            this.filePropertiesAction.Name = "filePropertiesAction";
            this.filePropertiesAction.Size = new System.Drawing.Size(134, 22);
            this.filePropertiesAction.Text = "Properties";
            this.filePropertiesAction.Click += new System.EventHandler(this.filePropertiesAction_Click);
            // 
            // menuSeparator1
            // 
            this.menuSeparator1.Name = "menuSeparator1";
            this.menuSeparator1.Size = new System.Drawing.Size(131, 6);
            // 
            // exitAction
            // 
            this.exitAction.Name = "exitAction";
            this.exitAction.Size = new System.Drawing.Size(134, 22);
            this.exitAction.Text = "Exit";
            this.exitAction.Click += new System.EventHandler(this.exitAction_Click);
            // 
            // pagesAction
            // 
            this.pagesAction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.firstPageAction,
            this.previousPageAction,
            this.nextPageAction,
            this.lastPageAction,
            this.menuSeparator2,
            this.goToAction,
            this.menuSeparator3,
            this.pagePropertiesAction});
            this.pagesAction.Name = "pagesAction";
            this.pagesAction.Size = new System.Drawing.Size(48, 20);
            this.pagesAction.Text = "&Pages";
            // 
            // firstPageAction
            // 
            this.firstPageAction.Name = "firstPageAction";
            this.firstPageAction.Size = new System.Drawing.Size(153, 22);
            this.firstPageAction.Text = "First Page";
            this.firstPageAction.Click += new System.EventHandler(this.firstPageAction_Click);
            // 
            // previousPageAction
            // 
            this.previousPageAction.Name = "previousPageAction";
            this.previousPageAction.Size = new System.Drawing.Size(153, 22);
            this.previousPageAction.Text = "Previous Page";
            this.previousPageAction.Click += new System.EventHandler(this.previousPageAction_Click);
            // 
            // nextPageAction
            // 
            this.nextPageAction.Name = "nextPageAction";
            this.nextPageAction.Size = new System.Drawing.Size(153, 22);
            this.nextPageAction.Text = "Next Page";
            this.nextPageAction.Click += new System.EventHandler(this.nextPageAction_Click);
            // 
            // lastPageAction
            // 
            this.lastPageAction.Name = "lastPageAction";
            this.lastPageAction.Size = new System.Drawing.Size(153, 22);
            this.lastPageAction.Text = "Last Page";
            this.lastPageAction.Click += new System.EventHandler(this.lastPageAction_Click);
            // 
            // menuSeparator2
            // 
            this.menuSeparator2.Name = "menuSeparator2";
            this.menuSeparator2.Size = new System.Drawing.Size(150, 6);
            // 
            // goToAction
            // 
            this.goToAction.Name = "goToAction";
            this.goToAction.Size = new System.Drawing.Size(153, 22);
            this.goToAction.Text = "Go To";
            // 
            // menuSeparator3
            // 
            this.menuSeparator3.Name = "menuSeparator3";
            this.menuSeparator3.Size = new System.Drawing.Size(150, 6);
            // 
            // pagePropertiesAction
            // 
            this.pagePropertiesAction.Name = "pagePropertiesAction";
            this.pagePropertiesAction.Size = new System.Drawing.Size(153, 22);
            this.pagePropertiesAction.Text = "Properties";
            this.pagePropertiesAction.Click += new System.EventHandler(this.pagePropertiesAction_Click);
            // 
            // viewAction
            // 
            this.viewAction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInAction,
            this.zoomOutAction});
            this.viewAction.Name = "viewAction";
            this.viewAction.Size = new System.Drawing.Size(41, 20);
            this.viewAction.Text = "&View";
            // 
            // zoomInAction
            // 
            this.zoomInAction.Name = "zoomInAction";
            this.zoomInAction.Size = new System.Drawing.Size(132, 22);
            this.zoomInAction.Text = "Zoom In";
            this.zoomInAction.Click += new System.EventHandler(this.zoomInAction_Click);
            // 
            // zoomOutAction
            // 
            this.zoomOutAction.Name = "zoomOutAction";
            this.zoomOutAction.Size = new System.Drawing.Size(132, 22);
            this.zoomOutAction.Text = "Zoom Out";
            this.zoomOutAction.Click += new System.EventHandler(this.zoomOutAction_Click);
            // 
            // picturePanel
            // 
            this.picturePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picturePanel.AutoScroll = true;
            this.picturePanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.picturePanel.Controls.Add(this.pagePicture);
            this.picturePanel.Location = new System.Drawing.Point(0, 24);
            this.picturePanel.Name = "picturePanel";
            this.picturePanel.Size = new System.Drawing.Size(537, 296);
            this.picturePanel.TabIndex = 2;
            // 
            // pagePicture
            // 
            this.pagePicture.BackColor = System.Drawing.SystemColors.Window;
            this.pagePicture.Location = new System.Drawing.Point(193, 48);
            this.pagePicture.Name = "pagePicture";
            this.pagePicture.Size = new System.Drawing.Size(150, 200);
            this.pagePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pagePicture.TabIndex = 0;
            this.pagePicture.TabStop = false;
            this.pagePicture.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 320);
            this.Controls.Add(this.picturePanel);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "MainForm";
            this.Text = "SPL Viewer";
            this.Resize += new System.EventHandler(this.SPLViewer_Resize);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.picturePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pagePicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileAction;
        private System.Windows.Forms.ToolStripMenuItem pagesAction;
        private System.Windows.Forms.ToolStripMenuItem openAction;
        private System.Windows.Forms.ToolStripMenuItem filePropertiesAction;
        private System.Windows.Forms.ToolStripSeparator menuSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitAction;
        private System.Windows.Forms.ToolStripMenuItem firstPageAction;
        private System.Windows.Forms.ToolStripMenuItem lastPageAction;
        private System.Windows.Forms.ToolStripMenuItem previousPageAction;
        private System.Windows.Forms.ToolStripMenuItem viewAction;
        private System.Windows.Forms.ToolStripMenuItem zoomInAction;
        private System.Windows.Forms.ToolStripMenuItem zoomOutAction;
        private System.Windows.Forms.ToolStripMenuItem nextPageAction;
        private System.Windows.Forms.ToolStripSeparator menuSeparator2;
        private System.Windows.Forms.ToolStripMenuItem goToAction;
        private System.Windows.Forms.ToolStripMenuItem pagePropertiesAction;
        private System.Windows.Forms.ToolStripSeparator menuSeparator3;
        private System.Windows.Forms.Panel picturePanel;
        private System.Windows.Forms.PictureBox pagePicture;
    }
}
