using System;
using System.IO;
using System.Web.UI;
using System.Collections.Generic;
using DocMageFramework.Parsing;
using DocMageFramework.AppUtils;
using DocMageFramework.Reporting;


namespace AccountingLib.ReportMailing
{
    /// <summary>
    /// Classe fornecida como mecanismo para renderizar relatórios, uma instância dessa classe
    /// deve ser injetada no gerador de relatórios (que conhece apenas ainterface IReportBuilder)
    /// esta classe gera relatórios em arquivo .CSV -> Comma separated values
    /// </summary>
    public class CsvReportBuilder: IReportBuilder
    {
        private Object media;

        private Stream outputStream;

        private String csvComment;

        private StreamWriter streamWriter;

        private Boolean decimalSeparatorIsComma;

        private ReportTotalizer totalizer;


        /// <summary>
        /// Abre a mídia, neste caso um arquivo .CSV em disco ou em memória
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

            streamWriter = new StreamWriter(outputStream);
        }

        /// <summary>
        /// Fecha a mídia
        /// </s1,ummary>
        public void CloseMedia()
        {
            streamWriter.Flush();
            streamWriter.Close();
            outputStream.Close();
        }

        public Boolean IsNavigable()
        {
            // Esse tipo de ReportBuilder não é navegável, o relatório é formatado para leitura (csv)
            return false;
        }

        /// <summary>
        /// Define as informações de cabeçalho, guarda como comentário no inicio do CSV gerado
        /// </summary>
        public void SetReportHeadings(String reportTitle, String tenantAlias, Dictionary<String, Object> reportFilter)
        {
            String creationDate = "Data Geração: " + DateTime.Now.ToString("dd/MM/yyyy");
            String tenantReference = "Empresa:  " + tenantAlias;
            DateTime startDate = (DateTime)reportFilter["startDate"];
            DateTime endDate = (DateTime)reportFilter["endDate"];
            String reportPeriod = "Período: de " + startDate.ToString("dd/MM/yyyy") + " até " + endDate.ToString("dd/MM/yyyy");

            this.csvComment = reportTitle + " " + creationDate + " " + tenantReference + " " + reportPeriod;
        }

        public void SetNavigationData(String reportClass, int recordCount, Dictionary<String, Object> exportOptions)
        {
            throw new Exception("Método não definido para esta classe.");
        }

        public void SetReportPage(String action, int currentPage)
        {
            throw new Exception("Método não definido para esta classe.");
        }

        /// <summary>
        /// Cria a tabela que receberá os dados do relatório, no .CSV apenas separa os nomes das
        /// colunas pois essa tabela não existe fisicamente
        /// </summary>
        public void CreateDataTable(String[] columnNames, int[] columnWidths, int rowCount)
        {
            // Grava os comentários no arquivo
            // streamWriter.WriteLine(csvComment);  removi para facilitar importação no excel

            // Grava os nomes das colunas no arquivo
            String columnList = null;
            for (int ndx = 0; ndx < columnNames.Length; ndx++)
            {
                if (!String.IsNullOrEmpty(columnList)) columnList += ",";
                columnList += columnNames[ndx];
            }
            streamWriter.WriteLine(columnList);

            // Verifica o separador decimal, para depois não misturar com as virgulas que separam os campos do CSV
            decimalSeparatorIsComma = FieldParser.IsCommaDecimalSeparator();

            // Cria o totalizador 
            totalizer = new ReportTotalizer(columnNames.Length);
        }

        /// <summary>
        /// Insere uma linha na tabela de dados do relatório
        /// </summary>
        public void InsertRow(int rowIndex, Object[] cells)
        {
            // Grava os valores em uma linha do CSV
            String valueList = null;
            for (int ndx = 0; ndx < cells.Length; ndx++)
            {
                ReportCell reportCell = (ReportCell)cells[ndx];
                
                if (!String.IsNullOrEmpty(valueList)) valueList += ",";
                valueList += FormatCell(reportCell, ndx);
            }
            streamWriter.WriteLine(valueList);
        }

        /// <summary>
        /// Insere o rodapé na tabela de dados do relatório
        /// </summary>
        public void InsertFooter(Object[] cells)
        {
            // Grava os valores em uma linha do CSV
            String valueList = null;
            for (int ndx = 0; ndx < cells.Length; ndx++)
            {
                ReportCell reportCell = (ReportCell)cells[ndx];
                
                if (!String.IsNullOrEmpty(valueList)) valueList += ",";
                valueList += FormatCell(reportCell, ndx);
            }
            streamWriter.WriteLine(valueList);
        }

        private String FormatCell(ReportCell cell, int columnIndex)
        {
            String cellContent;
            Boolean isText = false;

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
                    isText = true;
                    break;
            }

            // Ajusta apontuação decimal e de milhar para evitar virgulas extras pois o
            // csv fica quebrado se houverem virgulas além daquelas que separam colunas
            if ((!isText) && (decimalSeparatorIsComma))
            {
                cellContent = cellContent.Replace(".", ""); // retira pontuação de milhar
                cellContent = cellContent.Replace(",", "."); // substitui virgula por ponto
            }

            // Retira as vírulas dos campos contendo texto
            if (isText) cellContent = cellContent.Replace(",", "");

            return cellContent;
        }
    }

}
