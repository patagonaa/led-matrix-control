namespace LedMatrixController.Server.Effect.FlatColor
{
    public class FlatColorConfig : EffectBaseConfig
    {
        public FlatColorConfig(int width, int height, Color color) : base(width, height)
        {
            Color = color;
        }

        public Color Color { get; }
    }
}