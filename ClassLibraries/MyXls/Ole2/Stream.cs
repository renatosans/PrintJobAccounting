using System;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyOle2
{
    /// <summary>
    /// Represents a Stream (either standard or short) within an OLE2 Document.  This is the basic
    /// unit of storage within the OLE2 Compound Document format.
    /// </summary>
	public class Stream
	{
		private readonly Ole2Document _doc;
		private byte[] _name = new byte[0];
		private Bytes _bytes = new Bytes();

        /// <summary>
        /// Initializes a new instance of the Stream class for the provided Doc object.
        /// </summary>
        /// <param name="doc">The parent Doc object for this new Stream object.</param>
		public Stream(Ole2Document doc)
		{
			_doc = doc;
		}

        /// <summary>
        /// Gets whether this Stream is a short stream (is less than Doc.StandardStreamMinBytes
        /// in length) or a standard stream.
        /// </summary>
		public bool IsShort
		{
			get
			{
				if (Name == Directory.RootName)
					return false;
				else
				{
					if (_bytes.Length < _doc.StandardStreamMinBytes)
						return true;
					else
						return false;
				}
			}
		}

        /// <summary>
        /// Gets a Bytes object containing all the bytes of this Stream (not padded to standard or
        /// sector length).
        /// </summary>
		public Bytes Bytes
		{
			get { return _bytes; }
			set { _bytes = value; }
		}

        /// <summary>
        /// Gets or sets the byte[] representing the Name of this Stream.
        /// </summary>
		public byte[] Name
		{
			get { return _name; }
			set { if (value == null) throw new ArgumentNullException(); _name = value; }
		}

        /// <summary>
        /// Gets a count of the bytes contained in this Stream.
        /// </summary>
        public int ByteCount
		{
			get
			{
			    int count;

			    count = 0;
				if (Bytes.AreEqual(Name, Directory.RootName))
				{
					int bytesPerShortSector = _doc.BytesPerShortSector;
					int streamCount = _doc.Streams.Count;
					for (int i = 1; i <= streamCount; i++)
						count += (_doc.Streams[i].ShortSectorCount * bytesPerShortSector);
				}
				else
					count = _bytes.Length;

				return count;
			}
		}

        /// <summary>
        /// Gets a count of the sectors (standard only) required by this stream.
        /// </summary>
		public int SectorCount
		{
			get
			{
				if (!IsShort)
					return GetSectorCount();
				else
					return 0;
			}
		}

        /// <summary>
        /// Gets a count of the sectors (short only) required by this stream.
        /// </summary>
		public int ShortSectorCount
		{
			get
			{
				if (IsShort)
					return GetSectorCount();
				else
					return 0;
			}
		}

		private int GetSectorCount()
		{
			int bytesPerSector;
			decimal decimalSectors;

			if (IsShort)
				bytesPerSector = _doc.BytesPerShortSector;
			else
				bytesPerSector = _doc.BytesPerSector;

			decimalSectors = ((decimal)ByteCount)/bytesPerSector;
			return (int)Math.Ceiling(decimalSectors);
		}

        /// <summary>
        /// Gets the SID0, or SID of the first sector, of this Stream.
        /// </summary>
        public int SID0
		{
			get
			{
				int sid0;
				Stream stream;

				if (Bytes.AreEqual(Name, Directory.RootName))
					return _doc.SSAT.SID0 + _doc.SSAT.SectorCount;

				int j = _doc.Streams.GetIndex(Name) - 1;
				if (IsShort)
				{
					sid0 = 0;
					for (int i = 1; i <= j; i++)
					{
						stream = _doc.Streams[i];
						if (stream.IsShort)
							sid0 += stream.ShortSectorCount;
					}
				}
				else
				{
					sid0 = _doc.SSAT.SID0;
					if (sid0 == -2)
						sid0 = _doc.SAT.SID0 + _doc.SAT.SectorCount;
					else
						sid0 += _doc.SSAT.SectorCount;
					sid0 += _doc.Streams.ShortSectorStorage.SectorCount;
					for (int i = 1; i <= j; i++)
					{
						stream = _doc.Streams[i];
						if (!stream.IsShort)
							sid0 += stream.SectorCount;
					}
				}

				return sid0;
			}
		}
	}
}
