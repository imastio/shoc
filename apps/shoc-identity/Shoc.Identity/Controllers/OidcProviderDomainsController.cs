using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Provider;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The oidc provider domains controller
/// </summary>
[Route("api/oidc-providers/{providerId}/domains")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
public class OidcProviderDomainsController : ControllerBase
{
    /// <summary>
    /// The oidc provider domains service
    /// </summary>
    private readonly OidcProviderDomainService oidcProviderDomainService;

    /// <summary>
    /// Creates new instance of oidc provider domains controller
    /// </summary>
    /// <param name="oidcProviderDomainService">The oidc provider domains</param>
    public OidcProviderDomainsController(OidcProviderDomainService oidcProviderDomainService)
    {
        this.oidcProviderDomainService = oidcProviderDomainService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_OIDC_PROVIDERS_READ)]
    [HttpGet]
    public Task<IEnumerable<OidcProviderDomainModel>> GetAll(string providerId)
    {
        return this.oidcProviderDomainService.GetAll(providerId);
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="id">The id of object to request</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_OIDC_PROVIDERS_READ)]
    [HttpGet("{id}")]
    public Task<OidcProviderDomainModel> GetById(string providerId, string id)
    {
        return this.oidcProviderDomainService.GetById(providerId, id);
    }

    /// <summary>
    /// Creates new object based on input
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="input">The object input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_OIDC_PROVIDERS_MANAGE)]
    [HttpPost]
    public Task<OidcProviderDomainModel> Create(string providerId, [FromBody] OidcProviderDomainCreateModel input)
    {
        return this.oidcProviderDomainService.Create(providerId, input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="id">The id of object to request</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_OIDC_PROVIDERS_MANAGE)]
    [HttpPut("{id}")]
    public Task<OidcProviderDomainModel> UpdateById(string providerId, string id, [FromBody] OidcProviderDomainUpdateModel input)
    {
        return this.oidcProviderDomainService.UpdateById(providerId, id, input);
    }
    
    /// <summary>
    /// Deletes the given object
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="id">The id of object to delete</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_OIDC_PROVIDERS_MANAGE)]
    [HttpDelete("{id}")]
    public Task<OidcProviderDomainModel> DeleteById(string providerId, string id)
    {
        return this.oidcProviderDomainService.DeleteById(providerId, id);
    }
}