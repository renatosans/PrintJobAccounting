using System;
using System.Collections.Generic;
using System.Text;

namespace org.in2bits.MyXls
{
    /// <summary>
    /// This is not done by any stretch of the imagination.  What a can of worms!
    /// </summary>
    internal class XlsText
    {
        private string _text = null;

        public XlsText(string text)
        {
            _text = text;
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        internal class FormattingRun
        {
            private ushort _xfId = 0;
            private ushort _startOffset = 0;

            public FormattingRun(XF xf, ushort startOffset)
            {
                _xfId = xf.Id;
                _startOffset = startOffset;
            }

            public XF ExtendedFormat
            {
                //NOTE: Probably should be able to have a getter here - if we require a Workbook or XlsDocument ref in the constructor
                set { _xfId = value.Id; }
            }

            public ushort StartOffset
            {
                get { return _startOffset; }
                set { _startOffset = value; }
            }
        }
    }
}
