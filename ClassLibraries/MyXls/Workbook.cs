using System;
using System.Collections.Generic;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// Another main class in an XlsDocument.  The Workbook holds all Worksheets, and properties
    /// or settings global to all Worksheets.  It also holds the Fonts, Formats, Styles and XFs
    /// collections.
    /// </summary>
	public class Workbook
	{
		private readonly XlsDocument _doc;

		private readonly Worksheets _worksheets;
		private readonly Fonts _fonts;
		private readonly Formats _formats;
		private readonly Styles _styles;
		private readonly XFs _xfs;
        private readonly Palette _palette;
        private readonly SharedStringTable _sharedStringTable = new SharedStringTable();

        private bool _shareStrings = false;

        private bool _protectContents = false;
        private bool _protectWindowSettings = false;
        private string _password = string.Empty;
        private bool _protectRevisions = false;
        private string _revisionsPassword = string.Empty;

        internal Workbook(XlsDocument doc)
		{
			_doc = doc;

			_worksheets = new Worksheets(_doc);
			_fonts = new Fonts(_doc);
			_formats = new Formats(_doc);
			_styles = new Styles(_doc);
			_xfs = new XFs(_doc, this);
            _palette = new Palette(this);
		}

        internal Workbook(XlsDocument doc, Bytes bytes, BytesReadCallback bytesReadCallback) : this(doc)
        {
            ReadBytes(bytes, bytesReadCallback);
        }

        /// <summary>
        /// Gets the Worksheets collection for this Workbook.
        /// </summary>
		public Worksheets Worksheets
		{
			get { return _worksheets; }
		}

        /// <summary>
        /// Gets the Fonts collection for this Workbook.
        /// </summary>
		public Fonts Fonts
		{
			get { return _fonts; }
		}

        /// <summary>
        /// Gets the Formats collection for this Workbook.
        /// </summary>
		public Formats Formats
		{
			get { return _formats; }
		}

        /// <summary>
        /// Gets the Styles collection for this workbook.
        /// </summary>
		public Styles Styles
		{
			get { return _styles; }
		}

        /// <summary>
        /// Gets or sets whether the contents of this Workbook's Worksheets
        /// are protected (Adding/Removing/Reordering Worksheets, etc.).
        /// </summary>
        public bool ProtectContents
        {
            get { return _protectContents; }
            set { _protectContents = value; }
        }

        /// <summary>
        /// Gets or sets whether this Workbook's Window settings are protected
        /// (Un/Freezing panes, etc.).
        /// </summary>
        public bool ProtectWindowSettings
        {
            get { return _protectWindowSettings; }
            set { _protectWindowSettings = value; }
        }

        /// <summary>
        /// Gets or sets whether this Workbook will optimize for smaller file 
        /// size by utilizing a SharedStringTable for text values.
        /// </summary>
        public bool ShareStrings
        {
            get { return _shareStrings; }
            set { _shareStrings = value; }
        }

        internal SharedStringTable SharedStringTable
        {
            get { return _sharedStringTable; }
        }

        internal XFs XFs
		{
			get { return _xfs; }
		}

        internal Palette Palette
        {
            get { return _palette; }
        }

        internal Bytes Bytes
		{
			get
			{
				if (_worksheets.Count == 0)
					_worksheets.Add("Sheet1");

				Bytes bytesA = new Bytes(); //BOF (inclusive) to 1st BOUNDSHEET (exclusive)
				Bytes bytesB = new Bytes(); //BOUNDSHEETs
				Bytes bytesC = new Bytes(); //BOUNDSHEET (exclusive) to EOF (inclusive)
				Bytes bytesD = new Bytes(); //Worksheet Streams

				//TODO: Break this down to component option bits (BOF Function/Class?)
				bytesA.Append(Record.GetBytes(RID.BOF, new byte[] { 0x00, 0x06, 0x05, 0x00, 0xAF, 0x18, 0xCD, 0x07, 0xC9, 0x40, 0x00, 0x00, 0x06, 0x01, 0x00, 0x00 }));

                //<Workbook Protection Block>
                if (_protectContents)
                    bytesA.Append(Record.GetBytes(RID.PROTECT, new byte[] { 0x01, 0x00 }));
                if (_protectWindowSettings)
                    bytesA.Append(Record.GetBytes(RID.WINDOWPROTECT, new byte[] { 0x01, 0x00 }));
                //</Workbook Protection Block>

				//TODO: Break this down to component option bits (WINDOW1 Function/Class?)
				bytesA.Append(Record.GetBytes(RID.WINDOW1, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x58, 0x02 }));

				//TODO: Implement private functions for these
				bytesA.Append(_fonts.Bytes);
			    bytesA.Append(_formats.Bytes);
				bytesA.Append(_xfs.Bytes);
				bytesA.Append(Record.GetBytes(RID.STYLE, new byte[] { 0x10, 0x80, 0x00, 0xFF })); //STYLE

                if (SharedStringTable.CountUnique > 0)
                    bytesC.Append(SharedStringTable.Bytes);
                bytesC.Append(Record.GetBytes(RID.EOF, new byte[0])); //EOF

				int basePosition = bytesA.Length + bytesC.Length;
				for (int i = 0; i < _worksheets.Count; i++)
					basePosition += XlsDocument.GetUnicodeString(_worksheets[i].Name, 8).Length + 10; //Add length of BOUNDSHEET Records

				_worksheets.StreamOffset = basePosition;

				for (int i = 0; i < _worksheets.Count; i++)
				{
					if (i > 0)
						basePosition += _worksheets[i - 1].StreamByteLength;

					Worksheet sheet = _worksheets[i];
					bytesB.Append(BOUNDSHEET(sheet, basePosition));
					bytesD.Append(sheet.Bytes);
				}

				bytesA.Append(bytesB);
				bytesA.Append(bytesC);
				bytesA.Append(bytesD);

				if (_doc.ForceStandardOle2Stream)
					bytesA = _doc.GetStandardOLE2Stream(bytesA);

				return bytesA;
			}
        }

        #region For Unit Testing Only
        internal delegate void BytesReadCallback(List<Record> records);
        #endregion

        private void ReadBytes(Bytes bytes, BytesReadCallback bytesReadCallback)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            if (bytes.Length == 0)
                throw new ArgumentException("can't be zero-length", "bytes");

            //The XF's read in won't necessarily have the same ID (index) once added to this Workbook,
            //so we need to keep the cross-reference list for re-assignment as we read in the cell records later
            SortedList<ushort, ushort> xfIdLookups = new SortedList<ushort, ushort>();

            List<Record> records = Record.GetAll(bytes);

            List<Record> fontRecords = new List<Record>();
            List<Record> formatRecords = new List<Record>();
            List<Record> xfRecords = new List<Record>();
            List<Record> boundSheetRecords = new List<Record>();
            Record sstRecord = Record.Empty;

            SortedList<int, List<Record>> sheetRecords = new SortedList<int, List<Record>>();

            int sheetIndex = -1;

            foreach (Record record in records)
            {
                if (sheetIndex >= 0)
                {
                    if (!sheetRecords.ContainsKey(sheetIndex))
                        sheetRecords[sheetIndex] = new List<Record>();
                    sheetRecords[sheetIndex].Add(record);
                    if (record.RID == RID.EOF)
                        sheetIndex++;
                }
                else if (record.RID == RID.FONT)
                    fontRecords.Add(record);
                else if (record.RID == RID.FORMAT)
                    formatRecords.Add(record);
                else if (record.RID == RID.XF)
                    xfRecords.Add(record);
                else if (record.RID == RID.BOUNDSHEET)
                    boundSheetRecords.Add(record);
                else if (record.RID == RID.SST)
                    sstRecord = record;
                else if (record.RID == RID.EOF)
                    sheetIndex++;
            }

            SortedList<ushort, Font> fonts = new SortedList<ushort, Font>();
            SortedList<ushort, string> formats = new SortedList<ushort, string>();
            SortedList<ushort, XF> xfs = new SortedList<ushort, XF>();

            ushort index = 0;
            foreach (Record record in fontRecords)
            {
                Font font = new Font(_doc, record.Data);
                fonts[index++] = font;
                this.Fonts.Add(font);
            }

            foreach (Record record in formatRecords)
            {
                Bytes recordData = record.Data;
                string format = UnicodeBytes.Read(recordData.Get(2, recordData.Length - 2), 16);
                index = BitConverter.ToUInt16(recordData.Get(2).ByteArray, 0);
                formats[index] = format;
                this.Formats.Add(format);
            }

            index = 0;
            for (index = 0; index < xfRecords.Count; index++)
            {
                Record record = xfRecords[index];
                Bytes recordData = record.Data;
                ushort fontIndex = BitConverter.ToUInt16(recordData.Get(0, 2).ByteArray, 0);
                ushort formatIndex = BitConverter.ToUInt16(recordData.Get(2, 2).ByteArray, 0);
                //ushort styleIndex = BitConverter.ToUInt16(recordData.Get(4, 2))
                if (!fonts.ContainsKey(fontIndex))
                    continue; //TODO: Perhaps default to default XF?  NOTE: This is encountered with TestReferenceFile BlankBudgetWorksheet.xls
                Font font = fonts[fontIndex];
                string format;
                if (formats.ContainsKey(formatIndex))
                    format = formats[formatIndex];
                else if (_formats.ContainsKey(formatIndex))
                    format = _formats[formatIndex];
                else
                    throw new ApplicationException(string.Format("Format {0} not found in read FORMAT records or standard/default FORMAT records.", formatIndex));
                xfIdLookups[index] = this.XFs.Add(new XF(_doc, record.Data, font, format));
            }
            this.XFs.XfIdxLookups = xfIdLookups;

            if (sstRecord != Record.Empty)
                this.SharedStringTable.ReadBytes(sstRecord);

            if (bytesReadCallback != null)
                bytesReadCallback(records);

            for (int i = 0; i < boundSheetRecords.Count; i++)
            {
                _worksheets.Add(boundSheetRecords[i], sheetRecords[i]);
            }
        }

        private static Bytes BOUNDSHEET(Worksheet sheet, int basePosition)
		{
			Bytes bytes = new Bytes();

			Bytes sheetName = XlsDocument.GetUnicodeString(sheet.Name, 8);
			bytes.Append(WorksheetVisibility.GetBytes(sheet.Visibility));
			bytes.Append(WorksheetType.GetBytes(sheet.SheetType));
			bytes.Append(sheetName);
			bytes.Prepend(BitConverter.GetBytes((int) basePosition)); //TODO: this should probably be unsigned 32 instead

			bytes.Prepend(BitConverter.GetBytes((ushort) bytes.Length));
			bytes.Prepend(RID.BOUNDSHEET);

			return bytes;
		}
	}
}
