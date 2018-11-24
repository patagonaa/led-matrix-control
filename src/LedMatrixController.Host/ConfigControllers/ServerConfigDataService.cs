using LedMatrixController.Host.Server;
using LedMatrixController.Server.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace LedMatrixController.Host.ConfigControllers
{
    public class ServerConfigDataService : IDataService<ServerConfig>
    {
        private ServerConfig _serverConfig;

        public ServerConfigDataService(IOptions<ServerConfig> serverConfig)
        {
            _serverConfig = serverConfig.Value;
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public ServerConfig Get(Guid id)
        {
            return _serverConfig;
        }

        public IList<ServerConfig> Get()
        {
            throw new NotImplementedException();
        }

        public IObservable<ServerConfig> GetUpdateObservable()
        {
            throw new NotImplementedException();
        }

        public void Save(ServerConfig model)
        {
            throw new NotImplementedException();
        }
    }
}
