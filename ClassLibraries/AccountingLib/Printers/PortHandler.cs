using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace AccountingLib.Printers
{
    public static class PortHandler
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct PORT_INFO_2
        {
            public string pPortName;
            public string pMonitorName;
            public string pDescription;
            public PortTypeEnum fPortType;
            internal int Reserved;
        }

        [DllImport("winspool.drv", EntryPoint = "EnumPortsA", SetLastError = true)]
        private static extern int EnumPorts(string pName, int Level, IntPtr lpbPorts, int cbBuf, ref int pcbNeeded, ref int pcReturned);

        /// <summary>
        /// Lista as portas disponíveis no computador, "server" recebe uma string vazia para o
        /// servidor local ou nome do servidor remoto
        /// </summary>
        private static PORT_INFO_2[] GetAvailablePorts(String server)
        {
            int ret;
            int pcbNeeded = 0; int pcReturned = 0; int lastErr = 0; IntPtr TempBuff = IntPtr.Zero;
            PORT_INFO_2[] pinfo = null;
            ret = EnumPorts(server, 2, TempBuff, 0, ref pcbNeeded, ref pcReturned);

            try
            {

                TempBuff = Marshal.AllocHGlobal(pcbNeeded + 1);

                ret = EnumPorts(server, 2, TempBuff, pcbNeeded, ref pcbNeeded, ref pcReturned);
                lastErr = Marshal.GetLastWin32Error();
                if (ret != 0)
                {
                    IntPtr CurrentPort = TempBuff;

                    pinfo = new PORT_INFO_2[pcReturned];

                    for (int i = 0; i < pcReturned; i++)
                    {
                        pinfo[i] = (PORT_INFO_2)Marshal.PtrToStructure(CurrentPort, typeof(PORT_INFO_2));
                        CurrentPort = (IntPtr)(CurrentPort.ToInt32() + Marshal.SizeOf(typeof(PORT_INFO_2)));
                    }
                    CurrentPort = IntPtr.Zero;
                }
                return pinfo;
            }
            finally
            {
                if (TempBuff != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(TempBuff);
                    TempBuff = IntPtr.Zero;
                }
            }
        }


        public static List<PrinterPort> GetAllPorts()
        {
            PORT_INFO_2[] ports = GetAvailablePorts("");
            List<PrinterPort> portList = new List<PrinterPort>();
            if (ports == null) return portList; // retorna a lista vazia

            foreach (PORT_INFO_2 port in ports)
            {
                PrinterPort printerPort = new PrinterPort();
                printerPort.Name = port.pPortName;
                printerPort.Description = port.pDescription;
                printerPort.Type = port.fPortType;

                portList.Add(printerPort);
            }
            return portList;
        }


        public static PrinterPort GetPort(String portName)
        {
            PORT_INFO_2[] ports = GetAvailablePorts("");
            if (ports == null) return null; // nenhuma porta encontrada

            foreach (PORT_INFO_2 port in ports)
            {
                if (port.pPortName.ToUpper() == portName.ToUpper())
                {
                    PrinterPort printerPort = new PrinterPort();
                    printerPort.Name = port.pPortName;
                    printerPort.Description = port.pDescription;
                    printerPort.Type = port.fPortType;
                    return printerPort;
                }
            }
            return null;
        }
    }

}
