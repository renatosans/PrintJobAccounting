using System;


namespace AccountingLib.Printers
{
    /// <summary>
    /// Classe que representa uma impressora lógica, contem algumas das propriedades da impressora
    /// lógica do sistema operacional (incluída através de "adicionar impressora")
    /// </summary>
    public class SysPrinter
    {
        // Nome da impressora
        public String Name;

        // Porta de impressão (Ex.: LPT1, COM1, IP_192.168.0.67)
        public String Port;

        // Indica se é a impressora default instalada no computador
        public Boolean IsDefaultPrinter;

        // Indica se o spool está habilitado para a impressora
        public Boolean SpoolEnabled;

        // Indica se a impressora inicia a impressão do job somente após terminar seu spooling
        public Boolean IsQueued;

        // Indica se a ordem de impressão dos jobs deve ser "Complete First"
        public Boolean DoCompleteFirst;

        // Indica se a impressora mantem os jobs em spool (não exclui)
        public Boolean KeepPrintedJobs;

        // Indica o nome do computador onde se encontra esta impressora lógica
        public String ComputerName;

        // Indica se há suporte a comunicação bidirecional com a impressora
        public Boolean EnableBIDI;

        // Array com as capacidades/características da impressora
        public PrinterCapabilityEnum[] Capabilities;


        public SysPrinter()
        {
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

}
