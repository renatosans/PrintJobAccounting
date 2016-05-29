using System;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// Describes appearance of text in cells.
    /// </summary>
	public class Font : ICloneable
	{
        /*
            ----------------------------------------------------------------------------
            The font with index 4 is omitted in all BIFF versions. This means the first 
            four fonts have zero-based indexes, and the fifth font and all following 
            fonts are referenced with one-based indexes.
                                                - excelfileformat.pdf, section 6.43 FONT
            ----------------------------------------------------------------------------
        */
        private readonly XlsDocument _doc;
        private XF _target;
		private ushort? _id;

        private bool _isInitializing = false;

		private ushort _height;
		private bool _italic;
		private bool _underlined; //NOT IMPLEMENTED - USE 'Underline'
		private bool _struckOut;
		private ushort _colorIndex;
		private FontWeight _weight;
		private EscapementTypes _escapement;
		private UnderlineTypes _underline;
		private FontFamilies _fontFamily;
		private CharacterSets _characterSet;
		private byte _notUsed = 0x00;
		private string _fontName;

        internal Font(XlsDocument doc)
        {
            _isInitializing = true;

            _doc = doc;
            _id = null;

            SetDefaults();

            _isInitializing = false;
        }

        internal Font(XlsDocument doc, XF xf) : this(doc)
		{
            _target = xf;
        }

        internal Font(XlsDocument doc, Bytes bytes) : this(doc)
        {
            ReadBytes(bytes);
        }

        /// <summary>
        /// Calculates whether a given Font object is value-equal to this
        /// Font object.
        /// </summary>
        /// <param name="that">A Font object to compare to this Font.</param>
        /// <returns>true if equal, false otherwise</returns>
		public bool Equals(Font that)
		{
			if (_height != that._height) return false;
			if (_italic != that._italic) return false;
			if (_underlined != that._underlined) return false;
			if (_struckOut != that._struckOut) return false;
			if (_colorIndex != that._colorIndex) return false;
			if (_weight != that._weight) return false;
			if (_escapement != that._escapement) return false;
			if (_underline != that._underline) return false;
			if (_fontFamily != that._fontFamily) return false;
			if (_characterSet != that._characterSet) return false;
			if (string.Compare(_fontName, that._fontName, false) != 0) return false;

			return true;
		}

		private void SetDefaults()
		{
			_height = 200;
			_italic = false;
			_underlined = false;
			_struckOut = false;
			_colorIndex = 32767;
			_weight = FontWeight.Normal;
			_escapement = EscapementTypes.Default;
			_underline = UnderlineTypes.Default;
			_fontFamily = FontFamilies.Default;
			_characterSet = CharacterSets.Default;
			_notUsed = 0;
			_fontName = "Arial";

			OnChange();
		}

		internal ushort ID
		{
			get
			{
				if (_id == null)
					_id = _doc.Workbook.Fonts.Add(this);
				return (ushort) _id;
			}
            set
            {
                _id = value;
            }
		}

		private void OnChange()
		{
			if (_isInitializing)
			    return;

            _id = null;
		    _id = ID;
		    _target.OnFontChange(this);
		}

		/// <summary>
		/// Gets or sets the Height of Font (in twips = 1/20 of a point).
		/// </summary>
		public ushort Height
		{
			get { return _height; }
			set
			{
				if (value == _height)
					return;

				_height = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets whether Characters are italic.
		/// </summary>
		public bool Italic
		{
			get { return _italic; }
			set
			{
				if (value == _italic)
					return;

				_italic = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets whether Characters are struck out.
		/// </summary>
		public bool StruckOut
		{
			get { return _struckOut; }
			set
			{
				if (value == _struckOut)
					return;

				_struckOut = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets Color index of this Font.
		/// </summary>
		public ushort ColorIndex
		{
			get { return _colorIndex; }
			set
			{
				if (value == _colorIndex)
					return;

				_colorIndex = value;
				OnChange();
			}
		}

        /// <summary>
        /// Gets or sets whether this Font is Bold.
        /// </summary>
        public bool Bold
        {
			get { return (ushort)_weight >= (ushort)FontWeight.Bold; }
        	set { Weight = value ? FontWeight.Bold : FontWeight.Normal; }
        }

		///<summary>Gets or sets Font weight. </summary>
		/// <remarks>This replaces the Bold property.</remarks>
		public FontWeight Weight
		{
			get { return _weight; }
			set 
			{
				if (value != _weight)
				{
					_weight = value;
					OnChange();
				}
			}
		}

		/// <summary>
		/// Gets or sets the EscapementTypes value of this Font.
		/// </summary>
		public EscapementTypes Escapement
		{
			get { return _escapement; }
			set
			{
				if (value == _escapement)
					return;

				_escapement = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets UnderlineTypes for this Font.  This replaces the 'Underlined' property.
		/// </summary>
		public UnderlineTypes Underline
		{
			get { return _underline; }
			set
			{
				if (value == _underline)
					return;

				_underline = value;
				OnChange();
			}
		}

		/// <summary>
		/// /Gets or sets Font Family.
		/// </summary>
		public FontFamilies FontFamily
		{
			get { return _fontFamily; }
			set
			{
				if (value == _fontFamily)
					return;

				_fontFamily = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets Character Set.
		/// </summary>
		public CharacterSets CharacterSet
		{
			get { return _characterSet; }
			set
			{
				if (value == _characterSet)
					return;

				_characterSet = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets Font Name (255 characters max)
		/// </summary>
		public string FontName
		{
			get { return _fontName; }
			set
			{
				if (value == null)
				    value = string.Empty;
                
                if (value.Length > 255)
					value = value.Substring(0, 255);

				if (string.Compare(value, _fontName, true) == 0)
					return;

				_fontName = value;
				OnChange();
			}
		}

        internal XF Target
        {
            get { return _target; }   
            set { _target = value; }
        }

        private void ReadBytes(Bytes bytes)
        {
            byte[] byteArray = bytes.ByteArray;
            _height = BitConverter.ToUInt16(byteArray, 0);
            SetOptionsValue(bytes.Get(2, 2));
            _colorIndex = BitConverter.ToUInt16(byteArray, 4);
            _weight = FontWeightConverter.Convert(BitConverter.ToUInt16(byteArray, 6));
            _escapement = (EscapementTypes) BitConverter.ToUInt16(byteArray, 8);
            _underline = (UnderlineTypes) byteArray[10];
            _fontFamily = (FontFamilies) byteArray[11];
            _characterSet = (CharacterSets) byteArray[12];
            //skip byte index 13
            _fontName = UnicodeBytes.Read(bytes.Get(14, bytes.Length - 14), 8);
        }

        internal Bytes Bytes
		{
			get
			{
				Bytes bytes = new Bytes();

				bytes.Append(BitConverter.GetBytes(_height));
				bytes.Append(BitConverter.GetBytes(OptionsValue()));
				bytes.Append(BitConverter.GetBytes(_colorIndex));
				bytes.Append(BitConverter.GetBytes((ushort)_weight));
				bytes.Append(BitConverter.GetBytes((ushort)_escapement));
				bytes.Append((byte) _underline);
				bytes.Append((byte)_fontFamily);
				bytes.Append((byte)_characterSet);
				bytes.Append(_notUsed);
				bytes.Append(XlsDocument.GetUnicodeString(_fontName, 8));

			    return Record.GetBytes(RID.FONT, bytes);
			}
		}

        private void SetOptionsValue(Bytes bytes)
        {
            ushort options = BitConverter.ToUInt16(bytes.ByteArray, 0);

            if (options >= 8)
            {
                _struckOut = true;
                options -= 8;
            }
            else
                _struckOut = false;

            if (options >= 4)
            {
                _underlined = true;
                options -= 4;
            }
            else
                _underlined = false;

            if (options >= 2)
            {
                _italic = true;
            }
            else
                _italic = false;
        }

        private ushort OptionsValue()
		{
			ushort options = 0;

			if (Bold) options += 1;
			if (_italic) options += 2;
			if (_underlined) options += 4;
			if (_struckOut) options += 8;

			return options;
		}

		#region ICloneable members

        /// <summary>
        /// Creates and returns a new Font object value-equal to this Font object.
        /// </summary>
        /// <returns>A new Font object value-equal to this Font object.</returns>
		public object Clone()
		{
            Font clone = new Font(_doc, _target);

			clone._height = _height;
			clone._italic = _italic;
			//clone._underlined = _underlined;
			clone._struckOut = _struckOut;
			clone._colorIndex = _colorIndex;
			clone._weight = _weight;
			clone._escapement = _escapement;
			clone._underline = _underline;
			clone._fontFamily = _fontFamily;
			clone._characterSet = _characterSet;
			clone._fontName = _fontName;

			return clone;
		}

		#endregion
	}
}
