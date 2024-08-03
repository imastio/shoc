using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Workspace.Model;
using Shoc.Workspace.Model.Workspace;
using Shoc.Workspace.Services;

namespace Shoc.Workspace.Controllers;

/// <summary>
/// The workspace invitations endpoints
/// </summary>
[Route("api/workspaces/{workspaceId}/invitations")]
[ApiController]
[ShocExceptionHandler]
public class WorkspaceInvitationsController : ControllerBase
{
    /// <summary>
    /// The workspace invitations service
    /// </summary>
    private readonly WorkspaceInvitationService workspaceInvitationService;

    /// <summary>
    /// Creates new instance of employee skill controller
    /// </summary>
    /// <param name="workspaceInvitationService">The reference to service</param>
    public WorkspaceInvitationsController(WorkspaceInvitationService workspaceInvitationService)
    {
        this.workspaceInvitationService = workspaceInvitationService;
    }

    /// <summary>
    /// Gets all the invitations of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_READ)]
    [HttpGet]
    public Task<IEnumerable<WorkspaceInvitationModel>> GetAll(string workspaceId)
    {
        return this.workspaceInvitationService.GetAll(workspaceId);
    }
    
    /// <summary>
    /// Gets all the extended invitations of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_READ)]
    [HttpGet("extended")]
    public Task<IEnumerable<WorkspaceInvitationExtendedModel>> GetAllExtended(string workspaceId)
    {
        return this.workspaceInvitationService.GetAllExtended(workspaceId);
    }

    /// <summary>
    /// Creates new invitation in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The create input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_MANAGE)]
    [HttpPost]
    public Task<WorkspaceInvitationModel> Create(string workspaceId, WorkspaceInvitationCreateModel input)
    {
        return this.workspaceInvitationService.Create(workspaceId, input);
    }

    /// <summary>
    /// Update the existing invitations record
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_MANAGE)]
    [HttpPut("{id}")]
    public Task<WorkspaceInvitationModel> UpdateById(string workspaceId, string id, WorkspaceInvitationUpdateModel input)
    {
        return this.workspaceInvitationService.UpdateById(workspaceId, id, input);
    }

    /// <summary>
    /// Deletes the invitation user from the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_MANAGE)]
    [HttpDelete("{id}")]
    public Task<WorkspaceInvitationModel> DeleteById(string workspaceId, string id)
    {
        return this.workspaceInvitationService.DeleteById(workspaceId, id);
    }
}

