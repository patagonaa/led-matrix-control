using System.Threading.Tasks;

namespace LedMatrixController.Server.Effect
{
    public interface IEffect
    {
        Task<Frame> GetFrame();
    }
}