using System.Threading.Tasks;

namespace LedMatrixController.Server.PipelineElements.Source
{
    public class Rainbow : ISource<Frame>
    {
        private readonly IOutputSize _outputSize;
        private int _frameNum;

        public Rainbow(IOutputSize outputSize)
        {
            _frameNum = 0;
            _outputSize = outputSize;
        }

        public Task<Frame> Pop()
        {
            var width = _outputSize.Width;
            var height = _outputSize.Height;

            var pixels = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixels[(y * width) + x] = Color.FromHsv(((double)x / width) * 360 + ((double)y / height * 180) + _frameNum * 5, 1, 1);
                }
            }

            _frameNum++;
            return Task.FromResult(new Frame(width, height, pixels));
        }

        public void Dispose()
        {
        }
    }
}