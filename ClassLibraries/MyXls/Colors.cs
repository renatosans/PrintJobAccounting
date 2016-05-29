using System;
using System.Collections.Generic;
using System.Text;

namespace org.in2bits.MyXls
{
    ///<summary>
    /// Provides named references to all colors in the standard (BIFF8) Excel
    /// color palette.
    ///</summary>
    public static class Colors
    {
        #region (System?) EGA Colors (don't use - they seem to break Excel's Format dialog)

        /// <summary>EGA Black (use Black instead)</summary>
        public static readonly Color EgaBlack = new Color(0x00, 0x00, 0x00);

        /// <summary>EGA White (use White instead)</summary>
        public static readonly Color EgaWhite = new Color(0xFF, 0xFF, 0xFF);
        
        /// <summary>EGA Red (use Red instead)</summary>
        public static readonly Color EgaRed = new Color(0xFF, 0x00, 0x00);
        
        /// <summary>EGA Green (use Green instead)</summary>
        public static readonly Color EgaGreen = new Color(0x00, 0xFF, 0x00);
        
        /// <summary>EGA Blue (use Blue instead)</summary>
        public static readonly Color EgaBlue = new Color(0x00, 0x00, 0xFF);
        
        /// <summary>EGA Yellow (use Yellow instead)</summary>
        public static readonly Color EgaYellow = new Color(0xFF, 0xFF, 0x00);
        
        /// <summary>EGA Magenta (use Magenta instead)</summary>
        public static readonly Color EgaMagenta = new Color(0xFF, 0x00, 0xFF);
        
        /// <summary>EGA Cyan (use Cyan instead)</summary>
        public static readonly Color EgaCyan = new Color(0x00, 0xFF, 0xFF);

        #endregion

        #region Default Palette Colors

        /// <summary>Default Palette Color Index 0x08 (#000000)</summary>
        public static readonly Color Default08 = new Color(0x00, 0x00, 0x00);
        
        /// <summary>Default Palette Color Index 0x09 (#FFFFFF)</summary>
        public static readonly Color Default09 = new Color(0xFF, 0xFF, 0xFF);
        
        /// <summary>Default Palette Color Index 0x0A (#FF0000)</summary>
        public static readonly Color Default0A = new Color(0xFF, 0x00, 0x00);
        
        /// <summary>Default Palette Color Index 0x0B (#00FF00)</summary>
        public static readonly Color Default0B = new Color(0x00, 0xFF, 0x00);
        
        /// <summary>Default Palette Color Index 0x0C (#0000FF)</summary>
        public static readonly Color Default0C = new Color(0x00, 0x00, 0xFF);
        
        /// <summary>Default Palette Color Index 0x0D (#FFFF00)</summary>
        public static readonly Color Default0D = new Color(0xFF, 0xFF, 0x00);
        
        /// <summary>Default Palette Color Index 0x0E (#FF00FF)</summary>
        public static readonly Color Default0E = new Color(0xFF, 0x00, 0xFF);
        
        /// <summary>Default Palette Color Index 0x0F (#00FFFF)</summary>
        public static readonly Color Default0F = new Color(0x00, 0xFF, 0xFF);
        
        /// <summary>Default Palette Color Index 0x10 (#800000)</summary>
        public static readonly Color Default10 = new Color(0x80, 0x00, 0x00);
        
        /// <summary>Default Palette Color Index 0x11 (#008000)</summary>
        public static readonly Color Default11 = new Color(0x00, 0x80, 0x00);
        
        /// <summary>Default Palette Color Index 0x12 (#000080)</summary>
        public static readonly Color Default12 = new Color(0x00, 0x00, 0x80);
        
        /// <summary>Default Palette Color Index 0x13 (#808000)</summary>
        public static readonly Color Default13 = new Color(0x80, 0x80, 0x00);
        
        /// <summary>Default Palette Color Index 0x14 (#800080)</summary>
        public static readonly Color Default14 = new Color(0x80, 0x00, 0x80);
        
        /// <summary>Default Palette Color Index 0x15 (#008080)</summary>
        public static readonly Color Default15 = new Color(0x00, 0x80, 0x80);
        
        /// <summary>Default Palette Color Index 0x16 (#C0C0C0)</summary>
        public static readonly Color Default16 = new Color(0xC0, 0xC0, 0xC0);
        
        /// <summary>Default Palette Color Index 0x17 (#808080)</summary>
        public static readonly Color Default17 = new Color(0x80, 0x80, 0x80);
        
        /// <summary>Default Palette Color Index 0x18 (#9999FF)</summary>
        public static readonly Color Default18 = new Color(0x99, 0x99, 0xFF);
        
        /// <summary>Default Palette Color Index 0x19 (#993366)</summary>
        public static readonly Color Default19 = new Color(0x99, 0x33, 0x66);
        
        /// <summary>Default Palette Color Index 0x1A (#FFFFCC)</summary>
        public static readonly Color Default1A = new Color(0xFF, 0xFF, 0xCC);
        
        /// <summary>Default Palette Color Index 0x1B (#CCFFFF)</summary>
        public static readonly Color Default1B = new Color(0xCC, 0xFF, 0xFF);
        
        /// <summary>Default Palette Color Index 0x1C (#660066)</summary>
        public static readonly Color Default1C = new Color(0x66, 0x00, 0x66);
        
        /// <summary>Default Palette Color Index 0x1D (#FF8080)</summary>
        public static readonly Color Default1D = new Color(0xFF, 0x80, 0x80);
        
        /// <summary>Default Palette Color Index 0x1E (#0066CC)</summary>
        public static readonly Color Default1E = new Color(0x00, 0x66, 0xCC);
        
        /// <summary>Default Palette Color Index 0x1F (#CCCCFF)</summary>
        public static readonly Color Default1F = new Color(0xCC, 0xCC, 0xFF);
        
        /// <summary>Default Palette Color Index 0x20 (#000080)</summary>
        public static readonly Color Default20 = new Color(0x00, 0x00, 0x80);
        
        /// <summary>Default Palette Color Index 0x21 (#FF00FF)</summary>
        public static readonly Color Default21 = new Color(0xFF, 0x00, 0xFF);
        
        /// <summary>Default Palette Color Index 0x22 (#FFFF00)</summary>
        public static readonly Color Default22 = new Color(0xFF, 0xFF, 0x00);
        
        /// <summary>Default Palette Color Index 0x23 (#00FFFF)</summary>
        public static readonly Color Default23 = new Color(0x00, 0xFF, 0xFF);
        
        /// <summary>Default Palette Color Index 0x24 (#800080)</summary>
        public static readonly Color Default24 = new Color(0x80, 0x00, 0x80);
        
        /// <summary>Default Palette Color Index 0x25 (#800000)</summary>
        public static readonly Color Default25 = new Color(0x80, 0x00, 0x00);
        
        /// <summary>Default Palette Color Index 0x26 (#008080)</summary>
        public static readonly Color Default26 = new Color(0x00, 0x80, 0x80);
        
        /// <summary>Default Palette Color Index 0x27 (#0000FF)</summary>
        public static readonly Color Default27 = new Color(0x00, 0x00, 0xFF);
        
        /// <summary>Default Palette Color Index 0x28 (#00CCFF)</summary>
        public static readonly Color Default28 = new Color(0x00, 0xCC, 0xFF);
        
        /// <summary>Default Palette Color Index 0x29 (#CCFFFF)</summary>
        public static readonly Color Default29 = new Color(0xCC, 0xFF, 0xFF);
        
        /// <summary>Default Palette Color Index 0x2A (#CCFFCC)</summary>
        public static readonly Color Default2A = new Color(0xCC, 0xFF, 0xCC);
        
        /// <summary>Default Palette Color Index 0x2B (#FFFF99)</summary>
        public static readonly Color Default2B = new Color(0xFF, 0xFF, 0x99);
        
        /// <summary>Default Palette Color Index 0x2C (#99CCFF)</summary>
        public static readonly Color Default2C = new Color(0x99, 0xCC, 0xFF);
        
        /// <summary>Default Palette Color Index 0x2D (#FF99CC)</summary>
        public static readonly Color Default2D = new Color(0xFF, 0x99, 0xCC);
        
        /// <summary>Default Palette Color Index 0x2E (#CC99FF)</summary>
        public static readonly Color Default2E = new Color(0xCC, 0x99, 0xFF);
        
        /// <summary>Default Palette Color Index 0x2F (#FFCC99)</summary>
        public static readonly Color Default2F = new Color(0xFF, 0xCC, 0x99);
        
        /// <summary>Default Palette Color Index 0x30 (#3366FF)</summary>
        public static readonly Color Default30 = new Color(0x33, 0x66, 0xFF);
        
        /// <summary>Default Palette Color Index 0x31 (#33CCCC)</summary>
        public static readonly Color Default31 = new Color(0x33, 0xCC, 0xCC);
        
        /// <summary>Default Palette Color Index 0x32 (#99CC00)</summary>
        public static readonly Color Default32 = new Color(0x99, 0xCC, 0x00);
        
        /// <summary>Default Palette Color Index 0x33 (#FFCC00)</summary>
        public static readonly Color Default33 = new Color(0xFF, 0xCC, 0x00);
        
        /// <summary>Default Palette Color Index 0x34 (#FF9900)</summary>
        public static readonly Color Default34 = new Color(0xFF, 0x99, 0x00);
        
        /// <summary>Default Palette Color Index 0x35 (#FF6600)</summary>
        public static readonly Color Default35 = new Color(0xFF, 0x66, 0x00);
        
        /// <summary>Default Palette Color Index 0x36 (#666699)</summary>
        public static readonly Color Default36 = new Color(0x66, 0x66, 0x99);
        
        /// <summary>Default Palette Color Index 0x37 (#969696)</summary>
        public static readonly Color Default37 = new Color(0x96, 0x96, 0x96);
        
        /// <summary>Default Palette Color Index 0x38 (#003366)</summary>
        public static readonly Color Default38 = new Color(0x00, 0x33, 0x66);
        
        /// <summary>Default Palette Color Index 0x39 (#339966)</summary>
        public static readonly Color Default39 = new Color(0x33, 0x99, 0x66);
        
        /// <summary>Default Palette Color Index 0x3A (#003300)</summary>
        public static readonly Color Default3A = new Color(0x00, 0x33, 0x00);
        
        /// <summary>Default Palette Color Index 0x3B (#333300)</summary>
        public static readonly Color Default3B = new Color(0x33, 0x33, 0x00);
        
        /// <summary>Default Palette Color Index 0x3C (#993300)</summary>
        public static readonly Color Default3C = new Color(0x99, 0x33, 0x00);
        
        /// <summary>Default Palette Color Index 0x3D (#993366)</summary>
        public static readonly Color Default3D = new Color(0x99, 0x33, 0x66);
        
        /// <summary>Default Palette Color Index 0x3E (#333399)</summary>
        public static readonly Color Default3E = new Color(0x33, 0x33, 0x99);
        
        /// <summary>Default Palette Color Index 0x3F (#333333)</summary>
        public static readonly Color Default3F = new Color(0x33, 0x33, 0x33);

        #endregion

        /// <summary>Black - Alias for Default08</summary>
        public static readonly Color Black = Default08;
        
        /// <summary>White - Alias for Default09</summary>
        public static readonly Color White = Default09;
        
        /// <summary>Red - Alias for Default0A</summary>
        public static readonly Color Red = Default0A;
        
        /// <summary>Green - Alias for Default0B</summary>
        public static readonly Color Green = Default0B;
        
        /// <summary>Blue - Alias for Default0C</summary>
        public static readonly Color Blue = Default0C;
        
        /// <summary>Yellow - Alias for Default0D</summary>
        public static readonly Color Yellow = Default0D;
        
        /// <summary>Magenta - Alias for Default0E</summary>
        public static readonly Color Magenta = Default0E;
        
        /// <summary>Cyan - Alias for Default0F</summary>
        public static readonly Color Cyan = Default0F;
        
        /// <summary>DarkRed - Alias for Default10</summary>
        public static readonly Color DarkRed = Default10;
        
        /// <summary>DarkGreen - Alias for Default11</summary>
        public static readonly Color DarkGreen = Default11;
        
        /// <summary>DarkBlue - Alias for Default12</summary>
        public static readonly Color DarkBlue = Default12;
        
        /// <summary>Olive - Alias for Default13</summary>
        public static readonly Color Olive = Default13;
        
        /// <summary>Purple - Alias for Default14</summary>
        public static readonly Color Purple = Default14;
        
        /// <summary>Teal - Alias for Default15</summary>
        public static readonly Color Teal = Default15;
        
        /// <summary>Silver - Alias for Default16</summary>
        public static readonly Color Silver = Default16;
        
        /// <summary>Grey - Alias for Default17</summary>
        public static readonly Color Grey = Default17;

        /// <summary>System window text colour for border lines (used in records XF, CF, and WINDOW2 (BIFF8 only))</summary>
        public static readonly Color SystemWindowTextColorForBorderLines = new Color(64);

        /// <summary>System window background colour for pattern background (used in records XF, and CF)</summary>
        public static readonly Color SystemWindowBackgroundColorForPatternBackground = new Color(65);

        /// <summary>System face colour (dialogue background colour)</summary>
        public static readonly Color SystemFaceColor = new Color(67);

        /// <summary>System window text colour for chart border lines</summary>
        public static readonly Color SystemWindowTextColorForChartBorderLines = new Color(77);

        /// <summary>System window background colour for chart areas</summary>
        public static readonly Color SystemWindowBackgroundColorForChartAreas = new Color(78);

        /// <summary>Automatic colour for chart border lines (seems to be always Black)</summary>
        public static readonly Color SystemAutomaticColorForChartBorderLines = new Color(79);

        /// <summary>System tool tip background colour (used in note objects)</summary>
        public static readonly Color SystemToolTipBackgroundColor = new Color(80);

        /// <summary>System tool tip text colour (used in note objects)</summary>
        public static readonly Color SystemToolTipTextColor = new Color(81);
        
        //TODO: Figure out the SystemWindowTextColorForFonts value
        /// <summary>System window text colour for fonts (used in records FONT, EFONT, and CF)</summary>
        //public static readonly Color SystemWindowTextColorForFonts = new Color(??);

        internal static readonly Color DefaultLineColor = Black;
        internal static readonly Color DefaultPatternColor = new Color(64);
        internal static readonly Color DefaultPatternBackgroundColor = SystemWindowBackgroundColorForPatternBackground;
    }
}
