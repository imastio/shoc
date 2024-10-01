using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Shoc.Builder.Data;
using Shoc.Builder.Model;
using Shoc.Builder.Model.Project;
using Shoc.Core;

namespace Shoc.Builder.Services
{
    /// <summary>
    /// The projects service
    /// </summary>
    public class ProjectService
    {
        /// <summary>
        /// The name validation regex
        /// </summary>
        private static readonly Regex NAME_REGEX = new("^[a-zA-Z0-9_-]+$");

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
        /// <param name="query">The lookup query</param>
        /// <returns></returns>
        public Task<IEnumerable<ProjectModel>> GetBy(ProjectQuery query)
        {
            // get by query
            return this.projectRepository.GetBy(query);
        }

        /// <summary>
        /// Gets the project by id
        /// </summary>
        /// <param name="ownerId">The owner id</param>
        /// <param name="id">The id of project</param>
        /// <returns></returns>
        public async Task<ProjectModel> GetById(string ownerId, string id)
        {
            // try load the result
            var result = await this.projectRepository.GetById(ownerId, id);

            // not found
            if (result == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // the result
            return result;
        }

        /// <summary>
        /// Gets the versions of the given project
        /// </summary>
        /// <param name="ownerId">The owner id</param>
        /// <param name="id">The project id</param>
        /// <param name="version">The project version</param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectVersion>> GetVersions(string ownerId, string id, string version)
        {
            // try get the project
            var project = await this.GetById(ownerId, id);

            // get the versions of the project
            return await this.projectRepository.GetVersions(new ProjectVersion
            {
                ProjectId = project.Id,
                Version = version
            });
        }

        /// <summary>
        /// Creates the project by given input
        /// </summary>
        /// <param name="input">The project creation input</param>
        /// <returns></returns>
        public async Task<ProjectModel> Create(CreateProjectModel input)
        {
            // do basic validation
            var validation = ValidateCreateUpdateInput(input);

            // make sure valid
            if (validation.Count > 0)
            {
                throw new ShocException(validation);
            }

            // try get by name and owner
            var existing = await this.projectRepository.GetBy(new ProjectQuery
            {
                Name = input.Name,
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
        /// <param name="ownerId">The owner id</param>
        /// <param name="id">The id of project to delete</param>
        /// <returns></returns>
        public async Task<ProjectModel> DeleteById(string ownerId, string id)
        {
            // get the project by id
            _ = await this.GetById(ownerId, id);

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

            // if name is not given or does not match the required pattern report error
            if (string.IsNullOrWhiteSpace(input.Name) || !NAME_REGEX.IsMatch(input.Name))
            {
                result.Add(ErrorDefinition.Validation(BuilderErrors.INVALID_NAME));
            }

            // if type is not given or does not match the required pattern report error
            if (string.IsNullOrWhiteSpace(input.Type) || !NAME_REGEX.IsMatch(input.Type))
            {
                result.Add(ErrorDefinition.Validation(BuilderErrors.INVALID_TYPE));
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