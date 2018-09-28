using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LedMatrixController.Server;
using LedMatrixController.Server.Effect.FlatColor;
using LedMatrixController.Server.Output.ArtNet;
using Microsoft.Extensions.Hosting;

namespace LedMatrixController.Host.Server
{
    public class MainService : IHostedService, IDisposable
    {
        private readonly ServerConfig _serverConfig;
        private FlatColor _source;
        private ArtnetFrameOutput _sink;
        private Timer _timer;

        public MainService(ServerConfig serverConfig)
        {
            _serverConfig = serverConfig;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _source = new FlatColor(new FlatColorConfig(_serverConfig.Width, _serverConfig.Height, new Color(255, 0, 255)));
            var outputConfig = new ArtnetFrameOutputConfig(_serverConfig.Width, _serverConfig.Height, new ModLedArtnetPatchConfig(_serverConfig.Width, _serverConfig.Height, IPAddress.Parse("192.168.178.229")));
            _sink = new ArtnetFrameOutput(outputConfig);

            _timer = new Timer((o) => DoFrame().Wait(), null, 0, 1000 / _serverConfig.FrameRate);

            return Task.CompletedTask;
        }

        public async Task DoFrame()
        {
            Console.Write(".");
            await _sink.Output(await _source.GetFrame());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _sink?.Dispose();
            _timer?.Dispose();
        }
    }
}