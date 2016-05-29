using System;
using System.IO;
using System.Web;
using System.Threading;
using System.Reflection;


namespace DocMageFramework.FileUtils
{
    public static class FileResource
    {
        public static String MapWebResource(HttpServerUtility Server, String fileName)
        {
            String baseDir = Server.MapPath("App_Data");
            String resourceLocation = PathFormat.Adjust(baseDir) + fileName;

            return resourceLocation;
        }

        public static String MapDesktopResource(String fileName)
        {
            String baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.ToString());
            String resourceLocation = PathFormat.Adjust(baseDir) + fileName;

            return resourceLocation;
        }

        public static Boolean TryDelete(String filePath)
        {
            try
            {
                File.Delete(filePath);
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        private static Boolean TryOpen(String filePath)
        {
            try
            {
                // Apenas verifica se é possivel abrir o arquivo, não mantém ele aberto
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                // Fecha o arquivo e libera recursos
                fileStream.Close();
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        public static Boolean TryMove(String sourceFilePath, String destFilePath)
        {
            try
            {
                File.Move(sourceFilePath, destFilePath);
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Verifica se o recurso(arquivo) pode ser aberto, desiste caso o timeout(em milisegundos)
        /// seja atingido
        /// </summary>
        public static Boolean CanBeOpened(String filePath, int timeout)
        {
            // Faz a primeira tentativa de abertura do arquivo ( fora do loop e sem delay ), caso
            // não seja aberto tentará novamente abaixo até o timeout
            if (TryOpen(filePath))
                return true;

            DateTime startTime = DateTime.Now;
            Boolean fileOpened = false;
            Boolean timeoutExpired = false;

            // Faz outras tentativas de abertura do arquivo utilizando TryOpen()
            while ((!fileOpened) && (!timeoutExpired))
            {
                Thread.Sleep(100);
                TimeSpan elapsedTime = DateTime.Now.Subtract(startTime);

                fileOpened = TryOpen(filePath);
                timeoutExpired = elapsedTime.TotalMilliseconds > timeout;
            }

            return fileOpened; // booleano indicando se o arquivo "foi aberto"/"pode ser aberto"
        }
    }

}
