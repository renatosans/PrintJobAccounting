using System;
using DocMageFramework.CustomAttributes;


namespace AccountingLib.Entities
{
    public class Preference
    {
        [ItemId]
        public int id;

        public int tenantId;

        [ItemName]
        public String name;

        public String value;

        public String type = "System.String"; // String é o tipo padrão


        public Preference()
        {
        }
    }

}
