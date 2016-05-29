using System;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyOle2
{
    /// <summary>
    /// Represents the Directory stream of an OLE2 Document.
    /// </summary>
	public class Directory
	{
        /// <summary>
        /// The name of the Root entry of an OLE2 Directory.
        /// </summary>
		public static readonly byte[] RootName = new byte[] { 0x52, 0x00, 0x6f, 0x00, 0x6f, 0x00, 0x74, 0x00, 0x20, 0x00, 0x45, 0x00, 0x6e, 0x00, 0x74, 0x00, 0x72, 0x00, 0x79, 0x00, 0x00, 0x00 };

        /// <summary>
        /// The name of a BIFF8 Workbook Stream.
        /// </summary>
        public static readonly byte[] Biff8Workbook = new byte[] {0x57, 0x00, 0x6F, 0x00, 0x72, 0x00, 0x6B, 0x00, 0x62, 0x00, 0x6F, 0x00, 0x6F, 0x00, 0x6B, 0x00, 0x00, 0x00};

        private static readonly byte[] StreamNameSumaryInformation = new byte[] {
			0x05, 0x00, 0x53, 0x00, 0x75, 0x00, 0x6D, 0x00, 0x6D, 0x00, 0x61, 0x00, 0x72, 0x00, 0x79, 0x00,
			0x49, 0x00, 0x6E, 0x00, 0x66, 0x00, 0x6F, 0x00, 0x72, 0x00, 0x6D, 0x00, 0x61, 0x00, 0x74, 0x00,
			0x69, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x00, 0x00};

        private static readonly byte[] StreamNameDocumentSummaryInformation = new byte[] {
			0x05, 0x00, 0x44, 0x00, 0x6F, 0x00, 0x63, 0x00, 0x75, 0x00, 0x6D, 0x00, 0x65, 0x00, 0x6E, 0x00,
			0x74, 0x00, 0x53, 0x00, 0x75, 0x00, 0x6D, 0x00, 0x6D, 0x00, 0x61, 0x00, 0x72, 0x00, 0x79, 0x00,
			0x49, 0x00, 0x6E, 0x00, 0x66, 0x00, 0x6F, 0x00, 0x72, 0x00, 0x6D, 0x00, 0x61, 0x00, 0x74, 0x00,
			0x69, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x00, 0x00};

		private readonly Ole2Document _doc;

        /// <summary>
        /// Initializes a new instance of the Directory class for the provided Doc object.
        /// </summary>
        /// <param name="doc">The Doc object to which this new Directory is to belong.</param>
		public Directory(Ole2Document doc)
		{
			_doc = doc;
		}

		//TODO: I think this should be Streams.Count + Storages.Count + 1 (for Root Entry)
		//But this is okay until User Storage support is added (with Red-Black Tree Implementation)
        /// <summary>
        /// Gets the number of entries in this Directory object (not including blank/filler entries).
        /// </summary>
		public int EntryCount
		{
			get { return _doc.Streams.Count + 1; }
		}

        /// <summary>
        /// Gets the number of Sectors required by this Directory.
        /// </summary>
		public int SectorCount
		{
			get
			{
				return (int) Math.Ceiling((decimal)EntryCount / EntriesPerSector);
			}
		}

		private int EntriesPerSector
		{
			get { return _doc.BytesPerSector / 128; }
		}

        /// <summary>
        /// Gets the first SID of this Directory's stream.
        /// </summary>
		public int SID0
		{
			get
			{
				int sid0;
				if (_doc.SSAT.SID0 != -2)
					sid0 = _doc.SSAT.SID0 + _doc.SSAT.SectorCount;
				else
					sid0 = _doc.SAT.SID0 + _doc.SAT.SectorCount;
			    sid0 += _doc.Streams.SectorCount;
				return sid0;
			}
		}

        internal Bytes Bytes
		{
			get
			{
				int streamCount = _doc.Streams.Count;

				Bytes bytes = new Bytes();

				bytes.Append(StreamDirectoryBytes(_doc.Streams.ShortSectorStorage));
				for (int i = 1; i <= streamCount; i++)
					bytes.Append(StreamDirectoryBytes(_doc.Streams[i]));

				int padSectors = (streamCount + 1) % EntriesPerSector;
				if (padSectors > 0)
					padSectors = EntriesPerSector - padSectors;
				for (int i = 1; i <= padSectors; i++)
					bytes.Append(BlankEntryByteArray);

				return bytes;
			}
		}

		private static readonly byte[] BlankEntryByteArray = new byte[]
			{
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0xFE, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
			};

		private Bytes StreamDirectoryBytes(Stream stream)
		{
			Bytes bytes = new Bytes();
			
			//Stream Name
			bytes.Append(stream.Name);

			//Stream Name buffer fill
			bytes.Append(new byte[64 - bytes.Length]);

			//Stream Name length (including ending 0x00)
			bytes.Append(BitConverter.GetBytes((ushort)stream.Name.Length));

			//Type of entry {&H00 -> Empty, &H01 -> User Storage,
			//		&H02 -> User Stream, &H03 -> LockBytes (unknown),
			//		&H04 -> Property (unknown), &H05 -> Root storage}
			//TODO: UnHack this
			bytes.Append(HackDirectoryType(stream.Name));

			//TODO: Implement Red-Black Tree Node color {&H00 -> Red, &H01 -> Black} (Doesn't matter)
			bytes.Append(0x01);

			//TODO: UnHack Red-Black Tree Left-Child Node DID (-1 if no left child)
			bytes.Append(BitConverter.GetBytes(HackDirectoryDID(stream.Name, "LeftDID")));

			//TODO: UnHack Red-Black Tree Right-Child Node DID (-1 if no right child)
			bytes.Append(BitConverter.GetBytes(HackDirectoryDID(stream.Name, "RightDID")));

			//TODO: UnHack Storage Member Red-Black Tree Root Node DID (-1 if not storage)
			bytes.Append(BitConverter.GetBytes(HackDirectoryDID(stream.Name, "RootDID")));

			//Unique identifier for storage (Doesn't matter)
			bytes.Append(new byte[16]);

			//User flags (Doesn't matter)
			bytes.Append(new byte[4]);

			//Entry Creation Timestamp (Can be all 0's)
			bytes.Append(new byte[8]);

			//Entry Modification Timestamp (Can be all 0's)
			bytes.Append(new byte[8]);

			//SID of Stream's First Sector (for Short or Standard Streams)
			bytes.Append(BitConverter.GetBytes(stream.SID0));

			//Stream Size in Bytes (0 if storage, but not Root Storage entry)
			bytes.Append(BitConverter.GetBytes(stream.ByteCount));

			//Not used
			bytes.Append(new byte[4]);

			return bytes;
		}

		private static Bytes HackDirectoryType(byte[] streamName)
		{
			if (Bytes.AreEqual(streamName, RootName))
			{
				return new Bytes(new byte[]{0x05});
			}
			else if (Bytes.AreEqual(streamName, Biff8Workbook))
			{
				return new Bytes(new byte[] { 0x02 });
			}
			else if (Bytes.AreEqual(streamName, StreamNameSumaryInformation))
			{
				return new Bytes(new byte[] { 0x02 });
			}
			else if (Bytes.AreEqual(streamName, StreamNameDocumentSummaryInformation))
			{
				return new Bytes(new byte[] { 0x02 });
			}
			else
			{
				return new Bytes(new byte[] { 0xFF });
			}
		}

		private static int HackDirectoryDID(byte[] streamName, string didType)
		{
			if (Bytes.AreEqual(streamName, RootName))
			{
				switch(didType)
				{
					case "LeftDID":
						return -1;
					case "RightDID":
						return -1;
					case "RootDID":
						return 2;
				}
			}
			else if (Bytes.AreEqual(streamName, Biff8Workbook))
			{
				switch (didType)
				{
					case "LeftDID":
						return -1;
					case "RightDID":
						return -1;
					case "RootDID":
						return -1;
				}
			}
			else if (Bytes.AreEqual(streamName, StreamNameSumaryInformation))
			{
				switch (didType)
				{
					case "LeftDID":
						return 1;
					case "RightDID":
						return 3;
					case "RootDID":
						return -1;
				}
			}
			else if (Bytes.AreEqual(streamName, new byte[] { //BIFF8 '&05HDocumentSummaryInformation'
			    0x05, 0x00, 0x44, 0x00, 0x6F, 0x00, 0x63, 0x00, 0x75, 0x00, 0x6D, 0x00, 0x65, 0x00, 0x6E, 0x00,
				0x74, 0x00, 0x53, 0x00, 0x75, 0x00, 0x6D, 0x00, 0x6D, 0x00, 0x61, 0x00, 0x72, 0x00, 0x79, 0x00,
				0x49, 0x00, 0x6E, 0x00, 0x66, 0x00, 0x6F, 0x00, 0x72, 0x00, 0x6D, 0x00, 0x61, 0x00, 0x74, 0x00,
				0x69, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x00, 0x00}))
			{
				switch (didType)
				{
					case "LeftDID":
						return -1;
					case "RightDID":
						return -1;
					case "RootDID":
						return -1;
				}
			}
			else
			{
				switch (didType)
				{
					case "LeftDID":
						return 1000000; //Huh?  I think these are dummy values
					case "RightDID":
						return 1000000;
					case "RootDID":
						return 1000000;
				}
			}

			throw new Exception(string.Format("Unexpected didType {0} for HackDirectoryDID", didType));
		}
	}
}
