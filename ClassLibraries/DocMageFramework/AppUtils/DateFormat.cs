using System;


namespace DocMageFramework.AppUtils
{
    /// <summary>
    /// Classe utilitária que possui método para formatar uma data para saída texto
    /// </summary>
    public static class DateFormat
    {
        public static string Adjust(DateTime? date, Boolean appendHour)
        {
            // Retorna null caso o parâmetro não seja recebido
            if (date == null) return null;

            String year = String.Format("{0:0000}", date.Value.Year);
            String month = String.Format("-{0:00}", date.Value.Month);
            String day = String.Format("-{0:00}", date.Value.Day);

            String hour = String.Format("{0:00}", date.Value.Hour);
            String minute = String.Format(":{0:00}", date.Value.Minute);
            String second = String.Format(":{0:00}", date.Value.Second);

            String complement = "";
            if (appendHour) complement = " " + hour + minute + second;

            return year + month + day + complement;
        }

        public static string Adjust(DateTime? date)
        {
            return Adjust(date, false);
        }
    }

}
