using System;
using DocMageFramework.CustomAttributes;


namespace AccountingLib.Management
{
    public enum ExportFormatEnum
    {
        [AssociatedText("Portable Document(.PDF)")]
        PDF,
        [AssociatedText("Excel(.XLS)")]
        XLS,
        [AssociatedText("Raw Text(.CSV)")]
        CSV
    }

}
