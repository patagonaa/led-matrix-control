using Microsoft.AspNetCore.SignalR;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace LedMatrixController.Host.Endpoints
{
    public class SliderControlHub<TControl> : Hub
        where TControl : SliderControl
    {
        private readonly SliderControl _sliderControl;

        public SliderControlHub(TControl sliderControl)
        {
            _sliderControl = sliderControl;
        }

        public Task<double> GetValue()
        {
            return Task.FromResult(_sliderControl.Value.Value);
        }

        public async Task UpdateValue(double value)
        {
            _sliderControl.Value.OnNext(value);
            await Clients.Others.SendAsync("ValueUpdated", value);
        }
    }

    public abstract class SliderControl
    {
        public BehaviorSubject<double> Value { get; } = new BehaviorSubject<double>(0);
    }
}
