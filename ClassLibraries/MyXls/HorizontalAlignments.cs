namespace org.in2bits.MyXls
{
    /// <summary>
    /// The different horizontal alignments available in Excel.
    /// </summary>
	public enum HorizontalAlignments : byte
	{
        /// <summary>Default - General</summary>
		Default = General,

        /// <summary>General</summary>
        General = 0,

        /// <summary>Left</summary>
        Left = 1,

        /// <summary>Centered</summary>
        Centered = 2,

        /// <summary>Right</summary>
        Right = 3,

        /// <summary>Filled</summary>
        Filled = 4,

        /// <summary>Justified</summary>
        Justified = 5,

        /// <summary>Centered Across the Selection</summary>
        CenteredAcrossSelection = 6,

        /// <summary>Distributed</summary>
        Distributed = 7
	}
}
