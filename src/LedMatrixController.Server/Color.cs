using System;

namespace LedMatrixController.Server
{
    public class Color
    {
        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public static Color FromHsv(double h, double s, double v)
        {
            ColorHelper.HsvToRgb(h, s, v, out byte r, out byte g, out byte b);
            return new Color(r, g, b);
        }

        public static Color FromHex(string hexString)
        {
            if (hexString.Length != 6)
                throw new ArgumentException(hexString);

            int rgb = Convert.ToInt32(hexString, 16);

            return new Color((byte)(rgb >> 16 & 0xFF), (byte)(rgb >> 8 & 0xFF), (byte)(rgb & 0xFF));
        }
    }
}
