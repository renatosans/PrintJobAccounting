using System;
using System.Drawing;


namespace DocMageFramework.Reporting
{
    public class ReportCell
    {
        public Object value;

        public String link;

        public Color textColor;

        public Boolean IsNumeric;

        public ReportCellType type;

        public ReportCellType subType;

        public ReportCellAlign align = ReportCellAlign.Center;


        public ReportCell(String text)
        {
            this.value = text;
            this.textColor = Color.Blue;
            this.IsNumeric = false;
            this.type = ReportCellType.Text;
        }

        public ReportCell(String text, Color textColor)
        {
            this.value = text;
            this.textColor = textColor;
            this.IsNumeric = false;
            this.type = ReportCellType.Text;
        }

        public ReportCell(int number)
        {
            this.value = number;
            this.textColor = Color.Blue;
            this.IsNumeric = true;
            this.type = ReportCellType.Number;
        }

        public ReportCell(decimal amount)
        {
            this.value = amount;
            this.textColor = Color.Blue;
            this.IsNumeric = true;
            this.type = ReportCellType.Money;
        }

        public ReportCell(double percentage)
        {
            this.value = percentage;
            this.textColor = Color.Blue;
            this.IsNumeric = true;
            this.type = ReportCellType.Percentage;
        }

        public ReportCell(String totalizerName, ReportCellType totalizerType)
        {
            this.value = totalizerName;
            this.textColor = Color.Red;
            this.IsNumeric = false;
            this.type = ReportCellType.Totalizer;
            this.subType = totalizerType;
        }

        public ReportCell(String caption, String link)
        {
            this.value = caption;
            this.link = link;
            this.textColor = Color.Blue;
            this.IsNumeric = false;
            this.type = ReportCellType.Link;
        }
    }

}
