using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using LedMatrixController.Server.Config;

namespace LedMatrixController.Server.PipelineElements.Source
{
    public class FlatColor : ISource<Frame>
    {
        private readonly IOutputSize _outputSize;
        private volatile FlatColorConfig _config;

        public FlatColor(IOutputSize outputSize, FlatColorConfig config)
        {
            _outputSize = outputSize;
            _config = config;
        }

        public FlatColor(IOutputSize outputSize, Guid configId, IDataService<FlatColorConfig> configService)
        {
            _config = configService.Get(configId);
            configService.GetUpdateObservable().Subscribe(Observer.Create<FlatColorConfig>(Update));
            _outputSize = outputSize;
        }

        private void Update(FlatColorConfig config)
        {
            if(config.Id == _config.Id)
            {
                _config = config;
            }
        }

        public Task<Frame> Pop()
        {
            var color = Color.FromHex(_config.Color);
            Color[] pixels = Enumerable.Repeat(color, _outputSize.Width * _outputSize.Height).ToArray();
            return Task.FromResult(new Frame(_outputSize, pixels));
        }

        public void Dispose()
        {
        }
    }
}