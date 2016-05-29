using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using DocMageFramework.DataManipulation;
using AccountingLib.Entities;


namespace AccountingLib.DataAccessObjects
{
    public class MailingDAO
    {
        private SqlConnection sqlConnection;


        public MailingDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public void RemoveMailing(int mailingId)
        {
            ProcedureCall removeMailing = new ProcedureCall("pr_removeMailing", sqlConnection);
            removeMailing.parameters.Add(new ProcedureParam("@mailingId", SqlDbType.Int, 4, mailingId));
            removeMailing.Execute(false);
        }

        public Mailing GetMailing(int tenantId, int? mailingId)
        {
            ProcedureCall retrieveMailing = new ProcedureCall("pr_retrieveMailing", sqlConnection);
            retrieveMailing.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveMailing.parameters.Add(new ProcedureParam("@mailingId", SqlDbType.Int, 4, mailingId));
            retrieveMailing.Execute(true);
            List<Object> returnList = retrieveMailing.ExtractFromResultset(typeof(Mailing));
            if (returnList.Count == 1)
            {
                return (Mailing)returnList[0];
            }
            else
            {
                return null;
            }
        }

        public List<Object> GetAllMailings(int tenantId)
        {
            List<Object> mailingList;

            ProcedureCall retrieveMailings = new ProcedureCall("pr_retrieveMailing", sqlConnection);
            retrieveMailings.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveMailings.Execute(true);
            mailingList = retrieveMailings.ExtractFromResultset(typeof(Mailing));

            return mailingList;
        }

        public void SetMailing(Mailing mailing)
        {
            ProcedureCall storeMailing = new ProcedureCall("pr_storeMailing", sqlConnection);
            storeMailing.parameters.Add(new ProcedureParam("@mailingId", SqlDbType.Int, 4, mailing.id));
            storeMailing.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, mailing.tenantId));
            storeMailing.parameters.Add(new ProcedureParam("@smtpServer", SqlDbType.Int, 4, mailing.smtpServer));
            storeMailing.parameters.Add(new ProcedureParam("@frequency", SqlDbType.Int, 4, mailing.frequency));
            storeMailing.parameters.Add(new ProcedureParam("@reportType", SqlDbType.Int, 4, mailing.reportType));
            storeMailing.parameters.Add(new ProcedureParam("@recipients", SqlDbType.VarChar, 255, mailing.recipients));
            storeMailing.parameters.Add(new ProcedureParam("@lastSend", SqlDbType.DateTime, 8, mailing.lastSend));
            storeMailing.Execute(false);
        }
    }

}
