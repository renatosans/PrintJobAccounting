using System;
using System.Collections.Generic;
using System.Text;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// Defines a contiguous group of Merged Cells on a Worksheet.
    /// </summary>
    public struct MergeArea
    {
        /// <summary>
        /// The first Row in this group of Merged Cells (1-based).
        /// </summary>
        public ushort RowMin;

        /// <summary>
        /// The last Row in this group of Merged Cells (1-based).
        /// </summary>
        public ushort RowMax;

        /// <summary>
        /// The first Column in this group of Merged Cells (1-based).
        /// </summary>
        public ushort ColMin;

        /// <summary>
        /// The last Column in this group of Merged Cells (1-based).
        /// </summary>
        public ushort ColMax;

        /// <summary>
        /// Initializes a new MergeArea with the provided values.
        /// </summary>
        /// <param name="rowMin">The first Row in this group of Merged Cells (1-based).</param>
        /// <param name="rowMax">The last Row in this group of Merged Cells (1-based).</param>
        /// <param name="colMin">The first Column in this group of Merged Cells (1-based).</param>
        /// <param name="colMax">The last Column in this group of Merged Cells (1-based).</param>
        public MergeArea(ushort rowMin, ushort rowMax, ushort colMin, ushort colMax)
        {
            RowMin = rowMin;
            RowMax = rowMax;
            ColMin = colMin;
            ColMax = colMax;
        }

        /// <summary>
        /// Initializes a new MergeArea with the provided values.
        /// </summary>
        /// <param name="rowMin">The first Row in this group of Merged Cells (1-based).</param>
        /// <param name="rowMax">The last Row in this group of Merged Cells (1-based).</param>
        /// <param name="colMin">The first Column in this group of Merged Cells (1-based).</param>
        /// <param name="colMax">The last Column in this group of Merged Cells (1-based).</param>
        public MergeArea(int rowMin, int rowMax, int colMin, int colMax)
            : this((ushort)rowMin, (ushort)rowMax, (ushort)colMin, (ushort)colMax)
        {
            if (rowMin < 1) throw new ArgumentOutOfRangeException("rowMin", "must be >= 1");
            if (rowMin > BIFF8.MaxRows) throw new ArgumentOutOfRangeException("rowMin", "must be <= " + BIFF8.MaxRows);
            if (rowMax < rowMin) throw new ArgumentOutOfRangeException("rowMax", "must be >= rowMin (" + rowMin + ")");
            if (rowMax > BIFF8.MaxRows) throw new ArgumentOutOfRangeException("rowMax", "must be <=" + BIFF8.MaxRows);
            if (colMin < 1) throw new ArgumentOutOfRangeException("colMin", "must be >= 1");
            if (colMin > BIFF8.MaxCols) throw new ArgumentOutOfRangeException("colMin", "must be <= " + BIFF8.MaxCols);
            if (colMax < colMin) throw new ArgumentOutOfRangeException("colMax", "must be >= colMin (" + colMin + ")");
            if (colMax > BIFF8.MaxCols) throw new ArgumentOutOfRangeException("colMax", "must be <= " + BIFF8.MaxCols);

            //I know these will only be checked after the chained constructor, but 
            Util.ValidateUShort(rowMin, "rowMin");
            Util.ValidateUShort(rowMax, "rowMax");
            Util.ValidateUShort(colMin, "colMin");
            Util.ValidateUShort(colMax, "colMax");
        }
    }
}
