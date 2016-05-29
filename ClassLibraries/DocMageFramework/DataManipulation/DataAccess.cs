using System;
using System.Xml;
using System.Data;
using System.Data.SqlClient;


namespace DocMageFramework.DataManipulation
{
    // Classe singleton para acesso ao banco de dados
    public sealed class DataAccess
    {
        private static DataAccess instance = null;
        private static readonly Object safetyLock = new Object();

        private int simultaneousAccess = 0;
        private SqlConnection sqlConnection;

        private DataAccess()
        {
            // Construtor privado que cria a única instância da classe
            sqlConnection = new SqlConnection();
        }

        // Retorna a instancia da classe
        public static DataAccess Instance
        {
            get
            {
                // Verificar problemas com dataReaders simultaneos, por enquanto desliguei o singleton
                return new DataAccess();
                /*
                lock (safetyLock)
                {
                    if (instance == null)
                    {
                        instance = new DataAccess();
                    }
                    return instance;
                }
                */
            }
        }

        private String GetDatabaseName(DatabaseEnum database)
        {
            switch (database)
            {
                case DatabaseEnum.PrintAccounting:
                    return "Accounting";
                case DatabaseEnum.RemoteDeviceMngmt:
                    return "RemoteDevice";
                case DatabaseEnum.EntContentManagement:
                    return "ContentMngmt";
                case DatabaseEnum.VariableDataPrinting:
                    return "VariableData";
                default:
                    return "AppCommon";
            }
        }

        /// <summary>
        /// Monta a string de conexão a partir de um XML externo com os dados de login e servidor(BD),
        /// também define a base/catálogo inicial
        /// </summary>
        public void MountConnection(String xmlLocation, DatabaseEnum database)
        {
            String databaseName = GetDatabaseName(database);
            MountConnection(xmlLocation, databaseName);
        }

        public void MountConnection(String xmlLocation, String databaseName)
        {
            // Só pode alterar a string de conexão se ninguem estiver usando
            if (simultaneousAccess != 0) return;

            XmlTextReader xmlReader = new XmlTextReader(xmlLocation);
            xmlReader.ReadStartElement("dataaccess");
            String server = xmlReader.ReadElementString("server");
            String username = xmlReader.ReadElementString("username");
            String password = xmlReader.ReadElementString("password");
            xmlReader.ReadEndElement();
            xmlReader.Close();

            sqlConnection.ConnectionString = @"Data Source=" + server + ";Initial Catalog=" + databaseName + ";User=" + username + "; password=" + password;
        }

        public void OpenConnection()
        {
            // Só pode abrir a conexão se ela estiver fechada
            // o primeiro a entrar é quem abre a conexão
            if (simultaneousAccess == 0)
            {
                sqlConnection.Open();
            }

            simultaneousAccess++;
        }

        public void CloseConnection()
        {
            simultaneousAccess--;

            // Só pode fechar a conexão se ninguem estiver usando
            // o último a sair é quem fecha a conexão
            if (simultaneousAccess == 0)
            {
                sqlConnection.Close();
            }
        }

        public SqlConnection GetConnection()
        {
            return sqlConnection;
        }
    }

}
