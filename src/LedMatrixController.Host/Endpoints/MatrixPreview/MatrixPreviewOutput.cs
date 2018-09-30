using System.Linq;
using System.Threading.Tasks;
using LedMatrixController.Server;
using LedMatrixController.Server.PipelineElements;
using Microsoft.AspNetCore.SignalR;

namespace LedMatrixController.Host.Endpoints.MatrixPreview
{
    public class MatrixPreviewOutput : ISink<Frame>
    {
        private readonly IHubContext<MatrixPreviewHub> _hub;

        public MatrixPreviewOutput(IHubContext<MatrixPreviewHub> hub)
        {
            _hub = hub;
        }

        public async Task Push(Frame item)
        {
            var rgbaValues = item.Pixels.SelectMany(x => new[] { x.R, x.G, x.B, (byte)255 }).ToArray();

            await _hub.Clients.All.SendAsync("PreviewFrame", new { ImageData = rgbaValues });
        }

        public void Dispose()
        {
        }
    }
}
