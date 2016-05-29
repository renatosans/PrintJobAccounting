using System;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyOle2
{
    /// <summary>
    /// Represents the SSAT (Short Sector Allocation Table) of an OLE2 Document.
    /// </summary>
	public class Ssat
	{
		private readonly Ole2Document _doc;

        /// <summary>
        /// Initializes a new instance of the Ssat class for the provided Doc object.
        /// </summary>
        /// <param name="doc">The parent Doc object for this new Ssat object.</param>
		public Ssat(Ole2Document doc)
		{
			_doc = doc;
		}

        /// <summary>
        /// Gets a count of the sectors required for this Ssat in the Doc.
        /// </summary>
		public int SectorCount
		{
			get
			{
				int sectorCount;
				int i, j, count;

				count = 0;
				j = _doc.Streams.Count;
				for (i = 1; i <= j; i++)
					count += _doc.Streams[i].ShortSectorCount;

				sectorCount = (int) Math.Ceiling(count/(((decimal) _doc.BytesPerSector)/4));

				return sectorCount;
			}
		}

        /// <summary>
        /// Gets the SID0, or SID of the first sector, of this Ssat.
        /// </summary>
        public int SID0
		{
			get
			{
				if (SectorCount > 0)
					return _doc.SAT.SID0 + _doc.SAT.SectorCount;
				else
					return -2;
			}
		}

        internal Bytes Bytes
		{
			get
			{
				Bytes bytes = new Bytes();
			    int sidCount;
			    Stream stream;

				sidCount = 0;
				int streamCount = _doc.Streams.Count;
				for (int i = 1; i <= streamCount; i++)
				{
					stream = _doc.Streams[i];
					int shortSectorCount = stream.ShortSectorCount;
					sidCount += shortSectorCount;
					int sid0 = stream.SID0;
				    for (int k = 1; k <= shortSectorCount; k++)
					{
						if (k < shortSectorCount)
							bytes.Append(BitConverter.GetBytes((int) (sid0 + k)));
						else
							bytes.Append(BitConverter.GetBytes((int) -2));
					}
				}

				if (sidCount > 0)
				{
					//j = (int) Math.Floor((decimal) _doc.BytesPerSector/4);
					streamCount = (_doc.BytesPerSector/4) - (sidCount%(_doc.BytesPerSector/4));
					for (int i = 1; i <= streamCount; i++)
						bytes.Append(BitConverter.GetBytes(-1));
				}

				return bytes;
			}
		}
	}
}
