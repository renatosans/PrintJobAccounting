namespace org.in2bits.MyXls
{
    /// <summary>
    /// Vertical alignments available in an Excel document.
    /// </summary>
	public enum VerticalAlignments : byte
	{
        /// <summary>Default - Bottom</summary>
		Default = Bottom,
        
        /// <summary>Top</summary>
        Top = 0,

        /// <summary>Centered</summary>
        Centered = 1,

        /// <summary>Bottom</summary>
        Bottom = 2,

        /// <summary>Justified</summary>
        Justified = 3,

        /// <summary>Distributed</summary>
        Distributed = 4
	}
}
