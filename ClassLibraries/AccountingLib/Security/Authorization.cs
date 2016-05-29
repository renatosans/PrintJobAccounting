using System;
using System.Web.SessionState;


namespace AccountingLib.Security
{
    public static class Authorization
    {
        public static Boolean AuthorizedAsAdministrator(HttpSessionState session)
        {
            AccountingLib.Entities.Login login = (AccountingLib.Entities.Login) session["login"];
            UserGroupEnum userGroup = UserGroupEnum.CompanyMembers;
            if (login != null)
                userGroup = (UserGroupEnum) login.userGroup;

            if (userGroup == UserGroupEnum.Administrators)
                return true;

            return false;
        }

        public static String GetWarning()
        {
            String warningMessage = "Você não possui autorização para acessar esta página.<br>" +
                             "Consulte o administrador do sistema para mais informações.<br>";
            return warningMessage;
        }
    }

}
