using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.Access.Model;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.ApiCore.Auth;
using Shoc.Workspace.Model.User;
using Shoc.Workspace.Services;

namespace Shoc.Workspace.Controllers;

/// <summary>
/// The user invitations endpoints
/// </summary>
[Route("api/user-invitations")]
[ApiController]
[ShocExceptionHandler]
public class UserInvitationsController : ControllerBase
{
    /// <summary>
    /// The use workspace invitations service
    /// </summary>
    private readonly UserInvitationService invitationService;

    /// <summary>
    /// Creates new instance of the controller
    /// </summary>
    /// <param name="invitationService">The service</param>
    public UserInvitationsController(UserInvitationService invitationService)
    {
        this.invitationService = invitationService;
    }
    
    /// <summary>
    /// Gets all the user invitations 
    /// </summary>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpGet]
    public Task<IEnumerable<UserInvitationModel>> GetAll()
    {
        return this.invitationService.GetAll(this.HttpContext.GetPrincipal().Id);
    }

    /// <summary>
    /// Counts all the user invitations 
    /// </summary>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpGet("count")]
    public Task<UserInvitationsCountModel> CountAll()
    {
        return this.invitationService.CountAll(this.HttpContext.GetPrincipal().Id);
    }

    /// <summary>
    /// Accept the user invitation by the current user
    /// </summary>
    /// <param name="input">The accept input</param>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpPut("accept")]
    public Task<UserInvitationAcceptedModel> Accept(UserInvitationAcceptModel input)
    {
        return this.invitationService.Accept(this.HttpContext.GetPrincipal().Id, input);
    }
    
    /// <summary>
    /// Decline the user invitation by the current user
    /// </summary>
    /// <param name="input">The decline input</param>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpPut("decline")]
    public Task<UserInvitationDeclinedModel> Decline(UserInvitationDeclineModel input)
    {
        return this.invitationService.Decline(this.HttpContext.GetPrincipal().Id, input);
    }
}

