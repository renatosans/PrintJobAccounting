using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class SmtpServerDAO
    {
        private SqlConnection sqlConnection;


        public SmtpServerDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public void RemoveSmtpServer(int smtpServerId)
        {
            ProcedureCall removeSmtpServer = new ProcedureCall("pr_removeSmtpServer", sqlConnection);
            removeSmtpServer.parameters.Add(new ProcedureParam("@smtpServerId", SqlDbType.Int, 4, smtpServerId));
            removeSmtpServer.Execute(false);
        }

        public SmtpServer GetSmtpServer(int tenantId, int smtpServerId)
        {
            ProcedureCall retrieveSmtpServer = new ProcedureCall("pr_retrieveSmtpServer", sqlConnection);
            retrieveSmtpServer.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveSmtpServer.parameters.Add(new ProcedureParam("@smtpServerId", SqlDbType.Int, 4, smtpServerId));
            retrieveSmtpServer.Execute(true);
            List<Object> returnList = retrieveSmtpServer.ExtractFromResultset(typeof(SmtpServer));
            if (returnList.Count == 1)
            {
                return (SmtpServer)returnList[0];
            }
            else
            {
                return null;
            }
        }

        public List<Object> GetAllSmtpServers(int tenantId)
        {
            List<Object> smtpServers;

            ProcedureCall retrieveSmtpServers = new ProcedureCall("pr_retrieveSmtpServer", sqlConnection);
            retrieveSmtpServers.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveSmtpServers.Execute(true);
            smtpServers = retrieveSmtpServers.ExtractFromResultset(typeof(SmtpServer));

            return smtpServers;
        }

        public void SetSmtpServer(SmtpServer smtpServer)
        {
            ProcedureCall storeSmtpServer = new ProcedureCall("pr_storeSmtpServer", sqlConnection);
            storeSmtpServer.parameters.Add(new ProcedureParam("@smtpServerId", SqlDbType.Int, 4, smtpServer.id));
            storeSmtpServer.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, smtpServer.tenantId));
            storeSmtpServer.parameters.Add(new ProcedureParam("@name", SqlDbType.VarChar, 100, smtpServer.name));
            storeSmtpServer.parameters.Add(new ProcedureParam("@address", SqlDbType.VarChar, 100, smtpServer.address));
            storeSmtpServer.parameters.Add(new ProcedureParam("@port", SqlDbType.Int, 4, smtpServer.port));
            storeSmtpServer.parameters.Add(new ProcedureParam("@username", SqlDbType.VarChar, 100, smtpServer.username));
            storeSmtpServer.parameters.Add(new ProcedureParam("@password", SqlDbType.VarChar, 100, smtpServer.password));
            storeSmtpServer.parameters.Add(new ProcedureParam("@hash", SqlDbType.VarChar, 255, smtpServer.hash));
            storeSmtpServer.Execute(false);
        }
    }

}
