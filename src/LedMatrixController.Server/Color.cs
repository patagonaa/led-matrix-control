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
    }
}
