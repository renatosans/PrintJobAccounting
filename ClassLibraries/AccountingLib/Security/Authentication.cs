using System;
using System.Web.SessionState;
using DocMageFramework.AppUtils;


namespace AccountingLib.Security
{
    public static class Authentication
    {
        public static void Authenticate(ILogin login, Object tenant, HttpSessionState session)
        {
            String userToken = login.GetId().ToString() + session.SessionID;

            session.Add("login", login);
            session.Add("tenant", tenant);
            session.Add("hash", Cipher.GenerateHash(userToken));
        }


        public static void Disauthenticate(HttpSessionState session)
        {
            session.Remove("login");
            session.Remove("tenant");
            session.Remove("hash");
        }


        public static Boolean IsAuthenticated(HttpSessionState session)
        {
            ILogin login = (ILogin) session["login"];
            String userToken = "";
            if (login != null)
                userToken = login.GetId().ToString() + session.SessionID;

            if ((String)session["hash"] == Cipher.GenerateHash(userToken))
                return true;

            return false;
        }
    }

}
