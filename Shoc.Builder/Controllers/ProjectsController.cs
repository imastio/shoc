using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Protection;
using Shoc.Builder.Model.Project;
using Shoc.Builder.Services;
using Shoc.Identity.Model;

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
        /// <param name="name">The target name</param>
        /// <returns></returns>
        [HttpGet]
        public Task<IEnumerable<ProjectModel>> GetAll([FromQuery] string name = null)
        {
            // the request principal
            var principal = this.HttpContext.GetShocPrincipal();

            return this.projectService.GetBy(new ProjectQuery
            {
                OwnerId = principal.Subject,
                Name = name
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
            // get the principal
            var principal = this.HttpContext.GetShocPrincipal();

            // try get result
            return this.projectService.GetById(principal.Subject, id);
        }

        /// <summary>
        /// Gets the project by id
        /// </summary>
        /// <param name="id">The id of project</param>
        /// <param name="ownerId">The owner id of project</param>
        /// <returns></returns>
        [HttpGet("{id}/by-owner/{ownerId}")]
        [AuthorizeMinUserType(UserTypes.ADMIN, AllowInsiders = true)]
        public Task<ProjectModel> GetInternalById(string id, string ownerId)
        {
            // try get result
            return this.projectService.GetById(ownerId, id);
        }

        /// <summary>
        /// Gets the versions of the project by id
        /// </summary>
        /// <param name="id">The id of project</param>
        /// <param name="version">The version of project</param>
        /// <returns></returns>
        [HttpGet("{id}/versions")]
        public Task<IEnumerable<ProjectVersion>> GetVersions(string id, [FromQuery] string version = null)
        {
            // get the principal
            var principal = this.HttpContext.GetShocPrincipal();

            // try get result
            return this.projectService.GetVersions(principal.Subject, id, version);
        }

        /// <summary>
        /// Creates a project with the given input
        /// </summary>
        /// <param name="input">The project input</param>
        /// <returns></returns>
        [HttpPost]
        public Task<ProjectModel> Create([FromBody] CreateProjectModel input)
        {
            // get the principal
            var principal = this.HttpContext.GetShocPrincipal();

            // set owner id
            input.OwnerId = principal.Subject;

            return this.projectService.Create(input);
        }
        
        /// <summary>
        /// Deletes a project with the given id
        /// </summary>
        /// <param name="id">The id of project to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task<ProjectModel> DeleteById(string id)
        {
            // get the principal
            var principal = this.HttpContext.GetShocPrincipal();

            return this.projectService.DeleteById(principal.Subject, id);
        }
    }
}
