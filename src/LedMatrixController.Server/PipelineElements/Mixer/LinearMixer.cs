using System;
using System.Threading.Tasks;

namespace LedMatrixController.Server.PipelineElements.Mixer
{
    public class LinearMixer : IMixer
    {
        private double _mixerLevel = 0;
        private readonly int _width;
        private readonly int _height;
        private ISource<Frame> _input1;
        private ISource<Frame> _input2;

        public LinearMixer(int width, int height, ISource<Frame> input1, ISource<Frame> input2)
        {
            _width = width;
            _height = height;
            _input1 = input1 ?? throw new ArgumentNullException(nameof(input1));
            _input2 = input2 ?? throw new ArgumentNullException(nameof(input2));
        }

        public async Task<Frame> Pop()
        {
            var frames = await Task.WhenAll(_input1.Pop(), _input2.Pop());
            FrameHelper.EnsureValid(frames[0], _width, _height);
            FrameHelper.EnsureValid(frames[1], _width, _height);

            var outputPixels = new Color[_width * _height];

            for (int i = 0; i < _width * _height; i++)
            {
                outputPixels[i] = MixColors(frames[0].Pixels[i], frames[1].Pixels[i], _mixerLevel);
            }

            return new Frame(_width, _height, outputPixels);
        }

        private Color MixColors(Color c1, Color c2, double val){
            var r = (c1.R * val) + (c2.R * (1 - val));
            var g = (c1.G * val) + (c2.G * (1 - val));
            var b = (c1.B * val) + (c2.B * (1 - val));
            return new Color((byte)r, (byte)g, (byte)b);
        }

        public void SetInput1(ISource<Frame> input)
        {
            _input1 = input ?? throw new ArgumentNullException(nameof(input));
        }

        public void SetInput2(ISource<Frame> input)
        {
            _input2 = input ?? throw new ArgumentNullException(nameof(input));
        }

        public void SetMixerLevel(double level)
        {
            if(level < 0 || level > 1){
                throw new ArgumentException("Mixer Level must be between 0 and 1", nameof(level));
            }

            _mixerLevel = level;
        }

        public void Dispose()
        {
        }
    }
}