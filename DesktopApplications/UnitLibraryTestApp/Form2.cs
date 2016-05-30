using System;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AccountingLib.Printers;
using DocMageFramework.AppUtils;
using DocMageFramework.CustomAttributes;
using org.in2bits.MyXls;


namespace UnitLibraryTestApp
{
    public partial class Form2 : Form, IListener
    {
        private List<Object> notifications;

        public Form2()
        {
            InitializeComponent();
            notifications = new List<Object>();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            lblTime.Text = currentTime.ToShortTimeString() + "   " + currentTime.Second.ToString() + "s";
        }


        [DllImport("winspool.drv", EntryPoint = "SetJob")]
        static extern int SetJobA(IntPtr hPrinter, int JobId, int Level, ref byte pJob, int Command_Renamed);

        // Win32_Printer
        // Win32_PrinterConfiguration
        // Win32_PrinterController
        // Win32_PrinterDriver
        // Win32_PrinterDriverDll
        // Win32_PrinterSetting
        // Win32_PrinterShare
        // Win32_PrintJob

        private void button1_Click(object sender, EventArgs e)
        {
            infoBox.Clear();

            DeviceHandler.PreparePrinters(this);
            List<SysPrinter> sysPrinters = DeviceHandler.MapSystemPrinters(Environment.MachineName);
            foreach (SysPrinter printer in sysPrinters)
            {
                infoBox.Text += printer.Name + "   " + printer.Port + "   " + printer.ComputerName + " BIDI=" + printer.EnableBIDI + Environment.NewLine;
                foreach (PrinterCapabilityEnum capability in printer.Capabilities)
                {
                    String capabilityName = AssociatedText.GetFieldDescription(typeof(PrinterCapabilityEnum), capability.ToString());
                    infoBox.Text += capabilityName + Environment.NewLine;
                }
                infoBox.Text += Environment.NewLine + Environment.NewLine;
            }

            infoBox.Text += "Total de impressoras (sistema de accounting)  = " + sysPrinters.Count.ToString();
        }

        private String IndentXMLString(TextReader textReader)
        {
            XmlDocument document = new XmlDocument();
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlWriter = new XmlTextWriter(memoryStream, System.Text.Encoding.Unicode);
            StreamReader reader = new StreamReader(memoryStream);
            try
            {
                xmlWriter.Formatting = Formatting.Indented;
                document.Load(textReader);
                document.WriteContentTo(xmlWriter);
                xmlWriter.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);
                return reader.ReadToEnd();
            }
            catch
            {
                return String.Empty;
            }
            finally
            {
                reader.Close();
                xmlWriter.Close();
                memoryStream.Close();
            }
        }

        private void CopyStream(Stream source, Stream destination)
        {
            Byte[] buffer = new Byte[32768];
            int bytesRead;
            do
            {
                bytesRead = source.Read(buffer, 0, buffer.Length);
                destination.Write(buffer, 0, bytesRead);
            } while (bytesRead != 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = @"C:\Work\subversion_documentation\B1SDK_Samples\VideoStore\VidsSamp";
            fileDialog.Filter = "SBO Form (*.srf)|*.srf|Todos os arquivos (*.*)|*.*";
            // Exibe a caixa de seleção de arquivo e verifica o resultado da interação do usuário
            DialogResult openFileResult = fileDialog.ShowDialog();
            if (openFileResult == DialogResult.Cancel) return;
            String filename = fileDialog.FileName;
            if (!File.Exists(filename)) return;

            TextReader textReader = new StreamReader(filename);
            String formXml = IndentXMLString(textReader);
            
            String outputfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "_" + Path.GetFileName(filename));
            TextWriter textWriter = File.CreateText(outputfile);
            textWriter.Write(formXml);
            textWriter.Flush();

            MessageBox.Show("Conversão finalizada.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Define o destino de gravação do recurso embarcado
            String desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            String resourceName = "ServerFiles.zip";
            String destinationFile = Path.Combine(desktopPath, resourceName);

            // Extrai o recurso embarcado do executável
            Assembly targetExe = Assembly.LoadFile(@"C:\work\subversion_jobAccounting\Build\ServerSetup.exe");
            Stream zipStream = targetExe.GetManifestResourceStream(resourceName);

            // Grava em disco
            FileStream zipFile = new FileStream(destinationFile, FileMode.Create);
            CopyStream(zipStream, zipFile);
            zipFile.Flush();
            zipFile.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // not implemented yet
            // XlsDocument doc = new org.in2bits.MyXls.XlsDocument();
        }

        public void NotifyObject(Object obj)
        {
            if (obj is Exception)
            {
                Exception exception = (Exception) obj;
                // MessageBox.Show(exception.Message);
                return;
            }

            if (obj is String)
            {
                String traceMessage = (String)obj;
                // MessageBox.Show(traceMessage);
                return;
            }

            // Caso seja outro tipo de notificação
            notifications.Add(obj);
        }
    }

}
