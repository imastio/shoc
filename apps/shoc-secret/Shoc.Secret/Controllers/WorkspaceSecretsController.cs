using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.Access.Model;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.ApiCore.Auth;
using Shoc.Secret.Model.WorkspaceSecret;
using Shoc.Secret.Services;

namespace Shoc.Secret.Controllers;

/// <summary>
/// The secrets endpoint
/// </summary>
[Route("api/workspaces/{workspaceId}/secrets")]
[ApiController]
[ShocExceptionHandler]
[AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
public class WorkspaceSecretsController : ControllerBase
{
    /// <summary>
    /// The secret service
    /// </summary>
    private readonly WorkspaceSecretService secretService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="secretService">The reference to service</param>
    public WorkspaceSecretsController(WorkspaceSecretService secretService)
    {
        this.secretService = secretService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [HttpGet]
    public Task<IEnumerable<WorkspaceSecretModel>> GetAll(string workspaceId)
    {
        return this.secretService.GetAll(this.HttpContext.GetPrincipal().Id, workspaceId);
    }
    
    /// <summary>
    /// Counts all the objects
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [HttpGet("count")]
    public Task<WorkspaceSecretCountModel> CountAll(string workspaceId)
    {
        return this.secretService.CountAll(this.HttpContext.GetPrincipal().Id, workspaceId);
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    [HttpPost]
    public Task<WorkspaceSecretCreatedModel> Create(string workspaceId, [FromBody] WorkspaceSecretCreateModel input)
    {
        return this.secretService.Create(this.HttpContext.GetPrincipal().Id, workspaceId, input);
    }

    /// <summary>
    /// Updates the object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public Task<WorkspaceSecretUpdatedModel> UpdateById(string workspaceId, string id, [FromBody] WorkspaceSecretUpdateModel input)
    {
        return this.secretService.UpdateById(this.HttpContext.GetPrincipal().Id, workspaceId, id, input);
    }
    
    /// <summary>
    /// Updates value of the object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [HttpPut("{id}/value")]
    public Task<WorkspaceSecretUpdatedModel> UpdateValueById(string workspaceId, string id, [FromBody] WorkspaceSecretValueUpdateModel input)
    {
        return this.secretService.UpdateValueById(this.HttpContext.GetPrincipal().Id, workspaceId, id, input);
    }
    
    /// <summary>
    /// Deletes the object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    [HttpDelete]
    public Task<WorkspaceSecretDeletedModel> UpdateById(string workspaceId, string id)
    {
        return this.secretService.DeleteById(this.HttpContext.GetPrincipal().Id, workspaceId, id);
    }
}

