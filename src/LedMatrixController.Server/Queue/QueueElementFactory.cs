using LedMatrixController.Server.PipelineElements.Mixer;
using LedMatrixController.Server.Queue.Config;
using System;

namespace LedMatrixController.Server.Queue
{
    public class QueueElementFactory : IQueueElementFactory
    {
        private readonly ISourceFactory _sourceFactory;
        private readonly IOutputSize _outputSize;

        public QueueElementFactory(ISourceFactory sourceFactory, IOutputSize outputSize)
        {
            _sourceFactory = sourceFactory;
            _outputSize = outputSize;
        }

        public IQueueElement Provide(QueueElementConfig queueElement)
        {
            var source = _sourceFactory.Provide(queueElement.SourceName, queueElement.SourceConfigId);

            switch (queueElement.Type)
            {
                case QueueElementType.Static:
                    return new StaticQueueElement(source, TimeSpan.FromMilliseconds(queueElement.Duration));
                case QueueElementType.LinearMixer:
                    return new TransitionQueueElement(source, TimeSpan.FromMilliseconds(queueElement.Duration), new LinearMixer(_outputSize));
                default:
                    throw new ArgumentException("unhandled queue element type");
            }
        }
    }
}
