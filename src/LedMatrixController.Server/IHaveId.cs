using System;

namespace LedMatrixController.Server
{
    public interface IHaveId
    {
        Guid Id { get; set; }
    }
}
