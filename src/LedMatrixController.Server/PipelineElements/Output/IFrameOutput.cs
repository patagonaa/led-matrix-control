using System;
using System.Threading.Tasks;
using LedMatrixController.Server.PipelineElements;

namespace LedMatrixController.Server.Output
{
    public interface IFrameOutput : ISink<Frame>, IDisposable
    {
    }
}