using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Shoc.Webgtw.Controllers
{
    [Route("discovery")]
    [ApiController]
    public class DiscoveryController : ControllerBase
    {
        /// <summary>
        /// The configuration provider.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// The constructor.
        /// </summary>
        public DiscoveryController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Get public endpoints for identity and api
        /// </summary>
        /// <returns></returns>
        [HttpGet("endpoints")]
        public Task<object> Get()
        {
            return Task.FromResult<object>(new
            {
                Authority = configuration.GetValue<string>("Auth:Authority"),
                Api = configuration.GetValue<string>("Self:ExternalBaseAddress")
            });
        }
    }
}
