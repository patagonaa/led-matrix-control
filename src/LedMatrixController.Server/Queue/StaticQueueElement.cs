using LedMatrixController.Server.PipelineElements;
using System;

namespace LedMatrixController.Server.Queue
{
    public class StaticQueueElement : IQueueElement
    {
        public StaticQueueElement(ISource<Frame> source, TimeSpan duration)
        {
            Source = source;
            Duration = duration;
        }

        public ISource<Frame> Source { get; }
        public TimeSpan Duration { get; }

        public void Init()
        {
        }
    }
}
