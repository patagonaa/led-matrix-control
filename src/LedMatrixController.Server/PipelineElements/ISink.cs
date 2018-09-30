using System;
using System.Threading.Tasks;

namespace LedMatrixController.Server.PipelineElements
{
    public interface ISink<T> : IDisposable
    {
        Task Push(T item);
    }
}