using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Package.Model.BuildTask;

namespace Shoc.Package.Data;

/// <summary>
/// The repository interface
/// </summary>
public interface IBuildTaskRepository
{
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<BuildTaskModel>> GetAll(string workspaceId);
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    Task<BuildTaskModel> GetById(string workspaceId, string id);

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<BuildTaskModel> Create(string workspaceId, BuildTaskCreateModel input);

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<BuildTaskModel> UpdateById(string workspaceId, string id, BuildTaskUpdateModel input);
    
    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<BuildTaskModel> DeleteById(string workspaceId, string id);
}