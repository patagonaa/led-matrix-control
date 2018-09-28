using System.Linq;
using System.Threading.Tasks;

namespace LedMatrixController.Server.Effect.FlatColor
{
    public class FlatColor : IEffect
    {
        private readonly FlatColorConfig _config;

        public FlatColor(FlatColorConfig config)
        {
            _config = config;
        }

        public Task<Frame> GetFrame()
        {
            return Task.FromResult(new Frame{Pixels = Enumerable.Repeat(_config.Color, _config.Width * _config.Height).ToArray()});
        }
    }
}