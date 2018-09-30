using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LedMatrixController.Host.Endpoints.MixerControl
{
    internal class MixerControlHub : Hub
    {
        private readonly MainMixerControls _mixerControls;

        public MixerControlHub(MainMixerControls mixerControls)
        {
            _mixerControls = mixerControls;
        }

        public async Task GetMixerValue(){
            await Clients.Caller.SendAsync("MixerValueUpdated", _mixerControls.MixerValue.Value);
        }

        public async Task UpdateMixerValue(double value)
        {
            _mixerControls.MixerValue.OnNext(value);
            await Clients.Others.SendAsync("MixerValueUpdated", value);
        }
    }
}