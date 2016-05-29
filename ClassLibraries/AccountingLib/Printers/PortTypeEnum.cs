using System;


namespace AccountingLib.Printers
{
    [Flags]
    public enum PortTypeEnum : int
    {
        write = 0x01,
        read = 0x02,
        redirected = 0x04,
        netAttached = 0x08
    }

}
