using System;
using System.Windows.Forms;
using AccountingLib.Spool;


namespace SPLViewer
{
    public partial class FilePropertiesForm : Form
    {
        private JobShadowFile jobShadowFile;

        public FilePropertiesForm(JobShadowFile jobShadowFile)
        {
            this.jobShadowFile = jobShadowFile;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FilePropertiesForm_Shown(object sender, EventArgs e)
        {
            lblDocumentName.Text = jobShadowFile.DocumentName;
            lblSubmitted.Text = jobShadowFile.Submitted.ToString();
            lblDataType.Text = jobShadowFile.DataType;
            lblUser.Text = jobShadowFile.UserName;
            lblPaper.Text = jobShadowFile.DevMode.Paper;
            lblCopies.Text = jobShadowFile.DevMode.Copies.ToString();

            lblPrinterName.Text = jobShadowFile.PrinterName;
            lblDriver.Text = jobShadowFile.DriverName;
            lblProcessor.Text = jobShadowFile.PrintProcessor;
            lblPort.Text = jobShadowFile.Port;

            lblFormName.Text = jobShadowFile.DevMode.FormName;
            lblDeviceName.Text = jobShadowFile.DevMode.DeviceName;
            lblFileSize.Text = jobShadowFile.SpoolFileSize.ToString();
        }
    }

}
