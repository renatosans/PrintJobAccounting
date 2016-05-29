using System;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyOle2
{
    /// <summary>
    /// Represents the MSAT (Master Sector Allocation Table) for an OLE2 Document.
    /// </summary>
	public class Msat
	{
		private readonly Ole2Document _doc;

        /// <summary>
        /// Initializes a new instance of the Msat class for the given Doc object.
        /// </summary>
        /// <param name="doc">The parent Doc object for the new Msat object.</param>
		public Msat(Ole2Document doc)
		{
			_doc = doc;
		}

        /// <summary>
        /// Gets the count of MSAT sectors used by this MSAT.  This value is zero if
        /// the MSAT contains &lt;= 109 SIDs, which will be contained in the Header sector.
        /// </summary>
		public int SectorCount
		{
			get
			{
				int sectorCount = _doc.SAT.SectorCount;
				
				if (sectorCount <= 109)
					return 0;

				sectorCount -= 109;
				if ((decimal)sectorCount % 127 == 0)
					return (int)Math.Floor((decimal)sectorCount / 127);
				else
					return ((int) Math.Floor((decimal)sectorCount/127)) + 1;
			}
		}

        /// <summary>
        /// Gets the SID0, or SID of the first sector, of the MSAT.  If SectorCount
        /// is 0 (zero), the SID0 will return -2.  Currently, 0 is the only value 
        /// other than -2 returned, as the current MyOle2 implementation always
        /// places any MSAT sectors as the first sectors in the document.
        /// </summary>
		public int SID0
		{
			get
			{
				if (SectorCount == 0)
					return -2;
				else
					return 0; //TODO: See comment in Ole2Document.Bytes Property Get
			}
		}

        internal Bytes Head
		{
			get
			{
				return SectorBinData(0);
			}
		}

		private Bytes SectorBinData(int sectorIndex)
		{
			if (0 > sectorIndex || sectorIndex > SectorCount)
				throw new ArgumentOutOfRangeException(string.Format("sectorIndex must be >= 0 and <= SectorCount {0}", SectorCount));

			int satSectors, satSid0, startSector, stopSector;

			Bytes bytes = new Bytes();

			satSectors = _doc.SAT.SectorCount;
			satSid0 = _doc.SAT.SID0;

			if (sectorIndex == 0)
			{
				startSector = 1;
				stopSector = 109;
			}
			else
			{
				startSector = 110 + ((sectorIndex - 1)*127);
				stopSector = startSector + 126;
			}

			for (int i = startSector; i <= stopSector; i++)
			{
				if (i < (satSectors + 1))
					bytes.Append(BitConverter.GetBytes((int) (satSid0 + (i - 1))));
				else
					bytes.Append(BitConverter.GetBytes((int) -1));
			}

			if (sectorIndex > 0)
			{
				if (stopSector >= satSectors)
					bytes.Append(BitConverter.GetBytes((int) -2));
				else
					bytes.Append(BitConverter.GetBytes((int) (SID0 + sectorIndex)));
			}

			return bytes;
		}

        internal Bytes Bytes
		{
			get
			{
				Bytes bytes = new Bytes();
				int sectorCount = SectorCount;

				for (int i = 1; i <= sectorCount; i++)
					bytes.Append(SectorBinData(i));

				return bytes;
			}
		}
	}
}
