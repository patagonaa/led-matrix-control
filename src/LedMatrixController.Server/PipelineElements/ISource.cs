using System;
using System.Threading.Tasks;

namespace LedMatrixController.Server.PipelineElements
{
    public interface ISource<T> : IDisposable
    {
        Task<T> Pop();
    }
}