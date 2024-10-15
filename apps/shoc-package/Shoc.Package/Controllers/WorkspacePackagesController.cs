using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.Access.Model;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.ApiCore.Auth;
using Shoc.Package.Model.Package;
using Shoc.Package.Services;

namespace Shoc.Package.Controllers;

/// <summary>
/// The secrets endpoint
/// </summary>
[Route("api/workspaces/{workspaceId}/packages")]
[ApiController]
[ShocExceptionHandler]
[AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
public class WorkspacePackagesController : ControllerBase
{
    /// <summary>
    /// The service
    /// </summary>
    private readonly WorkspacePackageService packageService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="packageService">The reference to service</param>
    public WorkspacePackagesController(WorkspacePackageService packageService)
    {
        this.packageService = packageService;
    }

    /// <summary>
    /// Creates a new duplicate object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    [HttpPost("from-cache")]
    public Task<PackageModel> DuplicateFrom(string workspaceId, [FromBody] PackageDuplicateFromModel input)
    {
        return this.packageService.DuplicateFrom(this.HttpContext.GetPrincipal().Id, workspaceId, input);
    }
}

