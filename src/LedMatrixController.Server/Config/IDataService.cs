using System;
using System.Collections.Generic;

namespace LedMatrixController.Server.Config
{
    public interface IDataService<TModel>
        where TModel : IHaveId
    {
        TModel Get(Guid id);
        IList<TModel> Get();
        void Save(TModel model);
        void Delete(Guid id);
        IObservable<TModel> GetUpdateObservable();
    }
}
