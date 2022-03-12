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
        /// <param name="id">The id of project</param>
        /// <returns></returns>
        Task<ProjectModel> GetById(string id);

        /// <summary>
        /// Gets the project versions
        /// </summary>
        /// <param name="id">The id of projects</param>
        /// <returns></returns>
        Task<IEnumerable<ProjectVersion>> GetVersions(string id);
        
        /// <summary>
        /// Creates the project by given input
        /// </summary>
        /// <param name="input">The project creation input</param>
        /// <returns></returns>
        Task<ProjectModel> Create(CreateProjectModel input);
        
        /// <summary>
        /// Deletes the project by id
        /// </summary>
        /// <param name="id">The id of project to delete</param>
        /// <returns></returns>
        Task<ProjectModel> DeleteById(string id);
    }
}