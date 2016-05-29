using System;
using System.IO;
using DocMageFramework.FileUtils;


namespace AccountingLib.ServerCopyLog
{
    /// <summary>
    /// Classe utilitária, não armazena estado, contém métodos para manipular o arquivo de log de cópias
    /// </summary>
    public static class CopyLogFile
    {
        /// <summary>
        /// Obtem a data do último "append" no arquivo ( log de cópias )
        /// </summary>
        public static DateTime GetTimeStamp(String fileName)
        {
            return File.GetLastWriteTime(fileName);
        }

        /// <summary>
        /// Armazena um backup do antigo arquivo de log ( ele é recriado todos os dias
        /// quando a copiadora armazena o log de cópias )
        /// </summary>
        public static Boolean StoreOldFile(String filePath, String fileName, String extension)
        {
            Boolean success = false;

            String originalFile = filePath + fileName + extension;
            // Gera uma data de referência para acrescentar ao nome do arquivo de backup
            String referenceDate = GetTimeStamp(originalFile).ToString("yyyy-MM-dd");

            Boolean fileMoved = false;
            int attempts = 0;
            while ((!fileMoved) && (attempts < 10)) // realiza no máximo 10 tentativas
            {
                String suffix = (attempts > 0) ? attempts.ToString() : "";
                String storedFile = filePath + fileName + "_" + referenceDate + ".old" + suffix;

                fileMoved = FileResource.TryMove(originalFile, storedFile);
                attempts++;
            }
            if (attempts < 10) success = true;

            return success;
        }
    }

}
