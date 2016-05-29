using System;
using System.Collections.Generic;
using System.Text;

namespace org.in2bits.MyOle2.Metadata
{
    /// <summary>
    /// Versions available to describe the specified Operating System.
    /// </summary>
    public enum OriginOperatingSystemVersions
    {
        /// <summary>Default - this is the only known available value.</summary>
        Default
    }

    internal static class OriginOperatingSystemVersion
    {
        internal static byte[] GetBytes(OriginOperatingSystemVersions version)
        {
            return new byte[] { 0x05, 0x01 };
        }
    }
}
