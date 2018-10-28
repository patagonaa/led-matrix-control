using System;

namespace LedMatrixController.Server.Config.Source
{
    public abstract class SourceConfig : IHaveId
    {
        public Guid Id { get; set; }
    }
}
