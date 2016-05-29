using System;


namespace AccountingClientInstaller.Util
{
    public class ServiceInfo
    {
        public String Name;

        public String DisplayName;

        public String PathName;
        
        
        public ServiceInfo()
        {
            // constutor sem parâmetros
        }
        
        public ServiceInfo(String name, String displayName, String pathName)
        {
            this.Name = name;
            this.DisplayName = displayName;
            this.PathName = pathName;
        }
    }

}
