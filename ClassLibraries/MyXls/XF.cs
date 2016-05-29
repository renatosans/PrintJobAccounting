using System;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// The XF (eXtended Format) contains formatting information for cells, rows, columns or styles.
    /// </summary>
	public class XF : ICloneable
	{
        private IXFTarget _targetObject;

		private readonly XlsDocument _doc;
		private ushort? _id;

        internal ushort? ReadStyleXfIndex = null;

//        private const ushort DEFAULT_LINE_COLOUR_INDEX = 8;

	    internal XF(XlsDocument doc)
		{
			_doc = doc;

			_id = null;

			SetDefaults();
		}

        internal XF(XlsDocument doc, Bytes bytes, Font font, string format) : this(doc)
        {
            ReadBytes(bytes, font, format);
        }

        internal IXFTarget Target
        {
            get { return _targetObject; }
            set { _targetObject = value;
                _font.Target = this; }
        }

        internal void OnFontChange(Font newFont)
        {
            _font = (Font) newFont.Clone();
            //_font.ID = newFont.ID;
            OnChange();
        }

        internal void OnChange()
		{
			_id = null;
            if (_targetObject != null)
                _targetObject.UpdateId(this);
		}

		private void SetDefaults()
		{
		    _font = new Font(_doc, this);
		    _format = Formats.Default;
		    _style = new Style(_doc, this);

			_horizontalAlignment = HorizontalAlignments.Default;
			_textWrapRight = false;
			_verticalAlignment = VerticalAlignments.Default;
			_rotation = 0;
			_indentLevel = 0;
			_shrinkToCell = false;
			_textDirection = TextDirections.Default;
			_cellLocked = false; //NOTE: Unsure about this default (compare to Commented XF String in BinData)
			_formulaHidden = false; //NOTE: Unsure about this default (compare to Commented XF String in BinData)
			_isStyleXF = false; //NOTE: Unsure about this default (compare to Commented XF String in BinData)
			_useNumber = true;
			_useFont = true;
			_useMisc = true;
			_useBorder = true;
			_useBackground = true;
			_useProtection = true; //You should ALWAYS use protection ;-)
			_leftLineStyle = 0;
			_rightLineStyle = 0;
			_topLineStyle = 0;
			_bottomLineStyle = 0;
			_leftLineColor = Colors.DefaultLineColor;
			_rightLineColor = Colors.DefaultLineColor;
			_diagonalDescending = false;
			_diagonalAscending = false;
            _topLineColor = Colors.DefaultLineColor;
            _bottomLineColor = Colors.DefaultLineColor;
            _diagonalLineColor = Colors.DefaultLineColor;
			_diagonalLineStyle = LineStyle.None;
			_pattern = 0;
			_patternColor = Colors.DefaultPatternColor;
		    _patternBackgroundColor = Colors.DefaultPatternBackgroundColor;

			OnChange();
		}

		private Font _font;
		private string _format = Formats.Default;
		private Style _style;

		/// <summary>
		/// Gets or sets the Font for this XF.
		/// </summary>
		public Font Font
		{
			get
			{
				if (_font == null)
				    _font = new Font(_doc, this);
                
				return _font;
			}
			set
			{
				if (_font == null && value == null)
					return;
				else if (_font != null && value != null && value.Equals(_font))
					return;

				_font = (value == null ? null : (Font) value.Clone());
				OnChange(); 
			}
		}

		/// <summary>
		/// Gets or sets the Format for this XF.
		/// </summary>
		public string Format
		{
			get
			{
				return _format;
			}
			set
			{
                if (value == null)
                    value = Formats.Default;

                if (value.Length > 65535)
                    value = value.Substring(0, 65535);

				if (string.Compare(value, _format, false) == 0)
					return;

				_format = (string) value.Clone();
				OnChange();
			}
		}

        /// <summary>
		/// Gets or sets the Parent Style XF (absent on Style XF's).
		/// </summary>
		public Style Style
		{
			get
			{
				if (_style == null)
				    _style = new Style(_doc, this);
				return _style;
			}
			set
			{
				if (_style == null && value == null)
					return;
				else if (_style != null && value != null && value.Equals(_style))
					return;

				_style = (value == null ? null : (Style) value.Clone());
				OnChange();
			}
		}

		private HorizontalAlignments _horizontalAlignment;
		private bool _textWrapRight;
		private VerticalAlignments _verticalAlignment;
		private short _rotation;
		private ushort _indentLevel;
		private bool _shrinkToCell;
		private TextDirections _textDirection;

		/// <summary>
		/// Gets or sets the HorizontalAlignments value for this XF.
		/// </summary>
		public HorizontalAlignments HorizontalAlignment
		{
			get { return _horizontalAlignment; }
			set
			{
				if (value == _horizontalAlignment)
					return;

				_horizontalAlignment = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets whether Text is wrapped at right border.
		/// </summary>
		public bool TextWrapRight
		{
			get { return _textWrapRight; }
			set
			{
				if (value == _textWrapRight)
					return;

				_textWrapRight = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets the VerticalAlignments value for this XF.
		/// </summary>
		public VerticalAlignments VerticalAlignment
		{
			get { return _verticalAlignment; }
			set
			{
				if (value == _verticalAlignment)
					return;

				_verticalAlignment = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets the Text rotation angle for this XF (-360 or 0?) to 360.
		/// </summary>
		public short Rotation
		{
			get { return _rotation; }
			set
			{
				if (value == _rotation)
					return;

				//NOTE: This looks wrong!
				if (value < 0)
					value = 0;
				if (value > 180 && value != 255)
					value = 0;
				_rotation = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets the IndentLevel for this XF.
		/// </summary>
		public ushort IndentLevel
		{
			get { return _indentLevel; }
			set
			{
				if (value == _indentLevel)
					return;

				_indentLevel = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets whether to Shrink content to fit into cell for this XF.
		/// </summary>
		public bool ShrinkToCell
		{
			get { return _shrinkToCell; }
			set
			{
				if (value == _shrinkToCell)
					return;

				_shrinkToCell = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets the TextDirections value for this XF (BIFF8X only).
		/// </summary>
		public TextDirections TextDirection
		{
			get { return _textDirection; }
			set
			{
				if (value == _textDirection)
					return;

				_textDirection = value;
				OnChange();
			}
		}

		#region XF_TYP_PROT : XF Type, Cell Protection

		private bool _cellLocked;
		private bool _formulaHidden;
		private bool _isStyleXF;

		/// <summary>
		/// Gets or sets whether this XF's Cells are locked.
		/// </summary>
		public bool CellLocked
		{
			get { return _cellLocked; }
			set
			{
				if (value == _cellLocked)
					return;

				_cellLocked = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets whether this XF's Formulas are hidden
		/// </summary>
		public bool FormulaHidden
		{
			get { return _formulaHidden; }
			set
			{
				if (value == _formulaHidden)
					return;

				_formulaHidden = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets whether this XF is a Style XF.
		/// </summary>
		internal bool IsStyleXF
		{
			get { return _isStyleXF; }
			set
			{
				if (value == _isStyleXF)
					return;

			    _isStyleXF = value;

				OnChange();
			}
		}

		#endregion

		#region XF_USED_ATTRIB : Used Attributes
		//--------------------------------------------------------------------------------
		//Each bit describes the validity of a specific group of attributes. In cell XFs a 
		//cleared bit means the attributes of the parent style XF are used (but only if the 
		//attributes are valid there), a set bit means the attributes of this XF are used. 
		//In style XFs a cleared bit means the attribute setting is valid, a set bit means 
		//the attribute should be ignored.
		//                      - excelfileformat.pdf, section 6.115.1 under XF_USED_ATTRIB
		//--------------------------------------------------------------------------------
		//In this implementation True -> Set and False -> Cleared
		//NOTE: Is this how Excel implements Paste w(/o) Formats, Fonts, Borders, etc?
		//NOTE: Is this how Excel implements its Format Painter?

		private bool _useNumber;
		private bool _useFont;
		private bool _useMisc;
		private bool _useBorder;
		private bool _useBackground;
		private bool _useProtection;

		/// <summary>
		/// Gets or sets Flag for number format.
		/// </summary>
		public bool UseNumber
		{
			get { return _useNumber; }
			set
			{
				if (value == _useNumber)
					return;

				_useNumber = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets Flag for font.
		/// </summary>
		public bool UseFont
		{
			get { return _useFont; }
			set
			{
				if (value == _useFont)
					return;

				_useFont = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets Flag for horizontal and vertical alignment, text wrap, indentation, 
		/// orientation, rotation, and text direction.
		/// </summary>
		public bool UseMisc
		{
			get { return _useMisc; }
			set
			{
				if (value == _useMisc)
					return;

				_useMisc = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets Flag for border lines.
		/// </summary>
		public bool UseBorder
		{
			get { return _useBorder; }
			set
			{
				if (value == _useBorder)
					return;

				_useBorder = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets Flag for background area style.
		/// </summary>
		public bool UseBackground
		{
			get { return _useBackground; }
			set
			{
				if (value == _useBackground)
					return;

				_useBackground = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets Flag for cell protection (cell locked and formula hidden).
		/// </summary>
		public bool UseProtection
		{
			get { return _useProtection; }
			set
			{
				if (value == _useProtection)
					return;

				_useProtection = value;
				OnChange();
			}
		}

		#endregion

		#region XF_BORDER_LINES_BG : Border lines, background

		private ushort _leftLineStyle;
		private ushort _rightLineStyle;
		private ushort _topLineStyle;
		private ushort _bottomLineStyle;
		private Color _leftLineColor;
		private Color _rightLineColor;
		private bool _diagonalDescending;
		private bool _diagonalAscending;

		/// <summary>
		/// Gets or sets Left line style.
		/// </summary>
		public ushort LeftLineStyle
		{
			get { return _leftLineStyle; }
			set
			{
				if (value == _leftLineStyle)
					return;

				_leftLineStyle = value;

                //If LineStyle is set (!=0) and LineColour is not set (==0),
                //the Cell Formatting menu will fail to open, though the 
                //formatting will be displayed.  Setting LineColour without a
                //LineStyle does not appear to cause a problem.  This is true
                //with Left, Right, Top, Bottom, DiagonalAscending and
                //DiagonalDescending lines.
//                if (_leftLineStyle == 0)
//                    _leftLineColor = Colors.DefaultLineColo;
//                else if (_leftLineColorIndex == 0)
//                    _leftLineColorIndex = DEFAULT_LINE_COLOUR_INDEX;

				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets Right line style.
		/// </summary>
		public ushort RightLineStyle
		{
			get { return _rightLineStyle; }
			set
			{
				if (value == _rightLineStyle)
					return;

				_rightLineStyle = value;

                //If LineStyle is set (!=0) and LineColour is not set (==0),
                //the Cell Formatting menu will fail to open, though the 
                //formatting will be displayed.  Setting LineColour without a
                //LineStyle does not appear to cause a problem.  This is true
                //with Left, Right, Top, Bottom, DiagonalAscending and
                //DiagonalDescending lines.
//                if (_rightLineStyle == 0)
//                    _rightLineColorIndex = 0;
//                else if (_rightLineColorIndex == 0)
//                    _rightLineColorIndex = DEFAULT_LINE_COLOUR_INDEX;

				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets Top line style.
		/// </summary>
		public ushort TopLineStyle
		{
			get { return _topLineStyle; }
			set
			{
				if (value == _topLineStyle)
					return;

				_topLineStyle = value;

                //If LineStyle is set (!=0) and LineColour is not set (==0),
                //the Cell Formatting menu will fail to open, though the 
                //formatting will be displayed.  Setting LineColour without a
                //LineStyle does not appear to cause a problem.  This is true
                //with Left, Right, Top, Bottom, DiagonalAscending and
                //DiagonalDescending lines.
//                if (_topLineStyle == 0)
//                    _topLineColorIndex = 0;
//                else if (_topLineColorIndex == 0)
//                    _topLineColorIndex = DEFAULT_LINE_COLOUR_INDEX;

                OnChange();
			}
		}

		/// <summary>
		/// Gets or sets Bottom line style.
		/// </summary>
		public ushort BottomLineStyle
		{
			get { return _bottomLineStyle; }
			set
			{
				if (value == _bottomLineStyle)
					return;

				_bottomLineStyle = value;

                //If LineStyle is set (!=0) and LineColour is not set (==0),
                //the Cell Formatting menu will fail to open, though the 
                //formatting will be displayed.  Setting LineColour without a
                //LineStyle does not appear to cause a problem.  This is true
                //with Left, Right, Top, Bottom, DiagonalAscending and
                //DiagonalDescending lines.
//                if (_bottomLineStyle == 0)
//                    _bottomLineColorIndex = 0;
//                else if (_bottomLineColorIndex == 0)
//                    _bottomLineColorIndex = DEFAULT_LINE_COLOUR_INDEX;

				OnChange();
			}
		}

        /// <summary>
		/// Gets or sets Colour for left line.
		/// </summary>
		public Color LeftLineColor
		{
			get { return _leftLineColor; }
			set
			{
                if (value.Equals(_leftLineColor))
					return;

                _leftLineColor = (Color)value.Clone();
				OnChange();
			}
		}

        /// <summary>
		/// Gets or sets Colour for right line.
		/// </summary>
		public Color RightLineColor
		{
			get { return _rightLineColor; }
			set
			{
                if (value.Equals(_rightLineColor))
					return;

                _rightLineColor = (Color)value.Clone();
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets Diagonal line from top left to right bottom.
		/// </summary>
		public bool DiagonalDescending
		{
			get { return _diagonalDescending; }
			set
			{
				if (value == _diagonalDescending)
					return;

				_diagonalDescending = value;
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets Diagonal line from bottom left to top right.  Lines won't show up unless
		/// DiagonalLineStyle (and color - defaults to black, though) is set.
		/// </summary>
		public bool DiagonalAscending
		{
			get { return _diagonalAscending; }
			set
			{
				if (value == _diagonalAscending)
					return;

				_diagonalAscending = value;
				OnChange();
			}
		}

		#endregion

		#region XF_LINE_COLOUR_STYLE_FILL : Line colour, style, and fill

		private Color _topLineColor;
		private Color _bottomLineColor;
		private Color _diagonalLineColor;
		private LineStyle _diagonalLineStyle;
		private ushort _pattern;

        /// <summary>
		/// Gets or sets Colour for top line.
		/// </summary>
		public Color TopLineColor
		{
			get { return _topLineColor; }
			set
			{
                if (value.Equals(_topLineColor))
					return;

                _topLineColor = (Color)value.Clone();
				OnChange();
			}
		}

        /// <summary>
		/// Gets or sets Colour for bottom line.
		/// </summary>
		public Color BottomLineColor
		{
			get { return _bottomLineColor; }
			set
			{
                if (value.Equals(_bottomLineColor))
					return;

                _bottomLineColor = (Color)value.Clone();
				OnChange();
			}
		}

        /// <summary>
		/// Gets or sets Colour for diagonal line.
		/// </summary>
		public Color DiagonalLineColor
		{
			get { return _diagonalLineColor; }
			set
			{
                if (value.Equals(_diagonalLineColor))
					return;

				_diagonalLineColor = (Color)value.Clone();
				OnChange();
			}
		}

        /// <summary>
		/// Gets or sets Diagonal line style.
		/// </summary>
		public LineStyle DiagonalLineStyle
		{
			get { return _diagonalLineStyle; }
			set
			{
				if (value == _diagonalLineStyle)
					return;

				_diagonalLineStyle = value;

                //If LineStyle is set (!=0) and LineColour is not set (==0),
                //the Cell Formatting menu will fail to open, though the 
                //formatting will be displayed.  Setting LineColour without a
                //LineStyle does not appear to cause a problem.  This is true
                //with Left, Right, Top, Bottom, DiagonalAscending and
                //DiagonalDescending lines.
//                if (_diagonalLineStyle == 0)
//                    _diagonalLineColorIndex = 0;
//                else if (_diagonalLineColorIndex == 0)
//                    _diagonalLineColorIndex = DEFAULT_LINE_COLOUR_INDEX;

				OnChange();
			}
		}

        //TODO: Create Standard Fill Pattern constants
		/// <summary>
		/// Gets or sets Fill pattern.
		/// </summary>
		public ushort Pattern
		{
			get { return _pattern; }
			set
			{
				if (value == _pattern)
					return;

				_pattern = value;
				OnChange();
			}
		}

		#endregion

		#region XF_PATTERN : Pattern

		private Color _patternColor;
		private Color _patternBackgroundColor;

        /// <summary>
		/// Gets or sets Colour for pattern colour.
		/// </summary>
		public Color PatternColor
		{
			get { return _patternColor; }
			set
			{
				if (value.Equals(_patternColor))
					return;

			    _patternColor = (Color) value.Clone();
				OnChange();
			}
		}

		/// <summary>
		/// Gets or sets Colour for pattern background.
		/// </summary>
		public Color PatternBackgroundColor
		{
			get { return _patternBackgroundColor; }
			set
			{
                if (value.Equals(_patternBackgroundColor))
					return;

				_patternBackgroundColor = (Color)value.Clone();
				OnChange();
			}
		}

		#endregion

        private void ReadBytes(Bytes bytes, Font font, string format)
        {
            _font = font;
            _format = format;
            ReadXF_3(bytes.Get(4, 2));
        }

        internal Bytes Bytes
		{
			get
			{
				Bytes bytes = new Bytes();

				bytes.Append(BitConverter.GetBytes(_font.ID));
				bytes.Append(BitConverter.GetBytes(_doc.Workbook.Formats.GetFinalID(_format)));
				bytes.Append(XF_3());
				bytes.Append(XF_ALIGN());
				bytes.Append((byte) _rotation);
				bytes.Append(XF_6());
				bytes.Append(XF_USED_ATTRIB());
				bytes.Append(XF_BORDER_LINES_BG());
				bytes.Append(XF_LINE_COLOUR_STYLE_FILL());
				bytes.Append(XF_PATTERN());

                return Record.GetBytes(RID.XF, bytes);
			}
		}

        private void ReadXF_3(Bytes bytes)
        {
            Bytes.Bits bits = bytes.GetBits();
            
            ushort parentStyleXfIndex = bits.Get(4, 12).ToUInt16();
            if (parentStyleXfIndex == 4095)
            {
                //this is a Style XF -- do nothing
            }
            else
            {
                ReadStyleXfIndex = parentStyleXfIndex; //we'll assign the style xf index later using the xfIdxLookups collected by Workbook.ReadBytes()
            }

            ReadXF_TYPE_PROT(bits.Get(4));
        }

		private byte[] XF_3()
		{
			ushort value, styleXfIdx;

            if (_isStyleXF)
                styleXfIdx = 4095;
            else
                //NOTE: This forces all cell (non-Style) XF's to use Style XF with index 0;
                styleXfIdx = 0; // TODO : Change this to implement Style XF's

			value = 0;
			value += XF_TYP_PROT();
			value += (ushort)(styleXfIdx * 16);
			return BitConverter.GetBytes(value);
		}

        private void ReadXF_TYPE_PROT(Bytes.Bits bits)
        {
            
        }

		private ushort XF_TYP_PROT()
		{
			ushort value = 0;
			if (_cellLocked)
				value += 1;
			if (_formulaHidden)
				value += 2;
			if (_isStyleXF)
				value += 4;
			return value;
		}

		private byte XF_ALIGN()
		{
			byte value = 0;
			value += (byte) ((byte)_verticalAlignment * 16);
			if (_textWrapRight)
				value += 8;
			value += (byte)_horizontalAlignment;
			return value;
		}

		private byte XF_6()
		{
			ushort value = 0;
			value += (ushort) ((ushort) _textDirection * 64);
			if (_shrinkToCell)
				value += 16;
			value += _indentLevel;
			return (byte) value;
		}

		private byte XF_USED_ATTRIB()
		{
			ushort value = 0;
			if (_isStyleXF ? !_useNumber : _useNumber)
				value += 1;
			if (_isStyleXF ? !_useFont : _useFont)
				value += 2;
			if (_isStyleXF ? !_useMisc : _useMisc)
				value += 4;
			if (_isStyleXF ? !_useBorder : _useBorder)
				value += 8;
			if (_isStyleXF ? !_useBackground : _useBackground)
				value += 16;
			if (_isStyleXF ? !_useProtection : _useProtection)
				value += 32;
			value *= 4;
			return (byte) value;
		}

		//OPTIM: Use bit-shifting instead of Math.Pow
		private byte[] XF_BORDER_LINES_BG()
		{
			uint value = 0;
			value += _leftLineStyle;
			value += (uint)(Math.Pow(2, 4) * _rightLineStyle);
			value += (uint) (Math.Pow(2, 8) * _topLineStyle);
			value += (uint) (Math.Pow(2, 12) * _bottomLineStyle);
			value += (uint) (Math.Pow(2, 16) * _doc.Workbook.Palette.GetIndex(_leftLineColor));
            value += (uint)(Math.Pow(2, 23) * _doc.Workbook.Palette.GetIndex(_rightLineColor));
			if (_diagonalDescending)
				value += (uint) Math.Pow(2, 30);
			if (_diagonalAscending)
				value += (uint) Math.Pow(2, 31);
			return BitConverter.GetBytes(value);
		}

        //OPTIM: Use bit-shifting instead of Math.Pow
        private byte[] XF_LINE_COLOUR_STYLE_FILL()
		{
			uint value = 0;
            value += _doc.Workbook.Palette.GetIndex(_topLineColor);
            value += (uint)(Math.Pow(2, 7) * _doc.Workbook.Palette.GetIndex(_bottomLineColor));
            value += (uint)(Math.Pow(2, 14) * _doc.Workbook.Palette.GetIndex(_diagonalLineColor));
			value += (uint) (Math.Pow(2, 21) * (ushort)_diagonalLineStyle);
			value += (uint) (Math.Pow(2, 26) * _pattern);
			return BitConverter.GetBytes(value);
		}

		private byte[] XF_PATTERN()
		{
			ushort value = 0;
			value += _doc.Workbook.Palette.GetIndex(_patternColor);
			value += (ushort) (Math.Pow(2, 7) * _doc.Workbook.Palette.GetIndex(_patternBackgroundColor));
			return BitConverter.GetBytes(value);
		}

        internal bool Equals(XF that)
		{
			if (_horizontalAlignment != that._horizontalAlignment) return false;
			if (_textWrapRight != that._textWrapRight) return false;
			if (_verticalAlignment != that._verticalAlignment) return false;
			if (_rotation != that._rotation) return false;
			if (_indentLevel != that._indentLevel) return false;
			if (_shrinkToCell != that._shrinkToCell) return false;
			if (_textDirection != that._textDirection) return false;
			if (_cellLocked != that._cellLocked) return false;
			if (_formulaHidden != that._formulaHidden) return false;
			if (_isStyleXF != that._isStyleXF) return false;
			if (_useNumber != that._useNumber) return false;
			if (_useFont != that._useFont) return false;
			if (_useMisc != that._useMisc) return false;
			if (_useBorder != that._useBorder) return false;
			if (_useBackground != that._useBackground) return false;
			if (_useProtection != that._useProtection) return false;
			if (_leftLineStyle != that._leftLineStyle) return false;
			if (_rightLineStyle != that._rightLineStyle) return false;
			if (_topLineStyle != that._topLineStyle) return false;
			if (_bottomLineStyle != that._bottomLineStyle) return false;
			if (!_leftLineColor.Equals(that._leftLineColor)) return false;
			if (!_rightLineColor.Equals(that._rightLineColor)) return false;
			if (_diagonalDescending != that._diagonalDescending) return false;
			if (_diagonalAscending != that._diagonalAscending) return false;
			if (!_topLineColor.Equals(that._topLineColor)) return false;
			if (!_bottomLineColor.Equals(that._bottomLineColor)) return false;
			if (!_diagonalLineColor.Equals(that._diagonalLineColor)) return false;
			if (_diagonalLineStyle != that._diagonalLineStyle) return false;
			if (_pattern != that._pattern) return false;
			if (!_patternColor.Equals(that._patternColor)) return false;
			if (!_patternBackgroundColor.Equals(that._patternBackgroundColor)) return false;

			if (!Font.Equals(that.Font)) return false;
			if (!Format.Equals(that.Format)) return false;
			if (!Style.Equals(that.Style)) return false;

            //if (_targetObject != that._targetObject) return false;

			return true;
		}

        internal ushort Id
		{
			get
			{
				if (_id == null)
					_id = _doc.Workbook.XFs.Add(this);
				return (ushort)_id;
			}
		}

		#region ICloneable members

        /// <summary>
        /// Creates a duplicate instance of this XF objet.
        /// </summary>
        /// <returns>A duplicate instance of this XF object.</returns>
		public object Clone()
		{
			XF clone = new XF(_doc);

			clone.Font = (Font)_font.Clone();
			clone.Format = (string) _format.Clone();

			if (!IsStyleXF)
				clone.Style = (Style) _style.Clone();

			clone.HorizontalAlignment = HorizontalAlignment;
			clone.TextWrapRight = TextWrapRight;
			clone.VerticalAlignment = VerticalAlignment;
			clone.Rotation = Rotation;
			clone.IndentLevel = IndentLevel;
			clone.ShrinkToCell = ShrinkToCell;
			clone.TextDirection = TextDirection;
			clone.CellLocked = CellLocked;
			clone.FormulaHidden = FormulaHidden;
			clone.IsStyleXF = IsStyleXF;
			clone.UseNumber = UseNumber;
			clone.UseFont = UseFont;
			clone.UseMisc = UseMisc;
			clone.UseBorder = UseBorder;
			clone.UseBackground = UseBackground;
			clone.UseProtection = UseProtection;
			clone.LeftLineStyle = LeftLineStyle;
			clone.RightLineStyle = RightLineStyle;
			clone.TopLineStyle = TopLineStyle;
			clone.BottomLineStyle = BottomLineStyle;
			clone.LeftLineColor = LeftLineColor;
			clone.RightLineColor = RightLineColor;
			clone.DiagonalDescending = DiagonalDescending;
			clone.DiagonalAscending = DiagonalAscending;
			clone.TopLineColor = TopLineColor;
			clone.BottomLineColor = BottomLineColor;
			clone.DiagonalLineColor = DiagonalLineColor;
			clone.DiagonalLineStyle = DiagonalLineStyle;
			clone.Pattern = Pattern;
			clone.PatternColor = PatternColor;
			clone.PatternBackgroundColor = PatternBackgroundColor;

            clone.Target = Target;

			return clone;
		}

		#endregion
	}
}
