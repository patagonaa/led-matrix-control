using System.Reactive.Subjects;

namespace LedMatrixController.Host.Endpoints.MixerControl
{
    internal class MainMixerControls
    {
        public BehaviorSubject<double> MixerValue { get; } = new BehaviorSubject<double>(0);
    }
}