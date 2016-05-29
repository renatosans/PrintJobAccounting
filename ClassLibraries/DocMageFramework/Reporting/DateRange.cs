using System;


namespace DocMageFramework.Reporting
{
    public class DateRange
    {
        private DateTime firstDay;

        private DateTime lastDay;


        public DateRange(Boolean custom)
        {
            if (custom)
            {
                // Faixa de datas que será escolhida, apenas define um valor inicial (DateTime.Now)
                firstDay = DateTime.Now;
                lastDay = DateTime.Now;
            }
            else
            {
                // Faixa de datas pré-definida (último mês)
                DateTime lastMonth = DateTime.Now.AddMonths(-1);
                firstDay = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                lastDay = firstDay.AddMonths(1).AddDays(-1);
            }
        }

        public void SetRange(DateTime firstDay, DateTime lastDay)
        {
            this.firstDay = firstDay;
            this.lastDay = lastDay;
        }

        public DateTime GetFirstDay()
        {
            return firstDay;
        }

        public DateTime GetLastDay()
        {
            return lastDay;
        }
    }

}
