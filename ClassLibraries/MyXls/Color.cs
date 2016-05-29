using System;
using System.Collections.Generic;
using System.Text;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// Represents an RGB color value.  Use the values in Colors.  Custom colors are not yet supported.
    /// </summary>
    public class Color : ICloneable
    {
        internal byte Red;
        internal byte Green;
        internal byte Blue;
        internal ushort? Id;

        internal Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Id = null;
        }

        internal Color(ushort id)
        {
            Red = 0x00;
            Green = 0x00;
            Blue = 0x00;
            Id = id;
        }

        /// <summary>
        /// Determines whether this Color is equal to the specifed Color.
        /// </summary>
        /// <param name="obj">The Color to compare to this Color.</param>
        /// <returns>true if this Color is Equal to that Color, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            Color that = (Color) obj;
            if (this.Id != null || that.Id != null)
                return this.Id == that.Id;
            else
                return (this.Red == that.Red &&
                        this.Green == that.Green &&
                        this.Blue == that.Blue);
        }

        /// <summary>
        /// Returns a new Color instance Equal to this Color.
        /// </summary>
        /// <returns>A new Color instance Equal to this Color.</returns>
        public object Clone()
        {
            Color clone = new Color(Red, Green, Blue);
            clone.Id = Id;
            return clone;
        }
    }
}
