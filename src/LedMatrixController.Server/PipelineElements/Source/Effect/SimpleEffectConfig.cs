using System;

namespace LedMatrixController.Server.Config.Effect
{
    public class SimpleEffectConfig : EffectConfig
    {
        public string SourceName { get; set; }
        public Guid SourceConfigId { get; set; }
    }
}
