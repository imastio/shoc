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
/// The workspace members endpoints
/// </summary>
[Route("api/workspaces/{workspaceId}/members")]
[ApiController]
[ShocExceptionHandler]
public class WorkspaceMembersController : ControllerBase
{
    /// <summary>
    /// The workspace members service
    /// </summary>
    private readonly WorkspaceMemberService workspaceMemberService;

    /// <summary>
    /// Creates new instance of employee skill controller
    /// </summary>
    /// <param name="workspaceMemberService">The members service</param>
    public WorkspaceMembersController(WorkspaceMemberService workspaceMemberService)
    {
        this.workspaceMemberService = workspaceMemberService;
    }

    /// <summary>
    /// Gets all the members of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_READ)]
    [HttpGet]
    public Task<IEnumerable<WorkspaceMemberModel>> GetAll(string workspaceId)
    {
        return this.workspaceMemberService.GetAll(workspaceId);
    }
    
    /// <summary>
    /// Gets all the extended members of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_READ)]
    [HttpGet("extended")]
    public Task<IEnumerable<WorkspaceMemberExtendedModel>> GetAllExtended(string workspaceId)
    {
        return this.workspaceMemberService.GetAllExtended(workspaceId);
    }

    /// <summary>
    /// Creates new membership in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The workspace membership create input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_MANAGE)]
    [HttpPost]
    public Task<WorkspaceMemberModel> Create(string workspaceId, WorkspaceMemberCreateModel input)
    {
        return this.workspaceMemberService.Create(workspaceId, input);
    }

    /// <summary>
    /// Update the existing membership record
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <param name="input">The workspace membership update input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_MANAGE)]
    [HttpPut("{id}")]
    public Task<WorkspaceMemberModel> UpdateById(string workspaceId, string id, WorkspaceMemberUpdateModel input)
    {
        return this.workspaceMemberService.UpdateById(workspaceId, id, input);
    }

    /// <summary>
    /// Deletes the member user from the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_MANAGE)]
    [HttpDelete("{id}")]
    public Task<WorkspaceMemberModel> DeleteById(string workspaceId, string id)
    {
        return this.workspaceMemberService.DeleteById(workspaceId, id);
    }
}

