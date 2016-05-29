using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class AdministratorLoginDAO
    {
        private SqlConnection sqlConnection;


        public AdministratorLoginDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public AdministratorLogin GetLogin(int loginId)
        {
            ProcedureCall retrieveLogin = new ProcedureCall("pr_retrieveAdministratorLogin", sqlConnection);
            retrieveLogin.parameters.Add(new ProcedureParam("@loginId", SqlDbType.Int, 4, loginId));
            retrieveLogin.Execute(true);
            List<Object> returnList = retrieveLogin.ExtractFromResultset(typeof(AdministratorLogin));
            
            // Verifica se retornou apenas um item, mais de um indica falha, nenhum também
            if (returnList.Count != 1) return null;

            return (AdministratorLogin)returnList[0];
        }

        public AdministratorLogin GetLogin(String username)
        {
            ProcedureCall retrieveLogin = new ProcedureCall("pr_retrieveAdministratorLogin", sqlConnection);
            retrieveLogin.parameters.Add(new ProcedureParam("@username", SqlDbType.VarChar, 100, username));
            retrieveLogin.Execute(true);
            List<Object> returnList = retrieveLogin.ExtractFromResultset(typeof(AdministratorLogin));

            // Verifica se retornou apenas um item, mais de um indica falha, nenhum também
            if (returnList.Count != 1) return null;
            
            return (AdministratorLogin) returnList[0];
        }

        public List<Object> GetAllLogins()
        {
            List<Object> administratorLogins;

            ProcedureCall retrieveLogin = new ProcedureCall("pr_retrieveAdministratorLogin", sqlConnection);
            retrieveLogin.Execute(true);
            administratorLogins = retrieveLogin.ExtractFromResultset(typeof(AdministratorLogin));

            return administratorLogins;
        }

        public void SetLogin(AdministratorLogin login)
        {
            ProcedureCall storeLogin = new ProcedureCall("pr_storeAdministratorLogin", sqlConnection);
            storeLogin.parameters.Add(new ProcedureParam("@loginId", SqlDbType.Int, 4, login.id));
            storeLogin.parameters.Add(new ProcedureParam("@username", SqlDbType.VarChar, 100, login.username));
            storeLogin.parameters.Add(new ProcedureParam("@password", SqlDbType.VarChar, 100, login.password));
            storeLogin.Execute(false);
        }
    }

}
