using System;
using System.IO;
using DocMageFramework.FileUtils;


namespace AccountingLib.ServerPrintLog
{
    /// <summary>
    /// Classe utilitária, não armazena estado, contém métodos para manipular o arquivo de log de impressões
    /// </summary>
    public static class PrintLogFile
    {
        private const String baseName = "papercut-print-log";

        // Formatos dos nomes de arquivo:
        //     daily         Ex.:  papercut-print-log-2009-05-19.csv
        //     montlhy       Ex.:  papercut-print-log-2009-05.csv
        //     all-time      Ex.:  papercut-print-log-all-time.csv

        public static String MountName(String logDirectory, int day, int month, int year)
        {
            String baseDir = PathFormat.Adjust(logDirectory);
            String yearPart = String.Format("-{0:0000}", year);
            String monthPart = String.Format("-{0:00}", month);
            String dayPart = String.Format("-{0:00}", day);

            return baseDir + baseName + yearPart + monthPart + dayPart + ".csv";
        }

        public static String MountName(String logDirectory)
        {
            String baseDir = PathFormat.Adjust(logDirectory);

            return baseDir + baseName + "-all-time.csv";
        }

        public static String MountName(String logDirectory, DateTime logDate)
        {
            String filename;

            int day = logDate.Day;
            int month = logDate.Month;
            int year = logDate.Year;
            filename = MountName(logDirectory, day, month, year);

            return filename;
        }

        public static int? GetToken(String fileName)
        {
            // Remove o caminho e a extensão, mantém apenas o nome do arquivo
            String fileNameWithoutExtension = Path.GetFileName(fileName).Replace(".csv", "");

            string[] tokens = fileNameWithoutExtension.Split(new char[] { '-' });
            if (tokens.Length != 6)
                return null;

            try
            {
                int yearPart = int.Parse(tokens[3]);
                int monthPart = int.Parse(tokens[4]);
                int dayPart = int.Parse(tokens[5]);

                return (yearPart * 12 * 31) + (monthPart * 31) + (dayPart);
            }
            catch
            {
                return null;
            }
        }

        public static DateTime? GetDate(String fileName)
        {
            // Retorna "null" caso fileName não seja recebido
            if (fileName == null) return null;

            // Remove o caminho e a extensão, mantém apenas o nome do arquivo
            String fileNameWithoutExtension = Path.GetFileName(fileName).Replace(".csv", "");

            string[] tokens = fileNameWithoutExtension.Split(new char[] { '-' });
            if (tokens.Length != 6)
                return null;

            try
            {
                int yearPart = int.Parse(tokens[3]);
                int monthPart = int.Parse(tokens[4]);
                int dayPart = int.Parse(tokens[5]);

                return new DateTime(yearPart, monthPart, dayPart);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Procura pelo último arquivo finalizado do diretório, começa tentando por ontem pois
        /// o arquivo de hoje ainda está sofrendo append de logs (não está finalizado)
        /// </summary>
        public static String GetLastFile(String logDirectory)
        {
            String lastFile = null;

            // Cria uma proteção contra loops infinitos (MAX_ATTEMPTS)
            const int MAX_ATTEMPTS = 7; // tenta no máximo varrer todos os dias da semana

            DateTime businessDay = DateTime.Now.AddDays(-1);
            String fileName = MountName(logDirectory, businessDay);
            int attempts = 0;
            while ((!File.Exists(fileName)) && (attempts < MAX_ATTEMPTS))
            {
                businessDay = businessDay.AddDays(-1);
                fileName = MountName(logDirectory, businessDay);
                attempts++;
            }
            if (attempts < MAX_ATTEMPTS) lastFile = fileName;

            return lastFile;
        }
    }

}
