using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Data.SqlClient;
using Util;
using DataManipulation;


namespace BackupUtility
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            foreach (String argument in args)
            {
                // Deve exportar as tabelas do banco
                if (argument.ToUpper().Contains("/E"))
                {
                    // Busca parâmetros de conexão na linha de comando, caso existam
                    DBAccess saAccess = null;
                    saAccess = DBAccess.GetDbAccess(args);

                    // Cria o diretório onde para onde os dados serão exportados
                    String baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.ToString());
                    String dataDirectory = PathFormat.Adjust(baseDir) + "Data";
                    Directory.CreateDirectory(dataDirectory);

                    // Executa a exportação dos databases "AppCommon" e "Accounting"
                    Recovery recovery = new Recovery(saAccess, dataDirectory);
                    recovery.DBExport("AppCommon");
                    recovery.DBExport("Accounting");

                    return;
                }

            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

}
