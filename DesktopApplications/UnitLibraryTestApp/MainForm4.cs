using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using AccountingLib.Entities;
using AccountingLib.ServerPrintLog;
using DocMageFramework.Reporting;
using DocMageFramework.AppUtils;


namespace UnitLibraryTestApp
{
    public partial class MainForm4 : Form
    {
        public MainForm4()
        {
            InitializeComponent();
        }

        private SqlConnection sqlConnection;

        private Boolean OpenConnection()
        {
            try
            {
                sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = @"Data Source=" + txtDBServer.Text + ";User=" + txtDBUser.Text + "; password=" + txtDBPass.Text;
                sqlConnection.Open();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return false;
            }

            // Se não houve nenhuma falha retorna status de sucesso
            return true;
        }

        private void CloseConnection()
        {
            sqlConnection.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!OpenConnection()) return;

            DateTime? fileDate = PrintLogFile.GetDate(txtFileToImport.Text);
            if (fileDate == null)
            {
                MessageBox.Show("Arquivo inválido");
                return;
            }

            DateTime startDate = fileDate.Value;
            DateTime endDate = startDate.Add(new TimeSpan(23, 59, 59));
            DateRange dateRange = new DateRange(true);
            dateRange.SetRange(startDate, endDate);

            DBQuery query = new DBQuery(sqlConnection);
            query.Query = "use Accounting";
            query.Execute(false);
            query.Query = "SELECT" + Environment.NewLine +
                          "    PRN_LOG.id jobId," + Environment.NewLine +
                          "    PRN_LOG.tenantId," + Environment.NewLine +
                          "    PRN_LOG.jobTime," + Environment.NewLine +
                          "    USR.alias userName," + Environment.NewLine +
                          "    PRN.alias printerName," + Environment.NewLine +
                          "    PRN_LOG.documentName name," + Environment.NewLine +
                          "    PRN_LOG.pageCount," + Environment.NewLine +
                          "    PRN_LOG.copyCount," + Environment.NewLine +
                          "    PRN_LOG.duplex," + Environment.NewLine +
                          "    PRN_LOG.color" + Environment.NewLine +
                          "FROM" + Environment.NewLine +
                          "    tb_printLog PRN_LOG" + Environment.NewLine +
                          "    INNER JOIN tb_printer PRN WITH (NOLOCK)" + Environment.NewLine +
                          "        ON PRN_LOG.printerId = PRN.id" + Environment.NewLine +
                          "    INNER JOIN tb_user USR WITH (NOLOCK)" + Environment.NewLine +
                          "        ON PRN_LOG.userId = USR.id";
            query.Execute(true);

            DataTable printLog = query.ExtractFromResultset(typeof(PrintedDocument), "tb_ptintLog");
            //dataGridView1.DataSource = printLog.DefaultView;

            if (printLog.Rows.Count > 0)
            {
                //MessageBox.Show("Erro. Já existem registros correspondentes a data do arquivo!");
                //return;
            }

            Boolean imported = ImportFile(txtFileToImport.Text);

            CloseConnection();

            if (imported) MessageBox.Show("Arquivo importado com sucesso.");
        }

        /// <summary>
        /// Importa os registros do arquivo de log(.CSV) e insere no banco de dados
        /// </summary>
        public Boolean ImportFile(String fileName)
        {
            CSVReader reader = new CSVReader(fileName, null);
            DataTable printedDocumentTable = reader.Read();
            int rowCount = printedDocumentTable.Rows.Count;

            // Verifica se existem registros no CSV
            if (rowCount < 1)
            {
                MessageBox.Show("CSV inválido. Nenhum registro encontrado.");
                return false;
            }

            PrintedDocument printedDocument;
            foreach (DataRow row in printedDocumentTable.Rows)
            {
                printedDocument = new PrintedDocument();
                printedDocument.tenantId = 1;
                printedDocument.jobTime = DateTime.Parse(row["Time"].ToString());
                printedDocument.userName = row["User"].ToString();
                printedDocument.printerName = row["Printer"].ToString();
                printedDocument.name = row["Document Name"].ToString();
                printedDocument.pageCount = int.Parse(row["Pages"].ToString());
                printedDocument.copyCount = int.Parse(row["Copies"].ToString());
                printedDocument.duplex = ConvertToBool(row["Duplex"].ToString());
                printedDocument.color = !ConvertToBool(row["Grayscale"].ToString());
                
                DBQuery query = new DBQuery(sqlConnection);
                query.Query = "DECLARE @pageCount INT" + Environment.NewLine +
                              "SET @pageCount = " + printedDocument.pageCount + Environment.NewLine +
                              "DECLARE @copyCount INT" + Environment.NewLine +
                              "SET @copyCount = " + printedDocument.copyCount + Environment.NewLine +
                              "DECLARE @duplex BIT" + Environment.NewLine +
                              "SET @duplex = " + ConvertToBit(printedDocument.duplex) + Environment.NewLine +
                              "DECLARE @color BIT" + Environment.NewLine +
                              "SET @color = " + ConvertToBit(printedDocument.color) + Environment.NewLine +

                              "-- Executa procedimento para garantir a existência do usuário no banco" + Environment.NewLine +
                              "IF NOT EXISTS(SELECT 1 FROM tb_user WHERE name = '" + printedDocument.userName + "')" + Environment.NewLine +
                              "BEGIN" + Environment.NewLine +
                              "    INSERT INTO tb_user(tenantId, name, alias) VALUES (1, '" + printedDocument.userName +"', '" + printedDocument.userName + "')" + Environment.NewLine +
                              "END" + Environment.NewLine +
                              "-- Recupera os dados do usuário" + Environment.NewLine +
                              "DECLARE @userId INT" + Environment.NewLine +
                              "SELECT @userId = id" + Environment.NewLine +
                              "FROM tb_user" + Environment.NewLine +
                              "WHERE name = '" + printedDocument.userName + "'" + Environment.NewLine +

                              "-- Executa procedimento para garantir a existência da impresora no banco" + Environment.NewLine +
                              "IF NOT EXISTS(SELECT 1 FROM tb_printer WHERE name = '" + printedDocument.printerName + "')" + Environment.NewLine +
                              "BEGIN" + Environment.NewLine +
                              "    INSERT INTO tb_printer(tenantId, name, alias) VALUES (1, '" + printedDocument.printerName + "', '" + printedDocument.printerName + "')" + Environment.NewLine +
                              "END" + Environment.NewLine +
                              "-- Recupera os dados da impressora" + Environment.NewLine +
                              "DECLARE @printerId     INT" + Environment.NewLine +
                              "DECLARE @pageCost      MONEY" + Environment.NewLine +
                              "DECLARE @colorCostDiff MONEY" + Environment.NewLine +
                              "DECLARE @bwPrinter     BIT" + Environment.NewLine +
                              "SELECT @printerId = id, @pageCost = pageCost, @colorCostDiff = colorCostDiff, @bwPrinter = bwPrinter" + Environment.NewLine +
                              "FROM tb_printer" + Environment.NewLine +
                              "WHERE name = '" + printedDocument.printerName + "'" + Environment.NewLine +

                              "IF (@bwPrinter = 1) -- caso a impressora esteja definida como Monocromática define a impressão como Pb" + Environment.NewLine +
                              "BEGIN" + Environment.NewLine +
                              "    SET @color = 0" + Environment.NewLine +
                              "END" + Environment.NewLine +

                              "DECLARE @jobCost MONEY" + Environment.NewLine +
                              "SET @jobCost = (@pageCost + (@colorCostDiff * @color) ) * @pageCount * @copyCount" + Environment.NewLine +


                              "INSERT INTO" + Environment.NewLine +
                              "    tb_printLog(tenantId, jobTime, userId, printerId, documentName, pageCount, copyCount, duplex, color, jobCost)" + Environment.NewLine +
                              "VALUES" + Environment.NewLine +
                              "    (1, '" + printedDocument.jobTime.ToString("yyyy-MM-dd hh:mm:ss") + "', @userId, @printerId, '" + printedDocument.name + "', @pageCount, @copyCount, @duplex, @color, @jobCost)";
                query.Execute(false);
            }

            return true;
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == DialogResult.Cancel) return;

            txtFileToImport.Text = openFileDialog.FileName;
        }

        private Boolean ConvertToBool(String flag)
        {
            Boolean result = true;

            if (flag.Contains("NOT"))
                result = false;

            return result;
        }

        private String ConvertToBit(Boolean flag)
        {
            String bit = "0";

            if (flag) bit = "1";

            return bit;
        }
    }

}
