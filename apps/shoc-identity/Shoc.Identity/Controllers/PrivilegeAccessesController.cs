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
/// The privilege accesses controller.
/// </summary>
[Route("api/privileges/{privilegeId}/accesses")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
public class PrivilegeAccessesController : ControllerBase
{
    /// <summary>
    /// The privilege access service.
    /// </summary>
    private readonly PrivilegeAccessService privilegeAccessService;

    /// <summary>
    /// Creates new instance of the privilege accesses controller.
    /// </summary>
    /// <param name="privilegeAccessService">The privilege access service.</param>
    public PrivilegeAccessesController(PrivilegeAccessService privilegeAccessService)
    {
        this.privilegeAccessService = privilegeAccessService;
    }

    /// <summary>
    /// Gets the accesses assigned to the privilege.
    /// </summary>
    /// <param name="privilegeId">The privilege id.</param>
    /// <returns>A task containing the privilege accesses.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_PRIVILEGES_READ)]
    [HttpGet]
    public Task<IEnumerable<PrivilegeAccessModel>> Get(string privilegeId)
    {
        return this.privilegeAccessService.Get(privilegeId);
    }

    /// <summary>
    /// Updates the accesses assigned to the privilege.
    /// </summary>
    /// <param name="privilegeId">The privilege id.</param>
    /// <param name="input">The privilege access update input.</param>
    /// <returns>A task containing the privilege access update result.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_PRIVILEGE_MANAGE_ACCESS)]
    [HttpPost]
    [HttpPut]
    public Task<PrivilegeAccessUpdateResultModel> Update(string privilegeId, PrivilegeAccessUpdateModel input)
    {
        return this.privilegeAccessService.Update(privilegeId, input);
    }
}
