using System;
using System.Data.SqlClient;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;


namespace AccountingLib.ServerPrintLog
{
    /// <summary>
    /// Controla o acesso que o serviço faz ao BD para armazenar o log de impressões, este acesso deve
    /// ocorrer apenas ao final do dia, pouco depois da meia noite ( importação diária dos logs )
    /// </summary>
    public class PrintLogAccess
    {
        private SqlConnection sqlConnection;


        public PrintLogAccess(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        /// <summary>
        /// Recupera a data de último acesso do banco ( tarefa = printLogImport )
        /// </summary>
        public DateTime GetLastAccess()
        {
            ApplicationParamDAO applicationParamDAO = new ApplicationParamDAO(sqlConnection);
            ApplicationParam lastAccessParam = applicationParamDAO.GetParam("lastAccess", "printLogImport");

            return DateTime.Parse(lastAccessParam.value);
        }

        /// <summary>
        /// Armazena a data de último acesso no banco ( tarefa = printLogImport )
        /// </summary>
        public void SetLastAccess(DateTime date)
        {
            ApplicationParamDAO applicationParamDAO = new ApplicationParamDAO(sqlConnection);
            ApplicationParam lastAccessParam = applicationParamDAO.GetParam("lastAccess", "printLogImport");
            lastAccessParam.value = date.ToShortDateString();
            applicationParamDAO.SetParam(lastAccessParam);
        }
    }

}
