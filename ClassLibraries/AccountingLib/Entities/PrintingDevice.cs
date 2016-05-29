using System;


namespace AccountingLib.Entities
{
    public class PrintingDevice
    {
        public int id;

        public int tenantId;

        public String ipAddress;

        public String description;

        public String serialNumber;

        public int counter;

        public DateTime lastUpdated = new DateTime(2000, 01, 01);


        public PrintingDevice()
        {
        }

        public PrintingDevice(int tenantId, String ipAddress, String description, String serialNumber, int counter)
        {
            this.tenantId = tenantId;
            this.ipAddress = ipAddress;
            this.description = description;
            this.serialNumber = serialNumber;
            this.counter = counter;
        }
    }

}
