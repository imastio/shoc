using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Workspace.Model.Workspace;

namespace Shoc.Workspace.Data;

/// <summary>
/// The workspace repository
/// </summary>
public interface IWorkspaceRepository
{
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<WorkspaceModel>> GetAll();
    
    /// <summary>
    /// Gets the application by id
    /// </summary>
    /// <returns></returns>
    Task<WorkspaceModel> GetById(string id);
    
    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<WorkspaceModel> Create(WorkspaceCreateModel input);

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<WorkspaceModel> UpdateById(string id, WorkspaceUpdateModel input);

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<WorkspaceModel> DeleteById(string id);
}