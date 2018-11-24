using System;

namespace LedMatrixController.Server.Queue.Config
{
    public class QueueElementConfig
    {
        public Guid Id { get; set; }
        public string SourceName { get; set; }
        public Guid SourceConfigId { get; set; }
        public QueueElementType Type { get; set; }
        public int Duration { get; set; } // ms
    }
}