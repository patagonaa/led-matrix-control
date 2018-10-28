using LedMatrixController.Server;
using System;
using System.Collections.Generic;

namespace LedMatrixController.Host.Models
{
    public class SourcesModel : IHaveId
    {
        public SourcesModel()
        {
            Sources = new List<SourceListEntry>();
        }

        public Guid Id { get; set; }
        public IList<SourceListEntry> Sources { get; set; }
    }
}
