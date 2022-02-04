using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Protection;
using Shoc.Builder.Model;
using Shoc.Builder.Services;

namespace Shoc.Builder.Controllers
{
    /// <summary>
    /// The owned projects controller
    /// </summary>
    [Route("api/my-projects")]
    [ApiController]
    [ShocExceptionHandler]
    [AuthorizedSubject]
    public class MyProjectsController : ControllerBase
    {
        /// <summary>
        /// The projects service
        /// </summary>
        private readonly ProjectService projectService;
        
        /// <summary>
        /// Creates new instance of projects controller
        /// </summary>
        /// <param name="projectService">The projects service</param>
        public MyProjectsController(ProjectService projectService)
        {
            this.projectService = projectService;
        }

        /// <summary>
        /// Gets all the projects owned by current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<IEnumerable<ProjectModel>> GetAll()
        {
            // get all by owner
            return this.projectService.GetAllOwned(this.HttpContext.GetShocPrincipal());
        }

        /// <summary>
        /// Gets the projects by id of the current user
        /// </summary>
        /// <param name="id">The id of project</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            // try get result
            var result = await this.projectService.GetOwnedById(this.HttpContext.GetShocPrincipal(), id);

            // no such an object
            if (result == null)
            {
                return NotFound();
            }

            // return found object
            return Ok(result);
        }

        /// <summary>
        /// Creates a project with the given input
        /// </summary>
        /// <param name="input">The project input</param>
        /// <returns></returns>
        [HttpPost]
        public Task<ProjectModel> Create([FromBody] CreateUpdateProjectModel input)
        {
            return this.projectService.CreateOwned(this.HttpContext.GetShocPrincipal(), input);
        }

        /// <summary>
        /// Updates a project with the given input
        /// </summary>
        /// <param name="id">The id of project to update</param>
        /// <param name="input">The project input</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public Task<ProjectModel> Update(string id, [FromBody] CreateUpdateProjectModel input)
        {
            return this.projectService.UpdateOwned(this.HttpContext.GetShocPrincipal(), id, input);
        }

        /// <summary>
        /// Deletes a project with the given id
        /// </summary>
        /// <param name="id">The id of project to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task<ProjectModel> DeleteById(string id)
        {
            return this.projectService.DeleteOwnedById(this.HttpContext.GetShocPrincipal(), id);
        }
    }
}
