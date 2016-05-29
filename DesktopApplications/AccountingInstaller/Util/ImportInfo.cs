using System;


namespace AccountingInstaller.Util
{
    public class ImportInfo
    {
        public Boolean createData; // criação de massa inicial ou importação

        public String dataDirectory; // diretório de importação

        public int? fileCount; // quantidade de arquivos importados


        public ImportInfo(Boolean createData, String dataDirectory, int? fileCount)
        {
            this.createData = createData;
            this.dataDirectory = dataDirectory;
            this.fileCount = fileCount;
        }
    }

}
