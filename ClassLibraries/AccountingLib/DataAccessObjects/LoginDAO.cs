using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class LoginDAO
    {
        private SqlConnection sqlConnection;


        public LoginDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public void RemoveLogin(int loginId)
        {
            ProcedureCall removeLogin = new ProcedureCall("pr_removeLogin", sqlConnection);
            removeLogin.parameters.Add(new ProcedureParam("@loginId", SqlDbType.Int, 4, loginId));
            removeLogin.Execute(false);
        }

        public Login GetLogin(int tenantId, int loginId)
        {
            ProcedureCall retrieveLogin = new ProcedureCall("pr_retrieveLogin", sqlConnection);
            retrieveLogin.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveLogin.parameters.Add(new ProcedureParam("@loginId", SqlDbType.Int, 4, loginId));
            retrieveLogin.Execute(true);
            List<Object> returnList = retrieveLogin.ExtractFromResultset(typeof(Login));

            // Verifica se retornou apenas um item, mais de um indica falha, nenhum também
            if (returnList.Count != 1) return null;
            
            return (Login)returnList[0];
        }

        public Login GetLogin(int tenantId, String username)
        {
            ProcedureCall retrieveLogin = new ProcedureCall("pr_retrieveLogin", sqlConnection);
            retrieveLogin.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveLogin.parameters.Add(new ProcedureParam("@username", SqlDbType.VarChar, 100, username));
            retrieveLogin.Execute(true);
            List<Object> returnList = retrieveLogin.ExtractFromResultset(typeof(Login));

            // Verifica se retornou apenas um item, mais de um indica falha, nenhum também
            if (returnList.Count != 1) return null;
            
            return (Login) returnList[0];
        }

        public List<Object> GetAllLogins(int tenantId)
        {
            List<Object> logins;

            ProcedureCall retrieveLogins = new ProcedureCall("pr_retrieveLogin", sqlConnection);
            retrieveLogins.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveLogins.Execute(true);
            logins = retrieveLogins.ExtractFromResultset(typeof(Login));

            return logins;
        }

        public void SetLogin(Login login)
        {
            ProcedureCall storeLogin = new ProcedureCall("pr_storeLogin", sqlConnection);
            storeLogin.parameters.Add(new ProcedureParam("@loginId", SqlDbType.Int, 4, login.id));
            storeLogin.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, login.tenantId));
            storeLogin.parameters.Add(new ProcedureParam("@username", SqlDbType.VarChar, 100, login.username));
            storeLogin.parameters.Add(new ProcedureParam("@password", SqlDbType.VarChar, 100, login.password));
            storeLogin.parameters.Add(new ProcedureParam("@userGroup", SqlDbType.Int, 4, login.userGroup));
            storeLogin.Execute(false);
        }
    }

}
