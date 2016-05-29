namespace org.in2bits.MyXls
{
	/// <summary>
	/// Line Styles
	/// </summary>
	public enum LineStyle : ushort
	{
		/// <summary>No Style</summary>
		None = 0,

		/// <summary>Thin</summary>
		Thin,

		/// <summary>Medium</summary>
		Medium,

		/// <summary>Dashed</summary>
		Dashed,

		/// <summary>Dotted</summary>
		Dotted,

		/// <summary>Thick</summary>
		Thick,

		/// <summary>Double</summary>
		Double,

		/// <summary>Hair</summary>
		Hair,

		/// <summary>Medium dashed</summary>
		/// <remarks>BIFF8 Only</remarks>
		MediumDashed,

		/// <summary>Dash-dot</summary>
		/// <remarks>BIFF8 Only</remarks>
		DashDot,

		/// <summary>Medium dash-dot</summary>
		/// <remarks>BIFF8 Only</remarks>
		MediumDashDot,

		/// <summary>Dash-dot-dot</summary>
		/// <remarks>BIFF8 Only</remarks>
		DashDotDot,

		/// <summary>Medium dash-dot-dot</summary>
		/// <remarks>BIFF8 Only</remarks>
		MediumDashDotDot,

		/// <summary>Slanted dash-dot</summary>
		/// <remarks>BIFF8 Only</remarks>
		SlantedDashDot
	}
}