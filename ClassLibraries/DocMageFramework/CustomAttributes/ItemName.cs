using System;


namespace DocMageFramework.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ItemName : Attribute
    {
        public ItemName()
        {
        }
    }

}
