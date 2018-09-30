namespace LedMatrixController.Server.PipelineElements.Mixer
{
    public interface IMixer : ISource<Frame>
    {
        void SetInput1(ISource<Frame> input);
        void SetInput2(ISource<Frame> input);
        void SetMixerLevel(double level);
    }
}