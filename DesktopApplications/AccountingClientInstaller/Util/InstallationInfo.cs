using System;


namespace AccountingClientInstaller.Util
{
    public class InstallationInfo
    {
        public String TargetDirectory;

        public String PrintLogDirectories;

        public String CopyLogDirectory;


        public InstallationInfo()
        {
            // construtor sem parâmetros, necessário para serialização
        }

        public InstallationInfo(String targetDirectory, String printLogDirectories, String copyLogDirectory)
        {
            this.TargetDirectory = targetDirectory;
            this.PrintLogDirectories = printLogDirectories;
            this.CopyLogDirectory = copyLogDirectory;
        }
    }

}
