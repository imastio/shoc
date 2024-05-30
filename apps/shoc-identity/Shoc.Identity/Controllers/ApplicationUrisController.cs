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
/// The application uris controller
/// </summary>
[Route("api/applications/{applicationId}/uris")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
public class ApplicationUrisController : ControllerBase
{
    /// <summary>
    /// The application uri service
    /// </summary>
    private readonly ApplicationUriService applicationUriService;

    /// <summary>
    /// Creates new instance of application uris controller
    /// </summary>
    /// <param name="applicationUriService">The application service</param>
    public ApplicationUrisController(ApplicationUriService applicationUriService)
    {
        this.applicationUriService = applicationUriService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_APPLICATIONS_READ)]
    [HttpGet]
    public Task<IEnumerable<ApplicationUriModel>> GetAll(string applicationId)
    {
        return this.applicationUriService.GetAll(applicationId);
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of object to request</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_APPLICATIONS_READ)]
    [HttpGet("{id}")]
    public Task<ApplicationUriModel> GetById(string applicationId, string id)
    {
        return this.applicationUriService.GetById(applicationId, id);
    }

    /// <summary>
    /// Creates new object based on input
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="input">The object input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_APPLICATIONS_MANAGE)]
    [HttpPost]
    public Task<ApplicationUriModel> Create(string applicationId, [FromBody] ApplicationUriModel input)
    {
        return this.applicationUriService.Create(applicationId, input);
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
    public Task<ApplicationUriModel> UpdateById(string applicationId, string id, [FromBody] ApplicationUriModel input)
    {
        return this.applicationUriService.UpdateById(applicationId, id, input);
    }
    
    /// <summary>
    /// Deletes the given object
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of object to delete</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_APPLICATIONS_MANAGE)]
    [HttpDelete("{id}")]
    public Task<ApplicationUriModel> DeleteById(string applicationId, string id)
    {
        return this.applicationUriService.DeleteById(applicationId, id);
    }
}