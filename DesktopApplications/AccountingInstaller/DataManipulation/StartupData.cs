using System;
using System.Data.SqlClient;


namespace AccountingInstaller.DataManipulation
{
    public class StartupData
    {
        private DBAccess saAccess;

        private SqlConnection sqlConnection;

        private String lastError;
        

        public StartupData(DBAccess saAccess)
        {
            this.saAccess = saAccess;
            this.sqlConnection = new SqlConnection();
        }

        private Boolean OpenConnection()
        {
            if (saAccess == null) return false;

            try
            {
                sqlConnection.ConnectionString = @"Data Source=" + saAccess.server + ";User=" + saAccess.saLogin.username + "; password=" + saAccess.saLogin.password;
                sqlConnection.Open();
            }
            catch
            {
                return false;
            }

            // Se não houve nenhuma falha retorna status de sucesso
            return true;
        }

        private void CloseConnection()
        {
            sqlConnection.Close();
        }

        public Boolean Create()
        {
            if (!OpenConnection()) return false;

            DBQuery dbQuery;
            try
            {
                dbQuery = new DBQuery(sqlConnection);
                // Muda para o bando de dados "AppCommon"
                dbQuery.Query = "use AppCommon";
                dbQuery.Execute(false);
            }
            catch (Exception exc)
            {
                lastError = "Não foi possível acessar o database AppCommon. " + exc.Message;
                return false;
            }

            try
            {
                // Cria um login de administrador na tabela "tb_administratorLogin"
                dbQuery.Query = "INSERT INTO tb_administratorLogin VALUES ('admin', '1E588BE3A984524C7F2C278686F44E72')";
                dbQuery.Execute(false);
                // Insere os aplicativos na tabela "tb_application"
                dbQuery.Query = "INSERT INTO tb_application VALUES ('Print Accounting', 0) SELECT SCOPE_IDENTITY() id";
                dbQuery.Execute(true);
                int? accountingAppId = dbQuery.ExtractFromResultset();
                dbQuery.Query = "INSERT INTO tb_application VALUES ('Remote Device Management', 1)";
                dbQuery.Execute(false);
                dbQuery.Query = "INSERT INTO tb_application VALUES ('Enterprise Content Management', 0)";
                dbQuery.Execute(false);
                dbQuery.Query = "INSERT INTO tb_application VALUES ('Variable Data Printing', 1)";
                dbQuery.Execute(false);
                // Insere os parâmetros de aplicativo na tabela "tb_applicationParam"
                dbQuery.Query = "INSERT INTO tb_applicationParam VALUES ('interval', '599000', " + accountingAppId.Value + ", 'reportMailing')";
                dbQuery.Execute(false);
                dbQuery.Query = "INSERT INTO tb_applicationParam VALUES ('url', 'http://www.datacopy.com.br/Datacount', " + accountingAppId.Value + ", 'webAccounting')";
                dbQuery.Execute(false);
            }
            catch (Exception exc)
            {
                lastError = "Não foi possível inserir dados na tabelas. " + exc.Message;
                return false;
            }

            CloseConnection();
            return true;
        }

        public String GetLastError()
        {
            return lastError;
        }
    }

}
