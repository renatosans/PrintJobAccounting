using System;
using System.IO;
using System.Web.UI;
using System.Collections.Generic;
using DocMageFramework.AppUtils;
using DocMageFramework.Reporting;
using org.in2bits.MyXls;


namespace AccountingLib.ReportMailing
{
    /// <summary>
    /// Classe fornecida como mecanismo para renderizar relatórios, uma instância dessa classe
    /// deve ser injetada no gerador de relatórios (que conhece apenas ainterface IReportBuilder)
    /// esta classe gera relatórios em arquivo .XLS do Excel utilizando o framework "MyXLS"
    /// </summary>
    public class XlsReportBuilder: IReportBuilder
    {
        private Object media;

        private Stream outputStream;

        private XlsDocument document;

        private Worksheet reportSheet;

        private String[] reportHeaders;

        private int rowCount;

        private ReportTotalizer totalizer;


        /// <summary>
        /// Abre a mídia, neste caso um arquivo .XLS em disco ou em memória
        /// </summary>
        public void OpenMedia(Object media)
        {
            this.media = media;

            // Verifica se a mídia é uma referência para arquivo em disco, não trata todos
            // os possíveis erros ao criar o arquivo em disco, passar referências válidas
            if (media is FileInfo)
            {
                outputStream = ((FileInfo)media).Create();
            }

            // Verifica se a mídia é uma Web Page
            if (media is Page)
            {
                outputStream = ((Page)media).Response.OutputStream;
            }
            
            document = new XlsDocument();
        }

        /// <summary>
        /// Fecha a mídia
        /// </s1,ummary>
        public void CloseMedia()
        {
            document.Save(outputStream);
            outputStream.Close();
        }

        public Boolean IsNavigable()
        {
            // Esse tipo de ReportBuilder não é navegável, o relatório é formatado para leitura (xls)
            return false;
        }

        /// <summary>
        /// Define as informações de cabeçalho, o filtro deve conter "startDate" e "endDate"
        /// </summary>
        public void SetReportHeadings(String reportTitle, String tenantAlias, Dictionary<String, Object> reportFilter)
        {
            // Apenas define as informações de cabeçalho aqui, posteriormente fará o
            // processamento destas informações quando estiver preparando a planilha
            reportHeaders = new String[4];
            reportHeaders[0] = reportTitle;

            String dateParagraph = "Data Geração: " + DateTime.Now.ToString("dd/MM/yyyy");
            reportHeaders[1] = dateParagraph;

            String tenantParagraph = "Empresa:  " + tenantAlias;
            reportHeaders[2] = tenantParagraph;

            DateTime startDate = (DateTime)reportFilter["startDate"];
            DateTime endDate = (DateTime)reportFilter["endDate"];
            String reportPeriod = "de " + startDate.ToString("dd/MM/yyyy") + " até " + endDate.ToString("dd/MM/yyyy");
            String periodParagraph = "Período:  " + reportPeriod;
            reportHeaders[3] = periodParagraph;
        }

        // Processa as informações do cabeçalho
        private void ProcessHeaders(int columnSpan)
        {
            // Agrupa as células que fazem parte da data de geração do relatório
            reportSheet.Cells.Merge(1, 1, 2, columnSpan + 1);
            Cell dateCell = reportSheet.Cells.Add(1, 2, reportHeaders[1]);
            dateCell.HorizontalAlignment = HorizontalAlignments.Left;
            dateCell.Font.Height = 250;
            dateCell.Font.FontName = "Arial";

            // Agrupa as células que fazem parte da identificação da empresa
            reportSheet.Cells.Merge(2, 2, 2, columnSpan + 1);
            Cell tenantCell = reportSheet.Cells.Add(2, 2, reportHeaders[2]);
            tenantCell.HorizontalAlignment = HorizontalAlignments.Left;
            tenantCell.Font.Height = 250;
            tenantCell.Font.FontName = "Arial";

            // Agrupa as células que fazem parte do periodo de extração do relatório
            reportSheet.Cells.Merge(3, 3, 2, columnSpan + 1);
            Cell periodCell = reportSheet.Cells.Add(3, 2, reportHeaders[3]);
            periodCell.HorizontalAlignment = HorizontalAlignments.Left;
            periodCell.Font.Height = 250;
            periodCell.Font.FontName = "Arial";

            // Agrupa as celulas que fazem parte do título do relatório
            reportSheet.Cells.Merge(5, 5, 2, columnSpan + 1);
            Cell titleCell = reportSheet.Cells.Add(5, 2, reportHeaders[0]);
            titleCell.HorizontalAlignment = HorizontalAlignments.Centered;
            titleCell.Font.Height = 400;
            titleCell.Font.Bold = true;
            titleCell.Font.FontFamily = FontFamilies.Roman;
        }

        public void SetNavigationData(String reportClass, int recordCount, Dictionary<String, Object> exportOptions)
        {
            throw new Exception("Método não definido para esta classe.");
        }

        public void SetReportPage(String action, int currentPage)
        {
            throw new Exception("Método não definido para esta classe.");
        }

        private void SetCellPattern(int row, int col)
        {
            if (col > 255) return;
            if (row > 65535) return;

            Cell newCell = reportSheet.Cells.Add(row, col, "");
            newCell.VerticalAlignment = VerticalAlignments.Centered;
            newCell.HorizontalAlignment = HorizontalAlignments.Centered;
            newCell.PatternColor = Colors.White;
            newCell.Pattern = 1;
        }

        private void SetCellBorder(Cell cell)
        {
            cell.TopLineColor = Colors.Black;
            cell.BottomLineColor = Colors.Black;
            cell.RightLineColor = Colors.Black;
            cell.LeftLineColor = Colors.Black;

            cell.TopLineStyle = 1;
            cell.BottomLineStyle = 1;
            cell.RightLineStyle = 1;
            cell.LeftLineStyle = 1;
        }

        /// <summary>
        /// Cria a tabela que receberá os dados do relatório
        /// </summary>
        public void CreateDataTable(String[] columnNames, int[] columnWidths, int rowCount)
        {
            // Define o nome da planilha e a largura das colunas
            reportSheet = document.Workbook.Worksheets.Add(reportHeaders[0]);
            ColumnInfo info = new ColumnInfo(document, reportSheet);
            info.ColumnIndexStart = 1;
            info.ColumnIndexEnd = (ushort)columnNames.Length;
            info.Width = 5100;
            reportSheet.AddColumnInfo(info);

            // Prepara o plano de fundo da planilha
            this.rowCount = rowCount;
            for(int row = 1; row < rowCount + 10; row++)
                for(int col = 1; col < columnNames.Length + 3; col++) SetCellPattern(row, col);
            
            // Insere o cabeçalho da planilha
            ProcessHeaders(columnNames.Length);

            // Cria celulas com os nomes das colunas
            for (int ndx = 0; ndx < columnNames.Length; ndx++)
            {
                Cell columnName = reportSheet.Cells.Add(6, ndx+2, columnNames[ndx]);
                columnName.Font.Bold = true;
                columnName.PatternColor = Colors.Default1F;
                columnName.Pattern = 1;
                SetCellBorder(columnName);
            }

            // Cria o totalizador 
            totalizer = new ReportTotalizer(columnNames.Length);
        }

        /// <summary>
        /// Insere uma linha na tabela de dados do relatório
        /// </summary>
        public void InsertRow(int rowIndex, Object[] cells)
        {
            for (int ndx = 0; ndx < cells.Length; ndx++)
            {
                ReportCell reportCell = (ReportCell)cells[ndx];
                Cell newCell = reportSheet.Cells.Add(rowIndex+7, ndx+2, FormatCell(reportCell, ndx));
                newCell.HorizontalAlignment = HorizontalAlignments.Centered;
                if (reportCell.align == ReportCellAlign.Left)
                    newCell.HorizontalAlignment = HorizontalAlignments.Left;
                if (reportCell.align == ReportCellAlign.Right)
                    newCell.HorizontalAlignment = HorizontalAlignments.Right;
                newCell.VerticalAlignment = VerticalAlignments.Centered;
                SetCellBorder(newCell);
            }
        }

        /// <summary>
        /// Insere o rodapé na tabela de dados do relatório
        /// </summary>
        public void InsertFooter(Object[] cells)
        {
            for (int ndx = 0; ndx < cells.Length; ndx++)
            {
                ReportCell reportCell = (ReportCell)cells[ndx];
                Cell footerCell = reportSheet.Cells.Add(rowCount + 7, ndx + 2, FormatCell(reportCell, ndx));
                footerCell.HorizontalAlignment = HorizontalAlignments.Centered;
                footerCell.VerticalAlignment = VerticalAlignments.Centered;
                footerCell.PatternColor = Colors.Default33;
                footerCell.Pattern = 1;
                SetCellBorder(footerCell);
            }
        }

        private String FormatCell(ReportCell cell, int columnIndex)
        {
            String cellContent;
            switch (cell.type)
            {
                case ReportCellType.Number:
                    cellContent = String.Format("{0}", cell.value);
                    totalizer.IncTotal(columnIndex, cell.value, cell.type);
                    break;
                case ReportCellType.Money:
                    cellContent = String.Format("R$ {0:0.000}", cell.value);
                    totalizer.IncTotal(columnIndex, cell.value, cell.type);
                    break;
                case ReportCellType.Percentage:
                    cellContent = String.Format("{0:0.##}%", (double)cell.value * 100);
                    totalizer.IncTotal(columnIndex, cell.value, cell.type);
                    break;
                case ReportCellType.Totalizer:
                    cellContent = totalizer.GetTotal(columnIndex, cell.subType);
                    break;
                default: // por defalut considera o conteudo da célula como texto
                    cellContent = String.Format("{0}", cell.value);
                    break;
            }
            return cellContent;
        }
    }

}
