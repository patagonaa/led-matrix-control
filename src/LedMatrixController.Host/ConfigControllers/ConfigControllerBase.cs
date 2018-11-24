using LedMatrixController.Server;
using LedMatrixController.Server.Config;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LedMatrixController.Host.ConfigControllers
{
    public class ConfigController<TModel> : Hub
        where TModel : class, IHaveId, new()
    {
        private readonly IDataService<TModel> _dataService;

        public ConfigController(IDataService<TModel> dataService)
        {
            _dataService = dataService;
        }

        public Task<TModel> Get(Guid id)
        {
            return Task.FromResult(_dataService.Get(id) ?? new TModel() { Id = id });
        }

        public Task<IList<TModel>> GetList()
        {
            return Task.FromResult(_dataService.Get());
        }

        public Task Save(TModel model)
        {
            _dataService.Save(model);
            Clients.All.SendAsync("Update", model);
            return Task.CompletedTask;
        }

        public Task Delete(Guid id)
        {
            _dataService.Delete(id);
            Clients.All.SendAsync("Delete", id);
            return Task.CompletedTask;
        }
    }
}
