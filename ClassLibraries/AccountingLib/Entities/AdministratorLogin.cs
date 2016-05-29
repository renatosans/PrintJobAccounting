using System;
using AccountingLib.Security;


namespace AccountingLib.Entities
{
    public class AdministratorLogin: ILogin
    {
        public int id;

        public String username;

        public String password;  // password protegido através de hash MD5


        public AdministratorLogin()
        {
        }

        public AdministratorLogin(String username, String password)
        {
            this.id = 0;
            this.username = username;
            this.password = password;
        }

        public int GetId()
        {
            return this.id;
        }

        public String GetUsername()
        {
            return this.username;
        }

        public String GetPassword()
        {
            return this.password;
        }
    }

}
