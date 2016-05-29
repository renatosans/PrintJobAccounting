using System;
using DocMageFramework.CustomAttributes;


namespace AccountingLib.Printers
{
    public enum PrinterCapabilityEnum: short
    {
        [AssociatedText("Unknown")]                     Unknown                   = 0x0000,
        [AssociatedText("Other")]                       Other                     = 0x0001,
        [AssociatedText("Color Printing")]              ColorPrinting             = 0x0002,
        [AssociatedText("Duplex Printing")]             DuplexPrinting            = 0x0003,
        [AssociatedText("Copies")]                      Copies                    = 0x0004,
        [AssociatedText("Collation")]                   Collation                 = 0x0005,
        [AssociatedText("Stapling")]                    Stapling                  = 0x0006,
        [AssociatedText("Transparency Printing")]       TransparencyPrinting      = 0x0007,
        [AssociatedText("Punch")]                       Punch                     = 0x0008,
        [AssociatedText("Cover")]                       Cover                     = 0x0009,
        [AssociatedText("Bind")]                        Bind                      = 0x000A,
        [AssociatedText("Black and White Printing")]    BWPrinting                = 0x000B,
        [AssociatedText("One-Sided")]                   OneSided                  = 0x000C,
        [AssociatedText("Two-Sided Long Edge")]         TwoSidedLongEdge          = 0x000D,
        [AssociatedText("Two-Sided Short Edge")]        TwoSidedShortEdge         = 0x000E,
        [AssociatedText("Portrait")]                    Portrait                  = 0x000F,
        [AssociatedText("Landscape")]                   Landscape                 = 0x0010,
        [AssociatedText("Reverse Portrait")]            ReversePortrait           = 0x0011,
        [AssociatedText("Reverse Landscape")]           ReverseLandscape          = 0x0012,
        [AssociatedText("Quality High")]                QualityHigh               = 0x0013,
        [AssociatedText("Quality Normal")]              QualityNormal             = 0x0014,
        [AssociatedText("Quality Low")]                 QualityLow                = 0x0015
    }

}
