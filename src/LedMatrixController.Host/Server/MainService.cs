using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LedMatrixController.Host.Endpoints.MatrixPreview;
using LedMatrixController.Host.Endpoints.MixerControl;
using LedMatrixController.Server;
using LedMatrixController.Server.Config;
using LedMatrixController.Server.Output.ArtNet;
using LedMatrixController.Server.Queue;
using LedMatrixController.Server.Queue.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace LedMatrixController.Host.Server
{
    internal class MainService : IHostedService, IDisposable
    {
        private readonly ServerConfig _serverConfig;
        private readonly IOutputSize _outputSize;
        private readonly IDataService<QueueConfigModel> _queueConfigService;
        private readonly MatrixPreviewOutput _matrixPreviewOutput;
        private readonly MainMixerControl _mixerControls;
        private readonly IQueueElementFactory _queueElementFactory;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public MainService(IOptions<ServerConfig> serverConfig,
            IOutputSize outputSize,
            IDataService<QueueConfigModel> queueConfigService,
            MatrixPreviewOutput matrixPreviewOutput,
            MainMixerControl mixerControls,
            IQueueElementFactory queueElementFactory)
        {
            _serverConfig = serverConfig.Value;
            _outputSize = outputSize;
            _queueConfigService = queueConfigService;
            _matrixPreviewOutput = matrixPreviewOutput;
            _mixerControls = mixerControls;
            _queueElementFactory = queueElementFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var x = new Runner(_outputSize, _queueConfigService, _queueElementFactory);
            x.Sinks.Add(_matrixPreviewOutput);

            var outputConfig = new ArtnetFrameOutputConfig(_serverConfig.Width, _serverConfig.Height, new ModLedArtnetPatchConfig(_serverConfig.Width, _serverConfig.Height, IPAddress.Parse("192.168.178.229")));
            var sink = new ArtnetFrameOutput(outputConfig);
            //x.Sinks.Add(sink);
            return x.Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //todo cleanup
        }
    }
}