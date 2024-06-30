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
/// The workspace members endpoints
/// </summary>
[Route("api/user-workspaces/{workspaceId}/members")]
[ApiController]
[ShocExceptionHandler]
public class UserWorkspaceMembersController : ControllerBase
{
    /// <summary>
    /// The use workspace members service
    /// </summary>
    private readonly UserWorkspaceMemberService workspaceMemberService;

    /// <summary>
    /// Creates new instance of the controller
    /// </summary>
    /// <param name="workspaceMemberService">The employee skill service</param>
    public UserWorkspaceMembersController(UserWorkspaceMemberService workspaceMemberService)
    {
        this.workspaceMemberService = workspaceMemberService;
    }

    /// <summary>
    /// Gets all the members of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpGet]
    public Task<IEnumerable<UserWorkspaceMemberModel>> GetAll(string workspaceId)
    {
        return this.workspaceMemberService.GetAllExtended(this.HttpContext.GetPrincipal().Id, workspaceId);
    }
}

