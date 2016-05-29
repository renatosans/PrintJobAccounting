using System.Collections.Generic;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// Represents and manages a collection of Style objects for a Workbook.
    /// </summary>
	public class Styles
	{
		private readonly XlsDocument _doc;

		private List<Style> _styles = null;

        /// <summary>
        /// Initializes a new instance of the Styles object for the give XlsDocument.
        /// </summary>
        /// <param name="doc">The parent XlsDocument object for the new Styles object.</param>
		public Styles(XlsDocument doc)
		{
			_doc = doc;
		}

        /// <summary>
        /// Gets a count of the Style objects currently in this collection.
        /// </summary>
		public int Count
		{
			get { return _styles.Count; }
		}

        /// <summary>
        /// Adds a Style object to this collection and returns the Style object's
        /// id value.
        /// </summary>
        /// <param name="style">The Style object to add to this collection.</param>
        /// <returns>The id value of the given Style object which has been added
        /// to this collection.</returns>
		public ushort Add(Style style)
		{
			ushort? id = GetID(style);

			if (id == null)
			{
				if (_styles == null)
					_styles = new List<Style>();

				id = (ushort)_styles.Count;
				_styles.Add((Style) style.Clone());
			}

			return (ushort)id;
		}

        /// <summary>
        /// Gets whether the given Style object exists in this collection and so will be
        /// written to the XlsDocument.
        /// </summary>
        /// <param name="style">The Style object which is to be checked whether it exists
        /// in this collection.</param>
        /// <returns>true if the given Style object exists in this collection, false otherwise.</returns>
		public bool IsWritten(Style style)
		{
			return (GetID(style) != null);
		}

        /// <summary>
        /// Returns the ID of a given Style object in this collection.
        /// </summary>
        /// <param name="style">The Style object whose ID is to be returned.</param>
        /// <returns>The ID of the given Style object in this collection.</returns>
		public ushort? GetID(Style style)
		{
			ushort? id = null;

			if (_styles == null)
				return id;

			for (ushort i = 0; i < _styles.Count; i++)
			{
				Style styleItem = _styles[i];
				if (styleItem.Equals(style))
				{
					id = i;
					break;
				}
			}

			return id;
		}

        /// <summary>
        /// Gets the Style object from this collection at the specified index.
        /// </summary>
        /// <param name="index">The index in this collection from which to get a Style object.</param>
        /// <returns>The Style object from this collection at the specified index.</returns>
		public Style this[int index]
		{
			get
			{
				return (Style) _styles[index].Clone();
			}
		}

	    internal Bytes Bytes
		{
			get
			{
				Bytes bytes = new Bytes();

				foreach (Style style in _styles)
				{
					bytes.Append(style.Bytes);
				}

				return bytes;
			}
		}
	}
}
