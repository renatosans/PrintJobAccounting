using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Security.Principal;
using AccountingInstaller.Util;
using AccountingInstaller.DataManipulation;


namespace AccountingInstaller
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            String scriptsLocation = null;
            Boolean exportData = false;
            int formNum = -1;
            foreach (String argument in args)
            {
                // Deve compilar os scripts de banco (T-SQL) em um XML
                if (argument.ToUpper().Contains("/X:"))
                    scriptsLocation = ArgumentParser.GetValue(argument);
                // Deve exportar as tabelas do banco
                if (argument.ToUpper().Contains("/E"))
                    exportData = true;
                // Recebe como argumento o número do Form por onde a execução deve começar
                if (argument.ToUpper().Contains("/F:"))
                    formNum = ArgumentParser.GetValue(argument, -1);
                // Recebe como argumento o servidor de banco, o usuário e senha
                // Faz a verificação .Contains("/S:") .Contains("/U:") .Contains("/P:") dentro do
                // método DBAccess.GetDbAccess(args)
            }

            // Compila os scripts de banco caso o parâmetro /X tenha sido recebido bem como o caminho
            if (scriptsLocation != null)
            {
                BuildScriptsXml(scriptsLocation);
                return; // Encerra a execução do instalador
            }

            // Busca parâmetros de conexão na linha de comando, caso existam
            DBAccess saAccess = null;
            saAccess = DBAccess.GetDbAccess(args);

            // Exporta as tabelas caso o parâmetro /E tenha sido recebido bem como os parâmetros de conexão
            if ((exportData) && (saAccess != null))
            {
                ExportData(saAccess);
                return; // Encerra a execução do instalador
            }

            // Verifica se o instalador está sendo executado com permissões administrativas
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(windowsIdentity);
            Boolean executingAsAdmin = windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);

            // Para debugar este programa execute o Visual Studio como administrador, caso contrário
            // o programa não vai parar nos "breakpoints" (isso se deve ao código de controle do UAC)
            Process process = Process.GetCurrentProcess();
            if ((process.ProcessName.ToUpper().Contains("VSHOST")) && (!executingAsAdmin))
            {
                String errorMessage = "Execute o Visual Studio com permissões administrativas para debugar!";
                MessageBox.Show(errorMessage, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Verifica se a caixa de dialogo do UAC (User Account Control) é necessária
            if (!executingAsAdmin)
            {
                // Pede elevação de privilégios (executa como administrador se o usuário concordar), o programa
                // atual é encerrado e uma nova instancia é executada com os privilégios concedidos
                ProcessStartInfo processInfo = new ProcessStartInfo();
                processInfo.Verb = "runas";
                processInfo.FileName = Application.ExecutablePath;
                try { Process.Start(processInfo); } catch { }
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(saAccess, formNum));
        }


        // Gera o container de scripts (arquivo XML), recebe como parâmetro a localização dos scripts que é
        // o diretório raiz de uma estrutura de diretórios contendo scripts T-SQL
        private static void BuildScriptsXml(String scriptsLocation) // exemplo: "C:\Users\renato\Desktop\Scripts"
        {
            ContainerHandler containerHandler = new ContainerHandler();
            containerHandler.BuildContainer("ScriptFiles.xml", scriptsLocation);
        }

        // Exporta a massa de dados contida no banco de dados (dados das tabelas)
        private static void ExportData(DBAccess saAccess)
        {
            // Cria o diretório onde para onde os dados serão exportados
            String baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.ToString());
            String dataDirectory = PathFormat.Adjust(baseDir) + "Data";
            Directory.CreateDirectory(dataDirectory);

            // Executa a exportação dos databases "AppCommon" e "Accounting"
            Recovery recovery = new Recovery(saAccess, dataDirectory);
            recovery.DBExport("AppCommon");
            recovery.DBExport("Accounting");
        }
    }

}
