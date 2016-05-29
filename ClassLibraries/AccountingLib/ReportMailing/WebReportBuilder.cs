using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using DocMageFramework.AppUtils;
using DocMageFramework.Reporting;


namespace AccountingLib.ReportMailing
{
    /// <summary>
    /// Classe fornecida como mecanismo para renderizar relatórios, uma instancia dessa classe
    /// deve ser injetada no gerador de relatórios (que conhece apenas a interface IReportBuilder)
    /// esta classe gera relatórios em web page, porém difere de HtmlReportBuilder em alguns
    /// aspectos (possui paginação e ações de imprimir e exportar)
    /// </summary>
    public class WebReportBuilder: IReportBuilder
    {
        private Panel media;

        private String reportClass;

        private int recordCount;

        private Boolean openNewWindow = false;

        private ReportTable reportTable;

        private ReportTotalizer totalizer;

        private int currentPage;

        private PageControl pageControl;

        private int startIndex;

        private int endIndex;

        private Dictionary<String, Object> reportFilter;


        /// <summary>
        /// Abre a mídia, neste caso um panel ( superfície de renderização )
        /// </summary>
        public void OpenMedia(Object media)
        {
            // Verifica se a mídia é um panel
            if (media is Panel)
            {
                this.media = (Panel)media;
            }
        }

        /// <summary>
        /// Fecha a mídia aberta anteriormente ( renderiza os elementos pendentes )
        /// </summary>
        public void CloseMedia()
        {
            if (reportTable != null)
            {
                reportTable.FinishDrawing();
                AddLineBreak();
            }

            // Acrescenta os links para imprimir e exportar
            String query = "?report=" + reportClass;
            foreach (KeyValuePair<String, Object> param in reportFilter)
            {
                if (param.Value != null)
                    query += "&" + param.Key + "=" + param.Value;
            }
            String printUrl = "Print.aspx" + query;
            String exportUrl = "Export.aspx" + query;

            // A mídia de relatório é navegável então após fechar a tabela de dados, acrescenta
            // um painel para que o usuário possa navegar pelo relatório
            NavigationPanel navigationPanel = new NavigationPanel(media, printUrl, exportUrl, openNewWindow);
            navigationPanel.Show(currentPage, pageControl.GetPageCount());
            AddLineBreak();
            AddLineBreak();
        }

        private void AddLineBreak()
        {
            HtmlGenericControl lineBreak = new HtmlGenericControl("br");
            media.Controls.Add(lineBreak);
        }

        public Boolean IsNavigable()
        {
            // Esse tipo de ReportBuilder é navegável, permite navegação entre as páginas
            // do relatório
            return true;
        }

        /// <summary>
        /// Define as informações de cabeçalho
        /// </summary>
        public void SetReportHeadings(String reportTitle, String tenantAlias, Dictionary<String, Object> reportFilter)
        {
            // O relatório não possui cabeçalho pois utilizará o template de página do sistema
            this.reportFilter = reportFilter;
        }

        /// <summary>
        /// Define os dados de navegação ( utilizados pelos links imprimir/exportar e o pageControl)
        /// </summary>
        public void SetNavigationData(String reportClass, int recordCount, Dictionary<String, Object> exportOptions)
        {
            this.reportClass = reportClass;
            this.recordCount = recordCount;

            // Verifica se é necessário abrir uma nova janela ao exportar o relatório
            openNewWindow = false;
            if (exportOptions.ContainsKey("Disposition"))
            {
                String disposition = (String)exportOptions["Disposition"];
                if (disposition.Contains("inline")) openNewWindow = true;
            }
        }

        /// <summary>
        /// Muda para a página de relatório escolhida, action indica se é a próxima página, a anterior,
        /// a última ou a primeira
        /// </summary>
        public void SetReportPage(String action, int currentPage)
        {
            pageControl = new PageControl(recordCount);
            // Posiciona a página e delimita os registros contidos nela
            int page = pageControl.Perform(action, currentPage);
            startIndex = pageControl.GetStartRecord(page);
            endIndex = pageControl.GetEndRecord(page);
            this.currentPage = page;
        }

        /// <summary>
        /// Cria a tabela que receberá os dados do relatório
        /// </summary>
        public void CreateDataTable(String[] columnNames, int[] columnWidths, int rowCount)
        {
            reportTable = new ReportTable(media, columnNames);
            totalizer = new ReportTotalizer(columnNames.Length);
        }

        /// <summary>
        /// Insere uma linha na tabela de dados do relatório
        /// </summary>
        public void InsertRow(int rowIndex, Object[] rowCells)
        {
            ReportCell[] cells = (ReportCell[])rowCells;

            // So renderiza as linhas pertencentes a página corrente
            if ((rowIndex >= startIndex) && (rowIndex <= endIndex))
                reportTable.DrawRow(cells);

            // Só adiciona os totais na última página
            int lastPage = pageControl.GetPageCount();
            if (currentPage != lastPage)
                return;

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
            // Só insere o rodapé na última página
            int lastPage = pageControl.GetPageCount();
            if (currentPage != lastPage)
                return;

            ReportCell[] cells = (ReportCell[]) footerCells;

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
