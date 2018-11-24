using LedMatrixController.Server.Config;
using LedMatrixController.Server.PipelineElements;
using LedMatrixController.Server.PipelineElements.Source;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LedMatrixController.Server.Queue
{
    public interface ISourceFactory
    {
        ISource<Frame> Provide(string sourceName, Guid sourceConfigId);
    }

    public class SourceFactory : ISourceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SourceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISource<Frame> Provide(string sourceName, Guid sourceConfigId)
        {
            //TODO: this could eventually be replaced by refelction magic

            var outputSize = _serviceProvider.GetRequiredService<IOutputSize>();
            switch (sourceName)
            {
                case "flat-color":
                    return new FlatColor(outputSize, sourceConfigId, _serviceProvider.GetRequiredService<IDataService<FlatColorConfig>>());
                case "rainbow":
                    return new Rainbow(outputSize);
                default:
                    throw new ArgumentException($"source name {sourceName} not handled");
            }
        }
    }
}