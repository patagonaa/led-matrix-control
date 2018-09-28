using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LedMatrixController.Server.Output.ArtNet
{
    public class ArtnetSender : IDisposable
    {
        private UdpClient _udpClient;

        public ArtnetSender()
        {
            _udpClient = new UdpClient(6454);
        }

        public async Task Send(ArtDmxPacket artDmxPacket)
        {
            if(artDmxPacket.Values.Length > 512)
                throw new ArgumentException("ArtDmxPacket must not have more than 512 values!", nameof(artDmxPacket));

            int headerLen = 18;

            byte[] packet = new byte[headerLen + artDmxPacket.Values.Length];

            // fixed header
            Array.Copy(Encoding.ASCII.GetBytes("Art-Net"), 0, packet, 0, 7);
            //packet[7] = 0;

            // opcode 0x5000
            packet[8] = 0x00;
            packet[9] = 0x50;

            // protocol version 0x000E
            packet[10] = 0x00;
            packet[11] = 0x0E;

            // sequence
            packet[12] = 0x00;

            // physical
            packet[13] = 0x00;

            // port
            packet[14] = (byte)(artDmxPacket.Port & 0xFF);
            packet[15] = (byte)(artDmxPacket.Port >> 8);

            //length
            packet[16] = (byte)(artDmxPacket.Values.Length >> 8);
            packet[17] = (byte)(artDmxPacket.Values.Length & 0xFF);

            Array.Copy(artDmxPacket.Values, 0, packet, 18, artDmxPacket.Values.Length);

            await _udpClient.SendAsync(packet, packet.Length, new IPEndPoint(artDmxPacket.Ip, 6454));
        }

        public void Dispose()
        {
            _udpClient.Dispose();
        }
    }
}