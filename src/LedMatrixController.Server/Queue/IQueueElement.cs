using LedMatrixController.Server.PipelineElements;
using System;

namespace LedMatrixController.Server.Queue
{
    public interface IQueueElement
    {
        TimeSpan Duration { get; }
        ISource<Frame> Source { get; }
        void Init();
    }
}
