using System;
using DocMageFramework.AppUtils;
using DocMageFramework.CustomAttributes;


namespace AccountingLib.Entities
{
    public class SmtpServer
    {
        [ItemId]
        public int id;

        public int tenantId;

        [ItemName]
        public String name;

        public String address;

        public int port;

        public String username;

        public String password;

        public String hash;


        public SmtpServer()
        {
        }

        public SysSmtpServer CreateSysObject()
        {
            SysSmtpServer sysSmtpServer = new SysSmtpServer(name, address, port);
            sysSmtpServer.username = username;
            sysSmtpServer.password = password;

            return sysSmtpServer;
        }
    }

}
