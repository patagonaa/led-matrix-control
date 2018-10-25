using LedMatrixController.Server.Config.Source;
using LedMatrixController.Server.Effect.FlatColor;
using LedMatrixController.Server.Effect.Rainbow;
using LedMatrixController.Server.PipelineElements;
using LedMatrixController.Server.PipelineElements.Mixer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LedMatrixController.Server
{
    public class Runner
    {
        private readonly int _width;
        private readonly int _height;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public Runner(int width, int height)
        {
            Sinks = new List<ISink<Frame>>();
            _width = width;
            _height = height;
        }

        public IList<ISink<Frame>> Sinks { get; }

        private void TestInit(Queue<IQueueElement> queue)
        {
            var red = new FlatColor(_width, _height, new FlatColorConfig { Color = "FF0000"});
            var green = new FlatColor(_width, _height, new FlatColorConfig { Color = "00FF00"});
            var blue = new FlatColor(_width, _height, new FlatColorConfig { Color = "0000FF"});
            var rainbow = new Rainbow(_width, _height);
            var mixer = new LinearMixer(_width, _height);
            queue.Enqueue(new TransitionQueueElement(red, green, mixer, TimeSpan.FromMilliseconds(1000)));
            queue.Enqueue(new TransitionQueueElement(green, blue, mixer, TimeSpan.FromMilliseconds(1000)));
            queue.Enqueue(new TransitionQueueElement(blue, red, mixer, TimeSpan.FromMilliseconds(1000)));
            queue.Enqueue(new TransitionQueueElement(red, rainbow, mixer, TimeSpan.FromMilliseconds(1000)));
            queue.Enqueue(new StaticQueueElement(rainbow, TimeSpan.FromMilliseconds(2000)));
            queue.Enqueue(new TransitionQueueElement(rainbow, red, mixer, TimeSpan.FromMilliseconds(1000)));
        }

        public Task Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => Main());
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            _cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }

        private async void Main()
        {
            var executionQueue = new Queue<IQueueElement>();
            TestInit(executionQueue);

            ISource<Frame> currentSource = new FlatColor(_width, _height, new FlatColorConfig { Color = "000000" });
            DateTime elementStartTime = DateTime.UtcNow;
            IQueueElement currentQueueElement = new StaticQueueElement(currentSource, TimeSpan.Zero);

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                DateTime currentTime = DateTime.UtcNow;

                SwitchQueueElement(ref currentQueueElement, executionQueue, ref elementStartTime, currentTime);

                if (currentQueueElement is StaticQueueElement staticQueueElement)
                {
                }
                else if (currentQueueElement is TransitionQueueElement transitionQueueElement)
                {
                    var durationElapsed = (currentTime - elementStartTime);
                    var x = durationElapsed.TotalMilliseconds / transitionQueueElement.Duration.TotalMilliseconds;
                    if (x > 1)
                        x = 1;
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

        private void SwitchQueueElement(ref IQueueElement currentQueueElement, Queue<IQueueElement> executionQueue, ref DateTime elementStartTime, DateTime currentTime)
        {
            if ((currentTime - elementStartTime) >= currentQueueElement.Duration && executionQueue.Count > 0)
            {
                currentQueueElement = executionQueue.Dequeue();
                executionQueue.Enqueue(currentQueueElement);
                elementStartTime = currentTime;
                currentQueueElement.Init();
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

        public void Init()
        {
        }
    }

    public class TransitionQueueElement : IQueueElement
    {
        public TransitionQueueElement(ISource<Frame> source1, ISource<Frame> source2, IMixer mixer, TimeSpan duration)
        {
            _source1 = source1;
            _source2 = source2;
            Mixer = mixer;
            Duration = duration;
        }

        private readonly ISource<Frame> _source1;
        private readonly ISource<Frame> _source2;
        public TimeSpan Duration { get; }
        public IMixer Mixer { get; }
        

        public ISource<Frame> Source => Mixer;

        public void Init()
        {
            Mixer.SetInput1(_source1);
            Mixer.SetMixerLevel(0);
            Mixer.SetInput2(_source2);
        }
    }

    public interface IQueueElement
    {
        TimeSpan Duration { get; }
        ISource<Frame> Source { get; }
        void Init();
    }
}
