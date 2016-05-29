using System;


namespace AccountingLib.ServerCopyLog
{
    public class CopyLogDevice
    {
        public String printerName;

        public String logFile;

        public String logType;


        public CopyLogDevice(String printerName, String logFile, String logType)
        {
            this.printerName = printerName;
            this.logFile = logFile;
            this.logType = logType;
        }
    }

}
