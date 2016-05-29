using System;


namespace AccountingLib.Printers
{
    [Flags]
    public enum PrinterAttributesEnum: int
    {
        Queued           = 0x0001,     // Print jobs are buffered and queued
        Direct           = 0x0002,     // Print jobs should be sent directly to the printer. No spool files are created
        Default          = 0x0004,     // Printer is the default printer on the computer
        Shared           = 0x0008,     // Printer is available as a shared network resource
        Network          = 0x0010,     // Printer is attached to a network. If both Local and Network bits are set, this indicates a network printer
        Hidden           = 0x0020,     // Printer is hidden from some users on the network
        Local            = 0x0040,     // Printer is directly connected to a computer
        EnableDevQ       = 0x0080,     // Enable the queue on the printer if available
        KeepPrintedJobs  = 0x0100,     // Spooler should not delete jobs after they are printed
        DoCompleteFirst  = 0x0200,     // Printer should start jobs that have finished spooling first
        WorkOffline      = 0x0400,     // Queue print jobs when the printer is offline
        EnableBIDI       = 0x0800,     // Enable bi-directional printing
        RawOnly          = 0x1000,     // Printer accepts only raw data jobs to be spooled
        Published        = 0x2000      // Printer is published in the network directory service
    }

}
