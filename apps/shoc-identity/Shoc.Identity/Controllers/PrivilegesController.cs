using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Privileges;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The privileges controller.
/// </summary>
[Route("api/privileges")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
public class PrivilegesController : ControllerBase
{
    /// <summary>
    /// The privilege service.
    /// </summary>
    private readonly PrivilegeService privilegeService;

    /// <summary>
    /// Creates a new instance of the privileges controller.
    /// </summary>
    /// <param name="privilegeService">The privilege service.</param>
    public PrivilegesController(PrivilegeService privilegeService)
    {
        this.privilegeService = privilegeService;
    }

    /// <summary>
    /// Get all privileges.
    /// </summary>
    /// <returns>A task containing all privileges.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_PRIVILEGES_LIST)]
    [HttpGet]
    public Task<IEnumerable<PrivilegeModel>> GetAll()
    {
        return this.privilegeService.GetAll();
    }

    /// <summary>
    /// Get all privilege referential values.
    /// </summary>
    /// <returns>A task containing all privilege references.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_PRIVILEGES_LIST_REFERENCES)]
    [HttpGet("referential-values")]
    public async Task<IEnumerable<PrivilegeReferentialValueModel>> GetAllReferentialValues()
    {
        return await this.privilegeService.GetAllReferentialValues();
    }

    /// <summary>
    /// Get the privilege by id.
    /// </summary>
    /// <param name="id">The privilege id.</param>
    /// <returns>A task containing the privilege.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_PRIVILEGES_READ)]
    [HttpGet("{id}")]
    public Task<PrivilegeModel> GetById(string id)
    {
        return this.privilegeService.GetById(id);
    }

    /// <summary>
    /// Creates privilege by given input.
    /// </summary>
    /// <param name="input">The privilege creation input.</param>
    /// <returns>A task containing the newly created privilege.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_PRIVILEGES_CREATE)]
    [HttpPost]
    public Task<PrivilegeModel> Create([FromBody] PrivilegeCreateModel input)
    {
        return this.privilegeService.Create(input);
    }

    /// <summary>
    /// Updates the privilege by given input.
    /// </summary>
    /// <param name="id">The privilege id.</param>
    /// <param name="input">The privilege update input model.</param>
    /// <returns>A task containing the updated privilege.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_PRIVILEGES_EDIT)]
    [HttpPut("{id}")]
    public Task<PrivilegeModel> UpdateById(string id, PrivilegeUpdateModel input)
    {
        return this.privilegeService.UpdateById(id, input);
    }

    /// <summary>
    /// Deletes the privilege by specified id.
    /// </summary>
    /// <param name="id">The privilege id.</param>
    /// <returns>A task containing the deleted privilege.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_PRIVILEGES_DELETE)]
    [HttpDelete("{id}")]
    public Task<PrivilegeModel> DeleteById(string id)
    {
        return this.privilegeService.DeleteById(id);
    }
}
