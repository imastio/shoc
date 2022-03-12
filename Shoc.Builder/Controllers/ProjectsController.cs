using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.Ext.Core;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Protection;
using Shoc.Builder.Model.Project;
using Shoc.Builder.Services;

namespace Shoc.Builder.Controllers
{
    /// <summary>
    /// The projects controller
    /// </summary>
    [Route("api/projects")]
    [ApiController]
    [ShocExceptionHandler]
    [AuthorizedSubject]
    public class ProjectsController : ControllerBase
    {
        /// <summary>
        /// The projects service
        /// </summary>
        private readonly ProjectService projectService;

        /// <summary>
        /// Creates new instance of projects controller
        /// </summary>
        /// <param name="projectService">The projects service</param>
        public ProjectsController(ProjectService projectService)
        {
            this.projectService = projectService;
        }

        /// <summary>
        /// Gets the projects by given filters
        /// </summary>
        /// <param name="all">Indicates if all the projects should be included</param>
        /// <param name="owner">The owner to filter with</param>
        /// <param name="directory">The directory to filter</param>
        /// <param name="name">The target name</param>
        /// <returns></returns>
        [HttpGet]
        public Task<IEnumerable<ProjectModel>> GetAll([FromQuery] bool all = false, [FromQuery] string owner = null, [FromQuery] string directory = null, [FromQuery] string name = null)
        {
            // the request principal
            var principal = this.HttpContext.GetShocPrincipal();

            return this.projectService.GetBy(principal, new ProjectQuery
            {
                OwnerId = all ? null : owner.OnBlank(principal.Subject),
                Name = name,
                Directory = directory
            });
        }

        /// <summary>
        /// Gets the project by id
        /// </summary>
        /// <param name="id">The id of project</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Task<ProjectModel> GetById(string id)
        {
            // try get result
            return this.projectService.GetById(this.HttpContext.GetShocPrincipal(), id);
        }

        /// <summary>
        /// Gets the versions of the project by id
        /// </summary>
        /// <param name="id">The id of project</param>
        /// <returns></returns>
        [HttpGet("{id}/versions")]
        public Task<IEnumerable<ProjectVersion>> GetVersions(string id)
        {
            // try get result
            return this.projectService.GetVersions(this.HttpContext.GetShocPrincipal(), id);
        }

        /// <summary>
        /// Creates a project with the given input
        /// </summary>
        /// <param name="input">The project input</param>
        /// <returns></returns>
        [HttpPost]
        public Task<ProjectModel> Create([FromBody] CreateProjectModel input)
        {
            return this.projectService.Create(this.HttpContext.GetShocPrincipal(), input);
        }
        
        /// <summary>
        /// Deletes a project with the given id
        /// </summary>
        /// <param name="id">The id of project to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task<ProjectModel> DeleteById(string id)
        {
            return this.projectService.DeleteById(this.HttpContext.GetShocPrincipal(), id);
        }
    }
}
