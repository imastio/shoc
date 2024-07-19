using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Workspace.Model.Workspace;

namespace Shoc.Workspace.Data;

/// <summary>
/// The workspace invitation repository
/// </summary>  
public interface IWorkspaceInvitationRepository
{
    /// <summary>
    /// Gets all the invitations of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    Task<IEnumerable<WorkspaceInvitationModel>> GetAll(string workspaceId);
    
    /// <summary>
    /// Gets all the extended invitations of the workspace
    /// </summary>
    /// <param name="workspaceId">The record id</param>
    /// <returns></returns>
    Task<IEnumerable<WorkspaceInvitationExtendedModel>> GetAllExtended(string workspaceId);

    /// <summary>
    /// Gets the particular invitation record in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    Task<WorkspaceInvitationModel> GetById(string workspaceId, string id);
    
    /// <summary>
    /// Gets the particular invitation record in the workspace by email
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="email">The invitation email</param>
    /// <returns></returns>
    Task<WorkspaceInvitationModel> GetByEmail(string workspaceId, string email);
    
    /// <summary>
    /// Creates new invitation in the workspace
    /// </summary>
    /// <param name="input">The record create input</param>
    /// <returns></returns>
    Task<WorkspaceInvitationModel> Create(WorkspaceInvitationCreateModel input);

    /// <summary>
    /// Update the existing invitation record
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <param name="input">The record update input</param>
    /// <returns></returns>
    Task<WorkspaceInvitationModel> UpdateById(string workspaceId, string id, WorkspaceInvitationUpdateModel input);
    
    /// <summary>
    /// Deletes the record from the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    Task<WorkspaceInvitationModel> DeleteById(string workspaceId, string id);
}
