﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Package.Model;
using Shoc.Package.Model.Package;
using Shoc.Package.Services;

namespace Shoc.Package.Controllers;

/// <summary>
/// The secrets endpoint
/// </summary>
[Route("api/management/workspaces/{workspaceId}/packages")]
[ApiController]
[ShocExceptionHandler]
public class PackagesController : ControllerBase
{
    /// <summary>
    /// The service
    /// </summary>
    private readonly PackageService packageService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="packageService">The reference to service</param>
    public PackagesController(PackageService packageService)
    {
        this.packageService = packageService;
    }

    /// <summary>
    /// Gets page of the extended objects
    /// </summary>
    /// <returns></returns>
    [HttpGet("extended")]
    [AuthorizeAnyAccess(PackageAccesses.PACKAGE_PACKAGES_LIST)]
    public Task<PackagePageResult<PackageExtendedModel>> GetExtendedPageBy(
        string workspaceId, 
        [FromQuery] string userId,
        [FromQuery] string scope,
        [FromQuery] int page, 
        [FromQuery] int? size)
    {
        return this.packageService.GetExtendedPageBy(workspaceId, new PackageFilter
        {
            UserId = userId,
            Scope = scope
        }, page, size);
    }
    
    /// <summary>
    /// Gets the extended object by id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}/extended")]
    [AuthorizeAnyAccess(PackageAccesses.PACKAGE_PACKAGES_READ)]
    public Task<PackageExtendedModel> GetExtendedById(string workspaceId, string id)
    {
        return this.packageService.GetExtendedById(workspaceId, id);
    }
}

