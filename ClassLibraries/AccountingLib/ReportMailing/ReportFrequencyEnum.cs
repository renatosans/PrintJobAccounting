using System;
using DocMageFramework.CustomAttributes;


namespace AccountingLib.ReportMailing
{
    public enum ReportFrequencyEnum
    {
        [AssociatedText("Diário")]
        Daily,
        [AssociatedText("Semanal")]
        Weekly,
        [AssociatedText("Mensal")]
        Monthly
    }

}
