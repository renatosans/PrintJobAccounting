using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.DirectoryServices;


namespace AccountingInstaller.Util
{
    public class IISHandler
    {
        private String lastError;


        public IISHandler()
        {
            lastError = null;
        }

        /// <summary>
        /// Obtem o diretório wwwRoot do IIS
        /// </summary>
        public static String GetWebRootDirectory()
        {
            String wwwRoot;

            try
            {
                String regPath = @"SOFTWARE\Microsoft\InetStp";

                RegistryKey regKey = Registry.LocalMachine.OpenSubKey(regPath);
                // Obtem o diretório a partir da chave de registro
                wwwRoot = (String)regKey.GetValue("PathWWWRoot");
                regKey.Close();
            }
            catch
            {
                // Retorna null em caso de falha
                wwwRoot = null;
            }

            return wwwRoot;
        }

        /// <summary>
        /// Cria um diretório virtual no IIS e configura uma aplicação web neste diretório
        /// </summary>
        public Boolean CreateVirtualDirectory(String name, String path, String startPage)
        {
            // Obtem a entrada para o site padrão no IIS
            DirectoryEntry defaultWebSite = new DirectoryEntry("IIS://localhost/W3SVC/1/Root");
            String schemaClassName = "";
            try
            {
                // Caso não consiga popular o objeto COM dispara um COM Exception
                schemaClassName = defaultWebSite.SchemaClassName;
            }
            catch (Exception exc)
            {
                lastError = "Não foi possível configurar o IIS. (" + exc.Message + ")" + Environment.NewLine +
                            "Verifique se o IIS está instalado e também os módulos de compatibilidade com versões anteriores.";
                return false;
            }

            if ((defaultWebSite == null) || (schemaClassName != "IIsWebVirtualDir"))
            {
                lastError = "Não foi possivel abrir o site padrão do IIS." + Environment.NewLine +
                            "Verifique se o IIS está instalado e também os módulos de compatibilidade com versões anteriores.";
                return false;
            }

            // Verifica se o diretório já está mapeado no IIS
            foreach (DirectoryEntry dirEntry in defaultWebSite.Children)
            {
                if (dirEntry.Name == name)
                {
                    // Aborta caso o diretório esteja mapeado
                    lastError = "O diretório já existe no IIS. Escolha outro nome.";
                    return false;
                }
            }
            
            // Cria o diretório virtual
            DirectoryEntry appVirtualDir = defaultWebSite.Children.Add(name, "IIsWebVirtualDir");

            // Houve uma falha caso continue "null" apos tentativa de criação
            if (appVirtualDir == null)
            {
                lastError = "Não foi possivel criar o diretório virtual no IIS";
                return false;
            }

            try
            {
                // Configura as propriedades do diretório virtual
                appVirtualDir.Properties["Path"][0] = path;
                appVirtualDir.Properties["AccessRead"][0] = true;
                appVirtualDir.Properties["AccessExecute"][0] = true;
                appVirtualDir.Properties["AccessScript"][0] = true;
                appVirtualDir.Properties["AccessWrite"][0] = false;
                appVirtualDir.Properties["EnableDirBrowsing"][0] = false;
                appVirtualDir.Properties["AppIsolated"][0] = 1;
                appVirtualDir.Properties["EnableDefaultDoc"][0] = true;
                appVirtualDir.Properties["DefaultDoc"][0] = startPage;
                appVirtualDir.CommitChanges();

                // Configura a aplicação web no diretório
                appVirtualDir.Invoke("AppCreate", 1);
                appVirtualDir.Properties["AppFriendlyName"][0] = name + "App";
                appVirtualDir.CommitChanges();
            }
            catch (Exception exc)
            {
                lastError = exc.Message;
                return false;
            }

            // Se nenhum erro ocorreu retorna status de sucesso
            return true;
        }

        /// <summary>
        /// Reseta o serviço do Internet Information Services
        /// </summary>
        public Boolean ResetIIS()
        {
            try
            {
                ServiceHandler.StopService("W3SVC", 9000);
                ServiceHandler.StartService("W3SVC", 9000);
            }
            catch (Exception exc)
            {
                lastError = exc.Message;
                return false;
            }

            return true;
        }

        private static Boolean IsAspNetAllowed()
        {
            return true;
        }

        /// <summary>
        /// Verifica se o IIS está com a extensão ASP.NET 2.0 habilitada
        /// </summary>
        public static Boolean IsAspNetRegistered()
        {
            String aspNetDll;

            try
            {
                String regPath = @"SOFTWARE\Microsoft\ASP.NET\2.0.50727.0";

                RegistryKey regKey = Registry.LocalMachine.OpenSubKey(regPath);
                // Obtem a dll do Asp.Net a partir da chave de registro
                aspNetDll = (String)regKey.GetValue("DllFullPath");
                regKey.Close();
            }
            catch
            {
                // Retorna "false" em caso de falha
                return false;
            }

            // Confere o nome da dll (extensão/filtro ISAPI do ASP.NET)
            if (!aspNetDll.Contains("aspnet_isapi.dll")) return false;

            // Verifica se a extensão está marcada como "Permitir" no IIS
            if (!IsAspNetAllowed()) return false;

            // Caso tenha passado nas verificações retorna "true"
            return true;
        }

        /// <summary>
        /// Retorna a última exceção ou erro registrado na instância desta classe
        /// </summary>
        public String GetLastError()
        {
            return lastError;
        }
    }

}
