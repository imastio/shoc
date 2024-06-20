using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Workspace.Model.Workspace;

namespace Shoc.Workspace.Data;

/// <summary>
/// The workspace member repository
/// </summary>
public interface IWorkspaceMemberRepository
{
    /// <summary>
    /// Gets all the members of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    Task<IEnumerable<WorkspaceMemberModel>> GetAll(string workspaceId);
    
    /// <summary>
    /// Gets all the extended members of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    Task<IEnumerable<WorkspaceMemberExtendedModel>> GetAllExtended(string workspaceId);

    /// <summary>
    /// Gets the particular membership record in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The membership id</param>
    /// <returns></returns>
    Task<WorkspaceMemberModel> GetById(string workspaceId, string id);
    
    /// <summary>
    /// Gets the particular membership record in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    Task<WorkspaceMemberModel> GetByUserId(string workspaceId, string userId);

    /// <summary>
    /// Gets the particular extended membership record in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The membership id</param>
    /// <returns></returns>
    Task<WorkspaceMemberExtendedModel> GetExtendedById(string workspaceId, string id);
    
    /// <summary>
    /// Creates new membership in the workspace
    /// </summary>
    /// <param name="input">The workspace membership create input</param>
    /// <returns></returns>
    Task<WorkspaceMemberModel> Create(WorkspaceMemberCreateModel input);

    /// <summary>
    /// Update the existing membership record
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <param name="input">The workspace membership update input</param>
    /// <returns></returns>
    Task<WorkspaceMemberModel> UpdateById(string workspaceId, string id, WorkspaceMemberUpdateModel input);
    
    /// <summary>
    /// Deletes the member user from the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    Task<WorkspaceMemberModel> DeleteById(string workspaceId, string id);
}
