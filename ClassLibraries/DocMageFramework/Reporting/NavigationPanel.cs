using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DocMageFramework.WebUtils;


namespace DocMageFramework.Reporting
{
    /// <summary>
    /// Classe utilizada para gerar um painel de navegação em uma página de relatório. Permite
    /// a paginação de um relatório assim como sua impressão e exportação
    /// </summary>
    public class NavigationPanel
    {
        private Panel container;

        private String printUrl;

        private String exportUrl;

        private Boolean openNewWindow;

        private String pageControlUrl;

        private int currentPage; // indica o número da página sendo exibida, consultar método Show()

        private int pageCount; // indica o total de páginas, consultar método Show()


        /// <summary>
        /// Cria uma instância de NavigationPanel e associa ao container onde ele deverá ser
        /// exibido. O parâmetro "printUrl" indica a url que será chamada quando o botão de
        /// impressão for acionado, "exportUrl" indica a url que será chamada quando o botão
        /// de exportação for acionado( defina "openNewWindow = true" para abrir o relatório
        /// exportado em uma nova janela )
        /// </summary>
        public NavigationPanel(Panel container, String printUrl, String exportUrl, Boolean openNewWindow)
        {
            this.container = container;
            this.printUrl = printUrl;
            this.exportUrl = exportUrl;
            this.openNewWindow = openNewWindow;
            // Seta o atributo "pageControlUrl" apontando para a página web que controla a
            // navegação/paginação (a mesma onde o NavigationPanel se localiza)
            this.pageControlUrl = container.Page.Request.Url.ToString();
        }

        private void AddRenderingButtons(TableCell cell)
        {
            String clickScript = "window.location.replace('{0}'); // Botão {1} pressionado";
            if (openNewWindow) clickScript = "window.open('{0}', '{1}', 'width=1024,height=768,resizable=1,scrollbars=1');";

            HtmlGenericControl printButton = new HtmlGenericControl("input");
            printButton.Attributes.Add("type", "button");
            printButton.Attributes.Add("value", "Imprimir");
            printButton.Attributes.Add("onclick", "window.open('" + printUrl + "', 'Imprimir', 'width=1024,height=768,resizable=1,scrollbars=1');");
            printButton.Attributes.Add("class", "buttonStyle");
            cell.Controls.Add(printButton);

            HtmlGenericControl exportButton = new HtmlGenericControl("input");
            exportButton.Attributes.Add("type", "button");
            exportButton.Attributes.Add("value", "Exportar");
            exportButton.Attributes.Add("onclick", String.Format(clickScript, exportUrl, "Exportar"));
            exportButton.Attributes.Add("class", "buttonStyle");
            cell.Controls.Add(exportButton);
        }

        private void AddSpacing(TableCell cell)
        {
            LiteralControl spacing = new LiteralControl("&nbsp&nbsp");
            cell.Controls.Add(spacing);
        }

        private String BuildFormAction(String action, int currentPage)
        {
            String formAction = pageControlUrl;

            // Adiciona a variável action a url
            formAction = URLHandler.InsertQueryVariable(formAction, "action=" + action);
            // Adiciona a variável currentPage a url
            formAction = URLHandler.InsertQueryVariable(formAction, "currPage=" + currentPage);

            return formAction;
        }

        private String AvailabilityToText(Boolean enabled)
        {
            if (enabled)
                return "Enabled";
            else
                return "Disabled";
        }

        /// <summary>
        /// Cria um botão de navegação (uma imagem que permite mouse clicks)
        /// </summary>
        private Image CreateButton(String action, Boolean enabled)
        {
            System.Web.UI.WebControls.Image navigationButton = new System.Web.UI.WebControls.Image();
            navigationButton.ImageUrl = "images/" + action + AvailabilityToText(enabled) + ".ico";
            if (enabled)
            {
                String script = "document.forms[0].action='" + BuildFormAction(action, currentPage) + "';";
                navigationButton.Attributes.Add("onclick", script + "document.forms[0].submit();");
            }

            return navigationButton;
        }

        private void AddPageControlButtons(TableCell cell)
        {
            Boolean navigateBackward = true;
            Boolean navigateForward = true;
            if (currentPage <= 1) navigateBackward = false;
            if (currentPage >= pageCount) navigateForward = false;

            System.Web.UI.WebControls.Image firstPage = CreateButton("MoveFirst", navigateBackward);
            cell.Controls.Add(firstPage);

            AddSpacing(cell);
            System.Web.UI.WebControls.Image previousPage = CreateButton("MovePrevious", navigateBackward);
            cell.Controls.Add(previousPage);

            AddSpacing(cell);
            Label pageLabel = new Label();
            pageLabel.Text = "Página";
            cell.Controls.Add(pageLabel);

            AddSpacing(cell);
            TextBox pageDisplay = new TextBox();
            pageDisplay.Text = currentPage + "/" + pageCount;
            pageDisplay.Style.Add("width", "40px");
            pageDisplay.Style.Add("text-align", "center");
            pageDisplay.ReadOnly = true;
            cell.Controls.Add(pageDisplay);

            AddSpacing(cell);
            System.Web.UI.WebControls.Image nextPage = CreateButton("MoveNext", navigateForward);
            cell.Controls.Add(nextPage);

            AddSpacing(cell);
            System.Web.UI.WebControls.Image lastPage = CreateButton("MoveLast", navigateForward);
            cell.Controls.Add(lastPage);
        }

        public void Show(int currentPage, int pageCount)
        {
            // Define a página em exibição e a quantidade total de páginas
            this.currentPage = currentPage;
            this.pageCount = pageCount;

            Table contentTable = new Table();
            contentTable.Width = Unit.Percentage(100);

            TableRow tableRow = new TableRow();
            contentTable.Controls.Add(tableRow);

            TableCell renderingCell = new TableCell();
            renderingCell.HorizontalAlign = HorizontalAlign.Left;
            AddRenderingButtons(renderingCell);
            tableRow.Controls.Add(renderingCell);

            TableCell pageControlCell = new TableCell();
            pageControlCell.HorizontalAlign = HorizontalAlign.Right;
            AddPageControlButtons(pageControlCell);
            tableRow.Controls.Add(pageControlCell);

            Panel tableArea = new Panel();
            tableArea.Style.Add("width", "60%");
            tableArea.Style.Add("margin-left", "auto");
            tableArea.Style.Add("margin-right", "auto");
            tableArea.Controls.Add(contentTable);
            container.Controls.Add(tableArea);
        }
    }

}
