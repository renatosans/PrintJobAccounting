using System;


namespace AccountingLib.Security
{
    public class LoginValidator
    {
        private ILogin login;

        private String errorMessage;


        public LoginValidator(ILogin login)
        {
            this.login = login;
        }

        public Boolean CheckCredentials(String username, String password)
        {
            if (login == null)
            {
                errorMessage = "Usuário inexistente!";
                return false;
            }

            if (username != login.GetUsername()) // Case sensitive
            {
                errorMessage = "O usuário não confere!";
                return false;
            }

            if (password != login.GetPassword()) // Case sensitive
            {
                errorMessage = "Senha inválida!";
                return false;
            }

            return true;
        }

        public String GetLastError()
        {
            return errorMessage;
        }
    }

}
