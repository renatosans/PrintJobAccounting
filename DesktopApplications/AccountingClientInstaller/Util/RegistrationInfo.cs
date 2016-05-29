using System;


namespace AccountingClientInstaller.Util
{
    /// <summary>
    /// Classe de registro/licença utilizada para permitir a instalação do produto
    /// Disponível também no sistema ( duplicação proposital de linhas de código, não refatorar )
    /// </summary>
    public class RegistrationInfo
    {
        public String ServiceUrl;

        public int TenantId;

        public int LicenseId;

        public String Version;

        public DateTime ExpirationDate;

        public String Hash; // hash MD5 utilizado para segurança do registro


        public RegistrationInfo()
        {
            // construtor sem parâmetros, necessário para serialização
        }

        public RegistrationInfo(String serviceUrl, int tenantId, int licenseId, DateTime expirationDate)
        {
            this.ServiceUrl = serviceUrl;
            this.TenantId = tenantId;
            this.LicenseId = licenseId;
            this.ExpirationDate = expirationDate;
        }

        public License ConvertToLicense()
        {
            License license = new License();
            license.id = this.LicenseId;
            license.tenantId = this.TenantId;
            license.installationKey = ResourceProtector.GenerateHash(ResourceProtector.GetHardwareId());
            license.installationDate = DateTime.Now;
            license.computerName = Environment.MachineName;

            return license;
        }
    }

}
