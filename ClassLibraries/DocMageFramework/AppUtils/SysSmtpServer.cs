using System;


namespace DocMageFramework.AppUtils
{
    /// <summary>
    /// Classe utilizada para passagem de dados/atributos de um servidor SMTP
    /// </summary>
    public class SysSmtpServer
    {
        public String name;

        public String address;

        public int port;

        public Boolean requiresTLS;

        public String username;

        public String password;


        public SysSmtpServer(String name, String address, int port)
        {
            this.name = name;
            this.address = address;
            this.port = port;
            this.requiresTLS = true;
        }
    }

}
