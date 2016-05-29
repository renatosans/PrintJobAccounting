using System;
using System.IO;
using System.Text;
using AccountingLib.Spool.EMF;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;


namespace AccountingLib.Spool
{
    public class SpooledJob
    {
        private String filePath;
        public DateTime FileDate;
        public Boolean Processed;
        private IListener listener;

        // Utiliza lazy instantiation para evitar consumo de recursos enquanto eles
        // não são necessários
        private String splFilename = null;
        private String shdFilename = null;
        private JobSpoolFile spoolFile = null;
        private JobShadowFile shadowFile = null;


        public JobSpoolFile SpoolFile
        {
            get
            {
                if (splFilename == null)
                    return null;

                if (spoolFile == null) // primeiro acesso, aloca os recursos
                {
                    FileStream spoolFileStream = new FileStream(splFilename, FileMode.Open, FileAccess.Read);
                    BinaryReader spoolReader = new BinaryReader(spoolFileStream, Encoding.Unicode);
                    try
                    {
                        // Tenta ler o arquivo de spool
                        spoolFile = new EMFSpoolFile(spoolReader, listener);
                        spoolFile.FileSize = (int)spoolFileStream.Length;
                    }
                    finally
                    {
                        // Fecha o arquivo e libera recursos alocados
                        spoolReader.Close();
                        spoolFileStream.Close();
                    }
                }

                return spoolFile;
            }
        }

        public JobShadowFile ShadowFile
        {
            get
            {
                if (shdFilename == null)
                    return null;

                if (shadowFile == null) // primeiro acesso, aloca os recursos
                {
                    FileStream shadowFileStream = new FileStream(shdFilename, FileMode.Open, FileAccess.Read);
                    BinaryReader shadowReader = new BinaryReader(shadowFileStream, Encoding.Unicode);
                    try
                    {
                        // Tenta ler o arquivo Shadow
                        shadowFile = new JobShadowFile(shadowReader);
                    }
                    finally
                    {
                        // Fecha o arquivo e libera recursos alocados
                        shadowReader.Close();
                        shadowFileStream.Close();
                    }
                }

                return shadowFile;
            }
        }

        /// <summary>
        /// Classe que gerencia os arquivos de spool (.SPL e .SHD), recebe como parâmetro ao ser instanciada
        /// o caminho do arquivo contendo informações do job (Shadow file)
        /// </summary>
        public SpooledJob(String filePath, IListener listener)
        {
            this.filePath = filePath;
            this.FileDate = DateTime.Now; // A data de inserção do arquivo no spool pode diferir ligeiramente da data do Job
            this.Processed = false;
            this.listener = listener;

            SetInstance();
        }

        /// <summary>
        /// Reseta o objeto para que ele busque os arquivos atualizados no disco
        /// </summary>
        public void ResetInstance()
        {
            this.splFilename = null;
            this.shdFilename = null;
            this.spoolFile = null;
            this.shadowFile = null;

            SetInstance();
        }

        private void SetInstance()
        {
            // Verifica se o arquivo existe e se é um arquivo de shadow
            String shdFilename = this.filePath;
            if ((Path.GetExtension(shdFilename).ToUpper() != ".SHD") || (!File.Exists(shdFilename)))
                return;

            // Verifica se o arquivo de shadow pode ser aberto para leitura ( timeout = 5 segundos )
            if (!FileResource.CanBeOpened(shdFilename, 5000))
                return;

            // O arquivo .SPL costuma estar presente no mesmo diretório, porem as vezes existe um delay ao ser criado
            // então não faz a verificação, e a informação do shadow é suficiente para identificar o job
            String splFilename = Path.ChangeExtension(shdFilename, ".SPL");

            // Se o shadow está disponível para leitura armazena os filenames
            this.splFilename = splFilename;
            this.shdFilename = shdFilename;
        }

        public void CopyFiles(String destDirectory)
        {
            // Verifica se os arquivos estão presentes no disco (não são nulos), a consistência
            // usando File.Exists foi feita previamente
            if ((shdFilename == null) || (splFilename == null))
                return;

            String outputPath = PathFormat.Adjust(destDirectory);
            // Força a criação de diretórios/subdiretórios inexistentes em outputPath
            Directory.CreateDirectory(outputPath);
            File.Copy(shdFilename, outputPath + Path.GetFileName(shdFilename));
            File.Copy(splFilename, outputPath + Path.GetFileName(splFilename));
        }
    }

}
