using System;


namespace AccountingLib.Entities
{
    public class DevicePrintingCost
    {
        public int printerId;

        public String printerName;

        public int bwPageCount;

        public int colorPageCount;

        public int totalPageCount;

        public decimal bwCost;

        public decimal colorCost;

        public decimal totalCost;


        public DevicePrintingCost()
        {
        }
    }

}
