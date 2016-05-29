using System;


namespace AccountingLib.Entities
{
    public class PrintedDocument
    {
        public int jobId;

        public int tenantId;

        public DateTime jobTime;

        public String userName;

        public String printerName;

        public String name;

        public int pageCount;

        public int copyCount;

        public Boolean duplex;

        public Boolean color;


        public PrintedDocument()
        {
        }
    }

}
