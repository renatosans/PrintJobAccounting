using System;
using System.IO;


namespace AccountingClientInstaller.Util
{
    /// <summary>
    /// Classe utilitária que possui métodos para manipular a pasta de destino de uma instalação
    /// Disponível nos dois instaladores ( duplicação proposital de linhas de código, não refatorar )
    /// </summary>
    public class TargetDirectory
    {
        private String path;

        private String lastError;


        public TargetDirectory(String path)
        {
            this.path = path;
        }

        /// <summary>
        /// Prepara o diretório de destino para a cópia de arquivos
        /// </summary>
        public Boolean Mount()
        {
            DirectoryInfo targetDir = new DirectoryInfo(path);
            if (targetDir == null)
            {
                lastError = "Caminho inválido. ";
                return false;
            }

            try // Se o diretório não existir cria o mesmo
            {
                if (!targetDir.Exists) targetDir.Create();
            }
            catch (Exception exc)
            {
                lastError = exc.Message;
                return false;
            }

            if (targetDir.GetFiles().Length != 0)
            {
                lastError = "Existem arquivos no diretório informado. Escolha outro diretório. ";
                return false;
            }

            return true;
        }

        private String GetRootDirectory(FileInfo[] files)
        {
            // Este método apresenta uma limitação, só é possivel utiliza-lo quando existe pelo menos um arquivo
            // no diretório raiz
            int maxLenght = int.MaxValue;
            String rootDir = null;

            foreach (FileInfo file in files)
            {
                if (file.DirectoryName.Length < maxLenght)
                {
                    maxLenght = file.DirectoryName.Length;
                    rootDir = file.DirectoryName;
                }
            }

            return rootDir;
        }

        /// <summary>
        /// Copia uma lista de arquivos para o diretório de destino
        /// </summary>
        public Boolean CopyFilesFrom(FileInfo[] files)
        {
            String rootDir = PathFormat.Adjust(GetRootDirectory(files));

            try
            {
                foreach (FileInfo file in files)
                {
                    String relativePath = file.FullName.Replace(rootDir, ""); // apaga o caminho raiz
                    String destFileName = PathFormat.Adjust(path) + relativePath;
                    FileInfo destFileInfo = new FileInfo(destFileName);

                    // Se o diretório não existir cria o mesmo
                    DirectoryInfo destFilePath = new DirectoryInfo(destFileInfo.DirectoryName);
                    if (!destFilePath.Exists) destFilePath.Create();
                    
                    File.Copy(file.FullName, destFileName, true);
                }
                return true;
            }
            catch (Exception exc)
            {
                lastError = exc.Message;
                return false;
            }
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
