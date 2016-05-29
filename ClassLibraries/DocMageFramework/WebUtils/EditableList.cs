using System;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;


namespace DocMageFramework.WebUtils
{
    public class EditableList
    {
        private Panel container;

        private int columnCount;

        private EditableListButton[] buttons;

        private Table htmlTable;

        private Boolean preserveDefaultItem = false;


        public EditableList(Panel container, String[] columnNames, EditableListButton[] buttons)
        {
            if (buttons == null)
                buttons = new EditableListButton[] {};

            this.container = container;
            this.columnCount = columnNames.Length;
            this.buttons = buttons;
            this.htmlTable = new Table();

            TableRow header = new TableRow();
            header.ForeColor = Color.White;
            header.BackColor = Color.Blue;
            foreach (String name in columnNames)
            {
                TableCell newCell = new TableCell();
                Label newLabel = new Label();
                newLabel.Text = name;
                newLabel.Font.Bold = true;
                newCell.Controls.Add(newLabel);
                header.Controls.Add(newCell);
            }
            foreach (EditableListButton button in buttons)
            {
                TableCell newCell = new TableCell();
                Label newLabel = new Label();
                newLabel.Text = button.caption;
                newLabel.Font.Bold = true;
                newCell.Controls.Add(newLabel);
                header.Controls.Add(newCell);
            }

            htmlTable.Controls.Add(header);
        }

        public void PreserveDefaultItem()
        {
            preserveDefaultItem = true;
        }

        private String AdjustProperty(String property)
        {
            // Quando a propriedade não possui um valor ela deve ser uma string vazia "",
            // caso a propriedade esteja com um "null" isso pode indicar um problema ao
            // recuperar seu valor
            if (property == null) return "?????";

            return property;
        }

        public void InsertItem(int itemId, Boolean isDefaultItem, String[] itemProperties)
        {
            if (itemProperties.Length != columnCount)
                throw new FormatException();

            TableRow newRow = new TableRow();
            newRow.ForeColor = Color.Blue;
            newRow.BackColor = Color.LightSteelBlue;
            foreach(String property in itemProperties)
            {
                TableCell newCell = new TableCell();
                Label newLabel = new Label();
                newLabel.Text = AdjustProperty(property);
                newCell.Controls.Add(newLabel);
                newRow.Controls.Add(newCell);
            }
            InsertButtons(itemId, isDefaultItem, newRow);
            htmlTable.Controls.Add(newRow);
        }

        private void InsertButtons(int itemId, Boolean isDefaultItem, TableRow targetRow)
        {
            // Caso seja o item default da lista não é possível sua alteração nem sua exclusão
            if ((preserveDefaultItem) && (isDefaultItem))
            {
                foreach (EditableListButton button in buttons)
                {
                    TableCell emptyCell = new TableCell();
                    Label newLabel = new Label();
                    newLabel.Text = "-";
                    emptyCell.Controls.Add(newLabel);
                    targetRow.Controls.Add(emptyCell);
                }
                return;
            }

            // Para inserir javascripts que contenham abre e fecha chave, utilizar duplas "{{"  "}}"
            foreach (EditableListButton button in buttons)
            {
                TableCell newCell = new TableCell();
                System.Web.UI.WebControls.Image newImage = new System.Web.UI.WebControls.Image();
                newImage.Width = 16;
                newImage.Height = 16;
                switch (button.buttonType)
                {
                    case ButtonTypeEnum.Download:
                        newImage.ImageUrl = "images/Download.ico";
                        break;
                    case ButtonTypeEnum.Execute:
                        newImage.ImageUrl = "images/Execute.ico";
                        break;
                    case ButtonTypeEnum.Edit:
                        newImage.ImageUrl = "images/Edit.ico";
                        break;
                    default:
                        newImage.ImageUrl = "images/Remove.ico";
                        break;
                }
                newImage.Attributes.Add("onClick", String.Format(button.script, itemId));
                newCell.Controls.Add(newImage);
                targetRow.Controls.Add(newCell);
            }
        }

        public void DrawList()
        {
            htmlTable.BorderColor = Color.Black;
            htmlTable.BorderStyle = BorderStyle.Solid;
            htmlTable.BorderWidth = Unit.Pixel(1);
            htmlTable.Width = Unit.Percentage(100);
            htmlTable.GridLines = GridLines.Both;

            Panel tableArea = new Panel();
            tableArea.Style.Add("Width", "70%");
            tableArea.Style.Add("margin-left", "auto");
            tableArea.Style.Add("margin-right", "auto");
            tableArea.Controls.Add(htmlTable);
            container.Controls.Add(tableArea);

            HtmlGenericControl lineBreak = new HtmlGenericControl("br");
            container.Controls.Add(lineBreak);
        }
    }

}
