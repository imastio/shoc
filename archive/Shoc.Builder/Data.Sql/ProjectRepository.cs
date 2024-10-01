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
        /// Gets all projects for the query
        /// </summary>
        /// <param name="query">The project query</param>
        /// <returns></returns>
        public Task<IEnumerable<ProjectModel>> GetBy(ProjectQuery query)
        {
            return this.dataOps.Connect().Query("Project", "GetBy")
                .WithBinding("ByName", query.Name != null)
                .ExecuteAsync<ProjectModel>(query);
        }

        /// <summary>
        /// Gets the project by id
        /// </summary>
        /// <param name="ownerId">The owner id</param>
        /// <param name="id">The id of project</param>
        /// <returns></returns>
        public Task<ProjectModel> GetById(string ownerId, string id)
        {
            return this.dataOps.Connect().QueryFirst("Project", "GetById").ExecuteAsync<ProjectModel>(new
            {
                Id = id,
                OwnerId = ownerId
            });
        }

        /// <summary>
        /// Gets the project versions
        /// </summary>
        /// <param name="query">The project version query</param>
        /// <returns></returns>
        public Task<IEnumerable<ProjectVersion>> GetVersions(ProjectVersion query)
        {
            return this.dataOps.Connect().Query("Project", "GetProjectVersions")
                .WithBinding("ByVersion", query.Version != null)
                .ExecuteAsync<ProjectVersion>(query);
        }

        /// <summary>
        /// Assigns or updates version
        /// </summary>
        /// <param name="id">The id of project</param>
        /// <param name="packageId">The id of package</param>
        /// <param name="version">The version name</param>
        /// <returns></returns>
        public async Task AssignVersion(string id, string packageId, string version)
        {
            // add object to the database
            await this.dataOps.Connect().NonQuery("Project", "AssignVersion")
                .ExecuteAsync(new
                {
                    ProjectId = id,
                    PackageId = packageId,
                    Version = version
                });
        }

        /// <summary>
        /// Creates the project by given input
        /// </summary>
        /// <param name="input">The project creation input</param>
        /// <returns></returns>
        public Task<ProjectModel> Create(CreateProjectModel input)
        {
            // generate id if necessary
            input.Id ??= StdIdGenerator.Next(BuilderObjects.PROJECT)?.ToLowerInvariant();

            // add object to the database
            return this.dataOps.Connect().QueryFirst("Project", "Create").ExecuteAsync<ProjectModel>(input);
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