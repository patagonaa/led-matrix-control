namespace LedMatrixController.Server
{
    public class OutputSize : IOutputSize
    {
        public OutputSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }

        public int Height { get; }
    }
}
