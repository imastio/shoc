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
        /// Gets all projects for the owner
        /// </summary>
        /// <param name="ownerId">The owner user id</param>
        /// <returns></returns>
        Task<IEnumerable<ProjectModel>> GetAllByOwner(string ownerId);

        /// <summary>
        /// Gets the project by id
        /// </summary>
        /// <param name="id">The id of project</param>
        /// <returns></returns>
        Task<ProjectModel> GetById(string id);

        /// <summary>
        /// Gets the project by name
        /// </summary>
        /// <param name="directory">The target directory of the project</param>
        /// <param name="name">The name of project</param>
        /// <param name="ownerId">The owner id</param>
        /// <returns></returns>
        Task<ProjectModel> GetOwnedByPath(string directory, string name, string ownerId);

        /// <summary>
        /// Creates the project by given input
        /// </summary>
        /// <param name="input">The project creation input</param>
        /// <returns></returns>
        Task<ProjectModel> Create(CreateUpdateProjectModel input);

        /// <summary>
        /// Updates the project by given input
        /// </summary>
        /// <param name="input">The project update input</param>
        /// <returns></returns>
        Task<ProjectModel> Update(CreateUpdateProjectModel input);

        /// <summary>
        /// Deletes the project by id
        /// </summary>
        /// <param name="id">The id of project to delete</param>
        /// <returns></returns>
        Task<ProjectModel> DeleteById(string id);
    }
}