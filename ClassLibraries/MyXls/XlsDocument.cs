using System;
using System.IO;
using System.Text;
using org.in2bits.MyOle2;
using org.in2bits.MyOle2.Metadata;
using org.in2bits.MyXls.ByteUtil;
using System.Data;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// The root class of MyXls, containing general configuration properties and references to
    /// the Workbook as well as the MyOle2 Document which will contain and format the workbook
    /// when Sending or Saving.
    /// </summary>
    public class XlsDocument
    {
        private readonly MyOle2.Ole2Document _ole2Doc;
        private readonly Workbook _workbook;
        private readonly SummaryInformationSection _summaryInformation;
        private readonly DocumentSummaryInformationSection _documentSummaryInformation;

        private string _fileName = "Book1.xls";
        private bool _isLittleEndian = true;
        private bool _forceStandardOle2Stream = false;

        /// <summary>
        /// Initializes a new instance of the XlsDocument class.
        /// </summary>
        public XlsDocument()
        {
            _forceStandardOle2Stream = false;
            _isLittleEndian = true;

            _ole2Doc = new MyOle2.Ole2Document();
            SetOleDefaults();

            _summaryInformation = new SummaryInformationSection();
            _documentSummaryInformation = new DocumentSummaryInformationSection();

            _workbook = new Workbook(this);
        }

        /// <summary>
        /// Initializes a new XlsDocument from the provided file, reading in as much information
        /// from the file as possible and representing it appropriately with MyXls objects
        /// (Workbook, Worksheets, Cells, etc.).
        /// </summary>
        /// <param name="fileName">The name of the file to read into this XlsDocument.</param>
        public XlsDocument(string fileName)
            : this(fileName, null)
        { }

        internal XlsDocument(string fileName, Workbook.BytesReadCallback workbookBytesReadCallback)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("Can't be null or Empty", "fileName");

            if (!File.Exists(fileName))
                throw new FileNotFoundException("Excel File not found", fileName);

            _ole2Doc = new Ole2Document();
            using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _ole2Doc.Load(fileStream);
            }

            //TODO: SummaryInformationSection and DocumentSummaryInformationSections should be read by MyOle2
            _workbook = new Workbook(this, _ole2Doc.Streams[_ole2Doc.Streams.GetIndex(org.in2bits.MyOle2.Directory.Biff8Workbook)].Bytes, workbookBytesReadCallback);
        }

        /// <summary>
        /// Initializes a new XlsDocument from the provided stream, reading in as much information
        /// from the file as possible and representing it appropriately with MyXls objects
        /// (Workbook, Worksheets, Cells, etc.).
        /// </summary>
        /// <param name="stream">The stream to read into this XlsDocument.</param>
        public XlsDocument(System.IO.Stream stream)
        {

            if (stream == null)
                throw new ArgumentException("Can't be null", "steam");

            _ole2Doc = new Ole2Document();
            _ole2Doc.Load(stream);

            //TODO: SummaryInformationSection and DocumentSummaryInformationSections should be read by MyOle2
            _workbook = new Workbook(this, _ole2Doc.Streams[_ole2Doc.Streams.GetIndex(org.in2bits.MyOle2.Directory.Biff8Workbook)].Bytes, null);
        }

        /// <summary>
        /// Initializes a new XlsDocument from the provided DataSet.  Each Table in the
        /// DataSet will be written to a separate Worksheet.  The data for each table will
        /// be written starting at A1 on each sheet, with a Header Row (A1 will be the
        /// name of the first Column, A2 will be the first value).
        /// </summary>
        /// <param name="dataSet">The DataSet to write to this XlsDocument.</param>
        public XlsDocument(DataSet dataSet)
            : this()
        {
            this.FileName = dataSet.DataSetName;
            foreach (DataTable table in dataSet.Tables)
            {
                Worksheet sheet = Workbook.Worksheets.Add(table.TableName);
                sheet.Write(table, 1, 1);
            }
        }

        /// <summary>
        /// Gets or sets the FileName (no path) for this XlsDocument (effective when sending
        /// to a Web client via the Send method).
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("FileName cannot be null or Empty");

                if (string.Compare(value.Substring(value.Length - 4), ".xls", true) != 0)
                    value = string.Format("{0}.xls", value);

                _fileName = value;
            }
        }

        private void SetOleDefaults()
        {
            //Can be any 16-byte value per OOo compdocfileformat.pdf
            _ole2Doc.DocUID = new byte[16];

            //Most common (BIFF8) value per OOo compdocfileformat.pdf
            _ole2Doc.SectorSize = 9;

            //Most common (BIFF8) value per OOo compdocfileformat.pdf
            _ole2Doc.ShortSectorSize = 6;

            //Most common (BIFF8) value per OOo compdocfileformat
            _ole2Doc.StandardStreamMinBytes = 4096;
        }

        /// <summary>
        /// Gets this org.in2bits.MyXls XlsDocument's OLE2 Document.
        /// </summary>
        public MyOle2.Ole2Document OLEDoc { get { return _ole2Doc; } }

        /// <summary>
        /// Gets this org.in2bits.MyXls XlsDocument's Workbook.
        /// </summary>
        public Workbook Workbook { get { return _workbook; } }

        /// <summary>
        /// Gets a Bytes object containing all the bytes of this OleDocument's stream.
        /// </summary>
        public Bytes Bytes
        {
            get
            {
                _ole2Doc.Streams.AddNamed(_workbook.Bytes, BIFF8.NameWorkbook, true);

                MetadataStream summaryInformationStream = new MetadataStream(_ole2Doc);
                summaryInformationStream.Sections.Add(_summaryInformation);
                _ole2Doc.Streams.AddNamed(GetStandardOLE2Stream(summaryInformationStream.Bytes), BIFF8.NameSummaryInformation, true);

                MetadataStream documentSummaryInformationStream = new MetadataStream(_ole2Doc);
                documentSummaryInformationStream.Sections.Add(_documentSummaryInformation);
                _ole2Doc.Streams.AddNamed(GetStandardOLE2Stream(documentSummaryInformationStream.Bytes), BIFF8.NameDocumentSummaryInformation, true);

                return _ole2Doc.Bytes;
            }
        }

        /// <summary>
        /// Gets the SummaryInformationSection for this XlsDocument.
        /// </summary>
        public SummaryInformationSection SummaryInformation
        {
            get { return _summaryInformation; }
        }

        /// <summary>
        /// Gets the DocumentSummaryInformationSection for this XlsDocument.
        /// </summary>
        public DocumentSummaryInformationSection DocumentSummaryInformation
        {
            get { return _documentSummaryInformation; }
        }

        /// <summary>
        /// Methods available to send XlsDocument to HTTP Client (Content-disposition header setting)
        /// </summary>
        public enum SendMethods
        {
            /// <summary>The client browser should try to open the file within browser window.</summary>
            Inline,

            /// <summary>The client browser should prompt to Save or Open the file.</summary>
            Attachment
        }

        private static string GetContentDisposition(SendMethods sendMethod)
        {
            if (sendMethod == SendMethods.Attachment)
                return "attachment";
            else if (sendMethod == SendMethods.Inline)
                return "inline";
            else
                throw new NotSupportedException();
        }

        /// <summary>
        /// Valid in an ASP.NET context only.  Sends this XlsDocument to the client with the 
        /// given FileName as an attachment file, via the HttpResponse object.  Clears
        /// the Response before sending and ends the Response after sending.
        /// </summary>
        public void Send()
        {
            Send(SendMethods.Attachment);
        }

        /// <summary>
        /// Valid in an ASP.NET context only.  Sends this XlsDocument to the client with the 
        /// given FileName as an attached or inline file, via the HttpResponse object.  Clears
        /// the Response before sending and ends the Response after sending.
        /// </summary>
        /// <param name="sendMethod">Method to use to send document.</param>
        public void Send(SendMethods sendMethod)
        {
            //only applies in Classic ASP context?
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            if (context == null)
                throw new ApplicationException("Current System.Web.HttpContext not found - Send failed.");

            if (!context.Response.Buffer)
            {
                context.Response.Buffer = true;
                context.Response.Clear();
            }

            context.Response.ContentType = "application/vnd.ms-excel";
            context.Response.AddHeader("Content-Disposition", string.Format("{0};filename={1}", GetContentDisposition(sendMethod), FileName));
            context.Response.Flush();
            context.Response.BinaryWrite(Bytes.ByteArray);
        }

        /// <summary>
        /// Save this XlsDocument to the Current Directory. The FileName property will be used for
        /// the FileName.
        /// </summary>
        /// <param name="overwrite">Whether to overwrite if the specified file already exists.</param>
        public void Save(bool overwrite)
        {
            Save(null, overwrite);
        }

        /// <summary>
        /// Save this XlsDocument to the Current Directory.  The FileName property will be used
        /// for the FileName.  Will not overwrite.
        /// </summary>
        public void Save()
        {
            Save(null, false);
        }

        /// <summary>
        /// Save this XlsDocument to the given path.  The FileName property will be used for the
        /// FileName.  Will not overwrite.
        /// </summary>
        /// <param name="path">The Path to which to save this XlsDocument.</param>
        public void Save(string path)
        {
            Save(path, false);
        }

        /// <summary>
        /// Save this XlsDocument to the given path.  The FileName property will be used for the
        /// FileName.
        /// </summary>
        /// <param name="path">The Path to which to save this XlsDocument.</param>
        /// <param name="overwrite">Whether to overwrite if the specified file already exists.</param>
        public void Save(string path, bool overwrite)
        {
            path = path ?? Environment.CurrentDirectory;
            string fileName = Path.Combine(path, FileName);
            using (FileStream fs = new FileStream(fileName, overwrite ? FileMode.Create : FileMode.CreateNew))
            {
                Bytes.WriteToStream(fs);
                fs.Flush();
            }
        }

        /// <summary>
        /// Saves the XlsDocument to the given stream
        /// </summary>
        /// <param name="outStream"></param>
        public void Save(System.IO.Stream outStream)
        {
            Bytes.WriteToStream(outStream);
        }

        /// <summary>
        /// Gets whether this XlsDocument is Little Endian.  In the current implementation of
        /// MyXLS, this is always true.
        /// </summary>
        public bool IsLittleEndian
        {
            get { return _isLittleEndian; }
        }

        /// <summary>
        /// Gets or sets whether to force the XlsDocument data to be padded to the length
        /// of a Standard Stream in its MyOle2 document container, if it is less than 
        /// standard length without padding.
        /// </summary>
        public bool ForceStandardOle2Stream
        {
            get { return _forceStandardOle2Stream; }
            set { _forceStandardOle2Stream = value; }
        }

        internal Bytes GetStandardOLE2Stream(Bytes bytes)
        {
            uint standardLength = _ole2Doc.StandardStreamMinBytes;
            uint padLength = standardLength = ((uint)bytes.Length % standardLength);
            if (padLength < standardLength)
                bytes.Append(new byte[padLength]);
            return bytes;
        }

        internal static Bytes GetUnicodeString(string text, int lengthBits)
        {
            int textLength;
            int limit = lengthBits == 8 ? byte.MaxValue : ushort.MaxValue;
            byte[] binaryLength = new byte[0];
            byte[] compression;
            byte[] compressedText = new byte[0];

            textLength = text.Length;
            if (textLength > limit)
                text = text.Substring(0, limit); //NOTE: Should throw Exception here?

            if (limit == 255)
                binaryLength = new byte[1] { (byte)text.Length };
            else if (limit == 65535)
                binaryLength = BitConverter.GetBytes((ushort)text.Length);

            if (IsCompressible(text))
            {
                compression = new byte[1];
                char[] chars = text.ToCharArray();
                compressedText = new byte[chars.Length];
                for (int i = 0; i < chars.Length; i++)
                    compressedText[i] = (byte)chars[i];
            }
            else
            {
                compression = new byte[1] { 1 };
            }

            Bytes bytes = new Bytes();
            bytes.Append(binaryLength);
            bytes.Append(compression);
            if (compressedText.Length > 0)
                bytes.Append(compressedText);
            else
                bytes.Append(Encoding.Unicode.GetBytes(text));
            return bytes;
        }

        //TODO: Create optional setting for this optimization (to force all strings to unicode
        //storage so this check doesn't reduce performance - similar to Workbook.ShareStrings)
        private static bool IsCompressible(string text)
        {
            byte[] textBytes = Encoding.Unicode.GetBytes(text);

            for (int i = 1; i < textBytes.Length; i += 2)
            {
                if (textBytes[i] != 0)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Gets a new XF (Formatting) object for use on this document.
        /// </summary>
        /// <returns>New XF formatting object.</returns>
        public XF NewXF()
        {
            return new XF(this);
        }
    }
}

