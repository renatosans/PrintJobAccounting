using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using DocMageFramework.CustomAttributes;


namespace DocMageFramework.WebUtils
{
    public class SettingsInput
    {
        private Panel container;

        private NameValueCollection inputStyles;

        private Table htmlTable;


        public SettingsInput(Panel container, NameValueCollection inputStyles)
        {
            this.container = container;
            this.inputStyles = inputStyles;
            this.htmlTable = new Table();
            htmlTable.Width = Unit.Percentage(100);

            Panel tableArea = new Panel();
            tableArea.Style.Add("Width", "70%");
            tableArea.Style.Add("margin-left", "auto");
            tableArea.Style.Add("margin-right", "auto");
            tableArea.Controls.Add(htmlTable);
            container.Controls.Add(tableArea);

            HtmlGenericControl lineBreak = new HtmlGenericControl("br");
            container.Controls.Add(lineBreak);
        }


        private void SetLabel(TableRow targetRow, String caption)
        {
            TableCell captionCell = new TableCell();
            Label lblCaption = new Label();
            lblCaption.Text = caption;
            lblCaption.CssClass = "paramStyle";
            captionCell.Style.Add("text-align", "left");
            captionCell.Controls.Add(lblCaption);
            targetRow.Controls.Add(captionCell);
        }

        public void Add(String inputId, String inputCaption, String inputValue, Boolean password, String editPropertiesScript)
        {
            TableRow tableRow = new TableRow();
            htmlTable.Controls.Add(tableRow);
            SetLabel(tableRow, inputCaption);

            TableCell valueCell = new TableCell();
            TextBox txtValue = new TextBox();
            txtValue.ID = inputId;
            txtValue.Text = inputValue;
            if (password)
            {
                txtValue.TextMode = TextBoxMode.Password;
                txtValue.Attributes.Add("value", inputValue);
            }
            txtValue.Style.Add("width", "200px"); // Estilo default do input
            CSSHandler.ReplaceStyles(txtValue, inputStyles); // Substitui caso existam estilos do usuário
            valueCell.Style.Add("text-align", "right");
            valueCell.Controls.Add(txtValue);
            tableRow.Controls.Add(valueCell);

            if (!String.IsNullOrEmpty(editPropertiesScript))
            {
                TableCell newCell = new TableCell();
                System.Web.UI.WebControls.Image newImage = new System.Web.UI.WebControls.Image();
                newImage.Width = 16;
                newImage.Height = 16;
                newImage.ImageUrl = "images/Edit.ico";
                newImage.Attributes.Add("onClick", editPropertiesScript);
                newCell.Controls.Add(newImage);
                tableRow.Controls.Add(newCell);
            }
        }


        public void Add(String inputId, String inputCaption, String inputValue)
        {
            Add(inputId, inputCaption, inputValue, false, null);
        }


        public void AddDropDownList(String inputId, String inputCaption, int inputValue, Type enumType)
        {
            TableRow tableRow = new TableRow();
            htmlTable.Controls.Add(tableRow);
            SetLabel(tableRow, inputCaption);

            TableCell valueCell = new TableCell();
            DropDownList cmbValue = new DropDownList();
            cmbValue.ID = inputId;
            foreach (int value in Enum.GetValues(enumType))
            {
                ListItem listItem = new ListItem();
                listItem.Value = value.ToString();
                String enumItem = Enum.GetName(enumType, value);
                listItem.Text = AssociatedText.GetFieldDescription(enumType, enumItem);
                cmbValue.Items.Add(listItem);
            }
            ListItem referencedItem = cmbValue.Items.FindByValue(inputValue.ToString());
            if (referencedItem != null) referencedItem.Selected = true; // Seleciona o item que representa o valor do campo
            cmbValue.Style.Add("width", "205px"); // Estilo default do input
            CSSHandler.ReplaceStyles(cmbValue, inputStyles); // Substitui caso existam estilos do usuário
            valueCell.Style.Add("text-align", "right");
            valueCell.Controls.Add(cmbValue);
            tableRow.Controls.Add(valueCell);
        }


        public void AddDropDownList(String inputId, String inputCaption, int inputValue, ListItem[] items)
        {
            TableRow tableRow = new TableRow();
            htmlTable.Controls.Add(tableRow);
            SetLabel(tableRow, inputCaption);

            TableCell valueCell = new TableCell();
            DropDownList cmbValue = new DropDownList();
            cmbValue.ID = inputId;
            cmbValue.Items.AddRange(items);
            ListItem referencedItem = cmbValue.Items.FindByValue(inputValue.ToString());
            if (referencedItem != null) referencedItem.Selected = true; // Seleciona o item que representa o valor do campo
            cmbValue.Style.Add("width", "205px"); // Estilo default do input
            CSSHandler.ReplaceStyles(cmbValue, inputStyles); // Substitui caso existam estilos do usuário
            valueCell.Style.Add("text-align", "right");
            valueCell.Controls.Add(cmbValue);
            tableRow.Controls.Add(valueCell);
        }


        public void AddCheckBox(String inputId, String inputCaption, Boolean isChecked)
        {
            TableRow tableRow = new TableRow();
            htmlTable.Controls.Add(tableRow);
            SetLabel(tableRow, inputCaption);

            TableCell valueCell = new TableCell();
            Panel checkBoxSurface = new Panel();
            // Aplica o estilo default do input
            checkBoxSurface.Style.Add("margin-left", "auto");
            checkBoxSurface.Style.Add("margin-right", "0");
            checkBoxSurface.Style.Add("width", "200px");
            checkBoxSurface.Style.Add("text-align", "left");
            CSSHandler.ReplaceStyles(checkBoxSurface, inputStyles); // Substitui caso existam estilos do usuário

            CheckBox chkValue = new CheckBox();
            chkValue.ID = inputId;
            chkValue.Checked = isChecked;

            checkBoxSurface.Controls.Add(chkValue);
            valueCell.Controls.Add(checkBoxSurface);
            tableRow.Controls.Add(valueCell);
        }


        public void AddEditor(String inputId, String inputCaption, int inputValue, String editorUrl, ListItem[] items)
        {
            TableRow tableRow = new TableRow();
            htmlTable.Controls.Add(tableRow);
            SetLabel(tableRow, inputCaption);

            TableCell valueCell = new TableCell();
            DropDownList cmbValue = new DropDownList();
            cmbValue.ID = inputId;
            cmbValue.Items.AddRange(items);
            cmbValue.Items[0].Text = "<Criar...>";
            String editScript = "function editSettings(id) { window.open('" + editorUrl + "' + id, 'Settings', 'width=540,height=600'); };";
            cmbValue.Attributes.Add("onChange", editScript + "editSettings(this.value); if (this.selectedIndex == 0) this.selectedIndex = -1;");
            ListItem referencedItem = cmbValue.Items.FindByValue(inputValue.ToString());
            if (referencedItem != null) referencedItem.Selected = true; // Seleciona o item que representa o valor do campo
            cmbValue.Style.Add("width", "205px"); // Estilo default do input
            CSSHandler.ReplaceStyles(cmbValue, inputStyles); // Substitui caso existam estilos do usuário
            valueCell.Style.Add("text-align", "right");
            valueCell.Controls.Add(cmbValue);
            tableRow.Controls.Add(valueCell);
        }


        public void AddHidden(String inputId, String inputValue)
        {
            HiddenField hiddenField = new HiddenField();
            hiddenField.ID = inputId;
            hiddenField.Value = inputValue;
            container.Controls.Add(hiddenField);
        }
    }

}
