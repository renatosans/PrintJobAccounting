using System;
using DocMageFramework.CustomAttributes;


namespace AccountingLib.Management
{
    public enum PeriodDelimiterEnum
    {
        [AssociatedText("Dia 1")]
        _1st = 1,
        [AssociatedText("Dia 5")]
        _5th = 5,
        [AssociatedText("Dia 10")]
        _10th = 10,
        [AssociatedText("Dia 15")]
        _15th = 15,
        [AssociatedText("Dia 20")]
        _20th = 20,
        [AssociatedText("Dia 25")]
        _25th = 25,
        [AssociatedText("Dia 28")]
        _28th = 28
    }

}
