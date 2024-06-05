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
/// The application claims controller
/// </summary>
[Route("api/applications/{applicationId}/claims")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
public class ApplicationClaimsController : ControllerBase
{
    /// <summary>
    /// The application claim service
    /// </summary>
    private readonly ApplicationClaimService applicationClaimService;

    /// <summary>
    /// Creates new instance of application claims controller
    /// </summary>
    /// <param name="applicationClaimService">The application service</param>
    public ApplicationClaimsController(ApplicationClaimService applicationClaimService)
    {
        this.applicationClaimService = applicationClaimService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_APPLICATIONS_READ)]
    [HttpGet]
    public Task<IEnumerable<ApplicationClaimModel>> GetAll(string applicationId)
    {
        return this.applicationClaimService.GetAll(applicationId);
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of object to request</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_APPLICATIONS_READ)]
    [HttpGet("{id}")]
    public Task<ApplicationClaimModel> GetById(string applicationId, string id)
    {
        return this.applicationClaimService.GetById(applicationId, id);
    }

    /// <summary>
    /// Creates new object based on input
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="input">The object input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_APPLICATIONS_MANAGE)]
    [HttpPost]
    public Task<ApplicationClaimModel> Create(string applicationId, [FromBody] ApplicationClaimModel input)
    {
        return this.applicationClaimService.Create(applicationId, input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of object to request</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_APPLICATIONS_MANAGE)]
    [HttpPut("{id}")]
    public Task<ApplicationClaimModel> UpdateById(string applicationId, string id, [FromBody] ApplicationClaimModel input)
    {
        return this.applicationClaimService.UpdateById(applicationId, id, input);
    }
    
    /// <summary>
    /// Deletes the given object
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of object to delete</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_APPLICATIONS_MANAGE)]
    [HttpDelete("{id}")]
    public Task<ApplicationClaimModel> DeleteById(string applicationId, string id)
    {
        return this.applicationClaimService.DeleteById(applicationId, id);
    }
}