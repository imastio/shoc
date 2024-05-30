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
/// The applications controller
/// </summary>
[Route("api/applications")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
public class ApplicationsController : ControllerBase
{
    /// <summary>
    /// The application service
    /// </summary>
    private readonly ApplicationService applicationService;

    /// <summary>
    /// Creates new instance of application controller
    /// </summary>
    /// <param name="applicationService">The application service</param>
    public ApplicationsController(ApplicationService applicationService)
    {
        this.applicationService = applicationService;
    }
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_APPLICATIONS_LIST)]
    [HttpGet]
    public Task<IEnumerable<ApplicationModel>> GetAll()
    {
        return this.applicationService.GetAll();
    }
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <param name="id">The id of object to request</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_APPLICATIONS_READ)]
    [HttpGet("{id}")]
    public Task<ApplicationModel> GetById(string id)
    {
        return this.applicationService.GetById(id);
    }
    
    /// <summary>
    /// Creates new object based on input
    /// </summary>
    /// <param name="input">The object input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_APPLICATIONS_MANAGE)]
    [HttpPost]
    public Task<ApplicationModel> Create([FromBody] ApplicationCreateModel input)
    {
        return this.applicationService.Create(input);
    }

    /// <summary>
    /// Update the object with the given input
    /// </summary>
    /// <param name="id">The id of object to request</param>
    /// <param name="input">The update input model</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_APPLICATIONS_MANAGE)]
    [HttpPut("{id}")]
    public Task<ApplicationModel> UpdateById(string id, ApplicationModel input)
    {
        return this.applicationService.UpdateById(id, input);
    }
    
    /// <summary>
    /// Deletes the given object
    /// </summary>
    /// <param name="id">The id of object to delete</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_APPLICATIONS_MANAGE)]
    [HttpDelete("{id}")]
    public Task<ApplicationModel> DeleteById(string id)
    {
        return this.applicationService.DeleteById(id);
    }
}