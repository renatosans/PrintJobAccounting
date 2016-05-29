using System;
using System.Data.SqlClient;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;


namespace AccountingLib.ServerCopyLog
{
    /// <summary>
    /// Controla o acesso que o serviço faz ao BD para armazenar o log de cópias, este acesso deve ocorrer
    /// apenas ao final do dia, pouco depois da meia noite ( importação diária dos logs )
    /// </summary>
    public class CopyLogAccess
    {
        private SqlConnection sqlConnection;


        public CopyLogAccess(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        /// <summary>
        /// Recupera a data de último acesso do banco ( tarefa = copyLogImport )
        /// </summary>
        public DateTime GetLastAccess()
        {
            ApplicationParamDAO applicationParamDAO = new ApplicationParamDAO(sqlConnection);
            ApplicationParam lastAccessParam = applicationParamDAO.GetParam("lastAccess", "copyLogImport");

            return DateTime.Parse(lastAccessParam.value);
        }

        /// <summary>
        /// Armazena a data de último acesso no banco ( tarefa = copyLogImport )
        /// </summary>
        public void SetLastAccess(DateTime date)
        {
            ApplicationParamDAO applicationParamDAO = new ApplicationParamDAO(sqlConnection);
            ApplicationParam lastAccessParam = applicationParamDAO.GetParam("lastAccess", "copyLogImport");
            lastAccessParam.value = date.ToShortDateString();
            applicationParamDAO.SetParam(lastAccessParam);
        }

        /// <summary>
        /// Verifica se a data de último acesso está desatualizada
        /// </summary>
        public Boolean IsOutOfDate(DateTime lastAccess)
        {
            // Obtem a defasagem em dias
            double diff = Math.Abs(((TimeSpan)(lastAccess - DateTime.Now)).TotalDays);
            // Considera como desatualizada se ficou mais de um mês sem ser modificada
            double maxDiff = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            return (diff > maxDiff);
        }
    }

}
