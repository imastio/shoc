using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Registry.Model;
using Shoc.Registry.Model.Key;
using Shoc.Registry.Services;

namespace Shoc.Registry.Controllers;

/// <summary>
/// The registry signing keys endpoints
/// </summary>
[Route("api/registries/{registryId}/signing-keys")]
[ApiController]
[ShocExceptionHandler]
public class RegistrySigningKeysController : ControllerBase
{
    /// <summary>
    /// The signing keys service
    /// </summary>
    private readonly RegistrySigningKeyService registrySigningKeyService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="registrySigningKeyService">The registry signing key service</param>
    public RegistrySigningKeysController(RegistrySigningKeyService registrySigningKeyService)
    {
        this.registrySigningKeyService = registrySigningKeyService;
    }

    /// <summary>
    /// Gets all the signing keys of the registry
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_READ)]
    [HttpGet]
    public Task<IEnumerable<RegistrySigningKeyModel>> GetAll(string registryId)
    {
        return this.registrySigningKeyService.GetAll(registryId);
    }
    
    /// <summary>
    /// Creates new registry signing key
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="input">The create input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_MANAGE)]
    [HttpPost]
    public Task<RegistrySigningKeyModel> Create(string registryId, RegistrySigningKeyCreateModel input)
    {
        return this.registrySigningKeyService.Create(registryId, input);
    }

    /// <summary>
    /// Deletes the signing key from the registry
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_MANAGE)]
    [HttpDelete("{id}")]
    public Task<RegistrySigningKeyModel> DeleteById(string registryId, string id)
    {
        return this.registrySigningKeyService.DeleteById(registryId, id);
    }
}

