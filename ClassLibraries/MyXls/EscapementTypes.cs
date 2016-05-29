namespace org.in2bits.MyXls
{
    /// <summary>
    /// Text cell escapement (sub/super-script, etc.) types.
    /// </summary>
	public enum EscapementTypes : ushort
	{
        /// <summary>Default - None</summary>
		Default = None,
        
        /// <summary>None</summary>
        None = 0,

        /// <summary>Superscript</summary>
        Superscript = 256,

        /// <summary>Subscript</summary>
        Subscript = 512
	}
}
