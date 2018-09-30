using System.Threading.Tasks;
using LedMatrixController.Server.PipelineElements;

namespace LedMatrixController.Server.Effect.Rainbow
{
    public class Rainbow : ISource<Frame>
    {
        private readonly RainbowConfig _config;
        private int _frameNum;

        public Rainbow(RainbowConfig config)
        {
            _config = config;
            _frameNum = 0;
        }

        public Task<Frame> Pop()
        {
            var pixels = new Color[_config.Width * _config.Height];

            for (int y = 0; y < _config.Height; y++)
            {
                for (int x = 0; x < _config.Width; x++)
                {
                    pixels[(y * _config.Width) + x] = Color.FromHsv(((double)x / _config.Width) * 360 + ((double)y / _config.Height * 180) + _frameNum * 5, 1, 1);
                }
            }

            _frameNum++;
            return Task.FromResult(new Frame
            {
                Pixels = pixels
            });
        }

        public void Dispose()
        {
        }
    }

    public class RainbowConfig : EffectBaseConfig
    {
        public RainbowConfig(int width, int height) : base(width, height)
        {
        }
    }
}