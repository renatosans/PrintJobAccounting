using System;
using System.Collections.Generic;
using System.Text;

namespace org.in2bits.MyXls.ByteUtil
{
    public partial class Bytes
    {
        /// <summary>
        /// Gets a Bits object representing the Bits comprising these Bytes.
        /// </summary>
        /// <returns>A Bits object representing the Bits comprising these Bytes.</returns>
        public Bits GetBits()
        {
            return new Bits(this);
        }

        /// <summary>
        /// A helper class to manage bit (bool) arrays and lists, encapsulating helper
        /// methods for subdividing and converting to/from bytes.
        /// </summary>
        public class Bits
        {
            private bool[] _bits = new bool[0];

            /// <summary>
            /// Initializes a new instance of the Bits class from the given Bytes.
            /// </summary>
            /// <param name="bytes">The Bytes whose bits this Bits object will represent.</param>
            public Bits(Bytes bytes)
            {
                byte[] byteArray = bytes.ByteArray;
                _bits = new bool[byteArray.Length * 8];
                for (byte i = 0; i < byteArray.Length; i++)
                {
                    SetBits(i, byteArray[i]);
                }
            }

            /// <summary>
            /// Initializes a new instance of the Bits class from the given byte array.
            /// </summary>
            /// <param name="bits">The bytes whose bits this Bits object will represent.</param>
            public Bits(bool[] bits)
            {
                _bits = bits;
            }

            private void SetBits(byte byteIndex, byte fromByte)
            {
                for (byte b = 7; b >= 0 && b < 255; b--)
                {
                    byte value = (byte)Math.Pow(2, b);
                    if (fromByte >= value)
                    {
                        _bits[(byteIndex * 8) + b] = true;
                        fromByte -= value;
                    }
                }
            }

            /// <summary>
            /// Prepends the specified bit to the beginning of this Bits collection.
            /// </summary>
            /// <param name="bit">The bit to prepend.</param>
            public void Prepend(bool bit)
            {
                bool[] newBits = new bool[_bits.Length + 1];
                _bits.CopyTo(newBits, 1);
                _bits = newBits;
                _bits[0] = bit;
            }

            //public void Append

            /// <summary>
            /// Gets a new Bits object containing the first getLength bits in this Bits object.
            /// </summary>
            /// <param name="getLength">The number of bits to return from the beginning of
            /// this Bits object in a new Bits object.</param>
            /// <returns>A new Bits object containing the provided number of bits from the beginning
            /// of this Bits object.</returns>
            public Bits Get(int getLength)
            {
                return Get(0, getLength);
            }

            /// <summary>
            /// Gets a new Bits object containing intLength bits in this Bits object,
            /// beginning at offset.
            /// </summary>
            /// <param name="offset">The start index from which the new Bits object is to be returned
            /// from this Bits object.</param>
            /// <param name="getLength">The number of bits to return from this Bits object 
            /// in a new Bits object.</param>
            /// <returns>A new Bits object containing the provided number of bits from this Bits 
            /// object, beginning at the provided offset.</returns>
            public Bits Get(int offset, int getLength)
            {
                if (offset < 0)
                    throw new ArgumentOutOfRangeException(string.Format("offset {0} must be >= 0", offset));

                if (getLength < 0)
                    throw new ArgumentOutOfRangeException(string.Format("getLength {0} must be >= 0", getLength));

                if (offset >= Length)
                    throw new ArgumentOutOfRangeException(string.Format("offset {0} must be < Length {1}", offset, Length));

                if ((getLength + offset) > Length)
                    throw new ArgumentOutOfRangeException(
                        string.Format("offset {0} + getLength {1} = {2} must be < Length {3}", offset, getLength, offset + getLength,
                                      Length));

                bool[] subBits = new bool[getLength];
                Array.Copy(_bits, offset, subBits, 0, getLength);
                return new Bits(subBits);
            }

            /// <summary>
            /// Gets the length or number of bits in this Bits object.
            /// </summary>
            public int Length
            {
                get { return _bits.Length; }
            }

            /// <summary>
            /// Gets a bool[] representing the bits in this Bits object.
            /// </summary>
            public bool[] Values
            {
                get { return _bits; }
            }

            /// <summary>
            /// Calculates and returns the uint which these bits represnt.
            /// </summary>
            /// <returns>The uint value represented by these bits.</returns>
            public uint ToUInt32()
            {
                int length = _bits.Length;
                if (length > 32)
                    throw new ApplicationException(string.Format("Length {0} must be <= 32", length));
                uint value = 0;
                for (int i = (length - 1); i >= 0; i--)
                {
                    if (_bits[i])
                        value += (uint)Math.Pow(2, i);
                }
                return value;
            }

            /// <summary>
            /// Calculates and returns the int which these bits represent.
            /// </summary>
            /// <returns>The int value represented by these bits.</returns>
            public int ToInt32()
            {
                int length = _bits.Length;
                if (length > 32)
                    throw new ApplicationException(string.Format("Length {0} must be <= 32", length));
                int value = 0;
                for (int i = (length - 1); i >= 0; i--)
                {
                    if (_bits[i])
                        value += (int)Math.Pow(2, i);
                }
                return value;
            }

            /// <summary>
            /// Calculates and returns the ushort which these bits represent.
            /// </summary>
            /// <returns>The ushort value represented by these bits.</returns>
            public ushort ToUInt16()
            {
                int length = _bits.Length;
                if (length > 16)
                    throw new ApplicationException(string.Format("Length {0} must be <= 16", length));
                ushort value = 0;
                for (int i = 0; i < length; i++)
                {
                    if (_bits[i])
                        value += (ushort)Math.Pow(2, i);
                }
                return value;
            }

            /// <summary>
            /// Calculates and returns the ulong which these bits represent.
            /// </summary>
            /// <returns>The ulong value represented by these bits.</returns>
            public ulong ToUInt64()
            {
                int length = _bits.Length;
                if (length > 64)
                    throw new ApplicationException(string.Format("Length {0} must be <= 64", length));
                ushort value = 0;
                for (int i = 0; i < length; i++)
                {
                    if (_bits[i])
                        value += (ushort)Math.Pow(2, i);
                }
                return value;
            }

            /// <summary>
            /// Calculates and returns a Bytes object containing the bytes which these bits represent.
            /// </summary>
            /// <returns>A Bytes object containing the bytes represented by these bits.</returns>
            public Bytes GetBytes()
            {
                byte[] bytes = new byte[(int) Math.Ceiling(_bits.Length/8.0)];
                for (int i = (bytes.Length - 1); i >= 0; i--)
                {
                    byte b = 0x00;
                    for (int j = 7; j >= 0; j--)
                    {
                        int index = (8*i + j);
                        if (index < _bits.Length && _bits[index])
                            b += (byte) Math.Pow(2, j);
                    }
                    bytes[i] = b;
                }
                return new Bytes(bytes);
            }

            /// <summary>
            /// Calculates and returns the double precision floating-point value which these bits represent.
            /// </summary>
            /// <returns>The double precision floating-point value represented by these bits.</returns>
            public double ToDouble()
            {
                List<bool> bitList = new List<bool>();
                bitList.AddRange(new bool[64 - _bits.Length]);
                bitList.AddRange(_bits);
                byte[] bytes = new byte[8];
                for (int i = 7; i >= 0; i--)
                {
                    for (int j = 7; j >= 0; j--)
                    {
                        if (bitList[8 * i + j])
                            bytes[i] += (byte) Math.Pow(2, j);
                    }
                }
                return BitConverter.ToDouble(bytes, 0);
            }
        }
    }
}