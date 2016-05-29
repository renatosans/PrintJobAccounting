using System;
using System.Reflection;


namespace DocMageFramework.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AssociatedText : Attribute
    {
        public String text;

        public AssociatedText(String text)
        {
            this.text = text;
        }

        public static String GetFieldDescription(Type objectType, String field)
        {
            FieldInfo fieldInfo = objectType.GetField(field);
            if (fieldInfo == null) return null;

            AssociatedText[] attribs = fieldInfo.GetCustomAttributes(
                typeof(AssociatedText), false) as AssociatedText[];
            if (attribs.Length != 1) return null;

            return attribs[0].text;
        }
    }

}
