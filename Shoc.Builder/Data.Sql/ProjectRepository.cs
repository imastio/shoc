using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Builder.Model.Project;
using Shoc.Core;

namespace Shoc.Builder.Data.Sql
{
    /// <summary>
    /// The project repository implementation
    /// </summary>
    public class ProjectRepository : IProjectRepository
    {
        /// <summary>
        /// The data operations instance
        /// </summary>
        private readonly DataOperations dataOps;

        /// <summary>
        /// Creates new instance of project repository implementation
        /// </summary>
        /// <param name="dataOps">A DataOps instance</param>
        public ProjectRepository(DataOperations dataOps)
        {
            this.dataOps = dataOps;
        }

        /// <summary>
        /// Gets all projects
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<ProjectModel>> GetAll()
        {
            return this.dataOps.Connect().Query("Project", "GetAll").ExecuteAsync<ProjectModel>();
        }

        /// <summary>
        /// Gets all projects for the owner
        /// </summary>
        /// <param name="ownerId">The owner user id</param>
        /// <returns></returns>
        public Task<IEnumerable<ProjectModel>> GetAllByOwner(string ownerId)
        {
            return this.dataOps.Connect().Query("Project", "GetAllByOwner").ExecuteAsync<ProjectModel>(new { OwnerId = ownerId });
        }

        /// <summary>
        /// Gets the project by id
        /// </summary>
        /// <param name="id">The id of project</param>
        /// <returns></returns>
        public Task<ProjectModel> GetById(string id)
        {
            return this.dataOps.Connect().QueryFirst("Project", "GetById").ExecuteAsync<ProjectModel>(new
            {
                Id = id
            });
        }

        /// <summary>
        /// Gets the project by name
        /// </summary>
        /// <param name="directory">The target directory of the project</param>
        /// <param name="name">The name of project</param>
        /// <param name="ownerId">The owner id</param>
        /// <returns></returns>
        public Task<ProjectModel> GetOwnedByPath(string directory, string name, string ownerId)
        {
            return this.dataOps.Connect().QueryFirst("Project", "GetOwnedByPath").ExecuteAsync<ProjectModel>(new
            {
                Directory = directory,
                Name = name,
                OwnerId = ownerId
            });
        }

        /// <summary>
        /// Creates the project by given input
        /// </summary>
        /// <param name="input">The project creation input</param>
        /// <returns></returns>
        public Task<ProjectModel> Create(CreateUpdateProjectModel input)
        {
            // generate id if necessary
            input.Id ??= StdIdGenerator.Next(BuilderObjects.PROJECT)?.ToLowerInvariant();

            // add object to the database
            return this.dataOps.Connect().QueryFirst("Project", "Create").ExecuteAsync<ProjectModel>(input);
        }

        /// <summary>
        /// Updates the project by given input
        /// </summary>
        /// <param name="input">The project update input</param>
        /// <returns></returns>
        public Task<ProjectModel> Update(CreateUpdateProjectModel input)
        {
            // update object to the database
            return this.dataOps.Connect().QueryFirst("Project", "Update").ExecuteAsync<ProjectModel>(input);
        }

        /// <summary>
        /// Deletes the project by id
        /// </summary>
        /// <param name="id">The id of project to delete</param>
        /// <returns></returns>
        public Task<ProjectModel> DeleteById(string id)
        {
            // delete object from the database
            return this.dataOps.Connect().QueryFirst("Project", "DeleteById").ExecuteAsync<ProjectModel>(new
            {
                Id = id
            });
        }
    }
}