using System;
using System.Threading.Tasks;

namespace LedMatrixController.Server.PipelineElements.Mixer
{
    public class LinearMixer : IMixer
    {
        private double _mixerLevel = 0;
        private readonly IOutputSize _outputSize;
        private ISource<Frame> _input1;
        private ISource<Frame> _input2;

        public LinearMixer(IOutputSize outputSize)
        {
            _outputSize = outputSize;
        }

        public LinearMixer(IOutputSize outputSize, ISource<Frame> input1, ISource<Frame> input2)
        {
            _outputSize = outputSize;
            _input1 = input1 ?? throw new ArgumentNullException(nameof(input1));
            _input2 = input2 ?? throw new ArgumentNullException(nameof(input2));
        }

        public async Task<Frame> Pop()
        {
            if (_input1 == null)
                throw new ArgumentNullException();
            if (_input2 == null)
                throw new ArgumentNullException();

            var width = _outputSize.Width;
            var height = _outputSize.Height;

            var frames = await Task.WhenAll(_input1.Pop(), _input2.Pop());
            FrameHelper.EnsureValid(frames[0], width, height);
            FrameHelper.EnsureValid(frames[1], width, height);

            var outputPixels = new Color[width * height];

            for (int i = 0; i < width * height; i++)
            {
                outputPixels[i] = MixColors(frames[0].Pixels[i], frames[1].Pixels[i], _mixerLevel);
            }

            return new Frame(width, height, outputPixels);
        }

        private Color MixColors(Color c1, Color c2, double val)
        {
            var r = (c1.R * (1 - val)) + (c2.R * val);
            var g = (c1.G * (1 - val)) + (c2.G * val);
            var b = (c1.B * (1 - val)) + (c2.B * val);
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
            if (level < 0 || level > 1)
            {
                throw new ArgumentException("Mixer Level must be between 0 and 1", nameof(level));
            }

            _mixerLevel = level;
        }

        public void Dispose()
        {
        }
    }
}