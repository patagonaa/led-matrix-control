using System.Net;

namespace LedMatrixController.Server.Output.ArtNet
{
    public class ArtDmxPacket
    {
        public ArtDmxPacket(IPAddress ip, ushort port, byte[] values)
        {
            Ip = ip;
            Port = port;
            Values = values;
        }
        public IPAddress Ip { get; }
        public ushort Port { get; }
        public byte[] Values { get; }
    }
}