using System;


namespace AccountingInstaller.Util
{
    public class FrontendInfo
    {
        public String siteName;

        public String installDirectory;


        public FrontendInfo(String siteName, String installDirectory)
        {
            this.siteName = siteName;
            this.installDirectory = installDirectory;
        }
    }

}
