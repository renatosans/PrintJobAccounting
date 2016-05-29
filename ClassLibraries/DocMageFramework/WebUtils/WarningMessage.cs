using System;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace DocMageFramework.WebUtils
{
    public static class WarningMessage
    {
        public static void Show(Panel container, String message)
        {
            HtmlGenericControl lineBreak1 = new HtmlGenericControl("br");
            HtmlGenericControl lineBreak2 = new HtmlGenericControl("br");
            container.Controls.Add(lineBreak1);
            container.Controls.Add(lineBreak2);

            Label messageLabel = new Label();
            messageLabel.CssClass = "errorMessagesStyle";
            messageLabel.Font.Size = 18;
            messageLabel.Text = message;

            Panel warning = new Panel();
            warning.Style.Add("margin-left", "auto");
            warning.Style.Add("margin-right", "auto");
            warning.BorderStyle = BorderStyle.Solid;
            warning.BorderWidth = Unit.Pixel(1);
            warning.BorderColor = Color.Red;
            warning.BackColor = Color.LightYellow;
            warning.Width = Unit.Percentage(70);
            warning.Controls.Add(messageLabel);

            container.Controls.Add(warning);
        }
    }

}
