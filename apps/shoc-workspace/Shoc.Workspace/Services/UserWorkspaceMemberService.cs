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
/// The user workspace member service
/// </summary>
public class UserWorkspaceMemberService : UserWorkspaceServiceBase
{
    /// <summary>
    /// The member service
    /// </summary>
    private readonly WorkspaceMemberService memberService;

    /// <summary>
    /// The base implementation of the service
    /// </summary>
    /// <param name="memberService">Member service</param>
    /// <param name="workspaceService">The workspace service</param>
    /// <param name="userWorkspaceRepository">The user workspace repository</param>
    /// <param name="workspaceAccessEvaluator">The workspace access evaluator</param>
    public UserWorkspaceMemberService(WorkspaceMemberService memberService, WorkspaceService workspaceService, IUserWorkspaceRepository userWorkspaceRepository, IWorkspaceAccessEvaluator workspaceAccessEvaluator) : base(workspaceService, userWorkspaceRepository, workspaceAccessEvaluator)
    {
        this.memberService = memberService;
    }

    /// <summary>
    /// Gets all the extended members of the workspace
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    public async Task<IEnumerable<UserWorkspaceMemberModel>> GetAllExtended(string userId, string workspaceId)
    {
        // require a valid workspace and get members
        var members = await this.memberService.GetAllExtended(workspaceId);
        
        // ensure the access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId, 
            WorkspacePermissions.WORKSPACE_VIEW, 
            WorkspacePermissions.WORKSPACE_LIST_MEMBERS);
        
        // load from the database
        return members.Select(item => new UserWorkspaceMemberModel
        {
            Id = item.Id,
            WorkspaceId = item.WorkspaceId,
            UserId = item.UserId,
            Email = item.Email,
            FullName = item.FullName,
            Role = item.Role,
            Created = item.Created,
            Updated = item.Updated
        });
    }

    /// <summary>
    /// Updates the member user on the workspace
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public async Task<UserWorkspaceMemberUpdatedModel> UpdateById(string userId, string workspaceId, string id, UserWorkspaceMemberUpdateModel input)
    {
        // get existing object
        await this.memberService.GetById(workspaceId, id);

        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_MEMBERS,
            WorkspacePermissions.WORKSPACE_UPDATE_MEMBER);
        
        // ensure referring to the correct object
        input.Id = id;
        input.WorkspaceId = workspaceId;

        // update the object
        var result = await this.memberService.UpdateById(workspaceId, id, new WorkspaceMemberUpdateModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId,
            Role = input.Role
        });
        
        // return existing object
        return new UserWorkspaceMemberUpdatedModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId
        };
    }

    /// <summary>
    /// Deletes the member user from the workspace
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    public async Task<UserWorkspaceMemberDeletedModel> DeleteById(string userId, string workspaceId, string id)
    {
        // get existing object
        await this.memberService.GetById(workspaceId, id);

        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_MEMBERS,
            WorkspacePermissions.WORKSPACE_DELETE_MEMBER);

        // delete object
        var result = await this.memberService.DeleteById(workspaceId, id);
        
        // return existing object
        return new UserWorkspaceMemberDeletedModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId
        };
    }
}