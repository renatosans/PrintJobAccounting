using System;


namespace AccountingInstaller.DataManipulation
{
    public class ContainedFile
    {
        public String fileName;

        public String fileContent;


        public ContainedFile()
        {
            // Construtor default
        }

        public ContainedFile(String fileName, String fileContent)
        {
            this.fileName = fileName;
            this.fileContent = fileContent;
        }
    }

}
