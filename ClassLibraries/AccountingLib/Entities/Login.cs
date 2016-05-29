using System;
using AccountingLib.Security;


namespace AccountingLib.Entities
{
    public class Login: ILogin
    {
        public int id;

        public int tenantId;

        public String username;

        public String password;  // password protegido através de hash MD5

        public int userGroup;


        public Login()
        {
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
