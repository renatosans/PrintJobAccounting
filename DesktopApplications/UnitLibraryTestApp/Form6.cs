using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace UnitLibraryTestApp
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }


        private double GetDistanceFactor(int x, int y)
        {
            double maxDistance = Math.Sqrt(colorPanel.Width * colorPanel.Width + colorPanel.Height * colorPanel.Height) / 2;

            /*
            // reta
            double distance = x + y;
            if (distance > maxDistance)
                return 1;
            else
                return distance / maxDistance;
            */

            // circulo
            double distance = Math.Sqrt(x*x + y*y);
            if (distance > maxDistance)
                return 1;
            else
            {
                double ratio = distance/maxDistance;
                if ((ratio >= 0.9) && (ratio <= 1))
                    return ratio;
                else
                    return ratio;
            }
        }


        private Color Lighten(Color color, double distanceFactor)
        {
            int red = Convert.ToInt32(color.R * distanceFactor); if (red > 255) red = 255;
            int green = Convert.ToInt32(color.G * distanceFactor); if (green > 255) green = 255;
            int blue = Convert.ToInt32(color.B * distanceFactor); if (blue > 255) blue = 255;
            return Color.FromArgb(red, green, blue);
        }


        private Color Darken(Color color)
        {
            int red = (int)(color.R * 0.8f);
            if (red < 0) red = 0;

            int green = (int)(color.G * 0.8f);
            if (green < 0) green = 0;

            int blue = (int)(color.B * 0.8f);
            if (blue < 0) blue = 0;

            return Color.FromArgb(red, green, blue);
        }


        private void DrawIcon(PaintEventArgs args)
        {
            Graphics graphs = args.Graphics;

            Point p1 = new Point(12, 12);
            Point p2 = new Point(27, 22);
            Point p3 = new Point(12, 32);

            graphs.SmoothingMode = SmoothingMode.AntiAlias;
            graphs.FillPolygon(new SolidBrush(Color.FromArgb(33, 33, 33)), new Point[] { p1, p2, p3 });
            graphs.FillRectangle(new SolidBrush(Color.FromArgb(33, 33, 33)), new Rectangle(31, 12, 5, 20));


            Point p01 = new Point(10, 10);
            Point p02 = new Point(25, 20);
            Point p03 = new Point(10, 30);

            PathGradientBrush brush1 = new PathGradientBrush(new Point[] { p01, p02, p03 });
            brush1.CenterColor = Color.Lime;
            brush1.SurroundColors = new Color[] { Color.ForestGreen, Color.ForestGreen, Color.ForestGreen };

            Point p001 = new Point(29, 10);
            Point p002 = new Point(39, 10);
            Point p003 = new Point(39, 30);
            Point p004 = new Point(29, 30);

            PathGradientBrush brush2 = new PathGradientBrush(new Point[] { p001, p002, p003, p004 });
            brush2.CenterColor = Color.Lime;
            brush2.SurroundColors = new Color[] { Color.ForestGreen, Color.ForestGreen, Color.ForestGreen, Color.ForestGreen };

            graphs.SmoothingMode = SmoothingMode.Default;
            graphs.FillPolygon(brush1, new Point[] { p01, p02, p03 });
            args.Graphics.FillRectangle(brush2, new Rectangle(29, 10, 5, 20));

            graphs.SmoothingMode = SmoothingMode.AntiAlias;
            graphs.DrawPolygon(new Pen(Color.DarkGreen, 1.5f), new Point[] { p01, p02, p03 });
            graphs.DrawRectangle(new Pen(Color.DarkGreen, 1.5f), new Rectangle(29, 10, 5, 20));
        }


        private void ChangeImgColor(PaintEventArgs args)
        {
            Image icon = Bitmap.FromFile("firstPage.gif");

            GraphicsUnit gu = GraphicsUnit.Pixel;
            int width = (int)icon.GetBounds(ref gu).Width;
            int height = (int)icon.GetBounds(ref gu).Height;
            Bitmap canvas = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixelColor = ((Bitmap)icon).GetPixel(x, y);
                    Byte red = pixelColor.R;
                    Byte green = pixelColor.G;
                    Byte blue = pixelColor.B;

                    canvas.SetPixel(x, y, Darken(Color.FromArgb(red, blue, green)));
                }
            }
            args.Graphics.DrawImageUnscaled(canvas, new Point(0, 0));
        }


        private void DrawColorBox(PaintEventArgs args)
        {
            int ndx = 0;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    ndx++;
                    Brush brush = new SolidBrush(Color.FromKnownColor((KnownColor)ndx));
                    args.Graphics.DrawRectangle(new Pen(Color.Black), x * 40, y * 40, 30, 30);
                    args.Graphics.FillRectangle(brush, x * 40 + 1, y * 40 + 1, 29, 29);
                }
            }
        }


        private void ShowColorValues()
        {
            Image colorBox = Image.FromFile("firstPage.gif");

            Bitmap bitmap = new Bitmap(colorBox);
            Color pixelColor = bitmap.GetPixel(20, 20);
            MessageBox.Show("R=" + pixelColor.R + "  G=" + pixelColor.G + "  B=" + pixelColor.B);
        }


        private void DrawGradient(PaintEventArgs args)
        {
            Color color = Color.FromArgb(147, 107, 42);

            for (int y = 0; y < colorPanel.Height; y++)
            {
                for (int x = 0; x < colorPanel.Width; x++)
                {
                    double distanceFactor = GetDistanceFactor(x, y);
                    args.Graphics.FillRectangle(new SolidBrush(Lighten(color, distanceFactor)), x, y, 1, 1);
                }
            }
        }

        private void colorPanel_Paint(object sender, PaintEventArgs e)
        {
            // DrawIcon(e);

            // ChangeImgColor(e);

            // DrawColorBox(e);

            // ShowColorValues();

            DrawGradient(e);
        }

        private void Form6_Resize(object sender, EventArgs e)
        {
            colorPanel.Height = colorPanel.Width;
        }
    }

}
