using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Registry.Model;
using Shoc.Registry.Model.Credential;
using Shoc.Registry.Services;

namespace Shoc.Registry.Controllers;

/// <summary>
/// The registry credentials endpoints
/// </summary>
[Route("api/registries/{registryId}/credentials")]
[ApiController]
[ShocExceptionHandler]
public class RegistryCredentialsController : ControllerBase
{
    /// <summary>
    /// The registry credentials service
    /// </summary>
    private readonly RegistryCredentialService registryCredentialService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="registryCredentialService">The registry credential service</param>
    public RegistryCredentialsController(RegistryCredentialService registryCredentialService)
    {
        this.registryCredentialService = registryCredentialService;
    }

    /// <summary>
    /// Gets all the credentials of the registry
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_READ)]
    [HttpGet]
    public Task<IEnumerable<RegistryCredentialModel>> GetAll(string registryId)
    {
        return this.registryCredentialService.GetAll(registryId);
    }
    
    /// <summary>
    /// Gets all the extended credentials of the registry
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_READ)]
    [HttpGet("extended")]
    public Task<IEnumerable<RegistryCredentialExtendedModel>> GetAllExtended(string registryId)
    {
        return this.registryCredentialService.GetAllExtended(registryId);
    }

    /// <summary>
    /// Creates new registry credential
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="input">The create input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_MANAGE)]
    [HttpPost]
    public Task<RegistryCredentialModel> Create(string registryId, RegistryCredentialCreateModel input)
    {
        return this.registryCredentialService.Create(registryId, input);
    }

    /// <summary>
    /// Update the existing credential record
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The record id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_MANAGE)]
    [HttpPut("{id}")]
    public Task<RegistryCredentialModel> UpdateById(string registryId, string id, RegistryCredentialUpdateModel input)
    {
        return this.registryCredentialService.UpdateById(registryId, id, input);
    }
    
    /// <summary>
    /// Update the existing credential password record
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The record id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_MANAGE)]
    [HttpPut("{id}/password")]
    public Task<RegistryCredentialModel> UpdatePasswordById(string registryId, string id, RegistryCredentialPasswordUpdateModel input)
    {
        return this.registryCredentialService.UpdatePasswordById(registryId, id, input);
    }

    /// <summary>
    /// Deletes the credential from the registry
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_MANAGE)]
    [HttpDelete("{id}")]
    public Task<RegistryCredentialModel> DeleteById(string registryId, string id)
    {
        return this.registryCredentialService.DeleteById(registryId, id);
    }
}

