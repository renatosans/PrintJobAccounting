using System;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// Worksheet Visibility values available in Excel (whether the Worksheet will be visible to the user).
    /// </summary>
	public enum WorksheetVisibilities
	{
        /// <summary>Default - Visible</summary>
		Default = Visible,
        
        /// <summary>Visible</summary>
        Visible = 1,

        /// <summary>Hidden</summary>
        Hidden = 2,

        /// <summary>Strong Hidden (used for VBA modules)</summary>
        StrongHidden = 3,
	}

    internal static class WorksheetVisibility
    {
        internal static byte[] GetBytes(WorksheetVisibilities visibility)
        {
            switch (visibility)
            {
                case WorksheetVisibilities.Visible: return new byte[] { 0x00 };
                case WorksheetVisibilities.Hidden: return new byte[] { 0x01 }; 
                case WorksheetVisibilities.StrongHidden: return new byte[] { 0x02 };
                default: throw new ApplicationException(string.Format("Unexpected WorksheetVisibilities {0}", visibility));
            }
        }
    }
}
    