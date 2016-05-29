using System;


namespace AccountingLib.Spool
{
    [Flags]
    public enum JobStatusEnum: int
    {
        None                 = 0x00000000,
        Paused               = 0x00000001,     // Job is paused
        Error                = 0x00000002,     // An error is associated with the job.
        Deleting             = 0x00000004,     // Job is being deleted
        Spooling             = 0x00000008,     // Job is spooling
        Printing             = 0x00000010,     // Job is printing   
        Offline              = 0x00000020,     // Printer is offline
        Paperout             = 0x00000040,     // Printer is out of paper
        Printed              = 0x00000080,     // Job has printed
        Deleted              = 0x00000100,     // Job has been deleted
        Blocked_DevQ         = 0x00000200,     // Printer driver cannot print the job
        UserInterventionReq  = 0x00000400,     // Printer has an error that requires the user to do something
        Restart              = 0x00000800,     // Job has been restarted
        Complete             = 0x00001000,     // Job has been delivered to the printer
    }

}
