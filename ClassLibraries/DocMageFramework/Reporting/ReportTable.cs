using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace DocMageFramework.Reporting
{
    public class ReportTable
    {
        private Panel container;

        private Table htmlTable;


        private void AppendTextCell(TableRow container, String text)
        {
            TableCell newCell = new TableCell();
            container.Cells.Add(newCell);

            Label newLabel = new Label();
            newLabel.Text = text;
            newCell.Controls.Add(newLabel);
        }

        private void AppendCell(TableRow container, ReportCell cell)
        {
            TableCell newCell = new TableCell();
            container.Cells.Add(newCell);

            int columnIndex = container.Controls.Count-1;

            Control cellContent = null;
            switch (cell.type)
            {
                case ReportCellType.Number:
                    cellContent = new Label();
                    ((Label)cellContent).Text = String.Format("{0}", cell.value);
                    break;
                case ReportCellType.Money:
                    cellContent = new Label();
                    ((Label)cellContent).Text = String.Format("R$ {0:0.000}", cell.value);
                    break;
                case ReportCellType.Percentage:
                    cellContent = new Label();
                    ((Label)cellContent).Text = String.Format("{0:0.##}%", (double)cell.value * 100 );
                    break;
                case ReportCellType.Link:
                    cellContent = new HyperLink();
                    ((HyperLink)cellContent).Text = String.Format("{0}", cell.value);
                    ((HyperLink)cellContent).NavigateUrl = cell.link;
                    break;
                default: // por defalut considera o conteudo da célula como texto
                    cellContent = new Label();
                    String text = String.Format("{0}", cell.value);
                    if (text.Length > 45) text = text.Substring(0, 45) + "...";
                    ((Label)cellContent).Text = text;
                    break;
            }
            newCell.ForeColor = cell.textColor;
            newCell.HorizontalAlign = (HorizontalAlign)cell.align;
            newCell.Controls.Add(cellContent);
        }

        private Color Darken(Color color)
        {
            int red = (color.R - 99);
            if (red < 0) red = 0;

            int green = (color.G - 99);
            if (green < 0) green = 0;

            int blue = (color.B - 99);
            if (blue < 0) blue = 0;

            return Color.FromArgb(red, green, blue);
        }

        public ReportTable(Panel container, String[] columnNames)
        {
            // Especifica a cor default caso ela não seja fornecida
            Color headerColor = Color.FromArgb(165, 185, 250);
            StartDrawing(container, columnNames, headerColor);
        }

        public ReportTable(Panel container, String[] columnNames, Color headerColor)
        {
            StartDrawing(container, columnNames, headerColor);
        }

        private void StartDrawing(Panel container, String[] columnNames, Color headerColor)
        {
            this.container = container;
            this.htmlTable = new Table();

            htmlTable.BorderColor = Color.Black;
            htmlTable.BorderStyle = BorderStyle.Solid;
            htmlTable.BorderWidth = Unit.Pixel(2);
            htmlTable.Width = Unit.Percentage(100);
            htmlTable.GridLines = GridLines.Both;
            
            TableRow reportHeader = new TableRow();
            reportHeader.HorizontalAlign = HorizontalAlign.Center;
            reportHeader.Font.Name = "Arial";
            reportHeader.Font.Bold = true;
            reportHeader.Font.Size = FontUnit.Large;
            reportHeader.BackColor = headerColor;
            reportHeader.ForeColor = Darken(headerColor);            
            htmlTable.Rows.Add(reportHeader);
            foreach (String columnName in columnNames)
            {
                AppendTextCell(reportHeader, columnName);
            }
        }

        public void DrawRow(ReportCell[] cells)
        {
            TableRow newRow = new TableRow();
            newRow.HorizontalAlign = HorizontalAlign.Center;
            newRow.BackColor = Color.White;
            newRow.ForeColor = Color.Black;
            htmlTable.Rows.Add(newRow);
            foreach (ReportCell cell in cells)
            {
                AppendCell(newRow, cell);
            }
        }

        public void DrawFooter(ReportCell[] cells)
        {
            // Especifica a cor default caso ela não seja fornecida
            Color footerColor = Color.FromArgb(255, 255, 200);
            DrawFooter(cells, footerColor);
        }

        public void DrawFooter(ReportCell[] cells,  Color footerColor)
        {
            TableRow reportFooter = new TableRow();
            reportFooter.HorizontalAlign = HorizontalAlign.Center;
            reportFooter.BackColor = footerColor;
            reportFooter.ForeColor = Darken(footerColor);
            htmlTable.Rows.Add(reportFooter);
            foreach (ReportCell cell in cells)
            {
                AppendCell(reportFooter, cell);
            }
        }

        public void FinishDrawing()
        {
            Panel tableArea = new Panel();
            tableArea.Style.Add("Width", "90%");
            tableArea.Style.Add("margin-left", "auto");
            tableArea.Style.Add("margin-right", "auto");
            tableArea.Controls.Add(htmlTable);
            container.Controls.Add(tableArea);
        }
    }

}
