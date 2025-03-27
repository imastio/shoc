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
/// The oidc providers controller
/// </summary>
[Route("api/oidc-providers")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
public class OidcProvidersController : ControllerBase
{
    /// <summary>
    /// The oidc provider service
    /// </summary>
    private readonly OidcProviderService oidcProviderService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="oidcProviderService">The service implementation</param>
    public OidcProvidersController(OidcProviderService oidcProviderService)
    {
        this.oidcProviderService = oidcProviderService;
    }
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_OIDC_PROVIDERS_LIST)]
    [HttpGet]
    public Task<IEnumerable<OidcProviderModel>> GetAll()
    {
        return this.oidcProviderService.GetAll();
    }
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <param name="id">The id of object to request</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_OIDC_PROVIDERS_READ)]
    [HttpGet("{id}")]
    public Task<OidcProviderModel> GetById(string id)
    {
        return this.oidcProviderService.GetById(id);
    }
    
    /// <summary>
    /// Creates new object based on input
    /// </summary>
    /// <param name="input">The object input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_OIDC_PROVIDERS_CREATE)]
    [HttpPost]
    public Task<OidcProviderModel> Create([FromBody] OidcProviderCreateModel input)
    {
        return this.oidcProviderService.Create(input);
    }

    /// <summary>
    /// Update the object with the given input
    /// </summary>
    /// <param name="id">The id of object to request</param>
    /// <param name="input">The update input model</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_OIDC_PROVIDERS_MANAGE)]
    [HttpPut("{id}")]
    public Task<OidcProviderModel> UpdateById(string id, OidcProviderUpdateModel input)
    {
        return this.oidcProviderService.UpdateById(id, input);
    }
    
    /// <summary>
    /// Update the object with the given input (client secret)
    /// </summary>
    /// <param name="id">The id of object to request</param>
    /// <param name="input">The update input model</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_OIDC_PROVIDERS_MANAGE)]
    [HttpPut("{id}/client-secret")]
    public Task<OidcProviderModel> UpdateClientSecretById(string id, OidcProviderClientSecretUpdateModel input)
    {
        return this.oidcProviderService.UpdateClientSecretById(id, input);
    }
    
    /// <summary>
    /// Deletes the given object
    /// </summary>
    /// <param name="id">The id of object to delete</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_OIDC_PROVIDERS_DELETE)]
    [HttpDelete("{id}")]
    public Task<OidcProviderModel> DeleteById(string id)
    {
        return this.oidcProviderService.DeleteById(id);
    }
}