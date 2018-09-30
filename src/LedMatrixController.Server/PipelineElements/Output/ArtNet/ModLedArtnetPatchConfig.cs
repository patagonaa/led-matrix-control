using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LedMatrixController.Server.Output.ArtNet
{
    public class ModLedArtnetPatchConfig : IArtnetPatchConfig
    {
        public int _width;
        public int _height;
        public IPAddress _ip;

        public ModLedArtnetPatchConfig(int width, int height, IPAddress ip)
        {
            _width = width;
            _height = height;
            _ip = ip;
        }

        public IList<ArtDmxPacket> GetPackets(Frame frame)
        {
            var numPixels = _width * _height;
            var packetPixels = 16 * 8;
            var numPackets = numPixels / packetPixels;

            var toReturn = new List<ArtDmxPacket>();
            for (int i = 0; i < numPackets; i++)
            {
                toReturn.Add(new ArtDmxPacket(_ip, (ushort)(i + 1), frame.Pixels.Skip(i * packetPixels).Take(packetPixels).SelectMany(x => new[] { x.R, x.G, x.B }).ToArray()));
            }

            return toReturn;
        }
    }
}