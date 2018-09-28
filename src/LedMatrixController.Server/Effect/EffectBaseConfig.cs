namespace LedMatrixController.Server.Effect
{
    public class EffectBaseConfig
    {
        public int Width { get; }
        public int Height { get; }

        public EffectBaseConfig(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}