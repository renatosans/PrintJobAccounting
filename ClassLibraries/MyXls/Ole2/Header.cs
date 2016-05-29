using System;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyOle2
{
    /// <summary>
    /// Contains properties and information about the Header of an OLE2 document.
    /// </summary>
	public class Header
	{
		private static readonly byte[] LITTLE_ENDIAN = new byte[2] {0xFE, 0xFF};
		private static readonly byte[] BIG_ENDIAN = new byte[2] {0xFF, 0xFE};

		private readonly Ole2Document _doc;

        /// <summary>
        /// Initializes a new instance of the Header class for the given Document object.
        /// </summary>
        /// <param name="doc">The parent OleDocument object for this Header object.</param>
		public Header(Ole2Document doc)
		{
			_doc = doc;

			SetDefaults();
		}

		private void SetDefaults()
		{
			//empty in original
		}

        internal Bytes Bytes
		{
			get
			{
				Bytes bytes = new Bytes();

				bytes.Append(_doc.DocFileID);
				bytes.Append(_doc.DocUID);
				bytes.Append(_doc.FileFormatRevision);
				bytes.Append(_doc.FileFormatVersion);
				bytes.Append(_doc.IsLittleEndian ? LITTLE_ENDIAN : BIG_ENDIAN);
				bytes.Append(BitConverter.GetBytes(_doc.SectorSize));
				bytes.Append(BitConverter.GetBytes(_doc.ShortSectorSize));
				bytes.Append(_doc.Blank1);
				bytes.Append(BitConverter.GetBytes(_doc.SAT.SectorCount));
				bytes.Append(BitConverter.GetBytes(_doc.Directory.SID0));
				bytes.Append(_doc.Blank2);
				bytes.Append(BitConverter.GetBytes(_doc.StandardStreamMinBytes));
				bytes.Append(BitConverter.GetBytes(_doc.SSAT.SID0));
				bytes.Append(BitConverter.GetBytes(_doc.SSAT.SectorCount));
				bytes.Append(BitConverter.GetBytes(_doc.MSAT.SID0));
				bytes.Append(BitConverter.GetBytes(_doc.MSAT.SectorCount));
				bytes.Append(_doc.MSAT.Head);

				return bytes;
			}
		}
	}
}
