using System;
using System.Net;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using LedMatrixController.Host.Endpoints.MatrixPreview;
using LedMatrixController.Host.Endpoints.MixerControl;
using LedMatrixController.Server;
using LedMatrixController.Server.Effect.FlatColor;
using LedMatrixController.Server.Effect.Rainbow;
using LedMatrixController.Server.Output.ArtNet;
using LedMatrixController.Server.PipelineElements;
using LedMatrixController.Server.PipelineElements.Mixer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace LedMatrixController.Host.Server
{
    internal class MainService : IHostedService, IDisposable
    {
        private readonly ServerConfig _serverConfig;
        private readonly MatrixPreviewOutput _matrixPreviewOutput;
        private readonly MainMixerControls _mixerControls;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private ISource<Frame> _source1;
        private ISource<Frame> _source2;
        private IMixer _mixer;

        public MainService(IOptions<ServerConfig> serverConfig, MatrixPreviewOutput matrixPreviewOutput, MainMixerControls mixerControls)
        {
            _serverConfig = serverConfig.Value;
            _matrixPreviewOutput = matrixPreviewOutput;
            _mixerControls = mixerControls;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => Main());

            return Task.CompletedTask;
        }

        public async void Main()
        {
            _source1 = new FlatColor(new FlatColorConfig(_serverConfig.Width, _serverConfig.Height, new Color(255, 0, 255)));
            _source2 = new Rainbow(new RainbowConfig(_serverConfig.Width, _serverConfig.Height));

            _mixer = new LinearMixer(_serverConfig.Width, _serverConfig.Height, _source1, _source2);
            _mixerControls.MixerValue.Subscribe(Observer.Create<double>(val => _mixer.SetMixerLevel(val)));

            var outputConfig = new ArtnetFrameOutputConfig(_serverConfig.Width, _serverConfig.Height, new ModLedArtnetPatchConfig(_serverConfig.Width, _serverConfig.Height, IPAddress.Parse("192.168.178.229")));
            var sink = new ArtnetFrameOutput(outputConfig);

            var token = _cts.Token;
            while (!token.IsCancellationRequested)
            {
                Console.Write(".");
                var frame = await _mixer.Pop();
                await Task.WhenAll(sink.Push(frame), _matrixPreviewOutput.Push(frame));
                Thread.Sleep(1000 / _serverConfig.FrameRate);
            }
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