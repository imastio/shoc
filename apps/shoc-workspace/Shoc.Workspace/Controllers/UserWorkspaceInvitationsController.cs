using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.Access.Model;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.ApiCore.Auth;
using Shoc.Workspace.Model.UserWorkspace;
using Shoc.Workspace.Services;

namespace Shoc.Workspace.Controllers;

/// <summary>
/// The workspace invitations endpoints
/// </summary>
[Route("api/user-workspaces/{workspaceId}/invitations")]
[ApiController]
[ShocExceptionHandler]
public class UserWorkspaceInvitationsController : ControllerBase
{
    /// <summary>
    /// The use workspace invitations service
    /// </summary>
    private readonly UserWorkspaceInvitationService invitationService;

    /// <summary>
    /// Creates new instance of the controller
    /// </summary>
    /// <param name="invitationService">The service</param>
    public UserWorkspaceInvitationsController(UserWorkspaceInvitationService invitationService)
    {
        this.invitationService = invitationService;
    }

    /// <summary>
    /// Gets all the invitations of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpGet]
    public Task<IEnumerable<UserWorkspaceInvitationModel>> GetAll(string workspaceId)
    {
        return this.invitationService.GetAllExtended(this.HttpContext.GetPrincipal().Id, workspaceId);
    }

    /// <summary>
    /// Creates a new member invitation in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The create input</param>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpPost]
    public Task<UserWorkspaceInvitationCreatedModel> Create(string workspaceId, UserWorkspaceInvitationCreateModel input)
    {
        return this.invitationService.Create(this.HttpContext.GetPrincipal().Id, workspaceId, input);
    }
    
    /// <summary>
    /// Updates the invitation of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpPut("{id}")]
    public Task<UserWorkspaceInvitationUpdatedModel> UpdateById(string workspaceId, string id, UserWorkspaceInvitationUpdateModel input)
    {
        return this.invitationService.UpdateById(this.HttpContext.GetPrincipal().Id, workspaceId, id, input);
    }
    
    /// <summary>
    /// Deletes the invitation from the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpDelete("{id}")]
    public Task<UserWorkspaceInvitationDeletedModel> DeleteById(string workspaceId, string id)
    {
        return this.invitationService.DeleteById(this.HttpContext.GetPrincipal().Id, workspaceId, id);
    }
}

