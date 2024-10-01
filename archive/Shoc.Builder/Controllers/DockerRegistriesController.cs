using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Protection;
using Shoc.Builder.Model.Registry;
using Shoc.Builder.Services;
using Shoc.Identity.Model;

namespace Shoc.Builder.Controllers
{
    /// <summary>
    /// The docker registries controller
    /// </summary>
    [Route("api/docker-registries")]
    [ApiController]
    [ShocExceptionHandler]
    [AuthorizeMinUserType(UserTypes.ADMIN)]
    public class DockerRegistriesController : ControllerBase
    {
        /// <summary>
        /// The projects service
        /// </summary>
        private readonly DockerRegistryService dockerRegistryService;

        /// <summary>
        /// Creates new instance of projects controller
        /// </summary>
        /// <param name="dockerRegistryService">The docker registry service</param>
        public DockerRegistriesController(DockerRegistryService dockerRegistryService)
        {
            this.dockerRegistryService = dockerRegistryService;
        }

        /// <summary>
        /// Gets all the objects
        /// </summary>
        /// <param name="name">The name of the registry</param>
        /// <returns></returns>
        [HttpGet]
        public Task<IEnumerable<DockerRegistry>> GetAll([FromQuery] string name = null)
        {
            // the request principal
            var principal = this.HttpContext.GetShocPrincipal();

            // get the entities by owner
            return this.dockerRegistryService.GetBy(principal, new DockerRegistryQuery
            {
                Name = name
            });
        }

        /// <summary>
        /// Gets the object by id
        /// </summary>
        /// <param name="id">The id of target object</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Task<DockerRegistry> GetById(string id)
        {
            // try get result
            return this.dockerRegistryService.GetById(this.HttpContext.GetShocPrincipal(), id);
        }

        /// <summary>
        /// Creates an object with the given input
        /// </summary>
        /// <param name="input">The entity creation input</param>
        /// <returns></returns>
        [HttpPost]
        public Task<DockerRegistry> Create([FromBody] CreateDockerRegistry input)
        {
            return this.dockerRegistryService.Create(this.HttpContext.GetShocPrincipal(), input);
        }

        /// <summary>
        /// Deletes a entity with the given id
        /// </summary>
        /// <param name="id">The id of entity to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task<DockerRegistry> DeleteById(string id)
        {
            return this.dockerRegistryService.DeleteById(this.HttpContext.GetShocPrincipal(), id);
        }
    }
}
