using System;
using System.IO;
using System.Web.UI;
using System.Collections.Generic;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.Reporting;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace AccountingLib.ReportMailing
{
    /// <summary>
    /// Classe fornecida como mecanismo para renderizar relatórios, uma instancia dessa classe
    /// deve ser injetada no gerador de relatórios (que conhece apenas a interface IReportBuilder)
    /// esta classe gera relatórios em arquivo .PDF utilizando o framework "iTextSharp"
    /// </summary>
    public class PdfReportBuilder : IReportBuilder
    {
        private Object media;

        private Stream outputStream;

        private Document document;

        private Table reportTable;

        private ReportTotalizer totalizer;


        /// <summary>
        /// Abre a mídia, neste caso um arquivo .PDF em disco ou em memória
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

            document = new Document(PageSize.A4.Rotate()); // Cria o documento em landscape
            PdfWriter.GetInstance(document, outputStream);
            document.Open();
        }

        /// <summary>
        /// Fecha a mídia ( arquivo .PDF aberto anteriormente )
        /// </summary>
        public void CloseMedia()
        {
            document.Add(reportTable);
            document.Close();
            outputStream.Close();
        }

        public Boolean IsNavigable()
        {
            // Esse tipo de ReportBuilder não é navegável, o relatório é formatado para leitura (pdf)
            return false;
        }

        /// <summary>
        /// Define as informações de cabeçalho, o filtro deve conter "startDate" e "endDate"
        /// </summary>
        public void SetReportHeadings(String reportTitle, String tenantAlias, Dictionary<String, Object> reportFilter)
        {
            String logoFile = null;
            if (media is FileInfo)
                logoFile = FileResource.MapDesktopResource("Logo.png");
            if (media is Page)
                logoFile = ((Page)media).Server.MapPath("Images/Logo.png");

            Image logo = Image.GetInstance(logoFile);
            logo.ScaleToFit(150, 80);
            document.Add(logo);

            Paragraph dateParagraph = new Paragraph("Data Geração: " + DateTime.Now.ToString("dd/MM/yyyy"));
            document.Add(dateParagraph);

            Paragraph tenantParagraph = new Paragraph("Empresa:  " + tenantAlias);
            document.Add(tenantParagraph);

            DateTime startDate = (DateTime)reportFilter["startDate"];
            DateTime endDate = (DateTime)reportFilter["endDate"];
            String reportPeriod = "de " + startDate.ToString("dd/MM/yyyy") + " até " + endDate.ToString("dd/MM/yyyy");
            Paragraph periodParagraph = new Paragraph("Período:  " + reportPeriod);
            document.Add(periodParagraph);

            Font titleFont = new Font(Font.TIMES_ROMAN, 20, Font.BOLD, Color.BLUE);
            Paragraph titleParagraph = new Paragraph(reportTitle, titleFont);
            titleParagraph.SetAlignment(ElementTags.ALIGN_CENTER);
            document.Add(titleParagraph);
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
        /// Cria a tabela que receberá os dados do relatório
        /// </summary>
        public void CreateDataTable(String[] columnNames, int[] columnWidths, int rowCount)
        {
            reportTable = new Table(columnNames.Length, rowCount + 2);
            reportTable.Width = 100;
            reportTable.Cellspacing = 4;
            reportTable.SetWidths(columnWidths);
            for (int ndx = 0; ndx < columnNames.Length; ndx++)
            {
                Cell columnName = CreateCell(new ReportCell(columnNames[ndx]), ndx);
                columnName.BackgroundColor = new Color(165, 185, 250);
                reportTable.AddCell(columnName);
            }
            totalizer = new ReportTotalizer(columnNames.Length);
        }

        /// <summary>
        /// Insere uma linha na tabela de dados do relatório
        /// </summary>
        public void InsertRow(int rowIndex, Object[] cells)
        {
            for (int ndx = 0; ndx < cells.Length; ndx++)
            {
                reportTable.AddCell(CreateCell((ReportCell)cells[ndx], ndx));
            }
        }

        /// <summary>
        /// Insere o rodapé na tabela de dados do relatório
        /// </summary>
        public void InsertFooter(Object[] cells)
        {
            for (int ndx = 0; ndx < cells.Length; ndx++)
            {
                Cell footerCell = CreateCell((ReportCell)cells[ndx], ndx);
                footerCell.BackgroundColor = new Color(255, 255, 200);
                reportTable.AddCell(footerCell);
            }
        }

        private Cell CreateCell(ReportCell cell, int columnIndex)
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
            Cell newCell = new Cell(cellContent);
            newCell.SetHorizontalAlignment(cell.align.ToString());
            newCell.SetVerticalAlignment(ElementTags.ALIGN_MIDDLE);
            return newCell;
        }
    }

}
