using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Shoc.Builder.Data;
using Shoc.Builder.Model;
using Shoc.Core;

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
        private static readonly Regex DIR_REGEX = new("^(\\/[A-Za-z0-9_]+)*\\/$");

        /// <summary>
        /// The name validation regex
        /// </summary>
        private static readonly Regex NAME_REGEX = new("^[a-zA-Z0-9_]+$");

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
        /// Gets all projects
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<ProjectModel>> GetAll()
        {
            return this.projectRepository.GetAll();
        }

        /// <summary>
        /// Gets all projects for the owner
        /// </summary>
        /// <param name="ownerId">The owner user id</param>
        /// <returns></returns>
        public Task<IEnumerable<ProjectModel>> GetAllByOwner(string ownerId)
        {
            return this.projectRepository.GetAllByOwner(ownerId);
        }

        /// <summary>
        /// Gets the project by id
        /// </summary>
        /// <param name="id">The id of project</param>
        /// <returns></returns>
        public Task<ProjectModel> GetById(string id)
        {
            return this.projectRepository.GetById(id);
        }

        /// <summary>
        /// Creates the project by given input
        /// </summary>
        /// <param name="input">The project creation input</param>
        /// <returns></returns>
        public async Task<ProjectModel> Create(CreateUpdateProjectModel input)
        {
            // generate directory if missing
            input.Directory ??= DEFAULT_DIR;

            // do basic validation
            var validation = ValidateCreateUpdateInput(input);

            // make sure valid
            if (validation.Count > 0)
            {
                throw new ShocException(validation);
            }

            // try get by name
            var existing = await this.projectRepository.GetByName(input.Name);

            // check if name is already 
            if (existing != null)
            {
                throw ErrorDefinition.Validation(BuilderErrors.EXISTING_NAME).AsException();
            }

            // initiate the creation
            return await this.projectRepository.Create(input);
        }

        /// <summary>
        /// Updates the project by given input
        /// </summary>
        /// <param name="id">The id of entity to update</param>
        /// <param name="input">The project update input</param>
        /// <returns></returns>
        public async Task<ProjectModel> Update(string id, CreateUpdateProjectModel input)
        {
            // consider given id
            input.Id = id;

            // generate directory if missing
            input.Directory ??= DEFAULT_DIR;
            
            // make sure id is given
            if (string.IsNullOrWhiteSpace(id))
            {
                throw ErrorDefinition.Validation(BuilderErrors.INVALID_PROJECT).AsException();
            }

            // do basic validation
            var validation = ValidateCreateUpdateInput(input);

            // make sure valid
            if (validation.Count > 0)
            {
                throw new ShocException(validation);
            }

            // try get by name
            var existing = await this.projectRepository.GetByName(input.Name);

            // check if name is already 
            if (existing != null)
            {
                throw ErrorDefinition.Validation(BuilderErrors.EXISTING_NAME).AsException();
            }

            // initiate the update
            return await this.projectRepository.Update(input);
        }

        /// <summary>
        /// Deletes the project by id
        /// </summary>
        /// <param name="id">The id of project to delete</param>
        /// <returns></returns>
        public Task<ProjectModel> DeleteById(string id)
        {
            return this.projectRepository.DeleteById(id);
        }

        /// <summary>
        /// Validate the create or update input data
        /// </summary>
        /// <param name="input">The input to validate</param>
        /// <returns></returns>
        private static List<ErrorDefinition> ValidateCreateUpdateInput(CreateUpdateProjectModel input)
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

            // return the result
            return result;
        }
    }
}