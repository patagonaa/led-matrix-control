using System;

namespace LedMatrixController.Server
{
    public static class FrameHelper
    {
        public static void EnsureValid(Frame frame, int width, int height)
        {
            if (frame == null)
                throw new InvalidOperationException("Frame must not be null!");

            if (frame.Width != width || frame.Height != height)
                throw new InvalidOperationException($"Wrong frame size! Expected {width}x{height}, got {frame.Width}x{frame.Height}");
        }
    }
}
