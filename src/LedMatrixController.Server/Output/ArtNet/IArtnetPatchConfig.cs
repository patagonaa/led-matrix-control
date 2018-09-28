using System.Collections.Generic;

namespace LedMatrixController.Server.Output.ArtNet
{
    public interface IArtnetPatchConfig
    {
        IList<ArtDmxPacket> GetPackets(Frame frame);
    }
}