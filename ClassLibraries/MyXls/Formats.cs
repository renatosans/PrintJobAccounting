using System;
using System.Collections.Generic;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// A collection class to manage Formats for an XlsDocument.
    /// </summary>
	public class Formats
	{
		private readonly XlsDocument _doc;

        private static readonly Dictionary<string, ushort> _defaultFormatIds = new Dictionary<string, ushort>();
		private readonly Dictionary<string, ushort> _userFormatIds = new Dictionary<string, ushort>();
        private static List<string> _defaultFormatsToWrite = new List<string>();

        internal static string Default = StandardFormats.General;

        private ushort _nextUserFormatId = 164;

        static Formats()
        {
            AddDefaults();

            _defaultFormatsToWrite.Add(StandardFormats.Currency_1);
        }

        internal Formats(XlsDocument doc)
		{
			_doc = doc;
		}

        private static void AddDefaults()
        {
            _defaultFormatIds[StandardFormats.General] = 0;
            _defaultFormatIds[StandardFormats.Decimal_1] = 1;
            _defaultFormatIds[StandardFormats.Decimal_2] = 2;
            _defaultFormatIds[StandardFormats.Decimal_3] = 3;
            _defaultFormatIds[StandardFormats.Decimal_4] = 4;
            _defaultFormatIds[StandardFormats.Currency_1] = 5;
            _defaultFormatIds[StandardFormats.Currency_2] = 6;
            _defaultFormatIds[StandardFormats.Currency_3] = 7;
            _defaultFormatIds[StandardFormats.Currency_4] = 8;
            _defaultFormatIds[StandardFormats.Percent_1] = 9;
            _defaultFormatIds[StandardFormats.Percent_2] = 10;
            _defaultFormatIds[StandardFormats.Scientific_1] = 11;
            _defaultFormatIds[StandardFormats.Fraction_1] = 12;
            _defaultFormatIds[StandardFormats.Fraction_2] = 13;
            _defaultFormatIds[StandardFormats.Date_1] = 14;
            _defaultFormatIds[StandardFormats.Date_2] = 15;
            _defaultFormatIds[StandardFormats.Date_3] = 16;
            _defaultFormatIds[StandardFormats.Date_4] = 17;
            _defaultFormatIds[StandardFormats.Time_1] = 18;
            _defaultFormatIds[StandardFormats.Time_2] = 19;
            _defaultFormatIds[StandardFormats.Time_3] = 20;
            _defaultFormatIds[StandardFormats.Time_4] = 21;
            _defaultFormatIds[StandardFormats.Date_Time] = 22;
            _defaultFormatIds[StandardFormats.Accounting_1] = 37;
            _defaultFormatIds[StandardFormats.Accounting_2] = 38;
            _defaultFormatIds[StandardFormats.Accounting_3] = 39;
            _defaultFormatIds[StandardFormats.Accounting_4] = 40;
            _defaultFormatIds[StandardFormats.Currency_5] = 41;
            _defaultFormatIds[StandardFormats.Currency_6] = 42;
            _defaultFormatIds[StandardFormats.Currency_7] = 43;
            _defaultFormatIds[StandardFormats.Currency_8] = 44;
            _defaultFormatIds[StandardFormats.Time_5] = 45;
            _defaultFormatIds[StandardFormats.Time_6] = 46;
            _defaultFormatIds[StandardFormats.Time_7] = 47;
            _defaultFormatIds[StandardFormats.Scientific_2] = 48;
            _defaultFormatIds[StandardFormats.Text] = 49;
        }

        internal string this[ushort index]
        {
            get
            {
                foreach (string format in _userFormatIds.Keys)
                    if (_userFormatIds[format] == index)
                        return format;

                foreach (string format in _defaultFormatIds.Keys)
                    if (_defaultFormatIds[format] == index)
                        return format;

                throw new IndexOutOfRangeException(string.Format("index {0} not found", index));
            }
        }

        /// <summary>
        /// Adds a new Format object to this collection and returns its id.
        /// </summary>
        /// <param name="format">The Format sting to add to this collection.</param>
        /// <returns>The id of the added Format string.</returns>
        public ushort Add(string format)
        {
            return Add(format, null);
        }
        
		private ushort Add(string format, ushort? id)
		{
            bool isUserFormat = (id == null);

			ushort? existingId = GetID(format);

            bool exists = (existingId != null);

			if (exists)
                return (ushort)existingId;

            if (isUserFormat)
			    id = _nextUserFormatId++;

            _userFormatIds[format] = (ushort)id;

			return (ushort)id;
		}

        internal ushort GetFinalID(string format)
        {
            ushort? id = GetID(format);
            if (id == null)
                throw new ApplicationException(string.Format("Format {0} does not exist", format));
            return (ushort) id;
        }

        internal ushort? GetID(string format)
		{
			ushort? id = null;

            if (_defaultFormatIds.ContainsKey(format))
                id = _defaultFormatIds[format];

            if (_userFormatIds.ContainsKey(format))
                id = _userFormatIds[format];

			return id;
		}

	    internal Bytes Bytes
		{
			get
			{
				Bytes bytes = new Bytes();

			    foreach (string format in _defaultFormatsToWrite)
			    {
                    if (_defaultFormatIds.ContainsKey(format))
                        bytes.Append(GetFormatRecord(_defaultFormatIds[format], format));
                }

				foreach (string format in _userFormatIds.Keys)
				    bytes.Append(GetFormatRecord(_userFormatIds[format], format));

				return bytes;
			}
		}

        /// <summary>
        /// Gets the number of Formats currently contained in this Formats collection.
        /// </summary>
        public object Count
        {
            get { return _userFormatIds.Count + _defaultFormatsToWrite.Count; }
        }

        private Bytes GetFormatRecord(ushort id, string format)
        {
            Bytes bytes = new Bytes();

            bytes.Append(BitConverter.GetBytes(id));
            bytes.Append(XlsDocument.GetUnicodeString(format, 16));

            return Record.GetBytes(RID.FORMAT, bytes);
        }

        /// <summary>
        /// Determines whether this Formats collection contains a value with the given index.
        /// </summary>
        /// <param name="index">The index at which to check for a value.</param>
        /// <returns>true if a value exists at the specified index, false otherwise</returns>
        public bool ContainsKey(ushort index)
        {
            return _defaultFormatIds.ContainsValue(index) || _userFormatIds.ContainsValue(index);
        }
	}
}
