using System;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyXls
{
    internal class RowBlocks
	{
		private Worksheet _worksheet;

        internal RowBlocks(Worksheet worksheet)
		{
			_worksheet = worksheet;
		}

		private ushort BlockCount
		{
			get
			{
				if (_worksheet.Rows.MaxRow == 0)
					return 0;
				else
				{
					ushort rowSpan = (ushort)(_worksheet.Rows.MaxRow - _worksheet.Rows.MinRow);
					return (ushort)(((int)Math.Floor((decimal)rowSpan / 32)) + 1);
				}
			}
		}

		private Row GetBlockRow(ushort rbIdx, ushort brIdx)
		{
			Row blockRow = new Row();

			ushort j = 0;
			ushort row1 = (ushort)(_worksheet.Rows.MinRow + ((rbIdx - 1) * 32));

		    CachedBlockRow cachedBlockRow = _worksheet.CachedBlockRow;

			ushort rowMin = row1;
			ushort rowMax = (ushort)(rowMin + 31);
			if (cachedBlockRow.RowBlockIndex == rbIdx)
			{
				if (cachedBlockRow.BlockRowIndex == brIdx)
					return cachedBlockRow.Row;
				else if (brIdx < cachedBlockRow.BlockRowIndex)
					//NOTE: !!! Is this right? vba said ".Row.Row" !!!
					rowMax = cachedBlockRow.Row.RowIndex;
				else if (brIdx > cachedBlockRow.BlockRowIndex)
				{
					if (cachedBlockRow.RowBlockIndex > 0)
					{
						rowMin = (ushort)(cachedBlockRow.Row.RowIndex + 1);
						j = cachedBlockRow.BlockRowIndex;
					}
					else
						rowMin = 1;
				}
			}

			for (ushort i = rowMin; i <= rowMax; i++)
			{
				if (_worksheet.Rows.RowExists(i))
				{
					j++;
					if (j == brIdx)
					{
						blockRow = _worksheet.Rows[i];
						_worksheet.CachedBlockRow = new CachedBlockRow(rbIdx, brIdx, blockRow);
					}
				}
			}

			return blockRow;
		}

	    private ushort GetBlockRowCount(ushort idx)
		{
			ushort count = 0;

			ushort row1 = (ushort)(_worksheet.Rows.MinRow + ((idx - 1) * 32));
			for (ushort i = row1; i <= (row1 + 31); i++)
			{
				if (_worksheet.Rows.RowExists(i))
					count++;
			}

			return count;
		}

	    internal Bytes Bytes
		{
			get
			{
				Bytes bytes = new Bytes();

				ushort j = BlockCount;
				int[] bLen = new int[j + 1];

				for (ushort i = 1; i <= j; i++)
				{
					ushort m = GetBlockRowCount(i);
					ushort[] cOff = new ushort[m + 1];
					Bytes rows = new Bytes();
					Bytes cells = new Bytes();
					for (ushort k = 1; k <= m; k++)
					{
                        if (k == 1)
                            cOff[k] = (ushort)((m - 1) * 20);
                        else if (k == 2)
                            cOff[k] = (ushort) cells.Length;
                        else
                            cOff[k] = (ushort)(cells.Length - cOff[k - 1]);

                        Row row = GetBlockRow(i, k);
                        rows.Append(ROW(row));
                        int o = row.CellCount;
                        for (ushort n = 1; n <= o; n++)
                        {
                            //OPTIM: The greatest time factor is the Row.Cell(x) lookup
                            Cell cell = row.GetCell(n);
                            cells.Append(cell.Bytes);
                        }
                    }
                    bytes.Append(rows);
					bytes.Append(cells);
					cOff[0] = (ushort)(rows.Length + cells.Length);
					bLen[i] = bytes.Length;
					bytes.Append(DBCELL(cOff));
				}

				_worksheet.DBCellOffsets = bLen;

				return bytes;
			}
		}

		private static Bytes ROW(Row row)
		{
			Bytes bytes = new Bytes();

			//Index of this row
			bytes.Append(BitConverter.GetBytes((ushort)(row.RowIndex - 1)));

			//Index to column of the first cell which is described by a cell record
			bytes.Append(BitConverter.GetBytes((ushort)(row.MinCellCol - 1)));

			//Index to column of the last cell which is described by a cell record, + 1
			bytes.Append(BitConverter.GetBytes(row.MaxCellCol));

			//Height of row in twips, custom row height indicator
			//TODO: Implement Row height and custom height indicators (excelfileformat.pdf p.190)
			bytes.Append(new byte[] {0x08, 0x01});

			//Not used
			bytes.Append(new byte[] {0x00, 0x00});

			//Not used anymore in BIFF8 (DBCELL instead)
			bytes.Append(new byte[] {0x00, 0x00});

			//Option flags and default row formatting
			//TODO: Implement Row option flags and default row formatting (excelfileformat.pdf p.190)
			bytes.Append(new byte[] {0x00, 0x01, 0x0F, 0x00});

		    return Record.GetBytes(RID.ROW, bytes);
		}

		private static Bytes DBCELL(ushort[] cOff)
		{
			Bytes dbcell = new Bytes();

			for (int i = 0; i < cOff.Length; i++)
			{
				if (i == 0)
					dbcell.Append(BitConverter.GetBytes((uint) cOff[i]));
				else 
					dbcell.Append(BitConverter.GetBytes(cOff[i]));
			}

		    return Record.GetBytes(RID.DBCELL, dbcell);
		}
	}
}
