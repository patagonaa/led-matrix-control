using LedMatrixController.Server.Config;
using LedMatrixController.Server.PipelineElements;
using LedMatrixController.Server.PipelineElements.Source;
using LedMatrixController.Server.Queue.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LedMatrixController.Server.Queue
{
    public class Runner
    {
        private readonly IOutputSize _outputSize;
        private readonly Guid _queueId;
        private readonly IDataService<QueueConfigModel> _queueConfigService;
        private readonly IQueueElementFactory _queueElementFactory;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private volatile Queue<IQueueElement> _queue = new Queue<IQueueElement>();

        public Runner(IOutputSize outputSize, IDataService<QueueConfigModel> queueConfigService, IQueueElementFactory queueElementFactory)
        {
            Sinks = new List<ISink<Frame>>();
            _outputSize = outputSize;

            _queueId = Guid.Parse("00000000-0000-0000-0000-000000000000");
            _queueConfigService = queueConfigService;
            _queueElementFactory = queueElementFactory;
        }

        public IList<ISink<Frame>> Sinks { get; }

        public Task Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            UpdateQueue(_queueConfigService.Get(_queueId));
            _queueConfigService.GetUpdateObservable().Subscribe(Observer.Create<QueueConfigModel>(UpdateQueue));
            Task.Run(() => Main());
            return Task.CompletedTask;
        }

        public void UpdateQueue(QueueConfigModel queueConfig)
        {
            if(queueConfig?.Id != _queueId)
            {
                return;
            }

            var queue = new Queue<IQueueElement>();
            foreach (var elem in queueConfig.QueueElements)
            {
                queue.Enqueue(_queueElementFactory.Provide(elem));
            }

            _queue = queue;
        }

        public Task Stop()
        {
            _cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }

        private async void Main()
        {
            ISource<Frame> currentSource = new FlatColor(_outputSize, new FlatColorConfig { Color = "000000" });
            DateTime elementStartTime = DateTime.UtcNow;
            IQueueElement currentQueueElement = new StaticQueueElement(currentSource, TimeSpan.Zero);

            ISource<Frame> lastSource = currentSource;

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                var queue = _queue;

                DateTime currentTime = DateTime.UtcNow;
                
                SwitchQueueElement(ref currentQueueElement, ref lastSource, queue, ref elementStartTime, currentTime);

                if (currentQueueElement is StaticQueueElement staticQueueElement)
                {
                }
                else if (currentQueueElement is TransitionQueueElement transitionQueueElement)
                {
                    var durationElapsed = (currentTime - elementStartTime);
                    var x = durationElapsed.TotalMilliseconds / transitionQueueElement.Duration.TotalMilliseconds;
                    if (x > 1)
                        x = 1;
                    transitionQueueElement.Mixer.SetInput1(lastSource);
                    transitionQueueElement.Mixer.SetMixerLevel(x);
                }
                else
                {
                    throw new InvalidOperationException("Invalid TransitionQueueElement");
                }

                var frame = await currentQueueElement.Source.Pop();
                await Task.WhenAll(Sinks.Select(x => x.Push(frame)));
                await Task.Delay(1000 / 30);
            }
        }

        private void SwitchQueueElement(ref IQueueElement currentQueueElement, ref ISource<Frame> lastSource, Queue<IQueueElement> executionQueue, ref DateTime elementStartTime, DateTime currentTime)
        {
            if ((currentTime - elementStartTime) >= currentQueueElement.Duration && executionQueue.Count > 0)
            {
                lastSource = currentQueueElement is TransitionQueueElement transition 
                    ? transition.TargetSource 
                    : currentQueueElement.Source;
                currentQueueElement = executionQueue.Dequeue();
                executionQueue.Enqueue(currentQueueElement);
                elementStartTime = currentTime;
                currentQueueElement.Init();
            }
        }
    }
}
