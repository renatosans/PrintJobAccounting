using System;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyOle2
{
    /// <summary>
    /// Represents the SAT (Sector Allocation Table) for an OLE2 Document.
    /// </summary>
	public class Sat
	{
		private readonly Ole2Document _doc;

        /// <summary>
        /// Initializes a new instance of the Sat class for the provided Doc object.
        /// </summary>
        /// <param name="doc">The parent Doc object for this new Sat object.</param>
		public Sat(Ole2Document doc)
		{
			_doc = doc;
		}

        /// <summary>
        /// Gets a count of the sectors required by the body of this Sat.
        /// </summary>
		public int SectorCount
		{
			get
			{
				int sectorCount;
				
				int count = 
			            _doc.SSAT.SectorCount
			            + _doc.Streams.ShortSectorStorage.SectorCount
			            + _doc.Streams.SectorCount
			            + _doc.Directory.SectorCount;

			    int sidsPerSector = _doc.BytesPerSector / 4;
			    sectorCount = (int)Math.Ceiling((count + Math.Ceiling((double)count / sidsPerSector)) / sidsPerSector);

				return sectorCount;
			}
		}

        /// <summary>
        /// Gets the SID0, or SID of the first sector, of this Sat.
        /// </summary>
        public int SID0
		{
			get
			{
				if (_doc.MSAT.SID0 == -2)
					return 0;
				else
					return _doc.MSAT.SectorCount;
			}
		}

        internal Bytes Bytes
		{
			get
			{
				Bytes bytes = new Bytes();

				int i, j, k, m, sid, sectorCount;

			    sid = 0;

				sectorCount = _doc.MSAT.SectorCount;
				j = (sid + (sectorCount - 1));
				for (i = sid; i <= j; i++)
					bytes.Append(BitConverter.GetBytes((int) -4));
				sid = i;

				sectorCount = _doc.SAT.SectorCount;
				j = (sid + (sectorCount - 1));
				for (i = sid; i <= j; i++)
					bytes.Append(BitConverter.GetBytes((int) -3));
				sid = i;

				sectorCount = _doc.SSAT.SectorCount;
				j = (sid + (sectorCount - 1));
				for (i = sid; i <= j; i++)
				{
					if (i < j)
						bytes.Append(BitConverter.GetBytes((int) (i + 1)));
					else
						bytes.Append(BitConverter.GetBytes((int) -2));
				}
				sid = i;

				sectorCount = _doc.Streams.ShortSectorStorage.SectorCount;
				j = (sid + (sectorCount - 1));
				for (i = sid; i <= j; i++)
				{
					if (i < j)
						bytes.Append(BitConverter.GetBytes((int) (i + 1)));
					else
						bytes.Append(BitConverter.GetBytes((int) -2));
				}
				sid = i;

				m = _doc.Streams.Count;
				for (k = 1; k <= m; k++)
				{
					sectorCount = _doc.Streams[k].SectorCount;
					j = (sid + (sectorCount - 1));
					for (i = sid; i <= j; i++)
					{
						if (i < j)
							bytes.Append(BitConverter.GetBytes((int) (i + 1)));
						else
							bytes.Append(BitConverter.GetBytes((int) -2));
					}
					sid = i;
				}

				sectorCount = _doc.Directory.SectorCount;
				j = (sid + (sectorCount - 1));
				for (i = sid; i <= j; i++)
				{
					if (i < j)
						bytes.Append(BitConverter.GetBytes((int) (i + 1)));
					else
						bytes.Append(BitConverter.GetBytes((int) -2));
				}
				sid = i;

			    int remainingSlots = (int) (((decimal) sid) % ((decimal) _doc.BytesPerSector / 4));
//                if (remainingSlots != 0)
//                {
                    j = (_doc.BytesPerSector / 4) - remainingSlots;
                    for (i = 1; i <= j; i++)
                        bytes.Append(BitConverter.GetBytes((int)-1));
//                }

				return bytes;
			}
		}
	}
}
