using System;


namespace AccountingLib.Entities
{
    public class UserPrintingCost
    {
        public int userId;

        public String userName;

        public int bwPageCount;

        public int colorPageCount;

        public int totalPageCount;

        public decimal bwCost;

        public decimal colorCost;

        public decimal totalCost;


        public UserPrintingCost()
        {
        }
    }

}
