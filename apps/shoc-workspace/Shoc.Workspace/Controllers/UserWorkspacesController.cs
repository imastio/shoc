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
/// The user workspaces endpoint
/// </summary>
[Route("api/user-workspaces")]
[ApiController]
[ShocExceptionHandler]
public class UserWorkspacesController : ControllerBase
{
    /// <summary>
    /// The object service
    /// </summary>
    private readonly UserWorkspaceService userWorkspaceService;

    /// <summary>
    /// Creates new instance of the controller
    /// </summary>
    /// <param name="userWorkspaceService">The object service</param>
    public UserWorkspacesController(UserWorkspaceService userWorkspaceService)
    {
        this.userWorkspaceService = userWorkspaceService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpGet]
    public Task<IEnumerable<UserWorkspaceModel>> GetAll()
    {
        return this.userWorkspaceService.GetAll(this.HttpContext.GetPrincipal().Id);
    }

    /// <summary>
    /// Gets object by id
    /// </summary>
    /// <param name="id">The id of object</param>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpGet("{id}")]
    public Task<UserWorkspaceModel> GetById(string id)
    {
        return this.userWorkspaceService.GetById(this.HttpContext.GetPrincipal().Id, id);
    }
    
    /// <summary>
    /// Gets object by name
    /// </summary>
    /// <param name="name">The name of object</param>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpGet("by-name/{name}")]
    public Task<UserWorkspaceModel> GetByName(string name)
    {
        return this.userWorkspaceService.GetByName(this.HttpContext.GetPrincipal().Id, name);
    }
    
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="input">The object creation model</param>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpPost]
    public Task<UserWorkspaceCreatedModel> Create(UserWorkspaceCreateModel input)
    {
        return this.userWorkspaceService.Create(this.HttpContext.GetPrincipal().Id, input);
    }

    /// <summary>
    /// Updates the object by given input
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <param name="input">The object update input</param>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpPut("{id}")]
    public Task<UserWorkspaceUpdatedModel> UpdateById(string id, UserWorkspaceUpdateModel input)
    {
        return this.userWorkspaceService.UpdateById(this.HttpContext.GetPrincipal().Id, id, input);
    }

    /// <summary>
    /// Deletes object by id
    /// </summary>
    /// <param name="id">The id of object</param>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpDelete("{id}")]
    public Task<UserWorkspaceDeletedModel> DeleteById(string id)
    {
        return this.userWorkspaceService.DeleteById(this.HttpContext.GetPrincipal().Id, id);
    }
}

