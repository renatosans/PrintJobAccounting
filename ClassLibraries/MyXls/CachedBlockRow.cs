namespace org.in2bits.MyXls
{
	internal struct CachedBlockRow
	{
	    internal ushort RowBlockIndex;
	    internal ushort BlockRowIndex;
	    internal Row Row;
	    internal static CachedBlockRow Empty = new CachedBlockRow(0, 0, null);

	    internal CachedBlockRow(ushort rowBlockIndex, ushort blockRowIndex, Row row)
		{
			RowBlockIndex = rowBlockIndex;
			BlockRowIndex = blockRowIndex;
			Row = row;
		}
	}
}
