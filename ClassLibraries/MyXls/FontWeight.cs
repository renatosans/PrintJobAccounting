using System;

namespace org.in2bits.MyXls
{
	/// <summary>Relative Weight of a font.</summary>
	/// <remarks>Based on the <a href="http://www.microsoft.com/typography/otspec/otff.htm">OpenType</a> specification.</remarks>
	public enum FontWeight : ushort
	{
		/// <summary>Thin (100) font weight.</summary>
		Thin = 100,

		/// <summary>Extra light (200) font weight.</summary>
		ExtraLight = 200,

		/// <summary>Light (300) font weight.</summary>
		Light = 300,

		/// <summary>Normal (400) font weight.</summary>
		Normal = 400,

		/// <summary>Medium (500) font weight.</summary>
		Medium = 500,

		/// <summary>Semi-Bold (600) font weight.</summary>
		SemiBold = 600,

		/// <summary>Bold (700) font weight.</summary>
		Bold = 700,

		/// <summary>Extra bold (800) font weight.</summary>
		ExtraBold = 800,

		/// <summary>Heavy (900) font weight.</summary>
		Heavy = 900
	}

	/// <summary>Conversions for FontWeight.</summary>
	public static class FontWeightConverter
	{
		/// <summary>Convert from a ushort to a FontWeight value.</summary>
		/// <param name="weight">Font weight value</param>
		/// <returns>FontWeight</returns>
		public static FontWeight Convert(ushort weight)
		{
			FontWeight result = FontWeight.Thin;
			if (weight >= 900)
			{
				result = FontWeight.Heavy;
			}
			else if (weight > 100)
			{
				result = (FontWeight)(Math.Round(weight / 100.0) * 100);
			}
			return result;
		}
	}
}