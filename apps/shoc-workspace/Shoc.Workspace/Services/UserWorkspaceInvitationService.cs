using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoc.ObjectAccess.Model.Workspace;
using Shoc.ObjectAccess.Workspace;
using Shoc.Workspace.Data;
using Shoc.Workspace.Model.UserWorkspace;
using Shoc.Workspace.Model.Workspace;

namespace Shoc.Workspace.Services;

/// <summary>
/// The user workspace invitation service
/// </summary>
public class UserWorkspaceInvitationService : UserWorkspaceServiceBase
{
    /// <summary>
    /// The workspace invitation service 
    /// </summary>
    private readonly WorkspaceInvitationService workspaceInvitationService;

    /// <summary>
    /// Creates new instance of user workspace invitation service
    /// </summary>
    /// <param name="workspaceInvitationService">The invitation service</param>
    /// <param name="workspaceService">The workspace service</param>
    /// <param name="userWorkspaceRepository">The user workspace repository</param>
    /// <param name="workspaceAccessEvaluator">The workspace access evaluator</param>
    public UserWorkspaceInvitationService(WorkspaceInvitationService workspaceInvitationService, WorkspaceService workspaceService, IUserWorkspaceRepository userWorkspaceRepository, IWorkspaceAccessEvaluator workspaceAccessEvaluator) : base(workspaceService, userWorkspaceRepository, workspaceAccessEvaluator)
    {
        this.workspaceInvitationService = workspaceInvitationService;
    }
    
    /// <summary>
    /// Gets all the extended invitations of the workspace
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    public async Task<IEnumerable<UserWorkspaceInvitationModel>> GetAllExtended(string userId, string workspaceId)
    {
        // require a valid workspace and get invitations
        var invitations = await this.workspaceInvitationService.GetAllExtended(workspaceId);
        
        // ensure the access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId, 
            WorkspacePermissions.WORKSPACE_VIEW, 
            WorkspacePermissions.WORKSPACE_LIST_INVITATIONS);
        
        // load from the database
        return invitations.Select(item => new UserWorkspaceInvitationModel
        {
            Id = item.Id,
            WorkspaceId = item.WorkspaceId,
            Email = item.Email,
            Role = item.Role,
            InvitedBy = item.InvitedBy,
            InvitedByEmail = item.InvitedByEmail,
            InvitedByFullName = item.InvitedByFullName,
            Expiration = item.Expiration,
            Created = item.Created,
            Updated = item.Updated
        });
    }
    
    /// <summary>
    /// Creates an invitation to the workspace
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public async Task<UserWorkspaceInvitationCreatedModel> Create(string userId, string workspaceId, UserWorkspaceInvitationCreateModel input)
    {
        // get existing object
        await this.RequireById(userId, workspaceId);

        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_INVITATIONS,
            WorkspacePermissions.WORKSPACE_CREATE_INVITATION);
        
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;

        // create the object
        var result = await this.workspaceInvitationService.Create(workspaceId, new WorkspaceInvitationCreateModel
        {
            WorkspaceId = input.WorkspaceId,
            Email = input.Email,
            Role = input.Role,
            InvitedBy = userId
        });
        
        // return existing object
        return new UserWorkspaceInvitationCreatedModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId
        };
    }
    
    /// <summary>
    /// Updates the invitation user on the workspace
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public async Task<UserWorkspaceInvitationUpdatedModel> UpdateById(string userId, string workspaceId, string id, UserWorkspaceInvitationUpdateModel input)
    {
        // get existing object
        await this.workspaceInvitationService.GetById(workspaceId, id);

        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_INVITATIONS,
            WorkspacePermissions.WORKSPACE_UPDATE_INVITATION);
        
        // ensure referring to the correct object
        input.Id = id;
        input.WorkspaceId = workspaceId;

        // update the object
        var result = await this.workspaceInvitationService.UpdateById(workspaceId, id, new WorkspaceInvitationUpdateModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId,
            Role = input.Role,
            Expiration = null
        });
        
        // return existing object
        return new UserWorkspaceInvitationUpdatedModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId
        };
    }  
    
    /// <summary>
    /// Deletes the invitation user from the workspace
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    public async Task<UserWorkspaceInvitationDeletedModel> DeleteById(string userId, string workspaceId, string id)
    {
        // get existing object
        await this.workspaceInvitationService.GetById(workspaceId, id);

        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_INVITATIONS,
            WorkspacePermissions.WORKSPACE_DELETE_INVITATION);

        // delete object
        var result = await this.workspaceInvitationService.DeleteById(workspaceId, id);
        
        // return existing object
        return new UserWorkspaceInvitationDeletedModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId
        };
    }
}