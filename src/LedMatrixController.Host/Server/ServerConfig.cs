using System;
using LedMatrixController.Server;

namespace LedMatrixController.Host.Server
{
    public class ServerConfig : IHaveId
    {
        public int FrameRate { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Guid Id { get; set; }
    }
}