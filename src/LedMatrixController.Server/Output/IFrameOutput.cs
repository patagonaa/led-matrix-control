using System;
using System.Threading.Tasks;

namespace LedMatrixController.Server.Output
{
    public interface IFrameOutput : IDisposable
    {
        Task Output(Frame frame);
    }
}