using System;

namespace LedMatrixController.Server
{
    public class Frame
    {
        public Frame(IOutputSize outputSize, Color[] pixels)
        {
            Width = outputSize.Width;
            Height = outputSize.Height;

#if DEBUG
            if (pixels.Length != (Width * Height))
                throw new ArgumentException("pixels length not same as width * height", nameof(pixels));
#endif
            Pixels = pixels;
        }

        public Frame(int width, int height, Color[] pixels)
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException(nameof(width));
            if (height < 0)
                throw new ArgumentOutOfRangeException(nameof(height));
            Width = width;
            Height = height;
            if (pixels.Length != (width * height))
                throw new ArgumentException("pixels length not same as width * height", nameof(pixels));
            Pixels = pixels;
        }
        public int Width { get; }
        public int Height { get; }
        public Color[] Pixels { get; }
    }
}
