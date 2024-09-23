using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.Access.Model;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.ApiCore.Auth;
using Shoc.Secret.Model.WorkspaceUserSecret;
using Shoc.Secret.Services;

namespace Shoc.Secret.Controllers;

/// <summary>
/// The user secrets endpoint
/// </summary>
[Route("api/workspaces/{workspaceId}/user-secrets")]
[ApiController]
[ShocExceptionHandler]
[AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
public class WorkspaceUserSecretsController : ControllerBase
{
    /// <summary>
    /// The user secret service
    /// </summary>
    private readonly WorkspaceUserSecretService userSecretService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="userSecretService">The reference to service</param>
    public WorkspaceUserSecretsController(WorkspaceUserSecretService userSecretService)
    {
        this.userSecretService = userSecretService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [HttpGet]
    public Task<IEnumerable<WorkspaceUserSecretModel>> GetAll(string workspaceId)
    {
        return this.userSecretService.GetAll(this.HttpContext.GetPrincipal().Id, workspaceId);
    }
    
    /// <summary>
    /// Counts all the objects
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [HttpGet("count")]
    public Task<WorkspaceUserSecretCountModel> CountAll(string workspaceId)
    {
        return this.userSecretService.CountAll(this.HttpContext.GetPrincipal().Id, workspaceId);
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    [HttpPost]
    public Task<WorkspaceUserSecretCreatedModel> Create(string workspaceId, [FromBody] WorkspaceUserSecretCreateModel input)
    {
        return this.userSecretService.Create(this.HttpContext.GetPrincipal().Id, workspaceId, input);
    }

    /// <summary>
    /// Updates the object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public Task<WorkspaceUserSecretUpdatedModel> UpdateById(string workspaceId, string id, [FromBody] WorkspaceUserSecretUpdateModel input)
    {
        return this.userSecretService.UpdateById(this.HttpContext.GetPrincipal().Id, workspaceId, id, input);
    }
    
    /// <summary>
    /// Updates value of the object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [HttpPut("{id}/value")]
    public Task<WorkspaceUserSecretUpdatedModel> UpdateValueById(string workspaceId, string id, [FromBody] WorkspaceUserSecretValueUpdateModel input)
    {
        return this.userSecretService.UpdateValueById(this.HttpContext.GetPrincipal().Id, workspaceId, id, input);
    }
    
    /// <summary>
    /// Deletes the object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    [HttpDelete]
    public Task<WorkspaceUserSecretDeletedModel> UpdateById(string workspaceId, string id)
    {
        return this.userSecretService.DeleteById(this.HttpContext.GetPrincipal().Id, workspaceId, id);
    }
}

