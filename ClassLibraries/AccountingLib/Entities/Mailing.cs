using System;


namespace AccountingLib.Entities
{
    public class Mailing
    {
        public int id;

        public int tenantId;

        public int smtpServer = 1; // Servidor default de SMTP

        public int frequency;

        public int reportType;

        public String recipients;

        public DateTime lastSend = new DateTime(2000, 01, 01);


        public Mailing()
        {
        }
    }

}
