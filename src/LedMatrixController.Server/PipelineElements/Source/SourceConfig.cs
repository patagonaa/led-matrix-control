using System;

namespace LedMatrixController.Server.PipelineElements.Source
{
    public abstract class SourceConfig : IHaveId
    {
        public Guid Id { get; set; }
    }
}
