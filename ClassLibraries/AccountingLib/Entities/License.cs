using System;


namespace AccountingLib.Entities
{
    public class License
    {
        public int id;

        public int tenantId;

        public String installationKey;

        public DateTime? installationDate;

        public String computerName;


        public License()
        {
            // construtor sem parâmetros
        }

        public License(int id, int tenantId, String installationKey, DateTime installationDate, String computerName)
        {
            this.id = id;
            this.tenantId = tenantId;
            this.installationKey = installationKey;
            this.installationDate = installationDate;
            this.computerName = computerName;
        }
    }

}
