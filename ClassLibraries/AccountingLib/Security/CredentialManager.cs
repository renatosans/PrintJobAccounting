using System;
using System.Data.SqlClient;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.AppUtils;


namespace AccountingLib.Security
{
    public class CredentialManager
    {
        private String loginName;

        private String password;

        private SqlConnection sqlConnection;

        private String errorMessage;

        private Login login;

        private Tenant tenant;


        public CredentialManager(String loginName, String password, SqlConnection sqlConnection)
        {
            this.loginName = loginName;
            this.password = password;
            this.sqlConnection = sqlConnection;
        }

        public Boolean ValidateCredentials()
        {
            // Divide o loginName em dominio(empresa) e nome do usuário
            String[] nameParts = loginName.Split(new Char[] { '\\' });
            if (nameParts.Length != 2)
            {
                errorMessage = @"Informar empresa e usuário (Ex.: Datacopy\guest)!";
                return false;
            }

            // Verifica se foi entrado um domínio válido (se empresa existe)
            TenantDAO tenantDAO = new TenantDAO(sqlConnection);
            tenant = tenantDAO.GetTenant(nameParts[0]);
            if (tenant == null)
            {
                errorMessage = "Empresa inexistente!";
                return false;
            }

            // A consulta ao tenant no BD não é case sensitive, devido a isso o tenant é retornado
            // independente se as letras estão em maiúsculas ou minúsculas. Agora faço a comparação
            // case sensitive
            if (tenant.name != nameParts[0])
            {
                errorMessage = "A empresa não confere!";
                return false;
            }

            String username = nameParts[1];
            String userpass = Cipher.GenerateHash(password);

            // A consulta ao username no BD não é case sensitive, devido a isso o login é retornado
            // independente se as letras estão em maiúsculas ou minúsculas. Posteriormente  é  feita
            // uma comparação case sensitive no username ( em loginValidator )
            LoginDAO loginDAO = new LoginDAO(sqlConnection);
            login = loginDAO.GetLogin(tenant.id, username);

            LoginValidator loginValidator = new LoginValidator(login);
            if (!loginValidator.CheckCredentials(username, userpass))
            {
                errorMessage = loginValidator.GetLastError();
                return false;
            }

            // Se todas a verificações foram bem sucedidas retorna status de sucesso
            return true;
        }

        public String GetLastError()
        {
            return errorMessage;
        }

        public Login GetLogin()
        {
            return login;
        }

        public Tenant GetTenant()
        {
            return tenant;
        }
    }

}
