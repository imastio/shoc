using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.Builder.Services;
using Shoc.Engine.Model;

namespace Shoc.Builder.Controllers
{
    /// <summary>
    /// The engines controller
    /// </summary>
    [Route("api/engines")]
    [ApiController]
    [ShocExceptionHandler]
    public class EnginesController : ControllerBase
    {
        /// <summary>
        /// The container engine service
        /// </summary>
        private readonly EngineService engineService;

        /// <summary>
        /// Creates new instance of engines controller
        /// </summary>
        /// <param name="engineService">The engine service</param>
        public EnginesController(EngineService engineService)
        {
            this.engineService = engineService;
        }

        /// <summary>
        /// Gets all the payers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<IEnumerable<EngineInstanceInfo>> GetAll()
        {
            return this.engineService.GetAll();
        }
    }
}
