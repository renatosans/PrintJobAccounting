using System;
using System.Text;
using System.Reflection;
using DocMageFramework.AppUtils;


namespace AccountingLib.Management
{
    /// <summary>
    /// Classe utilitária usada para gerar/ler as chaves de ativação do produto
    /// Disponível também no instalador ( duplicação proposital de linhas de código, não refatorar )
    /// </summary>
    public static class LicenseKeyMaker
    {
        public static String GenerateKey(String serviceUrl, int tenantId, int licenseId, DateTime expirationDate)
        {
            String licenseKey;
            
            RegistrationInfo registrationInfo = new RegistrationInfo(serviceUrl, tenantId, licenseId, expirationDate);
            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
            registrationInfo.Version = assemblyName.Version.ToString();
            String hashInput = registrationInfo.ServiceUrl + registrationInfo.TenantId + registrationInfo.LicenseId +
                               registrationInfo.Version + registrationInfo.ExpirationDate.ToString("yyyy-MM-ddTHH:mm:ss");
            registrationInfo.Hash = Cipher.GenerateHash(hashInput);
            Byte[] serializedObject = ObjectSerializer.SerializeObjectToArray(registrationInfo);
            licenseKey = Convert.ToBase64String(serializedObject);

            return licenseKey;
        }

        public static RegistrationInfo ReadKey(String licenseKey, IListener listener)
        {
            RegistrationInfo registrationInfo = null;

            try
            {
                // Caso licenseKey esteja vazia estoura ArgumentNullException, caso licenseKey não
                // esteja em Base64 estoura FormatException e assim por diante
                Byte[] decodedKey = Convert.FromBase64String(licenseKey);
                String serializedObject = Encoding.UTF8.GetString(decodedKey);
                registrationInfo = (RegistrationInfo)ObjectSerializer.DeserializeObject(serializedObject, typeof(RegistrationInfo));
            }
            catch (Exception exception) // O conteudo da licença não é válido
            {
                AddExceptionData(exception, "License Key = " + licenseKey, null);
                listener.NotifyObject(exception);
                return null;
            }

            // Verifica se o conteúdo da licença está no formato esperado
            if (registrationInfo == null)
            {
                listener.NotifyObject(new Exception("O conteúdo da licença não está no formato esperado."));
                return null;
            }

            // Verifica se o usuário tentou forjar uma licença falsa
            if (registrationInfo.Hash == null)
            {
                Exception hashException = new Exception("O hash não estava presente na chave.");
                AddExceptionData(hashException, "License Key = " + licenseKey, null);
                listener.NotifyObject(hashException);
                return null;
            }
            String hashInput = registrationInfo.ServiceUrl + registrationInfo.TenantId + registrationInfo.LicenseId +
                               registrationInfo.Version + registrationInfo.ExpirationDate.ToString("yyyy-MM-ddTHH:mm:ss");
            String hash = Cipher.GenerateHash(hashInput);
            if (registrationInfo.Hash != hash)
            {
                Exception hashException = new Exception("O hash não confere.");
                AddExceptionData(hashException, "License Key = " + licenseKey, "Hash Input = " + hashInput);
                listener.NotifyObject(hashException);
                return null;
            }

            return registrationInfo;
        }

        private static void AddExceptionData(Exception exception, String data1, String data2)
        {
            // Caso os dados sejam pequenos (até 1000 bytes), adiciona na exceção para análise
            String customExceptionData = "";
            if ((!String.IsNullOrEmpty(data1)) && (data1.Length < 1000))
                customExceptionData += data1 + Environment.NewLine + Environment.NewLine;
            if ((!String.IsNullOrEmpty(data2)) && (data2.Length < 1000))
                customExceptionData += data2 + Environment.NewLine + Environment.NewLine;
            exception.Data.Add("customExceptionData", customExceptionData);
        }
    }

}
