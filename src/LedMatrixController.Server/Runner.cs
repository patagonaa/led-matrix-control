using LedMatrixController.Server.PipelineElements;
using LedMatrixController.Server.PipelineElements.Mixer;
using System;
using System.Collections.Generic;
using System.Text;

namespace LedMatrixController.Server
{
    public class Runner
    {
        public Runner()
        {
            Sinks = new List<ISink<Frame>>();
        }

        public IList<ISink<Frame>> Sinks { get; }

        private void Main()
        {
            var executionQueue = new Queue<IQueueElement>();

            while (true)
            {

            }
        }
    }

    public class StaticQueueElement : IQueueElement
    {
        public StaticQueueElement(ISource<Frame> source, TimeSpan duration)
        {
            Source = source;
            Duration = duration;
        }

        public ISource<Frame> Source { get; }
        public TimeSpan Duration { get; }
    }

    public class TransitionQueueElement : IQueueElement
    {
        public TransitionQueueElement(ISource<Frame> source1, ISource<Frame> source2, IMixer mixer, int duration)
        {
            Source1 = source1;
            Source2 = source2;
            Mixer = mixer;
            Duration = duration;
        }

        public ISource<Frame> Source1 { get; }
        public ISource<Frame> Source2 { get; }
        public IMixer Mixer { get; }
        public int Duration { get; }
    }

    public interface IQueueElement
    {
    }
}
