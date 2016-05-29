using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using AccountingLib.Spool;
using AccountingLib.Spool.EMF;
using DocMageFramework.AppUtils;


namespace SPLViewer
{
    public partial class MainForm : Form, IListener
    {
        public MainForm(String filename)
        {
            this.notifications = new List<Object>();
            InitializeComponent();
            RefreshMenus();

            if (!String.IsNullOrEmpty(filename))
                OpenSpoolFile(filename);
        }

        private List<Object> notifications;
        private SpooledJob spooledJob;
        private Page currentPage;
        private float scale;


        private void RefreshMenus()
        {
            fileAction.Enabled = true;
            filePropertiesAction.Enabled = spooledJob != null;
            pagesAction.Enabled = spooledJob != null;
            viewAction.Enabled = spooledJob != null;

            if (currentPage != null)
            {
                firstPageAction.Enabled = currentPage.PageNumber > 1;
                previousPageAction.Enabled = currentPage.PageNumber > 1;
                nextPageAction.Enabled = currentPage.PageNumber < spooledJob.SpoolFile.Pages.Count;
                lastPageAction.Enabled = currentPage.PageNumber < spooledJob.SpoolFile.Pages.Count;
            }

            zoomInAction.Enabled = scale < 1.2f;
            zoomOutAction.Enabled = scale > 0.2f;
        }

        private void ArrangePage()
        {
            if (picturePanel.HorizontalScroll.Visible)
                pagePicture.Left = 0;
            else
                pagePicture.Left = (int)(picturePanel.Width / 2 - pagePicture.Width / 2);

            if (picturePanel.VerticalScroll.Visible)
                pagePicture.Top = 0;
            else
                pagePicture.Top = (int) (picturePanel.Height / 2 - pagePicture.Height / 2);
        }

        private void LoadPage(int pageNumber)
        {
            currentPage = new Page(pageNumber, spooledJob.SpoolFile.Pages[pageNumber - 1]);
            EMFPage pageContent = (EMFPage)currentPage.Contents;
            pagePicture.Width  = (int)(pageContent.Header.Bounds.Width * scale);
            pagePicture.Height = (int)(pageContent.Header.Bounds.Height * scale);
            pagePicture.Image = pageContent.PageImage;
            RefreshMenus();
            ArrangePage();
        }

        private void SPLViewer_Resize(object sender, EventArgs e)
        {
            ArrangePage();
        }

        private void openAction_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Spool files|*.spl";

            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            OpenSpoolFile(openFileDialog.FileName);
        }

        private void OpenSpoolFile(String spoolFilename)
        {
            String shadowFilename = Path.ChangeExtension(spoolFilename, ".SHD");
            if (!File.Exists(shadowFilename))
            {
                MessageBox.Show("Arquivo de shadow não encontrado.");
                return;
            }

            spooledJob = new SpooledJob(shadowFilename, this);
            if (spooledJob.ShadowFile == null)
            {
                MessageBox.Show("Não foi possível abrir o arquivo de shadow.");
                return;
            }

            if (!spooledJob.ShadowFile.DataType.ToUpper().Contains("EMF"))
            {
                MessageBox.Show("Formato de arquivo não suportado (RAW data in PCL5e, PCL XL, PostScript, etc).");
                return;
            }

            EMFSpoolFile spoolFile = (EMFSpoolFile) spooledJob.SpoolFile;
            if (spoolFile.MalformedFile)
            {
                MessageBox.Show("O arquivo de spool não é um arquivo EMF válido.");
                return;
            }

            goToAction.DropDownItems.Clear();
            for (int index = 1; index <= spooledJob.SpoolFile.Pages.Count; index++)
            {
                goToAction.DropDownItems.Add(index.ToString(), null, pageNumber_Click);
            }

            pagePicture.Visible = true;
            scale = 0.4f; // Inicia em 40% do tamanho original
            LoadPage(1);
        }

        private void filePropertiesAction_Click(object sender, EventArgs e)
        {
            FilePropertiesForm filePropertiesForm = new FilePropertiesForm(spooledJob.ShadowFile);
            this.AddOwnedForm(filePropertiesForm);
            filePropertiesForm.StartPosition = FormStartPosition.CenterParent;

            filePropertiesForm.ShowDialog();
        }

        private void firstPageAction_Click(object sender, EventArgs e)
        {
            // carrega a primeira página
            LoadPage(1);
        }

        private void previousPageAction_Click(object sender, EventArgs e)
        {
            // muda para a página anterior
            int pageNum = currentPage.PageNumber - 1;

            LoadPage(pageNum);
        }

        private void nextPageAction_Click(object sender, EventArgs e)
        {
            // muda para a próxima página
            int pageNum = currentPage.PageNumber + 1;

            LoadPage(pageNum);
        }

        private void lastPageAction_Click(object sender, EventArgs e)
        {
            // carrega a última página
            LoadPage(spooledJob.SpoolFile.Pages.Count);
        }

        private void pageNumber_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripItem)
            {
                ToolStripItem menuItem = (ToolStripItem)sender;
                int pageNum = 0;
                int.TryParse(menuItem.Text, out pageNum);

                if (pageNum > 0)
                {
                    LoadPage(pageNum);
                }
            }
        }

        private void pagePropertiesAction_Click(object sender, EventArgs e)
        {
            PagePropertiesForm pagePropertiesForm = new PagePropertiesForm(currentPage);
            this.AddOwnedForm(pagePropertiesForm);
            pagePropertiesForm.StartPosition = FormStartPosition.CenterParent;

            pagePropertiesForm.ShowDialog();
        }

        private void zoomInAction_Click(object sender, EventArgs e)
        {
            scale += 0.2f;
            if (scale > 1.2f)
            {
                scale = 1.2f; // 120% - tamanho máximo
            }
            LoadPage(currentPage.PageNumber);
        }

        private void zoomOutAction_Click(object sender, EventArgs e)
        {
            scale -= 0.2f;
            if (scale < 0.2f)
            {
                scale = 0.2f; // 20% - tamanho mínimo
            }
            LoadPage(currentPage.PageNumber);
        }

        private void exitAction_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void NotifyObject(Object obj)
        {
            notifications.Add(obj);
        }
    }

}
