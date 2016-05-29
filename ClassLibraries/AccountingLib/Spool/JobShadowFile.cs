using System;
using System.IO;
using System.Text;
using System.Management;


namespace AccountingLib.Spool
{
    public class JobShadowFile
    {
        private SHDFileFormatEnum fileFormat;
        private Int32 systemType = 32; // por default considera como sendo um sistema operacional de 32 bits

        private Int32 headerSize;
        private UInt32 jobStatus;
        private Int32 priority;
        private String dataType;
        private DevMode devMode;
        private String documentName;
        private Int32 pageCount;
        private String driverName;
        private Int32 jobId;
        private String notifyName;
        private String port;
        private String printerName;
        private String printProcessor;
        private String userName;
        private Int32 spoolFileSize;

        private Int32 offsetUserName;
        private Int32 offsetNotifyName;
        private Int32 offsetDocumentName;
        private Int32 offsetPort;
        private Int32 offsetPrinterName;
        private Int32 offsetDriverName;
        private Int32 offsetDevMode;
        private Int32 offsetPrintProcessor;
        private Int32 offsetDataType;

        private Int16 year;
        private Int16 month;
        private Int16 dayOfWeek;
        private Int16 day;
        private Int16 hour;
        private Int16 minute;
        private Int16 second;
        private Int16 millisecond;

        private Int32 startTime;
        private Int32 endTime;


        public String DataType
        { 
            get { return dataType; }
        }

        public DevMode DevMode
        {
            get { return devMode; }
        }

        public String DocumentName
        {
            get { return documentName; }
        }

        public Int32 PageCount
        {
            get { return pageCount; }
        }

        public String DriverName
        {
            get { return driverName; }
        }

        public JobStatusEnum JobStatus
        {
            get { return (JobStatusEnum) jobStatus; }
        }

        public Int32 JobId
        {
            get { return jobId; }
        }

        public String NotifyName
        {
            get { return notifyName; }
        }

        public String Port
        {
            get { return port; }
        }

        public String PrinterName
        {
            get { return printerName; }
        }

        public String PrintProcessor
        {
            get { return printProcessor; }
        }

        public DateTime Submitted
        {
            get { return new DateTime(year, month, day, hour, minute, second); }
        }

        public String UserName
        {
            get { return userName; }
        }

        public int SpoolFileSize
        {
            get { return spoolFileSize; }
        }

        private Int32 ReadInteger(BinaryReader fileReader)
        {
            if (systemType == 64) // se o sistema operacional for de 64 bits faz uma leitura a mais
                fileReader.ReadInt32();
            return fileReader.ReadInt32();
        }

        private String ReadString(BinaryReader fileReader, Int32 offset)
        {
            // Caso o offset não seja informado corretamente retorna "null"
            if (offset < 1) return null;

            fileReader.BaseStream.Seek(offset, SeekOrigin.Begin);
            return new String(StringResource.Get(fileReader));
        }

        private Boolean Is64BitOperatingSystem()
        {
            try
            {
                ManagementObjectSearcher osSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectCollection operatingSystemCollection = osSearcher.Get();
                foreach (ManagementObject operatingSystem in operatingSystemCollection)
                {
                    String osArchitecture = (String)operatingSystem.GetPropertyValue("OSArchitecture");
                    if (osArchitecture.Contains("64")) return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public JobShadowFile(BinaryReader fileReader)
        {
            fileFormat = (SHDFileFormatEnum)fileReader.ReadInt32();
            if (Is64BitOperatingSystem()) systemType = 64; // sistema operacional de 64 bits
            // Nos shadow files do Windows 2000 e Windows 2003 existe o "HeaderSize"
            if ((fileFormat == SHDFileFormatEnum.SHD_SIGNATURE_WIN2K) ||
                  (fileFormat == SHDFileFormatEnum.SHD_SIGNATURE_WIN2003))
            {
                headerSize = fileReader.ReadInt32();
            }
            jobStatus = fileReader.ReadUInt16();
            // Atributo desconhecido, ocupa 16 bits (2 bytes) no arquivo
            Int16 unknown = fileReader.ReadInt16();
            jobId = fileReader.ReadInt32();
            priority = fileReader.ReadInt32();

            offsetUserName = ReadInteger(fileReader);
            offsetNotifyName = ReadInteger(fileReader);
            offsetDocumentName = ReadInteger(fileReader);
            offsetPort = ReadInteger(fileReader);
            offsetPrinterName = ReadInteger(fileReader);
            offsetDriverName = ReadInteger(fileReader);
            offsetDevMode = ReadInteger(fileReader);
            offsetPrintProcessor = ReadInteger(fileReader);
            offsetDataType = ReadInteger(fileReader);

            long offsetSubmitted = 4;
            if (systemType == 64) offsetSubmitted = 12;
            fileReader.BaseStream.Seek(offsetSubmitted, SeekOrigin.Current);
            year = fileReader.ReadInt16();
            month = fileReader.ReadInt16();
            dayOfWeek = fileReader.ReadInt16();
            day = fileReader.ReadInt16();
            hour = fileReader.ReadInt16();
            minute = fileReader.ReadInt16();
            second = fileReader.ReadInt16();
            millisecond = fileReader.ReadInt16();

            startTime = fileReader.ReadInt32();
            endTime = fileReader.ReadInt32();

            spoolFileSize = fileReader.ReadInt32();
            pageCount = fileReader.ReadInt32();

            fileReader.BaseStream.Seek(offsetDevMode, SeekOrigin.Begin);
            devMode = new DevMode(fileReader);

            userName = ReadString(fileReader, offsetUserName);
            notifyName = ReadString(fileReader, offsetNotifyName);
            documentName = ReadString(fileReader, offsetDocumentName);
            port = ReadString(fileReader, offsetPort);
            printerName = ReadString(fileReader, offsetPrinterName);
            driverName = ReadString(fileReader, offsetDriverName);
            printProcessor = ReadString(fileReader, offsetPrintProcessor);
            dataType = ReadString(fileReader, offsetDataType);
        }
    }

}
