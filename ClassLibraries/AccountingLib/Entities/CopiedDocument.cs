using System;


namespace AccountingLib.Entities
{
    public class CopiedDocument
    {
        public int jobId;

        public int tenantId;

        public DateTime jobTime;

        public String userName;

        public String printerName;

        public int pageCount;

        public Boolean duplex;

        public Boolean color;


        public CopiedDocument()
        {
        }
    }

}
