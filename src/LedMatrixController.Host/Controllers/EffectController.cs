using System;
using System.Collections.Generic;
using LedMatrixController.Server.Config.Effect;
using Microsoft.AspNetCore.Mvc;

namespace LedMatrixController.Host.Controllers
{
    public abstract class EffectController<TConfig> : ControllerBase
        where TConfig : EffectConfig, new()
    {
        //TODO: Implement Storage

        [HttpGet]
        public ActionResult<IEnumerable<TConfig>> Get()
        {
            return new TConfig[] { };
        }

        [HttpGet("{id}")]
        public ActionResult<TConfig> Get(Guid id)
        {
            return new TConfig() { Id = id };
        }

        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] TConfig value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
