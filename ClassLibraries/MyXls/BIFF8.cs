namespace org.in2bits.MyXls
{
    /// <summary>
    /// Contains constant values pertaining to the BIFF8 format for use in MyXLS.
    /// </summary>
	public static class BIFF8
	{
        /// <summary>
        /// The name of the Workbook stream in an OLE2 Document.
        /// </summary>
		public static readonly byte[] NameWorkbook = new byte[] {
			0x57, 0x00, 0x6F, 0x00, 0x72, 0x00, 0x6B, 0x00, 0x62, 0x00, 0x6F, 0x00, 0x6F, 0x00, 0x6B, 0x00, 
			0x00, 0x00};
        
        /// <summary>
        /// The name of the SummaryInformation stream in an OLE2 Document.
        /// </summary>
		public static readonly byte[] NameSummaryInformation = new byte[] {
			0x05, 0x00, 0x53, 0x00, 0x75, 0x00, 0x6D, 0x00, 0x6D, 0x00, 0x61, 0x00, 0x72, 0x00, 0x79, 0x00,
			0x49, 0x00, 0x6E, 0x00, 0x66, 0x00, 0x6F, 0x00, 0x72, 0x00, 0x6D, 0x00, 0x61, 0x00, 0x74, 0x00,
			0x69, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x00, 0x00};

        /// <summary>
        /// The name of the DocumentSummaryInformation stream in an OLE2 Document.
        /// </summary>
        public static readonly byte[] NameDocumentSummaryInformation = new byte[] {
            0x05, 0x00, 0x44, 0x00, 0x6F, 0x00, 0x63, 0x00, 0x75, 0x00, 0x6D, 0x00, 0x65, 0x00, 0x6E, 0x00,
            0x74, 0x00, 0x53, 0x00, 0x75, 0x00, 0x6D, 0x00, 0x6D, 0x00, 0x61, 0x00, 0x72, 0x00, 0x79, 0x00,
            0x49, 0x00, 0x6E, 0x00, 0x66, 0x00, 0x6F, 0x00, 0x72, 0x00, 0x6D, 0x00, 0x61, 0x00, 0x74, 0x00,
            0x69, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x00, 0x00};

        /// <summary>
        /// The maximum rows a BIFF8 document may contain.
        /// </summary>
        public const ushort MaxRows = ushort.MaxValue;

        /// <summary>
        /// The maximum columns a BIFF8 document may contain.
        /// </summary>
        public const ushort MaxCols = byte.MaxValue;

        /// <summary>
        /// The maximum number of bytes in a BIFF8 record (minus 4 bytes for 
        /// the Record ID and the data size, leaves 8224 bytes for data).
        /// </summary>
        public const ushort MaxBytesPerRecord = 8228;

        /// <summary>
        /// The maximum number of bytes available for data in a BIFF8 record
        /// (plus 4 bytes for the Record ID and the data size, gives 8228 total
        /// bytes).
        /// </summary>
        public const ushort MaxDataBytesPerRecord = 8224;

        /// <summary>
        /// The Maximum number of characters that can be written to or read
        /// from a Cell in Excel.  I'm guessing it is short.MaxValue instead 
        /// of ushort.MaxValue to allow for double-byte chars (Unicode).
        /// </summary>
        public const ushort MaxCharactersPerCell = (ushort)short.MaxValue;
	}
}
