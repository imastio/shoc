using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Builder.Model.Project;

namespace Shoc.Builder.Data
{
    /// <summary>
    /// The project repository interface
    /// </summary>
    public interface IProjectRepository
    {
        /// <summary>
        /// Gets all projects
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ProjectModel>> GetAll();

        /// <summary>
        /// Gets all projects for the query
        /// </summary>
        /// <param name="query">The project query</param>
        /// <returns></returns>
        Task<IEnumerable<ProjectModel>> GetBy(ProjectQuery query);

        /// <summary>
        /// Gets the project by id
        /// </summary>
        /// <param name="ownerId">The owner id</param>
        /// <param name="id">The id of project</param>
        /// <returns></returns>
        Task<ProjectModel> GetById(string ownerId, string id);

        /// <summary>
        /// Gets the project versions
        /// </summary>
        /// <param name="query">The project version query</param>
        /// <returns></returns>
        Task<IEnumerable<ProjectVersion>> GetVersions(ProjectVersion query);
        
        /// <summary>
        /// Creates the project by given input
        /// </summary>
        /// <param name="input">The project creation input</param>
        /// <returns></returns>
        Task<ProjectModel> Create(CreateProjectModel input);

        /// <summary>
        /// Assigns or updates version
        /// </summary>
        /// <param name="id">The id of project</param>
        /// <param name="packageId">The id of package</param>
        /// <param name="version">The version name</param>
        /// <returns></returns>
        Task AssignVersion(string id, string packageId, string version);

        /// <summary>
        /// Deletes the project by id
        /// </summary>
        /// <param name="id">The id of project to delete</param>
        /// <returns></returns>
        Task<ProjectModel> DeleteById(string id);
    }
}