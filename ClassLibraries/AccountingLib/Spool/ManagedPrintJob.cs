using System;
using System.Management;


namespace AccountingLib.Spool
{
    public class ManagedPrintJob
    {
        private ManagementObject jobObject;

        public UInt32 JobId;

        public String Name;

        public String Owner;

        public String Document;

        public UInt32 TotalPages;

        public String DataType;

        public String DriverName;

        public String PrintProcessor;

        public UInt32 StatusMask;

        public String HostPrintQueue;


        public Boolean IsPaused()
        {
            JobStatusEnum status = (JobStatusEnum)StatusMask;
            JobStatusEnum pausedFlag = status & JobStatusEnum.Paused;

            // Se possui a flag retorna true
            if ((pausedFlag) == JobStatusEnum.Paused)
                return true;

            // Se não possui a flag retorna false
            return false;
        }

        public Boolean IsSpooling()
        {
            JobStatusEnum status = (JobStatusEnum) StatusMask;
            JobStatusEnum spoolingFlag = status & JobStatusEnum.Spooling;

            // Se possui a flag retorna true
            if ((spoolingFlag) == JobStatusEnum.Spooling)
                return true;

            // Se não possui a flag retorna false
            return false;
        }

        public void Pause()
        {
            if (jobObject != null)
                jobObject.InvokeMethod("Pause", null);
        }

        public ManagedPrintJob(String jobName)
        {
            String query = "SELECT * FROM Win32_PrintJob WHERE Name='" + jobName + "'";
            ManagementObjectSearcher printJobSearcher = new ManagementObjectSearcher(query);

            ManagementObjectCollection printJobs = printJobSearcher.Get();
            if (printJobs.Count != 1) return;

            // Workarround necessário para posicionar a coleção no primeiro elemento, pois ela
            // não possui um indexador
            foreach (ManagementObject job in printJobs) { jobObject = job; }

            JobId = (UInt32)jobObject["JobId"];
            Name = (String)jobObject["Name"];
            Owner = (String)jobObject["Owner"];
            Document = (String)jobObject["Document"];
            TotalPages = (UInt32)jobObject["TotalPages"];
            DataType = (String)jobObject["DataType"];
            DriverName = (String)jobObject["DriverName"];
            PrintProcessor = (String)jobObject["PrintProcessor"];
            StatusMask = (UInt32)jobObject["StatusMask"];
            HostPrintQueue = (String)jobObject["HostPrintQueue"];
        }
    }

}
