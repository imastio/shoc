using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Workspace.Model.UserWorkspace;

namespace Shoc.Workspace.Data;

/// <summary>
/// The user workspace repository
/// </summary>
public interface IUserWorkspaceRepository
{
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<UserWorkspaceModel>> GetAll(string userId);
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    Task<UserWorkspaceModel> GetById(string userId, string id);
    
    /// <summary>
    /// Gets the object by name
    /// </summary>
    /// <returns></returns>
    Task<UserWorkspaceModel> GetByName(string userId, string name);
}