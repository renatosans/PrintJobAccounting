namespace org.in2bits.MyXls
{
    /// <summary>
    /// The Font Families available for formatting in Excel.
    /// </summary>
	public enum FontFamilies : byte
	{
        /// <summary>Default Font Family : None (unknown or don't care)</summary>
		Default = None,

        /// <summary>None (unknown or don't care)</summary>
        None = 0x00,

        /// <summary>Roman (variable width, serifed)</summary>
        Roman = 0x01,

        /// <summary>Swiss (variable width, sans-serifed)</summary>
        Swiss = 0x02,

        /// <summary>Modern (fixed width, serifed or sans-serifed)</summary>
        Modern = 0x03,

        /// <summary>Script (cursive)</summary>
        Script = 0x04,

        /// <summary>Decorative (specialised, for example Old English, Fraktur)</summary>
        Decorative = 0x05
	}
}
