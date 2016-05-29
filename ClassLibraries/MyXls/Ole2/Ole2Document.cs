using System;
using System.Collections.Generic;
using System.IO;
using org.in2bits.MyXls.ByteUtil;
using Io = System.IO;

namespace org.in2bits.MyOle2
{
    /// <summary>
    /// Represents an OLE2 Compound Document format Document.
    /// </summary>
	public class Ole2Document
	{
		private readonly Header _header;
		private readonly Msat _msat;
		private readonly Sat _sat;
		private readonly Ssat _ssat;
		private readonly Directory _directory;
		private readonly Streams _streams;

		private byte[] _docFileID;
		private byte[] _docUID;
		private byte[] _fileFormatRevision;
		private byte[] _fileFormatVersion;
		private bool _isLittleEndian;
		private ushort _sectorSize; //in power of 2 (min = 7)
		private ushort _shortSectorSize; //in power of 2 (max = ShortSectorSize)
		private byte[] _blank1;
		private byte[] _blank2;
		private uint _standardStreamMinBytes; //in bytes

		private int _bytesPerSector;
		private int _bytesPerShortSector;

        /// <summary>
        /// Initializes a new, blank, Ole2Document object.
        /// </summary>
		public Ole2Document()
		{
            SetDefaults();

            _header = new Header(this);
			_msat = new Msat(this);
			_sat = new Sat(this);
			_ssat = new Ssat(this);
			_directory = new Directory(this);
			_streams = new Streams(this);

			_streams.AddNamed(new Bytes(), Directory.RootName);
		}

        /// <summary>
        /// Gets this Ole2Document's Header object.
        /// </summary>
		public Header Header { get { return _header; } }

        /// <summary>
        /// Gets this Ole2Document's MSAT (Main Sector Allocation Table) object.
        /// </summary>
        public Msat MSAT { get { return _msat; } }

        /// <summary>
        /// Gets this Ole2Document's Directory object.
        /// </summary>
		public Directory Directory { get { return _directory; } }

        /// <summary>
        /// Gets this Ole2Document's Streams collection.
        /// </summary>
        public Streams Streams { get { return _streams; } }

        /// <summary>
        /// Gets this Ole2Document's SSAT (Short Sector Allocation Table) object.
        /// </summary>
        public Ssat SSAT { get { return _ssat; } }

        /// <summary>
        /// Gets this Ole2Document's SAT (Sector Allocation Table) object.
        /// </summary>
        public Sat SAT { get { return _sat; } }

        /// <summary>
        /// Gets the number of bytes per Short Sector in this Ole2Document (set with ShortSectorSize).
        /// </summary>
		public int BytesPerShortSector
		{
			get { return _bytesPerShortSector; }
		}

        /// <summary>
        /// Gets the number of bytes per Sector in this Ole2Document (set with SectorSize).
        /// </summary>
		public int BytesPerSector
		{
			get { return _bytesPerSector; }
		}

        /// <summary>
        /// Gets or sets the number of bytes per Standard (not short) Stream in this Ole2Document.
        /// This is not normally changed from its default.
        /// </summary>
		public uint StandardStreamMinBytes
		{
			get { return _standardStreamMinBytes; }
			set
			{
				if (value <= 2)
                    throw new ArgumentOutOfRangeException("value", "must be > 2");

				_standardStreamMinBytes = value;
			}
		}

        /// <summary>
        /// Gets or sets the Ole2Document FileID of this Ole2Document.
        /// This is not normally changed from its default.
        /// </summary>
		public byte[] DocFileID
		{
			get { return _docFileID; }
			set
			{
				if (value.Length != 8)
                    throw new ArgumentOutOfRangeException("value", "must be 8 bytes in length");

				_docFileID = value;
			}
		}

        /// <summary>
        /// Gets or sets the Ole2Document UID of this Ole2Document.
        /// This is not normally changed from its default.
        /// </summary>
		public byte[] DocUID
		{
			get { return _docUID; }
			set
			{
				if (value.Length != 16)
                    throw new ArgumentOutOfRangeException("value", "must be 16 bytes in length");

				_docUID = value;
			}
		}

        /// <summary>
        /// Gets or sets the FileFormatRevision of this Ole2Document.
        /// This is not normally changed from its default.
        /// </summary>
		public byte[] FileFormatRevision
		{
			get { return _fileFormatRevision; }
			set
			{
				if (value.Length != 2)
                    throw new ArgumentOutOfRangeException("value", "must be 2 bytes in length");
		
				_fileFormatRevision = value;
			}
		}

        /// <summary>
        /// Gets or sets the FileFormatVersion of this Ole2Document.
        /// This is not normally changed from its default.
        /// </summary>
		public byte[] FileFormatVersion
		{
			get { return _fileFormatVersion; }
			set
			{
				if (value.Length != 2)
                    throw new ArgumentOutOfRangeException("value", "must be 2 bytes in length");

				_fileFormatVersion = value;
			}
		}


        /// <summary>
        /// Gets whether this Ole2Document should be encoded in Little Endian (or
        /// Big Endian) format.  Currently only Little Endian is supported.
        /// </summary>
		public bool IsLittleEndian
		{
			get { return _isLittleEndian; }
			set { if (value == false) throw new NotSupportedException("Big Endian not currently supported"); _isLittleEndian = value; }
		}

        /// <summary>
        /// Gets or sets the Sector Size of this Ole2Document.  This is the number of bytes
        /// per standard sector as a power of 2 (e.g. setting this to 9 sets the
        /// BytesPerSector to 2 ^ 9 or 512).  This is not normally changed from its
        /// default of 9.
        /// </summary>
		public ushort SectorSize
		{
			get { return _sectorSize; }
			set
			{
				if (value < 7)
                    throw new ArgumentOutOfRangeException("value", "must be >= 7");

				_sectorSize = value;
				_bytesPerSector = (int)Math.Pow(2, value);
			}
		}

        /// <summary>
        /// Gets or sets the Short Sector Size of this Ole2Document.  This is the number of 
        /// bytes per short sector as a power of 2 (e.g. setting this to 6 sets the
        /// BytesPerShortSector to 2 ^ 6 or 128).  This is not normally changed from
        /// its default of 6.
        /// </summary>
		public ushort ShortSectorSize
		{
			get { return _shortSectorSize; }
			set
			{
				if (value > SectorSize)
					throw new ArgumentOutOfRangeException(string.Format("value must be <= SectorSize {0}", SectorSize));

				 _shortSectorSize = value;
				_bytesPerShortSector = (int)Math.Pow(2, ShortSectorSize);
			}
		}

		private void SetDefaults()
		{
			//Standard from OOo compdocfileformat.pdf
			DocFileID = new byte[] {0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1};

			//Can be any 16-byte value per OOo compdocfileformat.pdf
			DocUID = new byte[16];

			//Unused standard per OOo compdocfileformat.pdf
			FileFormatRevision = new byte[] {0x3E, 0x00};

			//Unused standard per OOo compdocfileformat.pdf
			FileFormatVersion = new byte[] {0x03, 0x00};

			//Standard per OOo compdocfileformat
			IsLittleEndian = true;

			//Most common (BIFF8) value per OOo compdocfileformat.pdf
			SectorSize = 9;

			//Most common (BIFF8) value per OOo compdocfileformat.pdf
			ShortSectorSize = 6;

			//Unused per OOo compdocfileformat.pdf
			_blank1 = new byte[10];

			//Unused per OOo compdocfileformat.pdf
			_blank2 = new byte[4];

			//Most common (BIFF8) value per OOo compdocfileformat
			StandardStreamMinBytes = 4096;
		}

        /// <summary>
        /// Gets a Bytes object containing all the bytes of this Ole2Document.  This Bytes object's
        /// ByteArray is what should be saved to disk to persist this Ole2Document as a file on disk.
        /// </summary>
        public Bytes Bytes
		{
			get
			{
				Bytes bytes = new Bytes();

				//===================================================
				//DON'T CHANGE THE ORDER HERE OR ALL SID0'S WILL BE OFF
				//TODO: Should refactor this to function which does
				//lookups via a section-order array (would have to
				//check this for determining SID0's)
				//===================================================
				bytes.Append(Header.Bytes);
				bytes.Append(MSAT.Bytes);
				bytes.Append(SAT.Bytes);
				bytes.Append(SSAT.Bytes);
				bytes.Append(Streams.Bytes);
				bytes.Append(Directory.Bytes);

				return bytes;
			}
		}

        /// <summary>
        /// Loads this Ole2Document object from the provided stream (e.g. a FileStream to load
        /// from a File).  This is only preliminarily supported and tested for Excel
        /// files.
        /// </summary>
        /// <param name="stream">Stream to load the document from.</param>
		public void Load(System.IO.Stream stream)
		{
			if (stream.Length == 0)
				throw new Exception("No data (or zero-length) found!");

			if (stream.Length < 512)
				throw new Exception(string.Format("File length {0} < 512 bytes", stream.Length));

			byte[] head = new byte[512];
			stream.Read(head, 0, 512);

			bool isLE = false;
			if (head[28] == 254 && head[29] == 255)
				isLE = true;

			if (!isLE)
				throw new NotSupportedException("File is not Little-Endian");
			_isLittleEndian = isLE;

			ushort sectorSize = BitConverter.ToUInt16(MidByteArray(head, 30, 2), 0);
			if (sectorSize < 7 || sectorSize > 32)
				throw new Exception(string.Format("Invalid Sector Size [{0}] (should be 7 <= sectorSize <= 32", sectorSize));
			_sectorSize = sectorSize;

			ushort shortSectorSize = BitConverter.ToUInt16(MidByteArray(head, 32, 2), 0);
			if (shortSectorSize > sectorSize)
				throw new Exception(
					string.Format("Invalid Short Sector Size [{0}] (should be < sectorSize; {1})", shortSectorSize, sectorSize));
			_shortSectorSize = shortSectorSize;

			//if (readError) ExitFunction;

			uint satSectorCount = BitConverter.ToUInt32(MidByteArray(head, 44, 4), 0);
			if (satSectorCount < 0)
				throw new Exception(string.Format("Invalid SAT Sector Count [{0}] (should be > 0)", satSectorCount));

		    int dirSID0 = BitConverter.ToInt32(MidByteArray(head, 48, 4), 0);
            if (dirSID0 < 0)
                throw new Exception(string.Format("Invalid Directory SID0 [{0}] (should be > 0)", dirSID0));

		    uint minStandardStreamSize = BitConverter.ToUInt32(MidByteArray(head, 56, 4), 0);
            if ((minStandardStreamSize < (Math.Pow(2, sectorSize))) || (minStandardStreamSize % (Math.Pow(2, sectorSize)) > 0))
                throw new Exception(string.Format("Invalid MinStdStreamSize [{0}] (should be multiple of (2^SectorSize)", minStandardStreamSize));
		    _standardStreamMinBytes = minStandardStreamSize;

		    int ssatSID0 = BitConverter.ToInt32(MidByteArray(head, 60, 4), 0);
		    uint ssatSectorCount = BitConverter.ToUInt32(MidByteArray(head, 64, 4), 0);
            if (ssatSID0 < 0 && ssatSID0 != -2)
                throw new Exception(string.Format("Invalid SSAT SID0 [{0}] (must be >=0 or -2", ssatSID0));
            if (ssatSectorCount > 0 && ssatSID0 < 0)
                throw new Exception(
                    string.Format("Invalid SSAT SID0 [{0}] (must be >=0 when SSAT Sector Count > 0)", ssatSID0));
            if (ssatSectorCount < 0)
                throw new Exception(string.Format("Invalid SSAT Sector Count [{0}] (must be >= 0)", ssatSectorCount));

		    int msatSID0 = BitConverter.ToInt32(MidByteArray(head, 68, 4), 0);
            if (msatSID0 < 1 && msatSID0 != -2)
                throw new Exception(string.Format("Invalid MSAT SID0 [{0}]", msatSID0));

		    uint msatSectorCount = BitConverter.ToUInt32(MidByteArray(head, 72, 4), 0);
            if (msatSectorCount < 0)
                throw new Exception(string.Format("Invalid MSAT Sector Count [{0}]", msatSectorCount));
            else if (msatSectorCount == 0 && msatSID0 != -2)
                throw new Exception(string.Format("Invalid MSAT SID0 [{0}] (should be -2)", msatSID0));

		    int i = 0;
		    int k = ((int)Math.Pow(2, sectorSize) / 4) - 1;
		    int[] msat = new int[108 + (k * msatSectorCount) + 1]; //add 1 compared to VBScript version due to C#/VBS array declaration diff
            for (int j = 0; j < 109; j++)
                msat[j] = BitConverter.ToInt32(MidByteArray(head, 76 + (j * 4), 4), 0);
		    int msatSidNext = msatSID0;
            while (i < msatSectorCount)
            {
                Bytes sector = GetSector(stream, sectorSize, msatSidNext);
                if (sector.Length == 0)
                    throw new Exception(string.Format("MSAT SID Chain broken - SID [{0}] not found / EOF reached", msatSidNext));
                for (int j = 0; j < k; j++)
                    msat[109 + (i * k) + j] = BitConverter.ToInt32(sector.Get(j * 4, 4).ByteArray, 0);
                msatSidNext = BitConverter.ToInt32(sector.Get(k * 4, 4).ByteArray, 0);
                i++;
            }

            //if (re) Exit Function;

            //Find number of Sectors in SAT --> i
		    i = msat.Length;
            while (msat[i - 1] < 0)
                i--;

            //Size and fill SAT SID array
		    int[] sat = new int[(uint) (i * (Math.Pow(2, sectorSize) / 4))];
		    int m = (int)(Math.Pow(2, sectorSize) / 4);
            for (int j = 0; j < i; j++)
            {
                Bytes sector = GetSector(stream, sectorSize, msat[j]);
                if (sector.Length == 0)
                    throw new Exception(string.Format("SAT SID Chain broken - SAT Sector SID{0} not found / EOF reached", msat[j]));
                for (k = 0; k < m; k++)
                    sat[(j * m) + k] = BitConverter.ToInt32(sector.Get(k * 4, 4).ByteArray, 0);
            }

            //Size and fill SSAT SID array
		    i = 0;
		    int ssatSidNext = ssatSID0;
//		    m = (int) (Math.Pow(2, sectorSize) / 4);
		    //Dictionary<int, int> ssat = new Dictionary<int, int>();
		    int[] ssat = new int[(ssatSectorCount + 1) * m];
            while (ssatSidNext > -2)
            {
                Bytes sector = GetSector(stream, sectorSize, ssatSidNext);
                if (sector.Length == 0)
                    throw new Exception(string.Format("SSAT Sector SID{0} not found", ssatSidNext));
                for (int j = 0; j < m; j++)
                    ssat[(i * m) + j] = BitConverter.ToInt32(sector.Get(j * 4, 4).ByteArray, 0);
                ssatSidNext = sat[ssatSidNext];
                i++;
            }
            if (i < ssatSectorCount)
                throw new Exception(string.Format("SSAT Sector chain broken: {0} found, header indicates {1}", i, ssatSectorCount));

            //Size and fill Directory byte array array
		    int dirSectorCount = 0;
		    int dirSidNext = dirSID0;
		    m = (int) (Math.Pow(2, sectorSize) / 128);
		    Dictionary<int, byte[]> dir = new Dictionary<int, byte[]>();
            while (dirSidNext > -2)
            {
                Bytes sector = GetSector(stream, sectorSize, dirSidNext);
                if (sector.Length == 0)
                    throw new Exception(string.Format("Directory Sector SID{0} not found", dirSidNext));
                for (int j = 0; j < m; j++)
                    dir[(dirSectorCount * m) + j] = sector.Get(j * 128, 128).ByteArray;
                dirSidNext = sat[dirSidNext];
                dirSectorCount++;
            }

            for (i = 0; i < dir.Count; i++)
            {
                byte[] dirEntry = dir[i];
                int nameLength = BitConverter.ToInt16(MidByteArray(dirEntry, 64, 2), 0);
                byte[] docStreamName = MidByteArray(dirEntry, 0, nameLength);
                bool overwrite = false;
                if (Bytes.AreEqual(docStreamName, Directory.RootName))
                    overwrite = true;
                Bytes docStream =
                    GetStream(stream, i, dir, sectorSize, sat, shortSectorSize, ssat, minStandardStreamSize);
                if (docStreamName.Length == 0 && docStream.Length == 0)
                    continue; //don't add streams for directory padding entries
                Streams.AddNamed(docStream, docStreamName, overwrite);
            }

		}

        private Bytes GetStream(System.IO.Stream fromDocumentStream, int did, Dictionary<int, byte[]> dir, ushort sectorSize, int[] sat, ushort shortSectorSize, int[] ssat, uint minStandardStreamSize)
        {
            Bytes stream = new Bytes();
            Bytes fromBytes;
            int[] fromSAT;
            ushort fromSectorSize;
        	int sidNext;
            string shortness;

            int streamLength = BitConverter.ToInt32(MidByteArray(dir[did], 120, 4), 0);

            Bytes streamBytes = null;

        	if (did == 0 || (streamLength >= minStandardStreamSize))
            {
            	byte[] streamByteArray;
            	streamByteArray = new byte[fromDocumentStream.Length];
                fromDocumentStream.Position = 0;
                fromDocumentStream.Read(streamByteArray, 0, streamByteArray.Length);
                streamBytes = new Bytes(streamByteArray);
            }

            if (did == 0)
            {
                fromSectorSize = sectorSize;
                fromSAT = sat;
                shortness = string.Empty;
                fromBytes = streamBytes;
            }
            else if (streamLength < minStandardStreamSize)
            {
                fromSectorSize = shortSectorSize;
                fromSAT = ssat;
                shortness = "Short ";
                fromBytes = GetStream(fromDocumentStream, 0, dir, sectorSize, sat, shortSectorSize, ssat, minStandardStreamSize);
            }
            else
            {
                fromSectorSize = sectorSize;
                fromSAT = sat;
                shortness = string.Empty;
                fromBytes = streamBytes;
            }

            sidNext = BitConverter.ToInt32(MidByteArray(dir[did], 116, 4), 0);
            while (sidNext > -2)
            {
            	Bytes sector;
            	if (did > 0 && streamLength < minStandardStreamSize)
                    sector = GetShortSectorBytes(fromBytes, fromSectorSize, sidNext);
                else
                    sector = GetSectorBytes(fromBytes, fromSectorSize, sidNext);

                if (sector.Length == 0)
                    throw new Exception(string.Format("{0}Sector not found [SID{1}]", shortness, sidNext));

                stream.Append(sector);

                sidNext = fromSAT[sidNext];
            }

            return stream.Get(streamLength);
        }

	    private static Bytes GetSectorBytes(Bytes fromStream, int sectorSize, int sid)
	    {
	        int i = (int) Math.Pow(2, sectorSize);
            
            if (fromStream.Length < (sid * i))
                throw new Exception(string.Format("Invalid SID [{0}] (EOF reached)", sid));

	        return fromStream.Get(512 + (sid * i), i);
	    }

	    private static Bytes GetShortSectorBytes(Bytes fromShortSectorStream, int shortSectorSize, int sid)
	    {
	        int i = (int) Math.Pow(2, shortSectorSize);

            if (fromShortSectorStream.Length < (sid * i))
                throw new Exception(string.Format("Invalid SID [{0}] (EOF reached)", sid));

//	        return fromShortSectorStream.Get((sid * i) + 1, i);
            return fromShortSectorStream.Get(sid * i, i);
        }

	    private static Bytes GetSector(System.IO.Stream stream, int sectorSize, int sidIndex)
        {
            int sectorLength = (int) Math.Pow(2, sectorSize);
            int offset = 512 + (sidIndex * sectorLength);
            if (stream.Length < (offset + sectorLength))
                return new Bytes();
            byte[] sector = new byte[sectorLength];
	        stream.Seek(offset, SeekOrigin.Begin);
//            stream.Position = offset;
	        ReadWholeArray(stream, sector);
//            int read = stream.Read(sector, 0, sectorLength);
            return new Bytes(sector);
        }

        /// <summary>
        /// Reads data into a complete array, throwing an EndOfStreamException
        /// if the stream runs out of data first, or if an IOException
        /// naturally occurs.
        /// http://www.yoda.arachsys.com/csharp/readbinary.html
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="data">The array to read bytes into. The array
        /// will be completely filled from the stream, so an appropriate
        /// size must be given.</param>
        public static void ReadWholeArray(System.IO.Stream stream, byte[] data)
        {
            int offset = 0;
            int remaining = data.Length;
            while (remaining > 0)
            {
                int read = stream.Read(data, offset, remaining);
                if (read <= 0)
                    throw new EndOfStreamException
                        (String.Format("End of stream reached with {0} bytes left to read", remaining));
                remaining -= read;
                offset += read;
            }
        }

        private static byte[] MidByteArray(byte[] byteArray, int offset, int length)
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

        /// <summary>
        /// Gets a blank array of bytes.  Included as documented filler field.
        /// </summary>
		public byte[] Blank1
		{
			get { return _blank1; }
		}

        /// <summary>
        /// Gets a blank array of bytes.  Included as documented filler field.
        /// </summary>
		public byte[] Blank2
		{
			get { return _blank2; }
		}
	}
}