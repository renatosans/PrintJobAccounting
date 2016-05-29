using System;
using System.Drawing;
using System.Windows.Forms;
using AccountingLib.Spool.EMF;


namespace SPLViewer
{
    public partial class PagePropertiesForm : Form
    {
        private Page currentPage;

        public PagePropertiesForm(Page currentPage)
        {
            this.currentPage = currentPage;
            InitializeComponent();
        }

        private void FilePropertiesForm_Shown(object sender, EventArgs e)
        {
            EMFPage emfPage = (EMFPage) currentPage.Contents;

            Rectangle boundingRect = emfPage.Header.Bounds;
            lblBoundsTop.Text = boundingRect.Top.ToString();
            lblBoundsLeft.Text = boundingRect.Left.ToString();
            lblBoundsWidth.Text = boundingRect.Width.ToString();
            lblBoundsHeight.Text = boundingRect.Height.ToString();

            Rectangle frameRect = emfPage.Header.Frame;
            lblFrameTop.Text = frameRect.Top.ToString();
            lblFrameLeft.Text = frameRect.Left.ToString();
            lblFrameWidth.Text = frameRect.Width.ToString();
            lblFrameHeight.Text = frameRect.Height.ToString();

            lblDescription.Text = emfPage.Header.Description;
            lblMetafileRecords.Text = emfPage.Header.RecordCount.ToString();
            lblMetafileSize.Text = emfPage.Header.FileSize.ToString();
            lblMilimeterDimensions.Text = String.Format("{0}", emfPage.Header.DeviceMilimeterDimensions);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
