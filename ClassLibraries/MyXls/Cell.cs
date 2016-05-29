using System;
using System.Collections.Generic;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// A single cell on or to be added to a worksheet.
    /// </summary>
	public class Cell : IXFTarget
	{
        private Worksheet _worksheet;

		private object _value;
		private CellTypes _type = CellTypes.Null;
		private ushort _row = 0;
		private ushort _column = 0;
		private CellCoordinate _coordinate;
		private int _xfIdx = -1;

        internal Cell(Worksheet worksheet)
        {
            _worksheet = worksheet;
        }

	    //NOTE: Could do this another way (used by Worksheet.AddCell)
        /// <summary>
        /// Gets or sets the CellCoordinate of this Cell object.
        /// </summary>
		public CellCoordinate Coordinate
		{
			get { return _coordinate; }
			set 
            {
                _coordinate = value;
                _row = value.Row;
                _column = value.Column;
            }
		}

        /// <summary>
        /// Gets the Row value of this Cell (1-based).
        /// </summary>
		public ushort Row
		{
			get { return _row; }
		}

        /// <summary>
        /// Gets the Column value of this Cell (1-based).
        /// </summary>
		public ushort Column
		{
			get { return _column; }
		}

        /// <summary>
        /// Gets the type of this Cell.
        /// </summary>
		public CellTypes Type
		{
			get { return _type; }
		}

        /// <summary>
        /// Gets or sets the Value in this Cell.
        /// </summary>
		public object Value
		{
			get
			{
                if (_type == CellTypes.Text && !(_value is string))
                    return _worksheet.Document.Workbook.SharedStringTable.GetString((uint) _value);
                else
			        return _value;
			}
			set
			{
                if (value == null)
                    _type = CellTypes.Null;
                else if (value is bool)
                    _type = CellTypes.Integer;
                else if (value is string)
                    _type = CellTypes.Text;
                else if (value is short)
                    _type = CellTypes.Integer;
                else if (value is int)
                    _type = CellTypes.Integer;
                else if (value is long)
                    _type = CellTypes.Integer;
                else if (value is Single)
                    _type = CellTypes.Float;
                else if (value is double)
                    _type = CellTypes.Float;
                else if (value is decimal)
                    _type = CellTypes.Float;
                else if (value is DateTime)
                {
                    value = ((DateTime)value).ToOADate();
                    _type = CellTypes.Float;
                }
                else
                    throw new NotSupportedException(string.Format("values of type {0}", value.GetType().Name));

                if (_type == CellTypes.Text && (value as string).Length > BIFF8.MaxCharactersPerCell)
                    throw new ApplicationException(
                        string.Format("Text in Cell Row {0} Col {1} is longer than maximum allowed {2}", Row, Column,
                                      BIFF8.MaxCharactersPerCell));

                if (_type == CellTypes.Text && _worksheet.Document.Workbook.ShareStrings)
                    _value = _worksheet.Document.Workbook.SharedStringTable.Add((string) value);
                else
			        _value = value;
			}
		}

        internal XF ExtendedFormat
		{
			get
			{
			    XF xf;
                if (_xfIdx == -1)
                    xf = _worksheet.Document.Workbook.XFs.DefaultUserXF;
                else
                    xf = _worksheet.Document.Workbook.XFs[_xfIdx];
			    xf.Target = this;
			    return xf;
			}
			set { _xfIdx = (value == null ? -1 : value.Id); }
		}

        internal void SetXfIndex(int xfIndex)
        {
            _xfIdx = xfIndex;
        }

        /// <summary>
        /// Gets or sets the Font for this Cell.
        /// </summary>
        public Font Font
        {
            get { return ExtendedFormat.Font; }
            set
            {
                XF xf = ExtendedFormat;
                xf.Font = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets the Format for this Cell.
        /// </summary>
        public string Format
        {
            get { return ExtendedFormat.Format; }
            set
            {
                XF xf = ExtendedFormat;
                xf.Format = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets the Parent Style XF (absent on Style XF's).
        /// </summary>
        public Style Style
        {
            get { return ExtendedFormat.Style; }
            set
            {
                XF xf = ExtendedFormat;
                xf.Style = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets the HorizontalAlignments value for this Cell.
        /// </summary>
        public HorizontalAlignments HorizontalAlignment
        {
            get { return ExtendedFormat.HorizontalAlignment; }
            set
            {
                XF xf = ExtendedFormat;
                xf.HorizontalAlignment = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets whether Text is wrapped at right border.
        /// </summary>
        public bool TextWrapRight
        {
            get { return ExtendedFormat.TextWrapRight; }
            set
            {
                XF xf = ExtendedFormat;
                xf.TextWrapRight = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets the VerticalAlignments value for this Cell.
        /// </summary>
        public VerticalAlignments VerticalAlignment
        {
            get { return ExtendedFormat.VerticalAlignment; }
            set
            {
                XF xf = ExtendedFormat;
                xf.VerticalAlignment = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets the Text rotation angle for this Cell (-360 or 0?) to 360.
        /// </summary>
        public short Rotation
        {
            get { return ExtendedFormat.Rotation; }
            set
            {
                XF xf = ExtendedFormat;
                xf.Rotation = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets the IndentLevel for this Cell.
        /// </summary>
        public ushort IndentLevel
        {
            get { return ExtendedFormat.IndentLevel; }
            set
            {
                XF xf = ExtendedFormat;
                xf.IndentLevel = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets whether to Shrink content to fit into cell for this Cell.
        /// </summary>
        public bool ShrinkToCell
        {
            get { return ExtendedFormat.ShrinkToCell; }
            set
            {
                XF xf = ExtendedFormat;
                xf.ShrinkToCell = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets the TextDirections value for this Cell (BIFF8X only).
        /// </summary>
        public TextDirections TextDirection
        {
            get { return ExtendedFormat.TextDirection; }
            set
            {
                XF xf = ExtendedFormat;
                xf.TextDirection = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets whether this Cell is locked.
        /// </summary>
        public bool Locked
        {
            get { return ExtendedFormat.CellLocked; }
            set
            {
                XF xf = ExtendedFormat;
                xf.CellLocked = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets whether this Cell's Formula is hidden
        /// </summary>
        public bool FormulaHidden
        {
            get { return ExtendedFormat.FormulaHidden; }
            set
            {
                XF xf = ExtendedFormat;
                xf.FormulaHidden = value;
                _xfIdx = xf.Id;
            }
        }

/*        /// <summary>
        /// Gets or sets whether this XF is a Style XF.
        /// </summary>
        public bool IsStyleXF
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }*/

        //TODO: Should UseNumber, UseFont, etc., be exposed to the user? or calculated internally
        /// <summary>
        /// Gets or sets Flag for number format.
        /// </summary>
        public bool UseNumber
        {
            get { return ExtendedFormat.UseNumber; }
            set
            {
                XF xf = ExtendedFormat;
                xf.UseNumber = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Flag for font.
        /// </summary>
        public bool UseFont
        {
            get { return ExtendedFormat.UseFont; }
            set
            {
                XF xf = ExtendedFormat;
                xf.UseFont = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Flag for horizontal and vertical alignment, text wrap, indentation, 
        /// orientation, rotation, and text direction.
        /// </summary>
        public bool UseMisc
        {
            get { return ExtendedFormat.UseMisc; }
            set
            {
                XF xf = ExtendedFormat;
                xf.UseMisc = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Flag for border lines.
        /// </summary>
        public bool UseBorder
        {
            get { return ExtendedFormat.UseBorder; }
            set
            {
                XF xf = ExtendedFormat;
                xf.UseBorder = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Flag for background area style.
        /// </summary>
        public bool UseBackground
        {
            get { return ExtendedFormat.UseBackground; }
            set
            {
                XF xf = ExtendedFormat;
                xf.UseBackground = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Flag for protection (cell locked and formula hidden).
        /// </summary>
        public bool UseProtection
        {
            get { return ExtendedFormat.UseProtection; }
            set
            {
                XF xf = ExtendedFormat;
                xf.UseProtection = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Left line style.
        /// </summary>
        public ushort LeftLineStyle
        {
            get { return ExtendedFormat.LeftLineStyle; }
            set
            {
                XF xf = ExtendedFormat;
                xf.LeftLineStyle = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Right line style.
        /// </summary>
        public ushort RightLineStyle
        {
            get { return ExtendedFormat.RightLineStyle; }
            set
            {
                XF xf = ExtendedFormat;
                xf.RightLineStyle = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Top line style.
        /// </summary>
        public ushort TopLineStyle
        {
            get { return ExtendedFormat.TopLineStyle; }
            set
            {
                XF xf = ExtendedFormat;
                xf.TopLineStyle = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Bottom line style.
        /// </summary>
        public ushort BottomLineStyle
        {
            get { return ExtendedFormat.BottomLineStyle; }
            set
            {
                XF xf = ExtendedFormat;
                xf.BottomLineStyle = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Colour for left line.
        /// </summary>
        public Color LeftLineColor
        {
            get { return ExtendedFormat.LeftLineColor; }
            set
            {
                XF xf = ExtendedFormat;
                xf.LeftLineColor = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Colour for right line.
        /// </summary>
        public Color RightLineColor
        {
            get { return ExtendedFormat.RightLineColor; }
            set
            {
                XF xf = ExtendedFormat;
                xf.RightLineColor = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Diagonal line from top left to right bottom.
        /// </summary>
        public bool DiagonalDescending
        {
            get { return ExtendedFormat.DiagonalDescending; }
            set
            {
                XF xf = ExtendedFormat;
                xf.DiagonalDescending = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Diagonal line from bottom left to top right.  Lines won't show up unless
        /// DiagonalLineStyle (and color - defaults to black, though) is set.
        /// </summary>
        public bool DiagonalAscending
        {
            get { return ExtendedFormat.DiagonalAscending; }
            set
            {
                XF xf = ExtendedFormat;
                xf.DiagonalAscending = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Colour for top line.
        /// </summary>
        public Color TopLineColor
        {
            get { return ExtendedFormat.TopLineColor; }
            set
            {
                XF xf = ExtendedFormat;
                xf.TopLineColor = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Colour for bottom line.
        /// </summary>
        public Color BottomLineColor
        {
            get { return ExtendedFormat.BottomLineColor; }
            set
            {
                XF xf = ExtendedFormat;
                xf.BottomLineColor = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Colour for diagonal line.
        /// </summary>
        public Color DiagonalLineColor
        {
            get { return ExtendedFormat.DiagonalLineColor; }
            set
            {
                XF xf = ExtendedFormat;
                xf.DiagonalLineColor = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Diagonal line style.
        /// </summary>
        public LineStyle DiagonalLineStyle
        {
            get { return ExtendedFormat.DiagonalLineStyle; }
            set
            {
                XF xf = ExtendedFormat;
                xf.DiagonalLineStyle = value;
                _xfIdx = xf.Id;
            }
        }

        //TODO: Create Standard Fill Pattern constants
        /// <summary>
        /// Gets or sets Fill pattern.
        /// </summary>
        public ushort Pattern
        {
            get { return ExtendedFormat.Pattern; }
            set
            {
                XF xf = ExtendedFormat;
                xf.Pattern = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Colour for pattern.
        /// </summary>
        public Color PatternColor
        {
            get { return ExtendedFormat.PatternColor; }
            set
            {
                XF xf = ExtendedFormat;
                xf.PatternColor = value;
                _xfIdx = xf.Id;
            }
        }

        /// <summary>
        /// Gets or sets Colour for pattern background.
        /// </summary>
        public Color PatternBackgroundColor
        {
            get { return ExtendedFormat.PatternBackgroundColor; }
            set
            {
                XF xf = ExtendedFormat;
                xf.PatternBackgroundColor = value;
                _xfIdx = xf.Id;
            }
        }

        internal Bytes Bytes
		{
			get
			{
				if (_xfIdx < 0)
				    _xfIdx = ExtendedFormat.Id;
                
                switch (Type)
				{
                    //NOTE: Optimization - cache the values for all these (RK, NUMBER)
					case CellTypes.Integer:
						return RK(false);
					case CellTypes.Float:
				        {
				            double dbl = Convert.ToDouble(_value);
				            bool div100 = false;
				            double tmp = dbl;
                            tmp *= 10; //doing a single (tmp *= 100) operation introduces float inaccuracies (1.1 * 10 * 10 = 110.0, whereas 1.1 * 100 = 110.000000000001)
                            tmp *= 10;
                            if (Math.Floor(tmp) == tmp)
                                div100 = true;

                            //per excelfileformat.pdf sec.5.83 (RK):
                            //If a floating-point value cannot be encoded to an RK value, a NUMBER
                            //record (5.69) will be written.
				            Bytes rk = RK(div100);
				            if (DecodeRKFloat(rk.GetBits(), div100) == dbl)
				                return rk;
                            else
                                return NUMBER();
                        }
					case CellTypes.Text:
				        {
                            if (_value is string)
                                return LABEL();
                            else
				                return LABELSST();
				        }

					case CellTypes.Formula:
					case CellTypes.Error:
						throw new NotSupportedException(string.Format("CellType {0}", Type));

					case CellTypes.Null:
				        return BLANK();

					default:
						throw new Exception(string.Format("unexpected CellTypes {0}", Type));
				}
			}
		}

        private Bytes BLANK()
        {
            Bytes blank = new Bytes();

            //Index to row
            blank.Append(BitConverter.GetBytes((ushort)(Row - 1)));

            //Index to column
            blank.Append(BitConverter.GetBytes((ushort)(Column - 1)));

            //Index to XF record
            blank.Append(BitConverter.GetBytes((ushort) _xfIdx));

            return Record.GetBytes(RID.BLANK, blank);
        }

        private Bytes LABEL()
		{
			Bytes label = new Bytes();

            label.Append(LABELBase());

			//Unicode string, 16-bit string length
			label.Append(XlsDocument.GetUnicodeString((string)Value ?? string.Empty, 16));

		    return Record.GetBytes(RID.LABEL, label);
		}

        private Bytes LABELSST()
        {
            Bytes labelsst = new Bytes();

            labelsst.Append(LABELBase());

            //Index of string value in Shared String Table
            labelsst.Append(BitConverter.GetBytes((uint) _value));

            return Record.GetBytes(RID.LABELSST, labelsst);
        }

        private Bytes LABELBase()
        {
            Bytes labelBase = new Bytes();

            //Index to row
            labelBase.Append(BitConverter.GetBytes((ushort)(Row - 1)));

            //Index to column
            labelBase.Append(BitConverter.GetBytes((ushort)(Column - 1)));

            //Index to XF record
            labelBase.Append(BitConverter.GetBytes((ushort)_xfIdx));

            return labelBase;
        }

		private Bytes RK(bool trueFalse)
		{
			Bytes rk = new Bytes();

			//Index to row
			rk.Append(BitConverter.GetBytes((ushort) (Row - 1)));

			//Index to column
			rk.Append(BitConverter.GetBytes((ushort) (Column - 1)));

			//Index to XF record
			rk.Append(BitConverter.GetBytes((ushort) _xfIdx));

			//RK Value
			if (Type == CellTypes.Integer)
				rk.Append(RKIntegerValue(Value, trueFalse));
			else if (Type == CellTypes.Float)
				rk.Append(RKDecimalValue(Value, trueFalse));

		    return Record.GetBytes(RID.RK, rk);
		}

		private Bytes NUMBER()
		{
		    double value = Convert.ToDouble(Value);

			Bytes number = new Bytes();

			//Index to row
			number.Append(BitConverter.GetBytes((ushort) (Row - 1)));

			//Index to column
			number.Append(BitConverter.GetBytes((ushort) (Column - 1)));

			//Index to XF record
			number.Append(BitConverter.GetBytes((ushort) _xfIdx));

			//NUMBER Value
			number.Append(NUMBERVal(value));

		    return Record.GetBytes(RID.NUMBER, number);
		}

        private static Bytes RKDecimalValue(object val, bool div100)
        {
            double rk = Convert.ToDouble(val);

            if (div100)
            {
                rk *= 10; //doing a single (tmp *= 100) operation introduces float inaccuracies (1.1 * 10 * 10 = 110.0, whereas 1.1 * 100 = 110.000000000001)
                rk *= 10;
            }

            Bytes bytes = new Bytes(BitConverter.GetBytes(rk));
            List<bool> bitList = new List<bool>();
            bitList.Add(div100);
            bitList.Add(false);
            bitList.AddRange(bytes.GetBits().Get(34, 30).Values);

            return (new Bytes.Bits(bitList.ToArray())).GetBytes();
        }

        private static Bytes RKIntegerValue(object val, bool div100)
        {
            int rk = Convert.ToInt32(val);
            if (rk < -536870912 || rk >= 536870912)
                throw new ArgumentOutOfRangeException("val", string.Format("{0}: must be between -536870912 and 536870911", rk));

            unchecked
            {
                rk = rk << 2;
            }

            if (div100)
                rk += 1;
            rk += 2;

            byte[] bytes = BitConverter.GetBytes(rk);

            return new Bytes(bytes);
        }

        private static Bytes NUMBERVal(double val)
		{
            return new Bytes(BitConverter.GetBytes(val));
		}

        #region IXFTarget Members

        ///<summary>
        /// (For internal use only) - Updates this Cell's XF id from the provided XF.
        ///</summary>
        ///<param name="fromXF">The XF from which to calculate this Cell's XF id.</param>
        public void UpdateId(XF fromXF)
        {
            _xfIdx = fromXF.Id;
        }

        #endregion

        internal void SetValue(byte[] rid, Bytes data)
        {
            if (rid == RID.RK)
                DecodeRK(data);
            else if (rid == RID.LABEL)
                DecodeLABEL(data);
            else if (rid == RID.LABELSST)
                DecodeLABELSST(data);
            else if (rid == RID.NUMBER)
                DecodeNUMBER(data);
            else
                throw new ApplicationException(string.Format("Unsupported RID {0}", RID.Name(rid)));
        }

        internal void SetFormula(Bytes data, Record stringRecord)
        {
            DecodeFORMULA(data, stringRecord);
        }

        private void DecodeFORMULA(Bytes data, Record stringRecord)
        {
            //TODO: Read in formula properties
            if (stringRecord != null)
                _value = UnicodeBytes.Read(stringRecord.Data, 16);
            _type = CellTypes.Formula;
        }

        private void DecodeNUMBER(Bytes data)
        {
            _value = BitConverter.ToDouble(data.ByteArray, 0);
            _type = CellTypes.Float;
        }

        private void DecodeLABELSST(Bytes data)
        {
            _value = BitConverter.ToUInt32(data.ByteArray, 0);
            _type = CellTypes.Text;
        }

        private void DecodeLABEL(Bytes data)
        {
            _value = UnicodeBytes.Read(data, 16);
            _type = CellTypes.Text;
        }

        private void DecodeRK(Bytes bytes)
        {
            Bytes.Bits bits = bytes.GetBits();
            bool div100 = bits.Values[0];
            bool isInt = bits.Values[1];
            if (isInt)
            {
				Value = DecodeRKInt(bits, div100);
				_type = (Value is Int32) ? CellTypes.Integer : CellTypes.Float;
            }
            else
            {
                Value = DecodeRKFloat(bits, div100);
                _type = CellTypes.Float;
            }
        }

        private static double DecodeRKFloat(Bytes.Bits bits, bool div100)
        {
            Bytes.Bits floatBits = bits.Get(2, 30);
            floatBits.Prepend(false); //right-shift to full 8 bytes
            floatBits.Prepend(false);
            byte[] floatBytes = new byte[8];
            floatBits.GetBytes().ByteArray.CopyTo(floatBytes, 4);
            byte[] double1Bytes = BitConverter.GetBytes((double) 1);
            double val = BitConverter.ToDouble(floatBytes, 0);
            if (div100)
            {
            	val /= 100.0;
            }
            return val;
        }

		private static object DecodeRKInt(Bytes.Bits bits, bool div100)
		{
			object val = bits.Get(2, 30).ToInt32();
			if (div100)
			{
				val = Convert.ToDouble(val) / 100.0;
			}
			return val;
		}
	}
}
