using System;
using System.IO;


namespace DocMageFramework.AppUtils
{
    public class FileLogger
    {
        private String filename;

        private StreamWriter streamWriter;

        public String[] FileHeader = null;


        public FileLogger(String filename)
        {
            this.filename = filename;
        }

        /// <summary>
        /// Muda o caminho do arquivo de saída
        /// </summary>
        public void Relocate(String filename)
        {
            if (filename != this.filename)
                this.filename = filename;
        }

        private void WriteHeader()
        {
            if (FileHeader == null)
                return;

            foreach (String line in FileHeader)
            {
                streamWriter.WriteLine(line);
            }
        }

        private void OpenLogFile()
        {
            if (File.Exists(filename))
            {
                streamWriter = File.AppendText(filename);
            }
            else
            {
                streamWriter = File.CreateText(filename);
                WriteHeader();
            }
        }

        private String Delimiter()
        {
            String delimiter = "";
            delimiter = delimiter.PadLeft(120, '-');

            return delimiter;
        }

        /// <summary>
        /// Insere uma mensagem de erro no log
        /// </summary>
        public void LogError(String error)
        {
            OpenLogFile();
            streamWriter.WriteLine(Delimiter());
            String dateLabel = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            String padding = "";
            padding = padding.PadLeft(dateLabel.Length);
            error = error.Replace(Environment.NewLine, Environment.NewLine + padding);
            streamWriter.WriteLine(dateLabel + " - " + error);
            streamWriter.Close();
        }

        /// <summary>
        /// Insere uma informação no log
        /// </summary>
        public void LogInfo(String info)
        {
            LogInfo(info, false);
        }

        /// <summary>
        /// Insere uma informação no log
        /// </summary>
        public void LogInfo(String info, Boolean startingDelimiter)
        {
            OpenLogFile();
            if (startingDelimiter)
                streamWriter.WriteLine(Delimiter());
            String dateLabel = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            streamWriter.WriteLine(dateLabel + " - " + info);
            streamWriter.Close();
        }

        /// <summary>
        /// Insere uma mensagem sem formatação no arquivo de log
        /// </summary>
        public void LogRawData(String rawData)
        {
            OpenLogFile();
            streamWriter.WriteLine(rawData);
            streamWriter.Close();
        }
    }

}
