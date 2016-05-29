using System;
using DocMageFramework.CustomAttributes;


namespace AccountingLib.Entities
{
    public class User
    {
        [ItemId]
        public int id;

        public int tenantId;

        public String name;

        [ItemName]
        public String alias;

        public Decimal? quota;


        public User()
        {
        }

        public User(int tenantId, String name, String alias)
        {
            this.id = 0;
            this.tenantId = tenantId;
            this.name = name;
            this.alias = alias;
        }

        public override string ToString()
        {
            return this.alias;
        }
    }

}
