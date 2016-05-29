using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using DocMageFramework.WebUtils;
using DocMageFramework.AppUtils;
using DocMageFramework.Reporting;


namespace AccountingLib.ReportMailing
{
    /// <summary>
    /// Classe fornecida como mecanismo para renderizar relatórios, uma instancia dessa classe
    /// deve ser injetada no gerador de relatórios (que conhece apenas a interface IReportBuilder)
    /// esta classe gera relatórios em web page (html) formatados para impressão
    /// </summary>
    public class HtmlReportBuilder: IReportBuilder
    {
        private Page media;

        private Panel reportSurface;

        private ReportTable reportTable;

        private ReportTotalizer totalizer;


        /// <summary>
        /// Abre a mídia, neste caso uma página Web
        /// </summary>
        public void OpenMedia(Object media)
        {
            // Verifica se a mídia é uma Web Page
            if (media is Page)
            {
                this.media = (Page)media;
            }
            
            reportSurface = new Panel();
            reportSurface.Style.Add("margin-left", "auto");
            reportSurface.Style.Add("margin-right", "auto");
            this.media.Form.Controls.Add(reportSurface);
        }

        /// <summary>
        /// Fecha a mídia ( página aberta anteriormente )
        /// </summary>
        public void CloseMedia()
        {
            if (reportTable != null)
                reportTable.FinishDrawing();

            EmbedClientScript.PrintWindow(media);
        }

        private void AddLineBreak(WebControl target)
        {
            HtmlGenericControl lineBreak = new HtmlGenericControl("br");
            target.Controls.Add(lineBreak);
        }

        public Boolean IsNavigable()
        {
            // Esse tipo de ReportBuilder não é navegável, o relatório é formatado para impressão
            return false;
        }

        /// <summary>
        /// Define as informações de cabeçalho, o filtro deve conter "startDate" e "endDate"
        /// </summary>
        public void SetReportHeadings(String reportTitle, String tenantAlias, Dictionary<String, Object> reportFilter)
        {
            AddLineBreak(reportSurface);
            Panel reportHeader = new Panel();
            reportHeader.Style.Add("Width", "90%");
            reportHeader.Style.Add("margin-left", "auto");
            reportHeader.Style.Add("margin-right", "auto");
            reportSurface.Controls.Add(reportHeader);

            Image logo = new Image();
            logo.ImageUrl = "images/logo.png";
            logo.Style.Add("width", "200px");
            logo.Style.Add("height", "80px");
            reportHeader.Controls.Add(logo);

            AddLineBreak(reportHeader);
            Label dateParagraph = new Label();
            dateParagraph.Text = "Data Geração: " + DateTime.Now.ToString("dd/MM/yyyy");
            reportHeader.Controls.Add(dateParagraph);

            AddLineBreak(reportHeader);
            Label tenantParagraph = new Label();
            tenantParagraph.Text = "Empresa:  " + tenantAlias;
            reportHeader.Controls.Add(tenantParagraph);

            AddLineBreak(reportHeader);
            DateTime startDate = (DateTime) reportFilter["startDate"];
            DateTime endDate = (DateTime) reportFilter["endDate"];
            String reportPeriod = "de " + startDate.ToString("dd/MM/yyyy") + " até " + endDate.ToString("dd/MM/yyyy");
            Label periodParagraph = new Label();
            periodParagraph.Text = "Período:  " + reportPeriod;
            reportHeader.Controls.Add(periodParagraph);

            AddLineBreak(reportSurface);
            AddLineBreak(reportSurface);
            Panel titleArea = new Panel();
            titleArea.Style.Add("margin-left", "auto");
            titleArea.Style.Add("margin-right", "auto");
            titleArea.HorizontalAlign = HorizontalAlign.Center;
            reportSurface.Controls.Add(titleArea);

            Label titleParagraph = new Label();
            titleParagraph.Text = reportTitle;
            titleParagraph.Style.Add("color", "blue");
            titleParagraph.Style.Add("font-family", "Arial");
            titleParagraph.Style.Add("font-size", "large");
            titleParagraph.Style.Add("font-weight", "bold");
            titleArea.Controls.Add(titleParagraph);

            AddLineBreak(reportSurface);
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
            reportTable = new ReportTable(reportSurface, columnNames);
            totalizer = new ReportTotalizer(columnNames.Length);
        }

        /// <summary>
        /// Insere uma linha na tabela de dados do relatório
        /// </summary>
        public void InsertRow(int rowIndex, Object[] rowCells)
        {
            ReportCell[] cells = (ReportCell[])rowCells;
            reportTable.DrawRow(cells);

            // Soma os valores aos totais de cada coluna
            for (int columnIndex = 0; columnIndex < cells.Length; columnIndex++)
            {
                if (cells[columnIndex].IsNumeric)
                    totalizer.IncTotal(columnIndex, cells[columnIndex].value, cells[columnIndex].type);
            }
        }

        /// <summary>
        /// Insere o rodapé na tabela de dados do relatório
        /// </summary>
        public void InsertFooter(Object[] footerCells)
        {
            ReportCell[] cells = (ReportCell[])footerCells;

            // Obtem os totais de cada coluna
            for (int columnIndex = 0; columnIndex < cells.Length; columnIndex++)
            {
                if (cells[columnIndex].type == ReportCellType.Totalizer)
                    cells[columnIndex].value = totalizer.GetTotal(columnIndex, cells[columnIndex].subType);
            }

            reportTable.DrawFooter(cells);
        }
    }

}
