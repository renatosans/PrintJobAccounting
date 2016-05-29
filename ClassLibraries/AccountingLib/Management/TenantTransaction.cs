using System;
using System.Data.SqlClient;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.AppUtils;


namespace AccountingLib.Management
{
    /// <summary>
    /// Classe responsavel pelo conjunto de operações ao se criar o tenant, por enquanto não está dando "rollback"
    /// caso alguma das operações falhe, será implementado posteriormente se necessário.
    /// </summary>
    public class TenantTransaction
    {
        private Tenant tenant;

        private SqlConnection sqlConnection;


        public TenantTransaction(Tenant tenant, SqlConnection sqlConnection)
        {
            this.tenant = tenant;
            this.sqlConnection = sqlConnection;
        }

        private void SetTenantPreference()
        {
            Preference tenantPreference1 = new Preference();
            tenantPreference1.tenantId = tenant.id;
            tenantPreference1.name = "sysSender";
            tenantPreference1.value = "datacount@datacopy.com.br";
            tenantPreference1.type = "System.String";

            Preference tenantPreference2 = new Preference();
            tenantPreference2.tenantId = tenant.id;
            tenantPreference2.name = "exportFormat";
            tenantPreference2.value = "0"; // Por default exporta para PDF
            tenantPreference2.type = "System.Int32";

            Preference tenantPreference3 = new Preference();
            tenantPreference3.tenantId = tenant.id;
            tenantPreference3.name = "periodEndDate";
            tenantPreference3.value = "1"; // Por default considera o periodo terminando no dia 1 as zero horas (na virada do mês)
            tenantPreference3.type = "System.Int32";

            PreferenceDAO preferenceDAO = new PreferenceDAO(sqlConnection);
            preferenceDAO.SetTenantPreference(tenantPreference1);
            preferenceDAO.SetTenantPreference(tenantPreference2);
            preferenceDAO.SetTenantPreference(tenantPreference3);
        }

        private void SetTenantAccess()
        {
            // Cria dois logins para acesso a conta
            Login login1 = new Login();
            login1.tenantId = tenant.id;
            login1.username = "admin";
            login1.password = "1E588BE3A984524C7F2C278686F44E72";
            login1.userGroup = 0;

            Login login2 = new Login();
            login2.tenantId = tenant.id;
            login2.username = "guest";
            login2.password = "1E588BE3A984524C7F2C278686F44E72";
            login2.userGroup = 1;

            LoginDAO loginDAO = new LoginDAO(sqlConnection);
            loginDAO.SetLogin(login1);
            loginDAO.SetLogin(login2);
        }

        private void SetTenantDefaultSmtp()
        {
            SmtpServer server = new SmtpServer();
            server.tenantId = tenant.id;
            server.name = "Servidor Default";
            server.address = "smtp.gmail.com";
            server.port = 587;
            server.username = "datacount@datacopy.com.br";
            server.password = "datacopy123";
            server.hash = Cipher.GenerateHash(tenant.name + server.name);

            SmtpServerDAO smtpServerDAO = new SmtpServerDAO(sqlConnection);
            smtpServerDAO.SetSmtpServer(server);
        }

        private void SetTenantRootCC()
        {
            CostCenter rootCC = new CostCenter();
            rootCC.tenantId = tenant.id;
            rootCC.name = tenant.alias;
            rootCC.parentId = null;

            CostCenterDAO costCenterDAO = new CostCenterDAO(sqlConnection);
            costCenterDAO.SetCostCenter(rootCC);
        }

        public void Execute()
        {
            TenantDAO tenantDAO = new TenantDAO(sqlConnection);
            CostCenterDAO costCenterDAO = new CostCenterDAO(sqlConnection);

            Tenant storedTenant = tenantDAO.GetTenant(tenant.id);
            // Se o tenant já existe no banco bem como sua massa de dados, então só altera alguns dados
            if (storedTenant != null)
            {
                // seta identificador e nome amigável
                tenantDAO.SetTenant(tenant);
                // seta departamento raiz com o nome do tenant
                CostCenter mainCostCenter = costCenterDAO.GetMainCostCenter(tenant.id);
                mainCostCenter.name = tenant.alias;
                costCenterDAO.SetCostCenter(mainCostCenter);
                return;
            }

            // Cria um novo tenant e sua massa de dados inicial
            int? tenantId = tenantDAO.SetTenant(tenant);
            tenant.id = tenantId.Value;

            SetTenantPreference();
            SetTenantAccess();
            SetTenantDefaultSmtp();
            SetTenantRootCC();
        }
    }

}
