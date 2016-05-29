using System;
using System.Collections.Generic;
using System.Data;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// A main class for an XlsDocument, representing one Worksheet in the Workbook.  The 
    /// Worksheet holds the Cells in its Rows.
    /// </summary>
    public class Worksheet
    {
        private readonly XlsDocument _doc;

        private readonly List<ColumnInfo> _columnInfos = new List<ColumnInfo>();
        private readonly List<MergeArea> _mergeAreas = new List<MergeArea>();

        private readonly Cells _cells;
        private readonly Rows _rows;
        private readonly RowBlocks _rowBlocks;

        private WorksheetVisibilities _visibility;
        private WorksheetTypes _sheettype;
        private string _name;
        private int _streamByteLength;
        private int[] _dbCellOffsets;
        private bool _protected = false;

        private CachedBlockRow _cachedBlockRow;

        internal Worksheet(XlsDocument doc)
        {
            _doc = doc;

            _visibility = WorksheetVisibilities.Default;
            _sheettype = WorksheetTypes.Default;
            _streamByteLength = 0;

            _dbCellOffsets = new int[0];

            _cells = new Cells(this);
            _rows = new Rows();
            _rowBlocks = new RowBlocks(this);

            _cachedBlockRow = CachedBlockRow.Empty;

            _columnInfos = new List<ColumnInfo>();
        }

        internal Worksheet(XlsDocument doc, Record boundSheet, List<Record> sheetRecords) : this(doc)
        {
            byte[] byteArray = boundSheet.Data.ByteArray;

            byte visibility = byteArray[4];
            if (visibility == 0x00)
                _visibility = WorksheetVisibilities.Visible;
            else if (visibility == 0x01)
                _visibility = WorksheetVisibilities.Hidden;
            else if (visibility == 0x02)
                _visibility = WorksheetVisibilities.StrongHidden;
            else 
                throw new ApplicationException(string.Format("Unknown Visibility {0}", visibility));

            byte type = byteArray[5];
            if (type == 0x00)
                _sheettype = WorksheetTypes.Worksheet;
            else if (type == 0x02)
                _sheettype = WorksheetTypes.Chart;
            else if (type == 0x06)
                _sheettype = WorksheetTypes.VBModule;
            else
                throw new ApplicationException(string.Format("Unknown Sheet Type {0}", type));

            List<Record> rowRecords = new List<Record>();
            List<Record> cellRecords = new List<Record>();

            for (int i = 0; i < sheetRecords.Count; i++)
            {
                Record record = sheetRecords[i];
                if (record.IsCellRecord())
                {
                    if (record.RID == RID.FORMULA)
                    {
                        Record formulaStringRecord = null;
                        if ((i + i) < sheetRecords.Count)
                        {
                            formulaStringRecord = sheetRecords[i + 1];
                            if (formulaStringRecord.RID != RID.STRING)
                                formulaStringRecord = null;
                        }
                        record = new FormulaRecord(record, formulaStringRecord);
                    }

                    cellRecords.Add(record);
                }
                else if (record.RID == RID.ROW)
                    rowRecords.Add(record);
            }

            //Add the Rows first so they exist for adding the Cells
            foreach (Record rowRecord in rowRecords)
            {
                Bytes rowBytes = rowRecord.Data;
                ushort rowIndex = rowBytes.Get(0, 2).GetBits().ToUInt16();
                Row row = Rows.AddRow(rowIndex);
                bool isDefaultHeight = rowBytes.Get(6, 2).GetBits().Values[15];
                ushort height = 0;
                if (!isDefaultHeight)
                {
                    height = rowBytes.Get(6, 2).GetBits().Get(0, 14).ToUInt16();
                    //TODO: Set height on Row when reading (after Row Height implemented)
                }
                bool defaultsWritten = (rowBytes.Get(10, 1).ByteArray[0] == 0x01);
                if (defaultsWritten)
                {
                    //TODO: Read ROW record defaults
                }
            }

            foreach (Record record in cellRecords)
                AddCells(record);

            _name = UnicodeBytes.Read(boundSheet.Data.Get(6, boundSheet.Data.Length - 6), 8);
        }

        private void AddCells(Record record)
        {
            Bytes bytes = record.Data;
            ushort rowIndex = bytes.Get(0, 2).GetBits().ToUInt16();
            ushort colIndex = bytes.Get(2, 2).GetBits().ToUInt16();
            ushort lastColIndex = colIndex;
            ushort offset = 4;

            byte[] rid = record.RID;
            bool isMulti = false;

            if (rid == RID.MULBLANK)
            {
                isMulti = true;
                rid = RID.BLANK;
            }
            else if (rid == RID.MULRK)
            {
                isMulti = true;
                rid = RID.RK;
            }

            if (isMulti)
                lastColIndex = bytes.Get(bytes.Length - 2, 2).GetBits().ToUInt16();


            while (colIndex <= lastColIndex)
            {
                Cell cell = Cells.Add((ushort)(rowIndex + 1), (ushort)(colIndex + 1));
                ushort xfIndex = bytes.Get(offset, 2).GetBits().ToUInt16();
                offset += 2;

                Bytes data;
                if (rid == RID.BLANK)
                    data = new Bytes();
                else if (rid == RID.RK)
                {
                    data = bytes.Get(offset, 4);
                    offset += 4;
                    cell.SetValue(rid, data);
                }
                else
                {
                    data = bytes.Get(offset, bytes.Length - offset);
                    if (rid == RID.FORMULA)
                    {
                        FormulaRecord formulaRecord = record as FormulaRecord;
                        cell.SetFormula(data, formulaRecord.StringRecord);
                    }
                    else
                        cell.SetValue(rid, data);
                }
                colIndex++;
            }
        }

        internal XlsDocument Document
        {
            get { return _doc; }
        }

        /// <summary>
        /// Gets or sets the Name of this Worksheet.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the Visibility of this Worksheet.
        /// </summary>
        public WorksheetVisibilities Visibility
        {
            get { return _visibility; }
            set { _visibility = value; }
        }

        /// <summary>
        /// Gets or sets the Worksheet Type of this Worksheet.
        /// </summary>
        public WorksheetTypes SheetType
        {
            get { return _sheettype; }
            set { _sheettype = value; }
        }

        /// <summary>
        /// Gets the Cells collection of this Worksheet.
        /// </summary>
        public Cells Cells
        {
            get { return _cells; }
        }

        /// <summary>
        /// Gets the Rows collection of this Worksheet.
        /// </summary>
        public Rows Rows
        {
            get { return _rows; }
        }

        /// <summary>
        /// Gets or sets whether this Worksheet is Protected (the Locked/Hidden status 
        /// of objects such as cells is enforced by Excel).
        /// </summary>
        public bool Protected
        {
            get { return _protected; }
            set { _protected = value; }
        }

        internal int StreamByteLength
        {
            get { return _streamByteLength; }
        }

        internal int[] DBCellOffsets
        {
            set { _dbCellOffsets = value; }
        }

        internal Bytes Bytes
        {
            get
            {
                //NOTE: See excelfileformat.pdf Sec. 4.2

                Bytes bytesA = new Bytes();
                Bytes bytesB = new Bytes();
                Bytes bytesC = new Bytes();
                Bytes bytesD = new Bytes();
                Bytes bytesE = new Bytes();
//                int defaultColumnWidthOffset = 0;
                int rowBlock1Offset;

                //TODO: Move the rest of these sections to private functions like WINDOW2
                bytesD.Append(_rowBlocks.Bytes); //This is first so the RowBlocks interface can calculate the
                //DBCELLOffsets array used below

                //bytesA = BOF (inc) to INDEX (exc)
                //bytesB = INDEX
                //bytesC = INDEX (exc) to RowBlocks (exc)
                //bytesD = RowBlocks
                //bytesE = RowBlocks (exc) to EOF (inc)

                bytesA.Append(Record.GetBytes(RID.BOF, new byte[] { 0x00, 0x06, 0x10, 0x00, 0xAF, 0x18, 0xCD, 0x07, 0xC1, 0x40, 0x00, 0x00, 0x06, 0x01, 0x00, 0x00 }));

                if (_protected)
                    bytesC.Append(Record.GetBytes(RID.PROTECT, new byte[] { 0x01, 0x00 }));
                bytesC.Append(COLINFOS()); //Out of order for a reason - see rowblock1offset calc below

                rowBlock1Offset = _doc.Workbook.Worksheets.StreamOffset;
                int j = _doc.Workbook.Worksheets.GetIndex(Name);
                for (int i = 1; i < j; i++)
                    rowBlock1Offset += _doc.Workbook.Worksheets[i].StreamByteLength;
                rowBlock1Offset += bytesA.Length + (20 + (4 * (_dbCellOffsets.Length - 1))) + bytesC.Length;

                bytesB.Append(INDEX(rowBlock1Offset));

                //BEGIN Worksheet View Settings Block
                bytesE.Append(WINDOW2());
                //END Worksheet View Settings Block

                bytesE.Append(MERGEDCELLS());
                bytesE.Append(Record.GetBytes(RID.EOF, new byte[0]));

                bytesA.Append(bytesB);
                bytesA.Append(bytesC);
                bytesA.Append(bytesD);
                bytesA.Append(bytesE);

                _streamByteLength = bytesA.Length;

                return bytesA;
            }
        }

        private Bytes INDEX(int baseLength)
        {
            Bytes index = new Bytes();

            //Not used
            index.Append(new byte[] { 0x00, 0x00, 0x00, 0x00 });

            //Index to first used row (0-based)
            index.Append(BitConverter.GetBytes(_rows.MinRow - 1));

            //Index to first row of unused tail of sheet(last row + 1, 0-based)
            index.Append(BitConverter.GetBytes(_rows.MaxRow));

            //Absolute stream position of the DEFCOLWIDTH record
            //TODO: Implement Worksheet.INDEX Absolute stream position of the DEFCOLWIDTH record (not necessary)
            index.Append(BitConverter.GetBytes((uint)0));

            for (int i = 1; i < _dbCellOffsets.Length; i++)
                index.Append(BitConverter.GetBytes((uint)(baseLength + _dbCellOffsets[i])));

            return Record.GetBytes(RID.INDEX, index);
        }

        private Bytes WINDOW2()
        {
            Bytes window2 = new Bytes();

            //TODO: Implement options - excelfileformat.pdf pp.210-211
            if (_doc.Workbook.Worksheets.GetIndex(Name) == 0) //NOTE: This was == 1, but the base of the worksheets collection must have changed
                window2.Append(new byte[] { 0xB6, 0x06 });
            else
                window2.Append(new byte[] { 0xB6, 0x04 });
            window2.Append(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

            return Record.GetBytes(RID.WINDOW2, window2);
        }

        private Bytes COLINFOS()
        {
            Bytes colinfos = new Bytes();

            for (int i = 0; i < _columnInfos.Count; i++)
                colinfos.Append(_columnInfos[i].Bytes);

            return colinfos;
        }

        private Bytes MERGEDCELLS()
        {
            Bytes mergedcells = new Bytes();

            int areaIndex = 0;
            int mergeAreaCount = _mergeAreas.Count;
            long areasPerRecord = 1027;
            int recordsRequired = (int)Math.Ceiling(_mergeAreas.Count/(double)areasPerRecord);
            for (int recordIndex = 0; recordIndex < recordsRequired; recordIndex++)
            {
                ushort blockAreaIndex = 0;
                Bytes rangeAddresses = new Bytes();
                while (areaIndex < mergeAreaCount && blockAreaIndex < areasPerRecord)
                {
                    rangeAddresses.Append(CellRangeAddress(_mergeAreas[areaIndex]));

                    blockAreaIndex++;
                    areaIndex++;
                }
                rangeAddresses.Prepend(BitConverter.GetBytes(blockAreaIndex));
                mergedcells.Append(Record.GetBytes(RID.MERGEDCELLS, rangeAddresses));
            }

            return mergedcells;
        }

        private Bytes CellRangeAddress(MergeArea mergeArea)
        {
            return CellRangeAddress(mergeArea.RowMin, mergeArea.RowMax, mergeArea.ColMin, mergeArea.ColMax);
        }

        private Bytes CellRangeAddress(ushort minRow, ushort maxRow, ushort minCol, ushort maxCol)
        {
            minRow--;
            maxRow--;
            minCol--;
            maxCol--;
            Bytes rangeAddress = new Bytes();
            rangeAddress.Append(BitConverter.GetBytes(minRow));
            rangeAddress.Append(BitConverter.GetBytes(maxRow));
            rangeAddress.Append(BitConverter.GetBytes(minCol));
            rangeAddress.Append(BitConverter.GetBytes(maxCol));
            return rangeAddress;
        }

        //TODO: I think this should actually be MRBlockRow --- see use in RowBlocks.BlockRow
        internal CachedBlockRow CachedBlockRow
        {
            get { return _cachedBlockRow; }
            set { _cachedBlockRow = value; }
        }

        /// <summary>
        /// Adds a Column Info record to this Worksheet.
        /// </summary>
        /// <param name="columnInfo">The ColumnInfo object to add to this Worksheet.</param>
        public void AddColumnInfo(ColumnInfo columnInfo)
        {
            //TODO: Implement existence checking & deletion / overwriting / not-adding
            //NOTE: Don't know if this is necessary (i.e. does Excel allow "adding" values of overlapping ColInfos?
            _columnInfos.Add(columnInfo);
        }

        //TODO: Optionally provide overload with bool parameter to decide whether to throw
        //exception instead of losing values.
        /// <summary>
        /// Adds a MergeArea to this Worksheet.  The mergeArea is verified not to
        /// overlap with any previously defined area.  NOTE Values and formatting
        /// in all cells other than the first in mergeArea (scanning left to right,
        /// top to bottom) will be lost.
        /// </summary>
        /// <param name="mergeArea">The MergeArea to add to this Worksheet.</param>
        public void AddMergeArea(MergeArea mergeArea)
        {
            foreach (MergeArea existingArea in _mergeAreas)
            {
                bool colsOverlap = false;
                bool rowsOverlap = false;

                //if they overlap, either mergeArea will surround existingArea, 
                if (mergeArea.ColMin < existingArea.ColMin && existingArea.ColMax < mergeArea.ColMax)
                    colsOverlap = true;
                //or existingArea will contain >= 1 of mergeArea's Min and Max indices
                else if ((existingArea.ColMin <= mergeArea.ColMin && existingArea.ColMax >= mergeArea.ColMin) ||
                    (existingArea.ColMin <= mergeArea.ColMax && existingArea.ColMax >= mergeArea.ColMax))
                    colsOverlap = true;

                if (mergeArea.RowMin < existingArea.RowMin && existingArea.RowMax < mergeArea.RowMax)
                    rowsOverlap = true;
                else if ((existingArea.RowMin <= mergeArea.RowMin && existingArea.RowMax >= mergeArea.RowMin) ||
                    (existingArea.RowMin <= mergeArea.RowMax && existingArea.RowMax >= mergeArea.RowMax))
                    rowsOverlap = true;

                if (colsOverlap && rowsOverlap)
                    throw new ArgumentException("overlaps with existing MergeArea", "mergeArea");
            }

            //TODO: Add ref to this mergeArea to all rows in its range, and add checking on Cell
            //addition methods to validate they are not being added within the mergedarea, other
            //than as the top-left cell.

            _mergeAreas.Add(mergeArea);
        }

        /// <summary>
        /// Writes a DataTable to this Worksheet, beginning at the provided Row
        /// and Column indices.  A Header Row will be written.
        /// </summary>
        /// <param name="table">The DataTable to write to this Worksheet.</param>
        /// <param name="startRow">The Row at which to start writing the DataTable
        /// to this Worksheet (1-based).</param>
        /// <param name="startCol">The Column at which to start writing the DataTable
        /// to this Worksheet (1-based).</param>
        public void Write(DataTable table, int startRow, int startCol)
        {
            if ((table.Columns.Count + startCol) > BIFF8.MaxCols)
                throw new ApplicationException(string.Format("Table {0} has too many columns {1} to fit on Worksheet {2} with the given startCol {3}",
                                               table.TableName, table.Columns.Count, BIFF8.MaxCols, startCol));
            if ((table.Rows.Count + startRow) > (BIFF8.MaxRows - 1))
                throw new ApplicationException(string.Format("Table {0} has too many rows {1} to fit on Worksheet {2} with the given startRow {3}",
                                               table.TableName, table.Rows.Count, (BIFF8.MaxRows - 1), startRow));
            int row = startRow;
            int col = startCol;
            foreach (DataColumn dataColumn in table.Columns)
                Cells.Add(row, col++, dataColumn.ColumnName);
            foreach (DataRow dataRow in table.Rows)
            {
                row++;
                col = startCol;
                foreach (object dataItem in dataRow.ItemArray)
                {
                    object value = dataItem;

                    if (dataItem == DBNull.Value)
                        value = null;
                    if (dataRow.Table.Columns[col - startCol].DataType == typeof(byte[]))
                        value = string.Format("[ByteArray({0})]", ((byte[]) value).Length);

                    Cells.Add(row, col++, value);
                }
            }
        }
    }

}
