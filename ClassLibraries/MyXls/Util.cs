using System;
using System.Collections.Generic;
using System.Text;

namespace org.in2bits.MyXls
{
    internal static class Util
    {
        internal static void ValidateUShort(int theInt, string fieldName)
        {
            if (theInt < ushort.MinValue || theInt > ushort.MaxValue)
                throw new ArgumentException(string.Format("{0} value {1} must be between 1 and {2}", fieldName, theInt, ushort.MaxValue - 1));
        }
    }
}
