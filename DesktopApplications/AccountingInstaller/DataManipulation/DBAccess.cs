using System;
using System.IO;
using AccountingInstaller.Util;


namespace AccountingInstaller.DataManipulation
{
    public class DBAccess
    {
        public String server;

        public DBLogin saLogin;


        public DBAccess(String server, DBLogin saLogin)
        {
            this.server = server;
            this.saLogin = saLogin;
        }

        /// <summary>
        /// Constroi o XML com informações de acesso ao banco de dados
        /// </summary>
        public static void BuildDataAccess(String server, String username, String password, String targetDirectory)
        {
            StreamWriter streamWriter = File.CreateText(PathFormat.Adjust(targetDirectory) + "DataAccess.xml");

            String xmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + Environment.NewLine +
                             "<dataaccess>" + Environment.NewLine +
                             "    <server>" + server + "</server>" + Environment.NewLine +
                             "    <username>" + username + "</username>" + Environment.NewLine +
                             "    <password>" + password + "</password>" + Environment.NewLine +
                             "</dataaccess>" + Environment.NewLine;

            streamWriter.Write(xmlContent);
            streamWriter.Close();
        }

        // Verifica se o parâmetro (linha de comando) foi recebido, utilizado em "GetDbAccess"
        private static Boolean ParamRecieved(String param)
        {
            if (String.IsNullOrEmpty(param))
                return false;

            return true;
        }

        /// <summary>
        /// Obtem os parâmetros de conexão com o banco caso eles existam na linha de comando
        /// se não estiverem presentes retorna "null"
        /// </summary>
        public static DBAccess GetDbAccess(String[] args)
        {
            DBAccess dbAccess = null;

            String server = null;
            String username = null;
            String password = null;
            foreach (String argument in args)
            {
                if (argument.ToUpper().Contains("/S:")) // Define o nome do servidor de banco a ser utilizado
                    server = ArgumentParser.GetValue(argument);
                if (argument.ToUpper().Contains("/U:")) // Define o usuário utilizado ao logar no servidor de banco
                    username = ArgumentParser.GetValue(argument);
                if (argument.ToUpper().Contains("/P:")) // Define a senha utilizada ao logar no servidor de banco
                    password = ArgumentParser.GetValue(argument);
            }
            if (ParamRecieved(server) && ParamRecieved(username) && ParamRecieved(password))
            {
                dbAccess = new DBAccess(server, new DBLogin(username, password));
            }

            return dbAccess;
        }
    }

}
