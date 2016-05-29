using System;
using System.Collections.Generic;
using System.Text;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyOle2.Metadata
{
    /// <summary>
    /// Represents a OLE2 Summary Information stream property.
    /// </summary>
    public class Property
    {
        /// <summary>
        /// The different OLE2 Summary Information stream property types.
        /// </summary>
        public enum Types : uint
        {
            /// <summary>nothing</summary>
            VT_EMPTY=0,
            /// <summary>SQL style Null</summary>
            VT_NULL=1,
            /// <summary>2 byte signed int</summary>
            VT_I2=2,
            /// <summary>4 byte signed int</summary>
            VT_I4=3,
            /// <summary>4 byte real</summary>
            VT_R4=4,
            /// <summary>8 byte real</summary>
            VT_R8=5,
            /// <summary>Currency</summary>
            VT_CY=6,
            /// <summary>Date</summary>
            VT_DATE=7,
            /// <summary>OLE Automation string</summary>
            VT_BSTR=8,
            /// <summary>*IDispatch</summary>
            VT_DISPATCH=9,
            /// <summary>SCODE</summary>
            VT_ERROR=10,
            /// <summary>Boolean (true=-1; false=0)</summary>
            VT_BOOL=11,
            /// <summary>Variant*</summary>
            VT_VARIANT=12,
            /// <summary>IUnknown*</summary>
            VT_UNKNOWN=13,
            /// <summary>16 byte fixed point</summary>
            VT_DECIMAL=14,
            /// <summary>signed char</summary>
            VT_I1=16,
            /// <summary>unsigned char</summary>
            VT_UI1=17,
            /// <summary>unsigned short</summary>
            VT_UI2=18,
            /// <summary>unsigned short</summary>
            VT_UI4=19,
            /// <summary>signed 64-bit int</summary>
            VT_I8=20,
            /// <summary>unsigned 64-bit int</summary>
            VT_UI8=21,
            /// <summary>signed machine int</summary>
            VT_INT=22,
            /// <summary>unsigned machine int</summary>
            VT_UINT=23,
            /// <summary>C style void</summary>
            VT_VOID=24,
            /// <summary>Standard return type</summary>
            VT_HRESULT=25,
            /// <summary>pointer type</summary>
            VT_PTR=26,
            /// <summary>(use VT_ARRAY in VARIANT)</summary>
            VT_SAFEARRAY=27,
            /// <summary>C style array</summary>
            VT_CARRAY=28,
            /// <summary>user defined type</summary>
            VT_USERDEFINED=29,
            /// <summary>null terminated string</summary>
            VT_LPSTR=30,
            /// <summary>wide null terminated string</summary>
            VT_LPWSTR=31,
            /// <summary>FILETIME</summary>
            VT_FILETIME=64,
            /// <summary>length prefixed bytes</summary>
            VT_BLOB=65,
            /// <summary>Name of the stream follows</summary>
            VT_STREAM=66,
            /// <summary>Name of the storage follows</summary>
            VT_STORAGE=67,
            /// <summary>Stream contains an object</summary>
            VT_STREAMED_OBJECT=68,
            /// <summary>Storage contains an object</summary>
            VT_STORED_OBJECT=69,
            /// <summary>Blob contains an object</summary>
            VT_BLOB_OBJECT=70,
            /// <summary>Clipboard format</summary>
            VT_CF=71,
            /// <summary>A Class ID</summary>
            VT_CLSID=72,
            /// <summary>simple counted array</summary>
            VT_VECTOR,//0x1000
            /// <summary>SAFEARRAY*</summary>
            VT_ARRAY,//0x2000
            /// <summary>void* for local use</summary>
            VT_BYREF,//0x4000
            /// <summary></summary>
            VT_RESERVED,//0x8000
            /// <summary></summary>
            VT_ILLEGAL,//0xFFFF
            /// <summary></summary>
            VT_ILLEGALMASKED,//0xFFF
            /// <summary></summary>
            VT_TYPEMASK//0xFFF
        }

        private object _value;
        private Types _type;
        private uint _id;

        /// <summary>
        /// Initializes a new instance of the Property class with the given id, type and value.
        /// </summary>
        /// <param name="id">The id for the new Property.</param>
        /// <param name="type">The type of the new Property.</param>
        /// <param name="value">The value of the new Property.</param>
        public Property(uint id, Types type, object value)
        {
            _id = id;
            _type = type;
            _value = value;
        }

        /// <summary>
        /// Gets the Types value of this Property.
        /// </summary>
        public Types Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets the Id of this Property.
        /// </summary>
        public uint Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets or sets the Value of this Property.
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        internal Bytes Bytes
        {
            get
            {
                if (_value == null)
                    throw new ApplicationException(string.Format("The Value of a Property can't be null - Property ID {0}", _id));

                Bytes bytes = new Bytes();

                bytes.Append(BitConverter.GetBytes((uint)_type));
                bytes.Append(GetBytes(this));

                return bytes;
            }
        }

        private static Bytes GetBytes(Property property)
        {
            Bytes bytes;
            switch(property.Type)
            {
                case Types.VT_LPSTR: bytes = GetBytesLPSTR(property.Value); break;
                case Types.VT_I2: bytes = GetBytesI2(property.Value); break;
                case Types.VT_I4: bytes = GetBytesI4(property.Value); break;
                case Types.VT_FILETIME: bytes = GetBytesFILETIME(property.Value); break;
                //case Types.VT_BOOL: bytes = GetBytesBOOL(property.Value); break;

                //TODO: Implement these as necessary
                case Types.VT_EMPTY:
                case Types.VT_NULL:
                case Types.VT_R4:
                case Types.VT_R8:
                case Types.VT_CY:
                case Types.VT_DATE:
                case Types.VT_BSTR:
                case Types.VT_DISPATCH:
                case Types.VT_ERROR:
                case Types.VT_BOOL:
                case Types.VT_VARIANT:
                case Types.VT_UNKNOWN:
                case Types.VT_DECIMAL:
                case Types.VT_I1:
                case Types.VT_UI1:
                case Types.VT_UI2:
                case Types.VT_UI4:
                case Types.VT_I8:
                case Types.VT_UI8:
                case Types.VT_INT:
                case Types.VT_UINT:
                case Types.VT_VOID:
                case Types.VT_HRESULT:
                case Types.VT_PTR:
                case Types.VT_SAFEARRAY:
                case Types.VT_CARRAY:
                case Types.VT_USERDEFINED:
                case Types.VT_LPWSTR:
                case Types.VT_BLOB:
                case Types.VT_STREAM:
                case Types.VT_STORAGE:
                case Types.VT_STREAMED_OBJECT:
                case Types.VT_STORED_OBJECT:
                case Types.VT_BLOB_OBJECT:
                case Types.VT_CF:
                case Types.VT_CLSID:
                case Types.VT_VECTOR:
                case Types.VT_ARRAY:
                case Types.VT_BYREF:
                case Types.VT_RESERVED:
                case Types.VT_ILLEGAL:
                case Types.VT_ILLEGALMASKED:
                case Types.VT_TYPEMASK:
                    throw new NotSupportedException(string.Format("Property Type {0}", property.Type));
                default:
                    throw new ApplicationException(string.Format("unexpected value {0}", property.Type));
            }

            //Documentation says it's padded to a multiple of 4;
            int partial4 = bytes.Length % 4;
            if (partial4 != 0)
                bytes.Append(new byte[4 - partial4]);

            return bytes;
        }

        private static Bytes GetBytesBOOL(object value)
        {
            bool theBool = (bool) value;
            int i = theBool ? -1 : 0;
            return new Bytes(BitConverter.GetBytes(i));
        }

        private static Bytes GetBytesFILETIME(object value)
        {
            DateTime theDate = (DateTime) value;
            return new Bytes(BitConverter.GetBytes(theDate.ToFileTime()));
        }

        private static Bytes GetBytesI4(object value)
        {
            int theInt = (int) value;
            return new Bytes(BitConverter.GetBytes(theInt));
        }

        private static Bytes GetBytesI2(object value)
        {
            short theShort = (short)value;
            return new Bytes(BitConverter.GetBytes(theShort));
        }

        private static Bytes GetBytesLPSTR(object value)
        {
            Bytes lpstr = new Bytes();

            string theString = value as string;
            Encoder encoder = Encoding.ASCII.GetEncoder();
            char[] theChars = theString.ToCharArray();
            int paddedLength = theChars.Length + 1; //add one for terminating null
            paddedLength += (paddedLength % 4); //must be multiple of 4
            byte[] bytes = new byte[paddedLength];
            encoder.GetBytes(theChars, 0, theChars.Length, bytes, 0, true);
            lpstr.Append(BitConverter.GetBytes((uint) paddedLength));
            lpstr.Append(bytes);
            
            return lpstr;
        }
    }
}
