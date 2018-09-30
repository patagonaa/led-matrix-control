using System.Threading.Tasks;

namespace LedMatrixController.Server.PipelineElements
{
    public interface IFilter<TIn, TOut>
    {
        Task<TOut> Execute(TIn item);
    }
}