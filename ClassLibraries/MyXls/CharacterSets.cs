namespace org.in2bits.MyXls
{
    /// <summary>
    /// Different Character Sets supported by Excel.
    /// </summary>
	public enum CharacterSets : byte
	{
        /// <summary>Default - ANSI Latin</summary>
		Default = ANSILatin,
        
        /// <summary>ANSI Latin</summary>
        ANSILatin = 0x00,

        /// <summary>System Default</summary>
        SystemDefault = 0x01,

        /// <summary>Symbol</summary>
        Symbol = 0x02,

        /// <summary>Apple Roman</summary>
        AppleRoman = 0x4D,

        /// <summary>ANSI Japanese Shift JIS</summary>
        ANSIJapaneseShiftJIS = 0x80,

        /// <summary>ANSI Korean Hangul</summary>
        ANSIKoreanHangul = 0x81,

        /// <summary>ANSI Korean Johab</summary>
        ANSIKoreanJohab = 0x82,

        /// <summary>ANSI Chinese Simplified JBK</summary>
        ANSIChineseSimplifiedJBK = 0x86,

        /// <summary>ANSI Chinese Traditional BIG5</summary>
        ANSIChineseTraditionalBIG5 = 0x88,

        /// <summary>ANSI Greek</summary>
        ANSIGreek = 0xA1,

        /// <summary>ANSI Turkish</summary>
        ANSITurkish = 0xA2,

        /// <summary>ANSI Vietnamese</summary>
        ANSIVietnamese = 0xA3,

        /// <summary>ANSI Hebrew</summary>
        ANSIHebrew = 0xB1,

        /// <summary>ANSI Arabic</summary>
        ANSIArabic = 0xB2,

        /// <summary>ANSI Baltic</summary>
        ANSIBaltic = 0xBA,

        /// <summary>ANSI Cyrillic</summary>
        ANSICyrillic = 0xCC,

        /// <summary>ANSI Thai</summary>
        ANSIThai = 0xDE,

        /// <summary>ANSI Latin II Central European</summary>
        ANSILatinIICentralEuropean = 0xEE,

        /// <summary>OEM Latin I</summary>
        OEMLatinI = 0xFF
	}
}
