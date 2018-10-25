using System.Threading.Tasks;
using LedMatrixController.Server.PipelineElements;

namespace LedMatrixController.Server.Effect.Rainbow
{
    public class Rainbow : ISource<Frame>
    {
        private readonly int _height;
        private readonly int _width;
        private int _frameNum;

        public Rainbow(int width, int height)
        {
            _frameNum = 0;
            _width = width;
            _height = height;
        }

        public Task<Frame> Pop()
        {
            var pixels = new Color[_width * _height];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    pixels[(y * _width) + x] = Color.FromHsv(((double)x / _width) * 360 + ((double)y / _height * 180) + _frameNum * 5, 1, 1);
                }
            }

            _frameNum++;
            return Task.FromResult(new Frame(_width, _height, pixels));
        }

        public void Dispose()
        {
        }
    }
}