using System;
using System.Collections.Generic;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// Represents a single row in a Worksheet.
    /// </summary>
	public class Row
	{
		private readonly SortedList<ushort, Cell> _cells = new SortedList<ushort, Cell>();

		private ushort _rowIndex;
//		private ushort _cellCount;
		private ushort _minCellCol;
		private ushort _maxCellCol;

        /// <summary>
        /// Initializes a new instance of the Row class.
        /// </summary>
	    public Row()
		{
			_minCellCol = 0;
			_maxCellCol = 0;
		}

        /// <summary>
        /// Gets the row index of this Row object.
        /// </summary>
		public ushort RowIndex
		{
			get { return _rowIndex; }
			internal set { _rowIndex = value; }
		}

        /// <summary>
        /// Returns whether a Cell exists on this Row at the specified Column.  A
        /// Cell will exist if a value or property has been specified for it.
        /// </summary>
        /// <param name="atCol">Column at which to check for a Cell in this Row.</param>
        /// <returns>true if a Cell exists on this Row at the specified Column, false otherwise</returns>
		public bool CellExists(ushort atCol)
		{
			return _cells.ContainsKey(atCol);
		}

        /// <summary>
        /// Adds a Cell to this Row.
        /// </summary>
        /// <param name="cell">The Cell to add to this Row</param>
		public void AddCell(Cell cell)
		{
			ushort cCol = cell.Column;

			if (CellExists(cCol))
				throw new Exception(string.Format("Cell already exists at column {0}", cCol));
			if (cCol < 1 || cCol > 256)
				throw new ArgumentOutOfRangeException(string.Format("cell.Col {0} must be between 1 and 256", cCol));

			if (_minCellCol == 0)
			{
				_minCellCol = cCol;
				_maxCellCol = cCol;
			}
			else
			{
				if (cCol < _minCellCol)
					_minCellCol = cCol;
				else if (cCol > _maxCellCol)
					_maxCellCol = cCol;
			}

			_cells.Add(cCol, cell);
		}

        /// <summary>
        /// Gets the count of Cells that exists on this Row.
        /// </summary>
		public ushort CellCount
		{
			get { return (ushort)_cells.Count; }
		}

        /// <summary>
        /// Returns the Cell at the specified column on this Row.
        /// </summary>
        /// <param name="col">The column from which to return the Cell.</param>
        /// <returns>The Cell from the specified column on this Row.</returns>
		public Cell CellAtCol(ushort col)
		{
			if (!CellExists(col))
				throw new Exception(string.Format("Cell at col {0} does not exist", col));

			return _cells[col];
		}

        /// <summary>
        /// Returns the Cell with the specified index from the existing Cells on this Row.
        /// (i.e. if there are three cells at columns 1, 3, and 5, specifying 2 will return
        /// the Cell from column 3).
        /// </summary>
        /// <param name="cellIdx">1-based index of Cell to return from existing Cells on this Row.</param>
        /// <returns>The Cell from this Row with the specified index among the existing Cells.</returns>
	    public Cell GetCell(ushort cellIdx)
		{
			if (cellIdx < 1 || cellIdx > 256)
				throw new ArgumentOutOfRangeException(string.Format("cellIdx {0} must be between 1 and 256", cellIdx));

			if (cellIdx > _cells.Count)
				throw new ArgumentOutOfRangeException(
					string.Format("cellIdx {0} is greater than the cell count {1}", cellIdx, _cells.Count));

			ushort idx = 1;
			foreach (ushort col in _cells.Keys)
			{
				if (idx == cellIdx)
					return _cells[col];

				idx++;
			}

			throw new Exception(string.Format("Cell number {0} not found in row", cellIdx));
		}

        /// <summary>
        /// Gets the first column at which a Cell exists on this Row.
        /// </summary>
		public ushort MinCellCol
		{
			get { return _minCellCol; }
		}

        /// <summary>
        /// Gets the last column at which a Cell exists on this Row.
        /// </summary>
		public ushort MaxCellCol
		{
			get { return _maxCellCol; }
		}
	}
}
