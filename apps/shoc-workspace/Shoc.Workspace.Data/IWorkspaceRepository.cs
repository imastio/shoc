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
    /// Gets the objects by userId
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<WorkspaceModel>> GetAllByUserId(string userId);
    
    /// <summary>
    /// Get all referential values
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<WorkspaceReferentialValueModel>> GetAllReferentialValues();
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    Task<WorkspaceModel> GetById(string id);
    
    /// <summary>
    /// Gets the object by name
    /// </summary>
    /// <returns></returns>
    Task<WorkspaceModel> GetByName(string name);
    
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