using System;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// Represents and manages a collection of Cells for a Worksheet.
    /// </summary>
	public class Cells
	{
//		private Doc _doc;
		private readonly Worksheet _worksheet;

		private ushort _cellCount = 0;
		private ushort _minRow = 0;
		private ushort _maxRow = 0;
		private ushort _minCol = 0;
		private ushort _maxCol = 0;

        /// <summary>
        /// Initializes a new instance of the Cells collection class for the given Worksheet.
        /// </summary>
        /// <param name="worksheet">The parent Worksheet object for this new Cells collection.</param>
		public Cells(Worksheet worksheet)
		{
//			_doc = doc;
			_worksheet = worksheet;
		}

		internal Cell Add(ushort cellRow, ushort cellColumn)
		{
			Cell cell = new Cell(_worksheet);
			bool haveCell = false;

			if (cellColumn < 1)
			    throw new ArgumentOutOfRangeException("cellColumn", string.Format("{0} must be >= 1", cellColumn));
			else if (cellColumn > BIFF8.MaxCols)
			    throw new ArgumentOutOfRangeException("cellColumn", string.Format("{0} cellColumn must be <= {1}", cellColumn, BIFF8.MaxCols));

            if (cellRow < 1)
                throw new ArgumentOutOfRangeException("cellRow", string.Format("{0} must be >= 1", cellColumn));
            else if (cellRow > BIFF8.MaxRows)
                throw new ArgumentOutOfRangeException("cellRow", string.Format("{0} cellRow must be <= {1}", cellRow, BIFF8.MaxRows));

			if (_worksheet.Rows.RowExists(cellRow))
			{
				if (_worksheet.Rows[cellRow].CellExists(cellColumn))
				{
					cell = _worksheet.Rows[cellRow].CellAtCol(cellColumn);
					haveCell = true;
				}
			}
			else
				_worksheet.Rows.AddRow(cellRow);

			if (haveCell)
				return cell;

			cell.Coordinate = new CellCoordinate(cellRow, cellColumn);

			if (_minRow == 0)
			{
				_minRow = cellRow;
				_minCol = cellColumn;
				_maxRow = cellRow;
				_maxCol = cellColumn;
			}
			else
			{
				if (cellRow < _minRow)
					_minRow = cellRow;
				else if (cellRow > _maxRow)
					_maxRow = cellRow;

				if (cellColumn < _minCol)
					_minCol = cellColumn;
				else if (cellColumn > _maxCol)
					_maxCol = cellColumn;
			}

			_worksheet.Rows[cellRow].AddCell(cell);
			_cellCount++;

			return cell;
		}

        /// <summary>
        /// Adds a new Cell to the Cells collection with the given Row, Column and
        /// Value.  If a Cell already exists with the given row and column, it is
        /// overwritten.
        /// </summary>
        /// <param name="cellRow">1-based Row index for the new Cell.</param>
        /// <param name="cellColumn">1-based Column index for the new Cell.</param>
        /// <param name="cellValue">Value for the new Cell.</param>
        /// <returns>The newly added Cell with the given Row, Column and Value.</returns>
        public Cell Add(int cellRow, int cellColumn, object cellValue)
        {
            Util.ValidateUShort(cellRow, "cellRow");
            Util.ValidateUShort(cellColumn, "cellColumn");
            return Add((ushort)cellRow, (ushort)cellColumn, cellValue);
        }

        /// <summary>
        /// OBSOLETE - Use Add(int, int, object) instead.  Adds a new Cell to the 
        /// Cells collection with the given Row, Column and Value.  If a Cell 
        /// already exists with the given row and column, it is overwritten.
        /// </summary>
        /// <param name="cellRow">1-based Row index for the new Cell.</param>
        /// <param name="cellColumn">1-based Column index for the new Cell.</param>
        /// <param name="cellValue">Value for the new Cell.</param>
        /// <returns>The newly added Cell with the given Row, Column and Value.</returns>
        [Obsolete]
        public Cell AddValueCell(int cellRow, int cellColumn, object cellValue)
        {
            return Add(cellRow, cellColumn, cellValue);
        }

        /// <summary>
        /// Adds a new Cell to the Cells collection with the given Row, Column and
        /// Value.  If a Cell already exists with the given row and column, it is
        /// overwritten.
        /// </summary>
        /// <param name="cellRow">1-based Row index for the new Cell.</param>
        /// <param name="cellColumn">1-based Column index for the new Cell.</param>
        /// <param name="cellValue">Value for the new Cell.</param>
        /// <returns>The newly added Cell with the given Row, Column and Value.</returns>
        public Cell Add(ushort cellRow, ushort cellColumn, object cellValue)
        {
            Cell cell = Add(cellRow, cellColumn);
            cell.Value = cellValue;
            return cell;
        }

        /// <summary>
        /// OBSOLETE - Use Add(ushort, ushort, object) instead.  Adds a new Cell to 
        /// the Cells collection with the given Row, Column and Value.  If a Cell 
        /// already exists with the given row and column, it is overwritten.
        /// </summary>
        /// <param name="cellRow">1-based Row index for the new Cell.</param>
        /// <param name="cellColumn">1-based Column index for the new Cell.</param>
        /// <param name="cellValue">Value for the new Cell.</param>
        /// <returns>The newly added Cell with the given Row, Column and Value.</returns>
        [Obsolete]
		public Cell AddValueCell(ushort cellRow, ushort cellColumn, object cellValue)
		{
            return Add(cellRow, cellColumn, cellValue);
		}

        /// <summary>
        /// Adds a new Cell to the Cells collection with the given Row, Column, Value
        /// and XF (style).  If a Cell already exists with the given row and column,
        /// it is overwritten.
        /// </summary>
        /// <param name="cellRow">1-based Row of new Cell.</param>
        /// <param name="cellColumn">1-based Column of new Cell.</param>
        /// <param name="cellValue">Value of new Cell.</param>
        /// <param name="xf">An Xf object describing the style of the cell.</param>
        /// <returns>The newly added Cell with the given Row, Column, Value and Style.</returns>
        public Cell Add(int cellRow, int cellColumn, object cellValue, XF xf)
        {
            Util.ValidateUShort(cellRow, "cellRow");
            Util.ValidateUShort(cellColumn, "cellColumn");
            return Add((ushort)cellRow, (ushort)cellColumn, cellValue, xf);
        }

        /// <summary>
        /// OBSOLETE - Use Add(int, int, object, XF) instead.  Adds a new Cell to the 
        /// Cells collection with the given Row, Column, Value and XF (style).  If a 
        /// Cell already exists with the given row and column, it is overwritten.
        /// </summary>
        /// <param name="cellRow">1-based Row of new Cell.</param>
        /// <param name="cellColumn">1-based Column of new Cell.</param>
        /// <param name="cellValue">Value of new Cell.</param>
        /// <param name="xf">An Xf object describing the style of the cell.</param>
        /// <returns>The newly added Cell with the given Row, Column, Value and Style.</returns>
        [Obsolete]
        public Cell AddValueCellXF(int cellRow, int cellColumn, object cellValue, XF xf)
        {
            return Add(cellRow, cellColumn, cellValue, xf);
        }

        /// <summary>
        /// Adds a new Cell to the Cells collection with the given Row, Column, Value
        /// and XF (style).  If a Cell already exists with the given row and column,
        /// it is overwritten.
        /// </summary>
        /// <param name="cellRow">1-based Row of new Cell.</param>
        /// <param name="cellColumn">1-based Column of new Cell.</param>
        /// <param name="cellValue">Value of new Cell.</param>
        /// <param name="xf">An Xf object describing the style of the cell.</param>
        /// <returns>The newly added Cell with the given Row, Column, Value and Style.</returns>
        public Cell Add(ushort cellRow, ushort cellColumn, object cellValue, XF xf)
        {
            Cell cell = Add(cellRow, cellColumn, cellValue);
            cell.ExtendedFormat = xf;
            return cell;
        }

        /// <summary>
        /// OBSOLETE - Use Add(ushort, ushort, object, XF) instead.  Adds a new Cell 
        /// to the Cells collection with the given Row, Column, Value and XF (style).  
        /// If a Cell already exists with the given row and column, it is overwritten.
        /// </summary>
        /// <param name="cellRow">1-based Row of new Cell.</param>
        /// <param name="cellColumn">1-based Column of new Cell.</param>
        /// <param name="cellValue">Value of new Cell.</param>
        /// <param name="xf">An Xf object describing the style of the cell.</param>
        /// <returns>The newly added Cell with the given Row, Column, Value and Style.</returns>
        [Obsolete]
		public Cell AddValueCellXF(ushort cellRow, ushort cellColumn, object cellValue, XF xf)
		{
            return Add(cellRow, cellColumn, cellValue, xf);
		}

        /// <summary>
        /// Merges cells within the defined range of Rows and Columns.  The ranges are
        /// verified not to overlap with any previously defined Merge areas.  NOTE 
        /// Values and formatting in all cells other than the first in the range 
        /// (scanning left to right, top to bottom) will be lost.
        /// </summary>
        /// <param name="rowMin">The first index in the range of Rows to merge.</param>
        /// <param name="rowMax">The last index in the range of Rows to merge.</param>
        /// <param name="colMin">The first index in the range of Columns to merge.</param>
        /// <param name="colMax">The last index in the range of Columns to merge.</param>
        public void Merge(int rowMin, int rowMax, int colMin, int colMax)
        {
            MergeArea mergeArea = new MergeArea(rowMin, rowMax, colMin, colMax);
            _worksheet.AddMergeArea(mergeArea);
        }

        /// <summary>
        /// Gets the count of cells in this collection (only counts cells with assigned
        /// values, styles or properties - not blank/unused cells).
        /// </summary>
		public ushort Count
		{
			get { return _cellCount; }
		}
	}
}
