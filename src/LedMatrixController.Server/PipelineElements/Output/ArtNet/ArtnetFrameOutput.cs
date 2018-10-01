using LedMatrixController.Server.PipelineElements;
using System.Threading.Tasks;

namespace LedMatrixController.Server.Output.ArtNet
{
    public class ArtnetFrameOutput : ISink<Frame>
    {
        private ArtnetSender _sender;
        private readonly ArtnetFrameOutputConfig _config;

        public ArtnetFrameOutput(ArtnetFrameOutputConfig config)
        {
            _sender = new ArtnetSender();
            _config = config;
        }

        public async Task Push(Frame frame)
        {
            FrameHelper.EnsureValid(frame, _config.Width, _config.Height);

            foreach (var packet in _config.PatchConfig.GetPackets(frame))
            {
                await _sender.Send(packet);
            }
        }

        public void Dispose()
        {
            _sender.Dispose();
        }
    }
}