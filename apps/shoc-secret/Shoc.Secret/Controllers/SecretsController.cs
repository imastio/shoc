using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Secret.Model;
using Shoc.Secret.Model.Secret;
using Shoc.Secret.Services;

namespace Shoc.Secret.Controllers;

/// <summary>
/// The secrets endpoint
/// </summary>
[Route("api/workspaces/{workspaceId}/secrets")]
[ApiController]
[ShocExceptionHandler]
public class SecretsController : ControllerBase
{
    /// <summary>
    /// The secret service
    /// </summary>
    private readonly SecretService secretService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="secretService">The reference to service</param>
    public SecretsController(SecretService secretService)
    {
        this.secretService = secretService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(SecretAccesses.SECRET_SECRETS_LIST)]
    [HttpGet]
    public Task<IEnumerable<SecretModel>> GetAll(string workspaceId)
    {
        return this.secretService.GetAll(workspaceId);
    }
    
    /// <summary>
    /// Gets all the extended objects
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(SecretAccesses.SECRET_SECRETS_LIST)]
    [HttpGet("extended")]
    public Task<IEnumerable<SecretExtendedModel>> GetAllExtended(string workspaceId)
    {
        return this.secretService.GetAllExtended(workspaceId);
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(SecretAccesses.SECRET_SECRETS_READ)]
    [HttpGet("{id}")]
    public Task<SecretModel> GetById(string workspaceId, string id)
    {
        return this.secretService.GetById(workspaceId, id);
    }
    
    /// <summary>
    /// Creates the new object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The create input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(SecretAccesses.SECRET_SECRETS_CREATE)]
    [HttpPost]
    public Task<SecretModel> Create(string workspaceId, SecretCreateModel input)
    {
        return this.secretService.Create(workspaceId, input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(SecretAccesses.SECRET_SECRETS_EDIT)]
    [HttpPut("{id}")]
    public Task<SecretModel> UpdateById(string workspaceId, string id, SecretUpdateModel input)
    {
        return this.secretService.UpdateById(workspaceId, id, input);
    }
    
    /// <summary>
    /// Updates the object value by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(SecretAccesses.SECRET_SECRETS_MANAGE)]
    [HttpPut("{id}/value")]
    public Task<SecretModel> UpdateValueById(string workspaceId, string id, SecretValueUpdateModel input)
    {
        return this.secretService.UpdateValueById(workspaceId, id, input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(SecretAccesses.SECRET_SECRETS_DELETE)]
    [HttpDelete("{id}")]
    public Task<SecretModel> DeleteById(string workspaceId, string id)
    {
        return this.secretService.DeleteById(workspaceId, id);
    }
}

