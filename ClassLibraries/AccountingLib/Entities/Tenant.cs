using System;


namespace AccountingLib.Entities
{
    public class Tenant
    {
        public int id;

        public String name;

        public String alias;


        public Tenant()
        {
        }

        public Tenant(String name, String alias)
        {
            this.id = 0;
            this.name = name;
            this.alias = alias;
        }

        public override String ToString()
        {
            String tenantInfo = "id=" + this.id + "&name=" + this.name + "&alias=" + this.alias;
            return tenantInfo;
        }
    }

}
