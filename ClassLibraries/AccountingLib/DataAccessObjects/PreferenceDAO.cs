using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class PreferenceDAO
    {
        private SqlConnection sqlConnection;


        public PreferenceDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public Preference GetTenantPreference(int tenantId, String preferenceName)
        {
            ProcedureCall retrieveTenantPreference = new ProcedureCall("pr_retrieveTenantPreference", sqlConnection);
            retrieveTenantPreference.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveTenantPreference.parameters.Add(new ProcedureParam("@preferenceName", SqlDbType.VarChar, 100, preferenceName));
            retrieveTenantPreference.Execute(true);
            List<Object> returnList = retrieveTenantPreference.ExtractFromResultset(typeof(Preference));

            // Verifica se retornou apenas um item, mais de um indica falha, nenhum também            
            if (returnList.Count != 1) return null;
            
            return (Preference) returnList[0];
        }

        public List<Object> GetAllTenantPreferences(int tenantId)
        {
            List<Object> tenantPreferences;

            ProcedureCall retrieveTenantPreferences = new ProcedureCall("pr_retrieveTenantPreference", sqlConnection);
            retrieveTenantPreferences.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveTenantPreferences.Execute(true);
            tenantPreferences = retrieveTenantPreferences.ExtractFromResultset(typeof(Preference));

            return tenantPreferences;
        }

        public void SetTenantPreference(Preference preference)
        {
            ProcedureCall storeTenantPreference = new ProcedureCall("pr_storeTenantPreference", sqlConnection);
            storeTenantPreference.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, preference.tenantId));
            storeTenantPreference.parameters.Add(new ProcedureParam("@preferenceId", SqlDbType.Int, 4, preference.id));
            storeTenantPreference.parameters.Add(new ProcedureParam("@name", SqlDbType.VarChar, 100, preference.name));
            storeTenantPreference.parameters.Add(new ProcedureParam("@value", SqlDbType.VarChar, 255, preference.value));
            storeTenantPreference.parameters.Add(new ProcedureParam("@type", SqlDbType.VarChar, 80, preference.type));
            storeTenantPreference.Execute(false);
        }

        public Preference GetUserPreference(int userId, String preferenceName)
        {
            // not implemented yet
            return null;
        }

        public List<object> GetAllUserPreferences(int userId)
        {
            // not implemented yet
            return null;
        }

        public void SetUserPreferences()
        {
            // not implemented yet
        }
    }

}
