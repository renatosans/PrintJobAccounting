using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class LicenseDAO
    {
        private SqlConnection sqlConnection;

        public LicenseDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public License GetLicense(int tenantId, int licenseId)
        {
            ProcedureCall retrieveLicense = new ProcedureCall("pr_retrieveLicense", sqlConnection);
            retrieveLicense.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveLicense.parameters.Add(new ProcedureParam("@licenseId", SqlDbType.Int, 4, licenseId));
            retrieveLicense.Execute(true);
            List<Object> returnList = retrieveLicense.ExtractFromResultset(typeof(License));

            // Verifica se retornou apenas um item, mais de um indica falha, nenhum também
            if (returnList.Count != 1) return null;
            
            return (License) returnList[0];
        }

        public List<Object> GetAllLicenses(int tenantId)
        {
            List<Object> licenses;

            ProcedureCall retrieveLicenses = new ProcedureCall("pr_retrieveLicense", sqlConnection);
            retrieveLicenses.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            retrieveLicenses.Execute(true);
            licenses = retrieveLicenses.ExtractFromResultset(typeof(License));

            return licenses;
        }

        public void SetLicense(License license)
        {
            ProcedureCall storeLicense = new ProcedureCall("pr_storeLicense", sqlConnection);
            storeLicense.parameters.Add(new ProcedureParam("@licenseId", SqlDbType.Int, 4, license.id));
            storeLicense.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, license.tenantId));
            storeLicense.parameters.Add(new ProcedureParam("@installationKey", SqlDbType.VarChar, 255, license.installationKey));
            storeLicense.parameters.Add(new ProcedureParam("@installationDate", SqlDbType.DateTime, 8, license.installationDate));
            storeLicense.parameters.Add(new ProcedureParam("@computerName", SqlDbType.VarChar, 100, license.computerName));
            storeLicense.Execute(false);
        }
    }

}
