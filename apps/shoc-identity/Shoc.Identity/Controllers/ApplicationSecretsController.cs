using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Application;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The application secrets controller
/// </summary>
[Route("api/applications/{applicationId}/secrets")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
public class ApplicationSecretsController : ControllerBase
{
    /// <summary>
    /// The application secret service
    /// </summary>
    private readonly ApplicationSecretService applicationSecretService;

    /// <summary>
    /// Creates new instance of application controller
    /// </summary>
    /// <param name="applicationSecretService">The application service</param>
    public ApplicationSecretsController(ApplicationSecretService applicationSecretService)
    {
        this.applicationSecretService = applicationSecretService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_APPLICATIONS_READ)]
    [HttpGet]
    public Task<IEnumerable<ApplicationSecretModel>> GetAll(string applicationId)
    {
        return this.applicationSecretService.GetAll(applicationId);
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of object to request</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_APPLICATIONS_READ)]
    [HttpGet("{id}")]
    public Task<ApplicationSecretModel> GetById(string applicationId, string id)
    {
        return this.applicationSecretService.GetById(applicationId, id);
    }

    /// <summary>
    /// Creates new object based on input
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="input">The object input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_APPLICATIONS_MANAGE)]
    [HttpPost]
    public Task<ApplicationSecretModel> Create(string applicationId, [FromBody] ApplicationSecretModel input)
    {
        return this.applicationSecretService.Create(applicationId, input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of object to request</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_APPLICATIONS_MANAGE)]
    [HttpPut("{id}")]
    public Task<ApplicationSecretModel> UpdateById(string applicationId, string id, [FromBody] ApplicationSecretModel input)
    {
        return this.applicationSecretService.UpdateById(applicationId, id, input);
    }
    
    /// <summary>
    /// Deletes the given object
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of object to delete</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_APPLICATIONS_MANAGE)]
    [HttpDelete("{id}")]
    public Task<ApplicationSecretModel> DeleteById(string applicationId, string id)
    {
        return this.applicationSecretService.DeleteById(applicationId, id);
    }
}