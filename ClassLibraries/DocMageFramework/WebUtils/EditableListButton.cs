using System;


namespace DocMageFramework.WebUtils
{
    public class EditableListButton
    {
        public String caption;

        public String script;

        public ButtonTypeEnum buttonType;


        public EditableListButton(String caption, String script, ButtonTypeEnum buttonType)
        {
            this.caption = caption;
            this.script = script;
            this.buttonType = buttonType;
        }
    }

}
