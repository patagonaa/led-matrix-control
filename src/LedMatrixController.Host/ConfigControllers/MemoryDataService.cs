using LedMatrixController.Server;
using LedMatrixController.Server.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;

namespace LedMatrixController.Host.ConfigControllers
{
    public class MemoryDataService<T> : IDataService<T>
        where T : IHaveId
    {
        private readonly List<T> _storage = new List<T>();
        private readonly Subject<T> _updateSubject = new Subject<T>();

        public void Delete(Guid id)
        {
            _storage.RemoveAll(stored => stored.Id == id);
        }

        public T Get(Guid id)
        {
            return _storage.FirstOrDefault(stored => stored.Id == id);
        }

        public IList<T> Get()
        {
            return _storage;
        }

        public IObservable<T> GetUpdateObservable()
        {
            return _updateSubject;
        }

        public void Save(T model)
        {
            Delete(model.Id);
            _storage.Add(model);
            _updateSubject.OnNext(model);
        }
    }
}
