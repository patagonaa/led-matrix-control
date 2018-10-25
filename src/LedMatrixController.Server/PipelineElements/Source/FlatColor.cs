using System.Linq;
using System.Threading.Tasks;
using LedMatrixController.Server.Config.Source;
using LedMatrixController.Server.PipelineElements;

namespace LedMatrixController.Server.Effect.FlatColor
{
    public class FlatColor : ISource<Frame>
    {
        private readonly int _width;
        private readonly int _height;
        private readonly FlatColorConfig _config;

        public FlatColor(int width, int height, FlatColorConfig config)
        {
            _width = width;
            _height = height;
            _config = config;
        }

        public Task<Frame> Pop()
        {
            var color = Color.FromHex(_config.Color);
            Color[] pixels = Enumerable.Repeat(color, _width * _height).ToArray();
            return Task.FromResult(new Frame(_width, _height, pixels));
        }

        public void Dispose()
        {
        }
    }
}