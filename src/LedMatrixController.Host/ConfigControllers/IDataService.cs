using LedMatrixController.Server;
using System;

namespace LedMatrixController.Host.ConfigControllers
{
    public interface IDataService<TModel>
        where TModel : IHaveId
    {
        TModel Get(Guid id);
        void Save(TModel model);
        void Delete(Guid id);
        IObservable<TModel> GetUpdateObservable();
    }
}
