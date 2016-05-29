using System;
using System.Collections.Generic;
using System.Text;

namespace org.in2bits.MyXls
{
    //http://www.mvps.org/dmcritchie/excel/colors.htm
    internal class Palette
    {
        private Workbook _workbook;
        private List<Color> _egaColors = new List<Color>();
        private List<Color> _colors = new List<Color>();

        internal Palette(Workbook workbook)
        {
            _workbook = workbook;

            InitDefaultPalette();
        }

        private void InitDefaultPalette()
        {
            _egaColors.Add(Colors.EgaBlack);
            _egaColors.Add(Colors.EgaWhite);
            _egaColors.Add(Colors.EgaRed);
            _egaColors.Add(Colors.EgaGreen);
            _egaColors.Add(Colors.EgaBlue);
            _egaColors.Add(Colors.EgaYellow);
            _egaColors.Add(Colors.EgaMagenta);
            _egaColors.Add(Colors.EgaCyan);
            _colors.Add(Colors.Black);
            _colors.Add(Colors.White);
            _colors.Add(Colors.Red);
            _colors.Add(Colors.Green);
            _colors.Add(Colors.Blue);
            _colors.Add(Colors.Yellow);
            _colors.Add(Colors.Magenta);
            _colors.Add(Colors.Cyan);
            _colors.Add(Colors.DarkRed);
            _colors.Add(Colors.DarkGreen);
            _colors.Add(Colors.DarkBlue);
            _colors.Add(Colors.Olive);
            _colors.Add(Colors.Purple);
            _colors.Add(Colors.Teal);
            _colors.Add(Colors.Silver);
            _colors.Add(Colors.Grey);
            _colors.Add(Colors.Default18);
            _colors.Add(Colors.Default19);
            _colors.Add(Colors.Default1A);
            _colors.Add(Colors.Default1B);
            _colors.Add(Colors.Default1C);
            _colors.Add(Colors.Default1D);
            _colors.Add(Colors.Default1E);
            _colors.Add(Colors.Default1F);
            _colors.Add(Colors.Default20);
            _colors.Add(Colors.Default21);
            _colors.Add(Colors.Default22);
            _colors.Add(Colors.Default23);
            _colors.Add(Colors.Default24);
            _colors.Add(Colors.Default25);
            _colors.Add(Colors.Default26);
            _colors.Add(Colors.Default27);
            _colors.Add(Colors.Default28);
            _colors.Add(Colors.Default29);
            _colors.Add(Colors.Default2A);
            _colors.Add(Colors.Default2B);
            _colors.Add(Colors.Default2C);
            _colors.Add(Colors.Default2D);
            _colors.Add(Colors.Default2E);
            _colors.Add(Colors.Default2F);
            _colors.Add(Colors.Default30);
            _colors.Add(Colors.Default31);
            _colors.Add(Colors.Default32);
            _colors.Add(Colors.Default33);
            _colors.Add(Colors.Default34);
            _colors.Add(Colors.Default35);
            _colors.Add(Colors.Default36);
            _colors.Add(Colors.Default37);
            _colors.Add(Colors.Default38);
            _colors.Add(Colors.Default39);
            _colors.Add(Colors.Default3A);
            _colors.Add(Colors.Default3B);
            _colors.Add(Colors.Default3C);
            _colors.Add(Colors.Default3D);
            _colors.Add(Colors.Default3E);
            _colors.Add(Colors.Default3F);
        }

        internal ushort GetIndex(Color color)
        {
            if (_colors.Contains(color))
                return (ushort)(_colors.IndexOf(color) + _egaColors.Count);
            else if (_egaColors.Contains(color))
                return (ushort) _egaColors.IndexOf(color);
            else if (color.Id == null)
                throw new ArgumentOutOfRangeException("Could not locate color in palette");
            else //is system color (not in user palette)
            {
                return (ushort)color.Id;
            }
        }
    }
}
