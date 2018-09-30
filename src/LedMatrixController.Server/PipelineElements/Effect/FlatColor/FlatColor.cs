using System.Linq;
using System.Threading.Tasks;
using LedMatrixController.Server.PipelineElements;

namespace LedMatrixController.Server.Effect.FlatColor
{
    public class FlatColor : ISource<Frame>
    {
        private readonly FlatColorConfig _config;

        public FlatColor(FlatColorConfig config)
        {
            _config = config;
        }

        public Task<Frame> Pop()
        {
            return Task.FromResult(new Frame { Pixels = Enumerable.Repeat(_config.Color, _config.Width * _config.Height).ToArray() });
        }

        public void Dispose()
        {
        }
    }
}