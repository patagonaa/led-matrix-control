using System.Net;

namespace LedMatrixController.Server.Output.ArtNet
{
    public class ArtnetFrameOutputConfig : FrameOutputBaseConfig
    {
        public IArtnetPatchConfig PatchConfig { get; }

        public ArtnetFrameOutputConfig(int width, int height, IArtnetPatchConfig patchConfig) : base(width, height)
        {
            PatchConfig = patchConfig;
        }
    }
}