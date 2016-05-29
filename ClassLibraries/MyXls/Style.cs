using System;
using org.in2bits.MyXls.ByteUtil;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// Represents a Style in Excel.
    /// </summary>
	public class Style : ICloneable
	{
        //TODO: Finish Style implementation
        private XlsDocument _doc;
        private XF _xf;

        private bool _isInitializing = false;

		private ushort? _id = null;

        internal Style(XlsDocument doc, XF xf)
		{
            _isInitializing = true;

			_doc = doc;
            _xf = xf;

            _isInitializing = false;
		}

		private void OnChange()
		{
            if (_isInitializing)
                return;

			_id = null;
		    _xf.OnChange();
		}

        /// <summary>
        /// Gets the ID value of this Style object.
        /// </summary>
		public ushort ID
		{
			get 
			{
				if (_id == null)
					_id = _doc.Workbook.Styles.Add(this);

				return (ushort)_id;
			}
		}

        /// <summary>
        /// Returns whether a given Style object is value-equal to this Style object.
        /// </summary>
        /// <param name="that">Another Style object to compare to this Style object.</param>
        /// <returns>true if that Style object is value-equal to this Style object,
        /// false otherwise.</returns>
		public bool Equals(Style that)
		{
			//TODO: Add comparisons when Class members are added
			return true;
		}

	    internal Bytes Bytes
		{
			get
			{
				throw new NotImplementedException();
//				Doc.GetRecBin(RID.STYLE, new byte[] {0x10, 0x80, 0x03, 0xFF});
			}
		}

		#region ICloneable members

        /// <summary>
        /// Returns a new Style object which is value-equal to this Style object.
        /// </summary>
        /// <returns>A new Style object which is value-equal to this Style object.</returns>
		public object Clone()
		{
			Style clone = new Style(this._doc, this._xf);

			//TODO: Add as properties are added to class.
			clone._doc = this._doc;

			return clone;
		}

		#endregion
	}
}
