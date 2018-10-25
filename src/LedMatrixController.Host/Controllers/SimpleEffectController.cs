using LedMatrixController.Server.Config.Effect;
using Microsoft.AspNetCore.Mvc;

namespace LedMatrixController.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimpleEffectController : EffectController<SimpleEffectConfig>
    {
    }
}
