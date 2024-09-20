using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Secret.Model;
using Shoc.Secret.Model.UserSecret;
using Shoc.Secret.Services;

namespace Shoc.Secret.Controllers;

/// <summary>
/// The secrets endpoint
/// </summary>
[Route("api/workspaces/{workspaceId}/users/{userId}/secrets")]
[ApiController]
[ShocExceptionHandler]
public class UserSecretsController : ControllerBase
{
    /// <summary>
    /// The secret service
    /// </summary>
    private readonly UserSecretService userSecretService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="userSecretService">The reference to service</param>
    public UserSecretsController(UserSecretService userSecretService)
    {
        this.userSecretService = userSecretService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(SecretAccesses.SECRET_USER_SECRETS_LIST)]
    [HttpGet]
    public Task<IEnumerable<UserSecretModel>> GetAll(string workspaceId, string userId)
    {
        return this.userSecretService.GetAll(workspaceId, userId);
    }

    /// <summary>
    /// Gets all the extended objects
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(SecretAccesses.SECRET_USER_SECRETS_LIST)]
    [HttpGet("extended")]
    public Task<IEnumerable<UserSecretExtendedModel>> GetAllExtended(string workspaceId, string userId)
    {
        return this.userSecretService.GetAllExtended(workspaceId, userId);
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(SecretAccesses.SECRET_USER_SECRETS_READ)]
    [HttpGet("{id}")]
    public Task<UserSecretModel> GetById(string workspaceId, string userId, string id)
    {
        return this.userSecretService.GetById(workspaceId, userId, id);
    }

    /// <summary>
    /// Creates the new object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <param name="input">The create input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(SecretAccesses.SECRET_USER_SECRETS_CREATE)]
    [HttpPost]
    public Task<UserSecretModel> Create(string workspaceId, string userId, UserSecretCreateModel input)
    {
        return this.userSecretService.Create(workspaceId, userId, input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(SecretAccesses.SECRET_USER_SECRETS_EDIT)]
    [HttpPut("{id}")]
    public Task<UserSecretModel> UpdateById(string workspaceId, string userId, string id, UserSecretUpdateModel input)
    {
        return this.userSecretService.UpdateById(workspaceId, userId, id, input);
    }

    /// <summary>
    /// Updates the object value by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(SecretAccesses.SECRET_USER_SECRETS_MANAGE)]
    [HttpPut("{id}/value")]
    public Task<UserSecretModel> UpdateValueById(string workspaceId, string userId, string id, UserSecretValueUpdateModel input)
    {
        return this.userSecretService.UpdateValueById(workspaceId, userId, id, input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(SecretAccesses.SECRET_USER_SECRETS_MANAGE)]
    [HttpDelete("{id}")]
    public Task<UserSecretModel> DeleteById(string workspaceId, string userId, string id)
    {
        return this.userSecretService.DeleteById(workspaceId, userId, id);
    }
}

