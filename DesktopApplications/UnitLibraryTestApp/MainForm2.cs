using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using AccountingLib.Spool;
using AccountingLib.Printers;
using AccountingLib.PrintInspect;
using DocMageFramework.AppUtils;


namespace UnitLibraryTestApp
{
    public partial class MainForm2 : Form, IListener
    {
        private SpoolMonitor spoolMonitor;

        public MainForm2()
        {
            InitializeComponent();
            this.Show();
            this.Refresh();
            this.spoolMonitor = new SpoolMonitor(this);
        }

        private delegate void PerformTextOutputDelegate(String text);

        private void LogJobInfo(String text)
        {
            jobInfoBox.Text += text + Environment.NewLine;
        }

        private void LogTraceInfo(String text)
        {
            traceInfoBox.Text += text + Environment.NewLine;
        }

        public void NotifyObject(Object obj)
        {
            if (obj is JobNotification)
            {

                JobNotification jobNotification = (JobNotification)obj;
                ProcessJobNotification(jobNotification);
                return;
            }

            if (obj is String)
            {
                String traceInfo = (String) obj;
                traceInfoBox.Invoke(new PerformTextOutputDelegate(LogTraceInfo), traceInfo);
                return;
            }
        }

        private void ProcessJobNotification(JobNotification jobNotification)
        {
            JobNotificationTypeEnum notificationType = jobNotification.NotificationType;
            String jobName = jobNotification.JobName;

            SpooledJob spooledJob = spoolMonitor.FindSpooledJob(jobName);
            ManagedPrintJob managedJob = null;
            if (spooledJob != null) managedJob = new ManagedPrintJob(jobName);

            if (notificationType == JobNotificationTypeEnum.JobCreated)
            {
                //if (!managedJob.IsPaused())
                //    managedJob.Pause();
            }
            
            if (notificationType == JobNotificationTypeEnum.JobChanged)
            {
                // Verifica se terminou o spooling para então processar os dados do job
                if (!managedJob.IsSpooling() && !spooledJob.Processed)
                {
                    // spooledJob.CopyFiles(@"C:\tempSpool\" + jobName.Split(new char[] { ',' })[0]);
                    String jobInfo = GetJobInfo(spooledJob, managedJob);
                    jobInfoBox.Invoke(new PerformTextOutputDelegate(LogJobInfo), jobInfo);
                    spooledJob.Processed = true;
                }
            }
        }

        private String GetJobInfo(SpooledJob spooledJob, ManagedPrintJob managedJob)
        {
            Dictionary<String, Object> jobSummary = PrintJobContext.GetJobSummary(spooledJob);
            if (jobSummary == null) return String.Empty;

            String jobInfo = "Print Job (" + managedJob.Name + " )   " +
                          "Status: " + (JobStatusEnum)managedJob.StatusMask + Environment.NewLine +
                          "Hora: " + DateFormat.Adjust((DateTime)jobSummary["jobTime"], true) + Environment.NewLine +
                          "UserName: " + jobSummary["userName"] + Environment.NewLine +
                          "PrinterName: " + jobSummary["printerName"] + Environment.NewLine +
                          "DocumentName: " + '\"' + jobSummary["documentName"] + '\"' + Environment.NewLine +
                          "Page Count: " + jobSummary["pageCount"] + Environment.NewLine +
                          "Copy Count: " + jobSummary["copyCount"] + Environment.NewLine +
                          "Duplex: " + jobSummary["duplex"] + Environment.NewLine +
                          "Color: " + jobSummary["color"] + Environment.NewLine +
                          "File size: " + jobSummary["spoolFileSize"] + Environment.NewLine;

            return jobInfo;
        }

        private void MenuItem1Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = DeviceHandler.GetSpoolDirectory();
            fileDialog.Filter = "Spool Shadow File (*.shd)|*.shd|Todos os arquivos (*.*)|*.*";
            // Exibe a caixa de seleção de arquivo e verifica o resultado da interação do usuário
            DialogResult openFileResult = fileDialog.ShowDialog();
            if (openFileResult == DialogResult.Cancel) return;
            
            String shadowFilename = fileDialog.FileName;
            if (File.Exists(shadowFilename))
            {
                SpooledJob spooledJob = new SpooledJob(shadowFilename, this);
                String jobName = spooledJob.ShadowFile.PrinterName + ", " + spooledJob.ShadowFile.JobId.ToString();
                ManagedPrintJob managedJob = new ManagedPrintJob(jobName);
                String jobInfo = GetJobInfo(spooledJob, managedJob);
                jobInfoBox.Invoke(new PerformTextOutputDelegate(LogJobInfo), jobInfo);
            }
        }

        private void MenuItem2Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
