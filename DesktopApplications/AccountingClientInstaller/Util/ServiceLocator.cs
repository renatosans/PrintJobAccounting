using System;
using System.Management;


namespace AccountingClientInstaller.Util
{
    public static class ServiceLocator
    {
        /// <summary>
        /// Busca os dados de um serviço do windows
        /// </summary>
        public static ServiceInfo LocateWindowsService(String serviceName)
        {
            ServiceInfo windowsService = null;
            
            ManagementObjectSearcher serviceSearcher;
            serviceSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service WHERE Name ='" + serviceName + "'");
            
            ManagementObjectCollection services = serviceSearcher.Get();
            foreach (ManagementObject service in services)
            {
                windowsService = new ServiceInfo();
                windowsService.Name = (String)service["Name"];
                windowsService.DisplayName = (String)service["DisplayName"];
                windowsService.PathName = RemoveQuotes((String)service["PathName"]);
            }
            
            return windowsService;
        }

        /// <summary>
        /// Remove as aspas duplas de uma string
        /// </summary>
        private static String RemoveQuotes(String text)
        {
            // Texto vazio ou de 1 caracter(não possui aspas), retorna o texto original
            if (String.IsNullOrEmpty(text) || (text.Length == 1))
                return text;

            // Texto sem abre aspas/fecha aspas, retorna o texto original
            if ((text[0] != '"') || (text[text.Length - 1] != '"'))
                return text;

            // Remove as aspas
            String processedText = text.Substring(1, text.Length - 2);
            return processedText;
        }
    }

}
