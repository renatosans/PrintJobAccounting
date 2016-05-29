using System;


namespace AccountingLib.Printers
{
    public class PrinterPort
    {
        // Nome da porta de impressão
        public String Name;

        // Descrição da porta de impressão
        public String Description;

        // Tipo da porta:  Soma de um ou mais flags {read, write, redirected, netAttached}
        public PortTypeEnum Type;
    }

}
