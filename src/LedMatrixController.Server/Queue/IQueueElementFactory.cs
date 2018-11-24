using LedMatrixController.Server.Queue.Config;

namespace LedMatrixController.Server.Queue
{
    public interface IQueueElementFactory
    {
        IQueueElement Provide(QueueElementConfig queueElement);
    }
}