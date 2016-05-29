namespace org.in2bits.MyXls
{
    /// <summary>
    /// Indicates local culture's text direction (left-to-right or right-to-left).
    /// </summary>
	public enum TextDirections : ushort
	{
        /// <summary>Default - By Context</summary>
        Default = ByContext,

        /// <summary>By Context</summary>
        ByContext = 0,

        /// <summary>Left to Right</summary>
        LeftToRight = 1,

        /// <summary>Right to Left</summary>
        RightToLeft = 2
	}
}
