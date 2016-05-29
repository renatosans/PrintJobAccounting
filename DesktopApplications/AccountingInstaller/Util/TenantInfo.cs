using System;

namespace AccountingInstaller.Util
{
    public class TenantInfo
    {
        public int id;

        public String name;

        public String alias;


        public TenantInfo()
        {
        }

        public TenantInfo(int tenantId, String tenantName, String tenantAlias)
        {
            this.id = tenantId;
            this.name = tenantName;
            this.alias = tenantAlias;
        }
    }

}
