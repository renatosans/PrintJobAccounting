namespace org.in2bits.MyXls
{
    /// <summary>
    /// Holds the Row and Column indices for a Cell's Coordinate value.
    /// </summary>
	public struct CellCoordinate
	{
        /// <summary>
        /// Row index (1-based).
        /// </summary>
		public ushort Row;

        /// <summary>
        /// Column index (1-based).
        /// </summary>
		public ushort Column;

        /// <summary>
        /// Initializes a new instance of the CellCoordinate struct with the given values.
        /// </summary>
        /// <param name="row">Row index (1-based).</param>
        /// <param name="column">Column index (1-based).</param>
		public CellCoordinate(ushort row, ushort column)
		{
			Row = row;
			Column = column;
		}
	}
}
