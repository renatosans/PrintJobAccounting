using System;
using System.Collections.Generic;
using System.Reflection;

namespace org.in2bits.MyXls
{
    internal static class RID
	{
        public static readonly byte[] Empty = new byte[2] { 0x00, 0x00 };

        internal static readonly byte[] ARRAY = new byte[2] { 0x21, 0x02 };
        internal static readonly byte[] BACKUP = new byte[2] { 0x40, 0x00 };
        internal static readonly byte[] BITMAP = new byte[2] { 0xE9, 0x00 };
        internal static readonly byte[] BLANK = new byte[2] { 0x01, 0x02 };
        internal static readonly byte[] BOF = new byte[2] { 0x09, 0x08 };
        internal static readonly byte[] BOOKBOOL = new byte[2] { 0xDA, 0x00 };
        internal static readonly byte[] BOOLERR = new byte[2] { 0x05, 0x02 };
        internal static readonly byte[] BOTTOMMARGIN = new byte[2] { 0x29, 0x00 };
        internal static readonly byte[] BOUNDSHEET = new byte[2] { 0x85, 0x00 };
        internal static readonly byte[] CALCCOUNT = new byte[2] { 0x0C, 0x00 };
        internal static readonly byte[] CALCMODE = new byte[2] { 0x0D, 0x00 };
        internal static readonly byte[] CODEPAGE = new byte[2] { 0x42, 0x00 };
        internal static readonly byte[] COLINFO = new byte[2] { 0x7D, 0x00 };
        internal static readonly byte[] CONDFMT = new byte[2] { 0xB0, 0x01 };
        internal static readonly byte[] CONTINUE = new byte[2] { 0x3C, 0x00 };
        internal static readonly byte[] COUNTRY = new byte[2] { 0x8C, 0x00 };
        internal static readonly byte[] CRN = new byte[2] { 0x5A, 0x00 };
        internal static readonly byte[] DATEMODE = new byte[2] { 0x22, 0x00 };
        internal static readonly byte[] DBCELL = new byte[2] { 0xD7, 0x00 };
        internal static readonly byte[] DCONREF = new byte[2] { 0x51, 0x00 };
        internal static readonly byte[] DEFAULTROWHEIGHT = new byte[2] { 0x25, 0x02 };
        internal static readonly byte[] DEFCOLWIDTH = new byte[2] { 0x55, 0x00 };
        internal static readonly byte[] DELTA = new byte[2] { 0x10, 0x00 };
        internal static readonly byte[] DIMENSIONS = new byte[2] { 0x00, 0x02 };
        internal static readonly byte[] DSF = new byte[2] { 0x61, 0x01 };
        internal static readonly byte[] DV = new byte[2] { 0xBE, 0x01 };
        internal static readonly byte[] DVAL = new byte[2] { 0xB2, 0x01 };
        internal static readonly byte[] EOF = new byte[2] { 0x0A, 0x00 };
        internal static readonly byte[] EXTERNNAME = new byte[2] { 0x23, 0x00 };
        internal static readonly byte[] EXTERNSHEET = new byte[2] { 0x17, 0x00 };
        internal static readonly byte[] EXTSST = new byte[2] { 0xFF, 0x00 };
        internal static readonly byte[] FILEPASS = new byte[2] { 0x2F, 0x00 };
        internal static readonly byte[] FILESHARING = new byte[2] { 0x5B, 0x00 };
        internal static readonly byte[] FONT = new byte[2] { 0x31, 0x00 };
        internal static readonly byte[] FOOTER = new byte[2] { 0x15, 0x00 };
        internal static readonly byte[] FORMAT = new byte[2] { 0x1E, 0x04 };
        internal static readonly byte[] FORMULA = new byte[2] { 0x06, 0x00 };
        internal static readonly byte[] GRIDSET = new byte[2] { 0x82, 0x00 };
        internal static readonly byte[] GUTS = new byte[2] { 0x80, 0x00 };
        internal static readonly byte[] HCENTER = new byte[2] { 0x83, 0x00 };
        internal static readonly byte[] HEADER = new byte[2] { 0x14, 0x00 };
        internal static readonly byte[] HIDEOBJ = new byte[2] { 0x8D, 0x00 };
        internal static readonly byte[] HLINK = new byte[2] { 0xB8, 0x01 };
        internal static readonly byte[] HORIZONTALPAGEBREAKS = new byte[2] { 0x1B, 0x00 };
        internal static readonly byte[] INDEX = new byte[2] { 0x0B, 0x02 };
        internal static readonly byte[] ITERATION = new byte[2] { 0x11, 0x00 };
        internal static readonly byte[] LABEL = new byte[2] { 0x04, 0x02 };
        internal static readonly byte[] LABELRANGES = new byte[2] { 0x5F, 0x01 };
        internal static readonly byte[] LABELSST = new byte[2] { 0xFD, 0x00 };
        internal static readonly byte[] LEFTMARGIN = new byte[2] { 0x26, 0x00 };
        internal static readonly byte[] MERGEDCELLS = new byte[2] { 0xE5, 0x00 };
        internal static readonly byte[] MULBLANK = new byte[2] { 0xBE, 0x00 };
        internal static readonly byte[] MULRK = new byte[2] { 0xBD, 0x00 };
        internal static readonly byte[] NAME = new byte[2] { 0x18, 0x00 };
        internal static readonly byte[] NOTE = new byte[2] { 0x1C, 0x00 };
        internal static readonly byte[] NUMBER = new byte[2] { 0x03, 0x02 };
        internal static readonly byte[] OBJECTPROTECT = new byte[2] { 0x63, 0x00 };
        internal static readonly byte[] PALETTE = new byte[2] { 0x92, 0x00 };
        internal static readonly byte[] PANE = new byte[2] { 0x41, 0x00 };
        internal static readonly byte[] PASSWORD = new byte[2] { 0x13, 0x00 };
        internal static readonly byte[] PHONETIC = new byte[2] { 0xEF, 0x00 };
        internal static readonly byte[] PRECISION = new byte[2] { 0x0E, 0x00 };
        internal static readonly byte[] PRINTGRIDLINES = new byte[2] { 0x2B, 0x00 };
        internal static readonly byte[] PRINTHEADERS = new byte[2] { 0x2A, 0x00 };
        internal static readonly byte[] PROTECT = new byte[2] { 0x12, 0x00 };
        internal static readonly byte[] QUICKTIP = new byte[2] { 0x00, 0x08 };
        internal static readonly byte[] RANGEPROTECTION = new byte[2] { 0x68, 0x08 };
        internal static readonly byte[] REFMODE = new byte[2] { 0x0F, 0x00 };
        internal static readonly byte[] RIGHTMARGIN = new byte[2] { 0x27, 0x00 };
        internal static readonly byte[] RK = new byte[2] { 0x7E, 0x02 };
        internal static readonly byte[] RSTRING = new byte[2] { 0xD6, 0x00 };
        internal static readonly byte[] ROW = new byte[2] { 0x08, 0x02 };
        internal static readonly byte[] SAVERECALC = new byte[2] { 0x5F, 0x00 };
        internal static readonly byte[] SCENPROTECT = new byte[2] { 0xDD, 0x00 };
        internal static readonly byte[] SCL = new byte[2] { 0xA0, 0x00 };
        internal static readonly byte[] SELECTION = new byte[2] { 0x1D, 0x00 };
        internal static readonly byte[] SETUP = new byte[2] { 0xA1, 0x00 };
        internal static readonly byte[] SHEETLAYOUT = new byte[2] { 0x62, 0x08 };
        internal static readonly byte[] SHEETPROTECTION = new byte[2] { 0x67, 0x08 };
        internal static readonly byte[] SHRFMLA = new byte[2] { 0xBC, 0x04 };
        internal static readonly byte[] SORT = new byte[2] { 0x90, 0x00 };
        internal static readonly byte[] SST = new byte[2] { 0xFC, 0x00 };
        internal static readonly byte[] STANDARDWIDTH = new byte[2] { 0x99, 0x00 };
        internal static readonly byte[] STRING = new byte[2] { 0x07, 0x02 };
        internal static readonly byte[] STYLE = new byte[2] { 0x93, 0x02 };
        internal static readonly byte[] SUPBOOK = new byte[2] { 0xAE, 0x01 };
        internal static readonly byte[] TABLEOP = new byte[2] { 0x36, 0x02 };
        internal static readonly byte[] TOPMARGIN = new byte[2] { 0x28, 0x00 };
        internal static readonly byte[] UNCALCED = new byte[2] { 0x5E, 0x00 };
        internal static readonly byte[] USESELFS = new byte[2] { 0x60, 0x01 };
        internal static readonly byte[] VCENTER = new byte[2] { 0x84, 0x00 };
        internal static readonly byte[] VERTICALPAGEBREAKS = new byte[2] { 0x1A, 0x00 };
        internal static readonly byte[] WINDOW1 = new byte[2] { 0x3D, 0x00 };
        internal static readonly byte[] WINDOW2 = new byte[2] { 0x3E, 0x02 };
        internal static readonly byte[] WINDOWPROTECT = new byte[2] { 0x19, 0x00 };
        internal static readonly byte[] WRITEACCESS = new byte[2] { 0x5C, 0x00 };
        internal static readonly byte[] WRITEPROT = new byte[2] { 0x86, 0x00 };
        internal static readonly byte[] WSBOOL = new byte[2] { 0x81, 0x00 };
        internal static readonly byte[] XCT = new byte[2] { 0x59, 0x00 };
        internal static readonly byte[] XF = new byte[2] { 0xE0, 0x00 };

	    private static readonly Dictionary<byte, Dictionary<byte, string>> _names = new Dictionary<byte, Dictionary<byte, string>>();
        private static readonly Dictionary<string, byte[]> _rids = new Dictionary<string, byte[]>();
        internal static readonly int NAME_MAX_LENGTH = 0;

        static RID()
        {
            foreach (FieldInfo fi in typeof(RID).GetFields(BindingFlags.Static | BindingFlags.NonPublic))
            {
                if (fi.FieldType == typeof(byte[]))
                {
                    byte[] rid = (byte[])fi.GetValue(null);

                    if (rid.Length == 2)
                    {
                        byte first = rid[0];
                        if (!_names.ContainsKey(first))
                            _names[first] = new Dictionary<byte, string>();
                        _names[first][rid[1]] = fi.Name;
                        _rids[fi.Name] = (byte[])fi.GetValue(null);
                        NAME_MAX_LENGTH = Math.Max(NAME_MAX_LENGTH, fi.Name.Length);
                    }
                }
            }
        }

        internal static string Name(byte[] rid)
        {
            if (_names.ContainsKey(rid[0]) && _names[rid[0]].ContainsKey(rid[1]))
                return _names[rid[0]][rid[1]];
            else
                return string.Format("??? {0:x2} {1:x2}", rid[0], rid[1]);
        }

        internal static byte[] ByteArray(byte[] rid)
        {
            string name = Name(rid);
            if (_rids.ContainsKey(name))
                return _rids[name];
            else
                return rid;
        }
	}
}