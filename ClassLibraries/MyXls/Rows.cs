using System;
using System.Collections.Generic;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// Represents and manages a collection of Row objects for a Worksheet.
    /// </summary>
	public class Rows : IEnumerable<Row>
	{
		private readonly SortedList<ushort, Row> _rows;

		private uint _minRow;
		private uint _maxRow;

        /// <summary>
        /// Initializes a new instance of the Rows object.
        /// </summary>
		public Rows()
		{
			_minRow = 0;
			_maxRow = 0;
			_rows = new SortedList<ushort, Row>();
		}

        /// <summary>
        /// Returns whether a Row exists at the specified index in this Collection.
        /// </summary>
        /// <param name="rowIdx">1-based index of Row to return from this collection.</param>
        /// <returns>The Row with the specified index from this collection.</returns>
		public bool RowExists(ushort rowIdx)
		{
			return _rows.ContainsKey(rowIdx);
		}

        /// <summary>
        /// Adds a Row at the specified row number.
        /// </summary>
        /// <param name="rowNum">1-based index of Row to add.</param>
        /// <returns>The Row added at the specified row number.</returns>
        public Row AddRow(ushort rowNum)
		{
			if (RowExists(rowNum))
				return _rows[rowNum];

			if (_minRow == 0)
			{
				_minRow = rowNum;
				_maxRow = rowNum;
			}
			else
			{
				if (rowNum < _minRow)
					_minRow = rowNum;
				if (rowNum > _maxRow)
					_maxRow = rowNum;
			}

			Row row = new Row();
		    row.RowIndex = rowNum;

			_rows.Add(rowNum, row);

			return row;
		}

        /// <summary>
        /// Gets the count of Rows in this collection.
        /// </summary>
		public int Count
		{
			get
			{
				return _rows.Count;
			}
		}

        /// <summary>
        /// Gets the Row from this collection with the specified row number.
        /// </summary>
        /// <param name="rowNumber">1-based row number to get.</param>
        /// <returns>Row at specified row number</returns>
		public Row this[ushort rowNumber]
		{
			get
			{
				if (!_rows.ContainsKey(rowNumber))
					throw new Exception(string.Format("Row {0} not found", rowNumber));

				return _rows[rowNumber];
			}
		}

        /// <summary>
        /// Gets the smallest row number populated in this collection.  A Row is populated if it has any Cells,
        /// formatting or has been explicitly added.
        /// </summary>
		public uint MinRow
		{
			get { return _minRow; }
		}

        /// <summary>
        /// Gets the largest row number populated in this collection.  A Row is populated if it has any Cells,
        /// formatting or has been explicitly added.
        /// </summary>
		public uint MaxRow
		{
			get { return _maxRow; }
		}

        /// <summary>
        /// Returns an enumerator that iterates through the Rows collection
        /// </summary>
        public IEnumerator<Row> GetEnumerator()
        {
            uint initialMinRow = MinRow;
            uint initialMaxRow = MaxRow;

            for (uint i = initialMinRow; i < initialMaxRow; i++)
            {
                if (initialMinRow != MinRow || initialMaxRow != MaxRow)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");
                else yield return this[(ushort)i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
	}
}
