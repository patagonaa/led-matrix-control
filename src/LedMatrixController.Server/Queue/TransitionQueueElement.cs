using LedMatrixController.Server.PipelineElements;
using LedMatrixController.Server.PipelineElements.Mixer;
using System;

namespace LedMatrixController.Server.Queue
{
    public class TransitionQueueElement : IQueueElement
    {
        public TransitionQueueElement(ISource<Frame> source, TimeSpan duration, IMixer mixer)
        {
            TargetSource = source;
            Mixer = mixer;
            Duration = duration;
        }
        
        public TimeSpan Duration { get; }
        public IMixer Mixer { get; }

        public ISource<Frame> TargetSource { get; }
        public ISource<Frame> Source => Mixer;

        public void Init()
        {
            Mixer.SetMixerLevel(0);
            Mixer.SetInput2(TargetSource);
        }
    }
}
