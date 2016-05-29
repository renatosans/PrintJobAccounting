using System;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// Excel Worksheet Types
    /// </summary>
    public enum WorksheetTypes
    {
        /// <summary>Default (Worksheet)</summary>
        Default = Worksheet,
        /// <summary>Worksheet (Default)</summary>
        Worksheet = 1,
        /// <summary>Chart Worksheet</summary>
        Chart = 2,
        /// <summary>VB Module Worksheet</summary>
        VBModule = 3
    }

    internal static class WorksheetType
	{
        internal static byte[] GetBytes(WorksheetTypes type)
        {
            switch (type)
            {
		        case WorksheetTypes.Worksheet: return new byte[] { 0x00 };
		        case WorksheetTypes.Chart: return new byte[] { 0x02 };
		        case WorksheetTypes.VBModule: return new byte[] { 0x04 };
                default: throw new ApplicationException(string.Format("Unexpected WorksheetTypes {0}", type));
            }
        }
	}
}
