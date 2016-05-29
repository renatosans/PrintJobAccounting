using System;


namespace AccountingInstaller.Util
{
    public class ServicesInfo
    {
        public String installDirectory;

        public String printLogImporterStatus;

        public String copyLogImporterStatus;

        public String reportMailerStatus;


        public ServicesInfo(String installDirectory)
        {
            this.installDirectory = installDirectory;
        }
    }

}
