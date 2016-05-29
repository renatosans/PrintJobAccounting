using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class UserDAO
    {
        private SqlConnection sqlConnection;


        public UserDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public User GetUser(int tenantId, int userId)
        {
            ProcedureCall retrieveUser = new ProcedureCall("pr_retrieveUser", sqlConnection);
            retrieveUser.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveUser.parameters.Add(new ProcedureParam("@userId", SqlDbType.Int, 4, userId));
            retrieveUser.Execute(true);
            List<Object> returnList = retrieveUser.ExtractFromResultset(typeof(User));

            // Verifica se retornou apenas um item, mais de um indica falha, nenhum também
            if (returnList.Count != 1) return null;
            
            return (User) returnList[0];
        }

        public List<Object> GetAllUsers(int tenantId)
        {
            List<Object> userList;

            ProcedureCall retrieveUsers = new ProcedureCall("pr_retrieveUser", sqlConnection);
            retrieveUsers.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveUsers.Execute(true);
            userList = retrieveUsers.ExtractFromResultset(typeof(User));

            return userList;
        }

        public int? SetUser(User user)
        {
            ProcedureCall storeUser = new ProcedureCall("pr_storeUser", sqlConnection);
            storeUser.parameters.Add(new ProcedureParam("@userId", SqlDbType.Int, 4, user.id));
            storeUser.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, user.tenantId));
            storeUser.parameters.Add(new ProcedureParam("@name", SqlDbType.VarChar, 100, user.name));
            storeUser.parameters.Add(new ProcedureParam("@alias", SqlDbType.VarChar, 100, user.alias));
            storeUser.parameters.Add(new ProcedureParam("@quota", SqlDbType.Money, 8, user.quota));
            storeUser.Execute(true);

            return storeUser.ExtractFromResultset(); // retorna o id do usuário
        }

        public void RemoveUser(int userId)
        {
            ProcedureCall removeUser = new ProcedureCall("pr_removeUser", sqlConnection);
            removeUser.parameters.Add(new ProcedureParam("@userId", SqlDbType.Int, 4, userId));
            removeUser.Execute(false);
        }
    }

}
