using System;
using System.Collections.Generic;

namespace LedMatrixController.Server.Queue.Config
{
    public class QueueConfigModel : IHaveId
    {
        public QueueConfigModel()
        {
            QueueElements = new List<QueueElementConfig>();
        }

        public Guid Id { get; set; }
        public IList<QueueElementConfig> QueueElements { get; set; }
    }
}
