using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using DocMageFramework.DataManipulation;
using AccountingLib.Entities;


namespace AccountingLib.DataAccessObjects
{
    public class TenantDAO
    {
        private SqlConnection sqlConnection;


        public TenantDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public Tenant GetTenant(int tenantId)
        {
            ProcedureCall retrieveTenant = new ProcedureCall("pr_retrieveTenant", sqlConnection);
            retrieveTenant.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveTenant.Execute(true);
            List<Object> returnList = retrieveTenant.ExtractFromResultset(typeof(Tenant));

            // Verifica se retornou apenas um item, mais de um indica falha, nenhum também
            if (returnList.Count != 1) return null;

            return (Tenant)returnList[0];
        }

        public Tenant GetTenant(String tenantName)
        {
            ProcedureCall retrieveTenant = new ProcedureCall("pr_retrieveTenant", sqlConnection);
            retrieveTenant.parameters.Add(new ProcedureParam("@tenantName", SqlDbType.VarChar, 100, tenantName));
            retrieveTenant.Execute(true);
            List<Object> returnList = retrieveTenant.ExtractFromResultset(typeof(Tenant));

            // Verifica se retornou apenas um item, mais de um indica falha, nenhum também
            if (returnList.Count != 1) return null;
            
            return (Tenant)returnList[0];
        }

        public List<Object> GetAllTenants()
        {
            List<Object> tenantList;

            ProcedureCall retrieveTenants = new ProcedureCall("pr_retrieveTenant", sqlConnection);
            retrieveTenants.Execute(true);
            tenantList = retrieveTenants.ExtractFromResultset(typeof(Tenant));

            return tenantList;
        }

        public int? SetTenant(Tenant tenant)
        {
            ProcedureCall storeTenant = new ProcedureCall("pr_storeTenant", sqlConnection);
            storeTenant.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenant.id));
            storeTenant.parameters.Add(new ProcedureParam("name", SqlDbType.VarChar, 100, tenant.name));
            storeTenant.parameters.Add(new ProcedureParam("@alias", SqlDbType.VarChar, 100, tenant.alias));
            storeTenant.Execute(true);

            return storeTenant.ExtractFromResultset(); // retorna o id da empresa
        }

        public void RemoveTenant(int tenantId)
        {
            ProcedureCall removeTenant = new ProcedureCall("pr_removeTenant", sqlConnection);
            removeTenant.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            removeTenant.Execute(false);
        }
    }

}
