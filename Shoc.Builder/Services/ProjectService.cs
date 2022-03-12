using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Shoc.ApiCore;
using Shoc.Builder.Data;
using Shoc.Builder.Model;
using Shoc.Builder.Model.Project;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.ModelCore;

namespace Shoc.Builder.Services
{
    /// <summary>
    /// The projects service
    /// </summary>
    public class ProjectService
    {
        /// <summary>
        /// The directory validation regex
        /// </summary>
        private static readonly Regex DIR_REGEX = new("^(\\/[A-Za-z0-9_-]+)*\\/$");

        /// <summary>
        /// The name validation regex
        /// </summary>
        private static readonly Regex NAME_REGEX = new("^[a-zA-Z0-9_-]+$");

        /// <summary>
        /// The default directory
        /// </summary>
        private const string DEFAULT_DIR = "/";

        /// <summary>
        /// The project repository
        /// </summary>
        private readonly IProjectRepository projectRepository;

        /// <summary>
        /// Creates new instance of project service
        /// </summary>
        /// <param name="projectRepository">The project repository</param>
        public ProjectService(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        /// <summary>
        /// Gets all projects for the query
        /// </summary>
        /// <param name="principal">The authenticated principal</param>
        /// <param name="query">The lookup query</param>
        /// <returns></returns>
        public Task<IEnumerable<ProjectModel>> GetBy(ShocPrincipal principal, ProjectQuery query)
        {
            // require a proper access
            AccessGuard.Require(() => Roles.ADMINS.Contains(principal.Role) || principal.Subject == query.OwnerId);

            // get by query
            return this.projectRepository.GetBy(query);
        }

        /// <summary>
        /// Gets the project by id
        /// </summary>
        /// <param name="principal">The authenticated principal</param>
        /// <param name="id">The id of project</param>
        /// <returns></returns>
        public async Task<ProjectModel> GetById(ShocPrincipal principal, string id)
        {
            // try load the result
            var result = await this.projectRepository.GetById(id);

            // not found
            if (result == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // require to be either administrator or owner
            AccessGuard.Require(() => Roles.ADMINS.Contains(principal.Role) || result.OwnerId == principal.Subject);

            // the result
            return result;
        }

        /// <summary>
        /// Gets the versions of the given project
        /// </summary>
        /// <param name="principal">The authenticated principal</param>
        /// <param name="id">The project id</param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectVersion>> GetVersions(ShocPrincipal principal, string id)
        {
            // try get the project
            var project = await this.GetById(principal, id);

            // get the versions of the project
            return await this.projectRepository.GetVersions(project.Id);
        }

        /// <summary>
        /// Creates the project by given input
        /// </summary>
        /// <param name="principal">The current principal</param>
        /// <param name="input">The project creation input</param>
        /// <returns></returns>
        public async Task<ProjectModel> Create(ShocPrincipal principal, CreateProjectModel input)
        {
            // generate directory if missing
            input.Directory ??= DEFAULT_DIR;

            // assign default owner if not assigned yet
            input.OwnerId ??= principal.Subject;

            // make sure proper owner id is set
            AccessGuard.Require(() => Roles.ADMINS.Contains(principal.Role) || input.OwnerId == principal.Subject);

            // do basic validation
            var validation = ValidateCreateUpdateInput(input);

            // make sure valid
            if (validation.Count > 0)
            {
                throw new ShocException(validation);
            }

            // try get by name and directory
            var existing = await this.projectRepository.GetBy(new ProjectQuery
            {
                Name = input.Name,
                Directory = input.Directory,
                OwnerId = input.OwnerId
            });

            // check if name is already 
            if (existing.Any())
            {
                throw ErrorDefinition.Validation(BuilderErrors.EXISTING_NAME).AsException();
            }

            // initiate the creation
            return await this.projectRepository.Create(input);
        }

        /// <summary>
        /// Deletes the project by id
        /// </summary>
        /// <param name="principal">The current principal</param>
        /// <param name="id">The id of project to delete</param>
        /// <returns></returns>
        public async Task<ProjectModel> DeleteById(ShocPrincipal principal, string id)
        {
            // get the object by id
            var result = await this.GetById(principal, id);

            // assure the access (administrator or the owner)
            AccessGuard.Require(() => Roles.ADMINS.Contains(principal.Role) || result.OwnerId == principal.Subject);

            return await this.projectRepository.DeleteById(id);
        }
        
        /// <summary>
        /// Validate the create or update input data
        /// </summary>
        /// <param name="input">The input to validate</param>
        /// <returns></returns>
        private static List<ErrorDefinition> ValidateCreateUpdateInput(CreateProjectModel input)
        {
            // validation result
            var result = new List<ErrorDefinition>();

            // if directory is not given or does not match the required pattern report error
            if (string.IsNullOrWhiteSpace(input.Directory) || !DIR_REGEX.IsMatch(input.Directory))
            {
                result.Add(ErrorDefinition.Validation(BuilderErrors.INVALID_DIRECTORY));
            }

            // if name is not given or does not match the required pattern report error
            if (string.IsNullOrWhiteSpace(input.Name) || !NAME_REGEX.IsMatch(input.Name))
            {
                result.Add(ErrorDefinition.Validation(BuilderErrors.INVALID_NAME));
            }

            // owner is mandatory to be there
            if (string.IsNullOrWhiteSpace(input.OwnerId))
            {
                result.Add(ErrorDefinition.Validation(BuilderErrors.INVALID_OWNER));
            }

            // return the result
            return result;
        }
    }
}