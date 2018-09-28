namespace LedMatrixController.Server.Output
{
    public class FrameOutputBaseConfig
    {
        public int Width { get; }
        public int Height { get; }

        public FrameOutputBaseConfig(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}