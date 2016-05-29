using System;
using System.Collections.Generic;
using System.IO;

namespace org.in2bits.MyXls.ByteUtil
{
    /// <summary>
    /// A helper class to manage byte arrays, allowing subdividing and combining
    /// arrays without incurring the cost of copying the bytes from one array to
    /// another.
    /// </summary>
    public partial class Bytes
    {
        private byte[] _byteArray = null;
        internal List<Bytes> _bytesList = null;
        private int _length = 0;

        /// <summary>
        /// Initializes a new, empty, instance of the Bytes class.
        /// </summary>
        public Bytes() { }

        /// <summary>
        /// Initializes a new instance of the Bytes class containing the provided byte.
        /// </summary>
        /// <param name="b">byte with which to initialize this Bytes instance.</param>
        public Bytes(byte b)
            : this(new byte[] { b })
        {
        }

        /// <summary>
        /// Initializes a new instance of the Bytes class containing the provided byte
        /// array.
        /// </summary>
        /// <param name="byteArray">Array of bytes to initialize this Bytes instance.</param>
        public Bytes(byte[] byteArray) : this()
        {
            CheckNewLength(byteArray);
            _byteArray = byteArray;
            _length = byteArray.Length;
        }

        /// <summary>
        /// Initializes a new instance of the Bytes class containing the same bytes as the
        /// provided Bytes object.
        /// </summary>
        /// <param name="bytes">Bytes class to initialize this Bytes instance.</param>
        public Bytes(Bytes bytes) : this()
        {
            CheckNewLength(bytes);
            _bytesList = new List<Bytes>();
            _bytesList.Add(bytes);
            _length = bytes.Length;
        }

        /// <summary>
        /// Gets the length or number of bytes in this Bytes object.
        /// </summary>
        public int Length { get { return _length; } }

        internal bool IsEmpty { get { return _length == 0; } }
        internal bool IsArray { get { return _byteArray != null; } }

        /// <summary>
        /// Appends the provided byte array at the end of the existing bytes in this
        /// Bytes object.
        /// </summary>
        /// <param name="byteArray">The byte array to append at the end of this Bytes
        /// object.</param>
        public void Append(byte[] byteArray)
        {
            if (byteArray.Length == 0)
                return;
			
            CheckNewLength(byteArray);

            if (IsEmpty)
            {
                _byteArray = byteArray;
            }
            else if (IsArray)
            {
                ConvertToList();
                _bytesList.Add(new Bytes(byteArray));
            }
            else
            {
                _bytesList.Add(new Bytes(byteArray));
            }

            _length += byteArray.Length;
        }

        /// <summary>
        /// Appends a single byte to the end of this Bytes object.
        /// </summary>
        /// <param name="b">They byte to append to the end of this object.</param>
        public void Append(byte b)
        {
            Append(new byte[] {b});
        }

        /// <summary>
        /// Appends the contents of the provided Bytes object to the end of this
        /// Bytes object.
        /// </summary>
        /// <param name="bytes">The Bytes object whose contents are to be appended
        /// at the end of this Bytes object.</param>
        public void Append(Bytes bytes)
        {
            if (bytes.Length == 0)
                return;

            CheckNewLength(bytes);

            if (IsEmpty)
            {
                _bytesList = new List<Bytes>();
                _bytesList.Add(bytes);
            }
            else if (IsArray)
            {
                ConvertToList();
                _bytesList.Add(bytes);
            }
            else
            {
                _bytesList.Add(bytes);
            }

            _length += bytes.Length;
        }

        /// <summary>
        /// Prepends the provided byte array at the beginning of the existing bytes in this
        /// Bytes object.
        /// </summary>
        /// <param name="byteArray">The byte array to prepend at the beginning of this Bytes
        /// object.</param>
        public void Prepend(byte[] byteArray)
        {
            Prepend(new Bytes(byteArray));
        }

        /// <summary>
        /// Prepends the contents of the provided Bytes object to the beginning of this
        /// Bytes object.
        /// </summary>
        /// <param name="bytes">The Bytes object whose contents are to be prepended
        /// at the beginning of this Bytes object.</param>
        public void Prepend(Bytes bytes)
        {
            if (bytes.Length == 0)
                return;

            CheckNewLength(bytes);

            if (IsEmpty)
            {
                Append(bytes);
                return;
            }

            if (IsArray)
                ConvertToList();

            _bytesList.Insert(0, bytes);

            _length += bytes.Length;
        }

        /// <summary>
        /// Gets a byte array containing all bytes in this Bytes object.
        /// </summary>
        public byte[] ByteArray
        {
            get
            {
                if (IsEmpty)
                    return new byte[0];
				
                MemoryStream memoryStream = new MemoryStream();
                WriteToStream(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Gets a Bytes object containing the first intLength bytes in this Bytes object.
        /// </summary>
        /// <param name="getLength">The number of bytes to return from the beginning of
        /// this Bytes object in a new Bytes object.</param>
        /// <returns>A Bytes object containing the provided number of bytes from the beginning
        /// of this Bytes object.</returns>
        public Bytes Get(int getLength)
        {
            return Get(0, getLength);
        }

        /// <summary>
        /// Gets a Bytes object containing intLength bytes in this Bytes object,
        /// beginning at offset.
        /// </summary>
        /// <param name="offset">The index at which the sub-array of bytes is to be returned
        /// from this Bytes object in a new Bytes object.</param>
        /// <param name="getLength">The number of bytes to return from this Bytes object 
        /// in a new Bytes object.</param>
        /// <returns>A Bytes object containing the provided number of bytes from this Bytes 
        /// object, beginning at the provided offset.</returns>
        public Bytes Get(int offset, int getLength)
        {
            Bytes bytes = new Bytes();

            if (getLength == 0)
                return bytes;

            Get(offset, getLength, bytes);

            return bytes;
        }

        private void Get(int offset, int getLength, Bytes intoBytes)
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

            if (IsArray)
            {
                if (offset == 0 && getLength == Length)
                {
                    intoBytes.Append(_byteArray);
                    return;
                }

                intoBytes.Append(MidByteArray(_byteArray, offset, getLength));
                return;
            }

            foreach (Bytes bytes in _bytesList)
            {
                if (bytes.Length <= offset)
                {
                    offset -= bytes.Length;
                    continue;
                }

                if (bytes.Length >= (offset + getLength))
                {
                    bytes.Get(offset, getLength, intoBytes);
                    return;
                }

                int lengthToGet = bytes.Length - offset;
                bytes.Get(offset, lengthToGet, intoBytes);
                getLength -= lengthToGet;
                offset = 0;
            }
        }

        internal static byte[] MidByteArray(byte[] byteArray, int offset, int length)
        {
            if (offset >= byteArray.Length)
                throw new ArgumentOutOfRangeException(string.Format("offset {0} must be less than byteArray.Length {1}", offset, byteArray.Length));

            if (offset + length > byteArray.Length)
                throw new ArgumentOutOfRangeException(string.Format("offset {0} + length {1} must be <= byteArray.Length {2}", offset, length, byteArray.Length));

            if (offset == 0 && length == byteArray.Length)
                return byteArray;

            byte[] subArray = new byte[length];
            for (int i = 0; i < length; i++)
                subArray[i] = byteArray[offset + i];
            return subArray;
        }

        internal void WriteToStream(Stream stream)
        {
            if (IsEmpty)
                return;
            else if (IsArray)
            {
                stream.Write(_byteArray, 0, _length);
                return;
            }
            else
            {
                foreach (Bytes bytes in _bytesList)
                {
                    bytes.WriteToStream(stream);
                }
                return;
            }
        }

        private void CheckNewLength(byte[] withAddition)
        {
            CheckNewLength(withAddition.Length);
        }

        private void CheckNewLength(Bytes withAddition)
        {
            CheckNewLength(withAddition.Length);
        }

        private void CheckNewLength(int withAddition)
        {
            if ((_length + withAddition) > int.MaxValue)
                throw new Exception(
                    string.Format("Addition of {0} bytes would exceed current limit of {1} bytes", withAddition, int.MaxValue));
        }

        private void ConvertToList()
        {
            _bytesList = new List<Bytes>();

            if (IsEmpty)
                return;

            Bytes newBytes = new Bytes(_byteArray);
            _byteArray = null;
            _bytesList.Add(newBytes);
        }

        /// <summary>
        /// Determines whether the two provided byte arrays are equal by byte-values.
        /// </summary>
        /// <param name="a">The first byte array to compare.</param>
        /// <param name="b">The second byte array to copare.</param>
        /// <returns>true if a and b are byte-equal, false otherwise</returns>
        public static bool AreEqual(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i]) return false;

            return true;
        }
    }
}