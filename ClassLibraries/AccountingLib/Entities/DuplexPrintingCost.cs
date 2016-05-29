using System;


namespace AccountingLib.Entities
{
    public class DuplexPrintingCost
    {
        public int userId;

        public String userName;

        public int simplexPageCount;

        public int duplexPageCount;

        public int totalPageCount;

        public decimal simplexCost;

        public decimal duplexCost;

        public decimal totalCost;


        public DuplexPrintingCost()
        {
        }
    }

}
