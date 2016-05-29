using System;
using System.Collections.Generic;
using System.Text;

namespace org.in2bits.MyOle2.Metadata
{
    ///<summary>
    /// Origin Operating System choices in OLE2 SummaryInformation streams.
    ///</summary>
    public enum OriginOperatingSystems
    {
        /// <summary>16-bit Windows</summary>
        Win16,

        /// <summary>Macintosh</summary>
        Macintosh,
        
        /// <summary>32-bit Windows</summary>
        Win32,
        
        /// <summary>Default - 32-bit Windows</summary>
        Default = Win32
    }

    internal static class OriginOperatingSystem
    {
        internal static byte[] GetBytes(OriginOperatingSystems system)
        {
            switch (system)
            {
                case OriginOperatingSystems.Win16:
                    return new byte[] { 0x00, 0x00 };
                case OriginOperatingSystems.Macintosh:
                    return new byte[] { 0x01, 0x00 };
                case OriginOperatingSystems.Win32:
                    return new byte[] { 0x02, 0x00 };
                default:
                    throw new ArgumentException(string.Format("unexpected value {0}", system.ToString()), "system");
            }
        }
    }
}
