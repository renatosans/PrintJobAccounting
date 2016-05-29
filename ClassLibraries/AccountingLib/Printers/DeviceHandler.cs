using System;
using Microsoft.Win32;
using System.Management;
using System.Collections.Generic;
using DocMageFramework.AppUtils;
using DocMageFramework.JobExecution;


namespace AccountingLib.Printers
{
    public static class DeviceHandler
    {
        /// <summary>
        /// Busca as impressoras disponíveis no servidor de impressão
        /// </summary>
        private static ManagementObjectCollection GetServerPrinters()
        {
            ManagementObjectSearcher printerSearcher;
            printerSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            return printerSearcher.Get();
        }

        /// <summary>
        /// Busca os dados de uma impressora lógica no sistema operacional. Caso duas ou mais
        /// impressoras possuam o mesmo nome a última encontrada é a que será retornada
        /// </summary>
        public static SysPrinter LocateSystemPrinter(String printerName)
        {
            SysPrinter sysPrinter = null;

            ManagementObjectSearcher printerSearcher;
            printerSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer WHERE Name ='" + printerName + "'");

            ManagementObjectCollection printers = printerSearcher.Get();
            foreach (ManagementObject printer in printers)
            {
                sysPrinter = new SysPrinter();
                sysPrinter.Name = (String)printer["Name"];
                sysPrinter.Port = (String)printer["PortName"];
                sysPrinter.IsDefaultPrinter = (Boolean)printer["Default"];
                sysPrinter.SpoolEnabled = (Boolean)printer["SpoolEnabled"];
                sysPrinter.IsQueued = (Boolean)printer["Queued"];
                sysPrinter.DoCompleteFirst = (Boolean)printer["DoCompleteFirst"];
                sysPrinter.KeepPrintedJobs = (Boolean)printer["KeepPrintedJobs"];
                sysPrinter.ComputerName = (String)printer["SystemName"];
                sysPrinter.Capabilities = (PrinterCapabilityEnum[])printer["Capabilities"];
            }
           
            return sysPrinter;
        }

        /// <summary>
        /// Obtem todas as portas de impressão e cria um dicionário de dados com elas
        /// </summary>
        private static Dictionary<String, PrinterPort> MapPrinterPorts()
        {
            Dictionary<String, PrinterPort> portDic = new Dictionary<String, PrinterPort>();

            List<PrinterPort> printerPorts = PortHandler.GetAllPorts();
            foreach (PrinterPort port in printerPorts)
            {
                portDic.Add(port.Name, port);
            }

            return portDic;
        }

        /// <summary>
        /// Verifica se a impressora está ligada em rede (através de porta TCP/IP)
        /// Localiza a porta da impressora no dicionário de portas e checa a propriedade "netAttached"
        /// </summary>
        private static Boolean IsNetworkAttached(ManagementObject printer, Dictionary<String, PrinterPort> portDic)
        {
            Boolean isNetworkAttached = false;

            // Verifica se a porta da impressora está presente no dicionário
            if (!portDic.ContainsKey((String)printer["PortName"])) return false;

            PrinterPort printerPort = portDic[(String)printer["PortName"]];
            if (printerPort != null)
            {
                if ((printerPort.Type & PortTypeEnum.netAttached) == PortTypeEnum.netAttached)
                {
                    isNetworkAttached = true;
                }
            }

            return isNetworkAttached;
        }

        /// <summary>
        /// Verifica se a impressora imprime em cores
        /// </summary>
        public static Boolean IsColorPrinter(String printerName)
        {
            Boolean isColorPrinter = false;

            SysPrinter printer = LocateSystemPrinter(printerName);
            foreach (PrinterCapabilityEnum capability in printer.Capabilities)
            {
                if (capability == PrinterCapabilityEnum.ColorPrinting) isColorPrinter = true;
            }

            return isColorPrinter;
        }

        /// <summary>
        /// Mapeia as impressoras lógicas disponíveis para utilização no sistema de accounting
        /// Apenas impressoras associadas a um hardware e com spool habilitado, o parâmetro
        /// machineName pode ser fornecido para filtrar impressoras instaladas no computador
        /// com esse nome (ou null para considarar também impressoras compartilhadas por outros
        /// computadores)
        /// </summary>
        public static List<SysPrinter> MapSystemPrinters(String machineName)
        {
            // Lista as impressoras lógicas (incluídas no Sistema Operacional através de
            // "adicionar impressora"), as impressoras físicas podem ser localizadas através
            // de mensagens SNMP quando estão ligadas e escutando a porta 161
            List<SysPrinter> sysPrinters = new List<SysPrinter>();

            Dictionary<String, PrinterPort> portDic = MapPrinterPorts();
            ManagementObjectCollection printers = GetServerPrinters();

            foreach (ManagementObject printer in printers)
            {
                // Verifica se a impressora está associada a um dispositivo físico (hardware)
                Boolean hasPhysicalDevice = IsNetworkAttached(printer, portDic) || (Boolean)printer["EnableBIDI"];
                // Verifica se a impressora está fazendo "spool" dos documentos
                Boolean spoolEnabled = (Boolean)printer["SpoolEnabled"];
                // Verifica se a impressora é do computador ou se é uma impressora compartilhada por outros
                Boolean machineMatches = true;
                if (!String.IsNullOrEmpty(machineName))
                {
                    String computerName = (String)printer["SystemName"];
                    machineMatches = (machineName == computerName);
                }

                if (hasPhysicalDevice && spoolEnabled && machineMatches)
                {
                    SysPrinter sysPrinter = new SysPrinter();
                    sysPrinter.Name = (String)printer["Name"];
                    sysPrinter.Port = (String)printer["PortName"];
                    sysPrinter.IsDefaultPrinter = (Boolean)printer["Default"];
                    sysPrinter.SpoolEnabled = (Boolean)printer["SpoolEnabled"];
                    sysPrinter.IsQueued = (Boolean)printer["Queued"];
                    sysPrinter.DoCompleteFirst = (Boolean)printer["DoCompleteFirst"];
                    sysPrinter.KeepPrintedJobs = (Boolean)printer["KeepPrintedJobs"];
                    sysPrinter.ComputerName = (String)printer["SystemName"];
                    sysPrinter.Capabilities = (PrinterCapabilityEnum[])printer["Capabilities"];
                    sysPrinter.EnableBIDI = (Boolean)printer["EnableBIDI"];
                    sysPrinters.Add(sysPrinter);
                }
            }

            return sysPrinters;
        }

        /// <summary>
        /// Prepara as impressoras lógicas do servidor para o monitoramento do spool. Seta seus
        /// atributos Direct, Queued e DoCompleteFirst. Desliga o file pooling de todas elas para
        /// que os print jobs tenham seus arquivos de spool(.SPL/.SHD) gravados corretamente
        /// </summary>
        public static void PreparePrinters(IListener listener)
        {
            // O file pooling gera arquivos no formato FP00000.SPL/SHD e impede o funcionamento
            // correto do monitor de spool (é um feature de SOs a partir do Win XP e Win 2003)
            // No site de suporte da Microsoft existe uma artigo descrevendo o problema
            // Artigo :  Third-Party Print Management Program Does Not Work as Expected
            //           After You Upgrade to Windows Server 2003 or Windows XP
            RegistryKey regKey = null;
            String defaultSpoolDir = null;
            try
            {
                regKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Print\Printers");
                defaultSpoolDir = (String)regKey.GetValue("DefaultSpoolDirectory");
            }
            catch (Exception exception)
            {
                if (listener != null) listener.NotifyObject(exception);
                return;
            }

            // Para cada impressora faz ajustes necessários para o funcionamento do spool
            foreach (String printer in regKey.GetSubKeyNames())
            {
                try
                {
                    RegistryKey printerKey = regKey.OpenSubKey(printer, true);

                    PrinterAttributesEnum printerAttributes = (PrinterAttributesEnum)printerKey.GetValue("Attributes");
                    // Desabilita impressão direto na impressora (força o uso de spool)
                    printerAttributes = printerAttributes & ~PrinterAttributesEnum.Direct;
                    // Habilita o uso de EMF para os trabalhos de impressão (RAW ou EMF)
                    printerAttributes = printerAttributes & ~PrinterAttributesEnum.RawOnly;
                    // Habilita buferização completa do job pré impressão
                    printerAttributes = printerAttributes | PrinterAttributesEnum.Queued;
                    // Determina que a ordem de impressão dos jobs deve ser "Complete First"
                    printerAttributes = printerAttributes | PrinterAttributesEnum.DoCompleteFirst;
                    printerKey.SetValue("Attributes", printerAttributes, RegistryValueKind.DWord);

                    // O preenchimento da propriedade SpoolDirectory desliga o "file pooling"
                    printerKey.SetValue("SpoolDirectory", defaultSpoolDir);

                    printerKey.Close();
                }
                catch (Exception exception)
                {
                    // Notifica a exceção e passa para a próxima impressora do "foreach"
                    if (listener != null) listener.NotifyObject(exception);
                }
            }
            regKey.Close();
            
            // Reinicia o spooler de impressão para que as alterações tenham efeito
            ServiceHandler.StopService("Spooler", 5000);
            ServiceHandler.StartService("Spooler", 5000);
        }

        /// <summary>
        /// Obtem o diretório de spool do sistema operacional
        /// </summary>
        public static String GetSpoolDirectory()
        {
            String spoolDir;

            try
            {
                String regPath = @"SYSTEM\CurrentControlSet\Control\Print\Printers";

                RegistryKey regKey = Registry.LocalMachine.OpenSubKey(regPath);
                // Obtem o diretório default de spool setado no servidor de impressão
                spoolDir = (String)regKey.GetValue("DefaultSpoolDirectory");
                regKey.Close();
            }
            catch
            {
                // Retorna null em caso de falha
                spoolDir = null;
            }

            return spoolDir;
        }
    }

}
