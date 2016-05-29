using System;


namespace DocMageFramework.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ItemId : Attribute
    {
        public ItemId()
        {
        }
    }

}
