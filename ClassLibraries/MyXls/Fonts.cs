using System.Collections.Generic;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// A collection of Font objects.
    /// </summary>
	public class Fonts
	{
        //	----------------------------------------------------------------------------
        //	The font with index 4 is omitted in all BIFF versions. This means the first 
        //	four fonts have zero-based indexes, and the fifth font and all following 
        //	fonts are referenced with one-based indexes.
        //	                                    - excelfileformat.pdf, section 6.43 FONT
        //	----------------------------------------------------------------------------
        private readonly XlsDocument _doc;

		private readonly List<Font> _fonts;

        /// <summary>
        /// Initializes a new instance of the Fonts collection for the given XlsDocument.
        /// </summary>
        /// <param name="doc">The parent XlsDocument object for the new Fonts collection.</param>
		public Fonts(XlsDocument doc)
		{
			_doc = doc;

			_fonts = new List<Font>();

            AddDefaultFonts();
		}

        private void AddDefaultFonts()
        {
            Font font = new Font(_doc, (XF)null);
            _fonts.Add(font);
            _fonts.Add((Font) font.Clone());
            _fonts.Add((Font) font.Clone());
            _fonts.Add((Font) font.Clone());
            _fonts.Add((Font)font.Clone()); //we won't write this one out - just leave it here to fill the 4-index that's never written
        }

        /// <summary>
        /// Adds a new Font object to this collection.
        /// </summary>
        /// <param name="font">The Font object to add to this collection.</param>
        /// <returns>The id of the Font within this collection.</returns>
		public ushort Add(Font font)
		{
			ushort? fontId = GetId(font);

			if (fontId == null)
			{
				fontId = (ushort)_fonts.Count;
				_fonts.Add((Font)font.Clone());
			}

			return (ushort) fontId;
		}

        /// <summary>
        /// Gets the id of the specified Font in this collection.
        /// </summary>
        /// <param name="font">The Font for which to return an id.</param>
        /// <returns>the ushort id of the Font if it exists in this collection, 
        /// null otherwise</returns>
		public ushort? GetId(Font font)
		{
			for (ushort i = 0; i < (ushort)_fonts.Count; i++)
			{
				if (_fonts[i].Equals(font))
					return i;
			}

			return null;
		}

        internal Font this[ushort idx]
		{
            get { return (Font) _fonts[idx].Clone(); }
		}

	    internal Bytes Bytes
		{
			get
			{
				Bytes bytes = new Bytes();

			    int i = -1;
				foreach (Font font in _fonts)
				{
                    i++;
                    if (i == 4)
                        continue;
				    bytes.Append(font.Bytes);
				}

				return bytes;
			}
		}
	}
}
