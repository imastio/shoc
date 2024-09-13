using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Registry.Model;
using Shoc.Registry.Model.Registry;
using Shoc.Registry.Services;

namespace Shoc.Registry.Controllers;

/// <summary>
/// The registries endpoint
/// </summary>
[Route("api/registries")]
[ApiController]
[ShocExceptionHandler]
public class RegistriesController : ControllerBase
{
    /// <summary>
    /// The object service
    /// </summary>
    private readonly RegistryService registryService;

    /// <summary>
    /// Creates new instance of the controller
    /// </summary>
    /// <param name="registryService">The object service</param>
    public RegistriesController(RegistryService registryService)
    {
        this.registryService = registryService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_LIST)]
    [HttpGet]
    public Task<IEnumerable<RegistryModel>> GetAll()
    {
        return this.registryService.GetAll();
    }
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_LIST)]
    [HttpGet("extended")]
    public Task<IEnumerable<RegistryExtendedModel>> GetAllExtended()
    {
        return this.registryService.GetAllExtended();
    }
    
    /// <summary>
    /// Gets all the object options
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_LIST_REFERENCES)]
    [HttpGet("referential-values")]
    public Task<IEnumerable<RegistryReferentialValueModel>> GetAllReferentialValues()
    {
        return this.registryService.GetAllReferentialValues();
    }

    /// <summary>
    /// Gets object by id
    /// </summary>
    /// <param name="id">The id of object</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_READ)]
    [HttpGet("{id}")]
    public Task<RegistryModel> GetById(string id)
    {
        return this.registryService.GetById(id);
    }
    
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="input">The object creation model</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_CREATE)]
    [HttpPost]
    public Task<RegistryModel> Create(RegistryCreateModel input)
    {
        return this.registryService.Create(input);
    }

    /// <summary>
    /// Updates the object by given input
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <param name="input">The object update input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_EDIT)]
    [HttpPut("{id}")]
    public Task<RegistryModel> UpdateById(string id, RegistryUpdateModel input)
    {
        return this.registryService.UpdateById(id, input);
    }

    /// <summary>
    /// Deletes object by id
    /// </summary>
    /// <param name="id">The id of object</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(RegistryAccesses.REGISTRY_REGISTRIES_DELETE)]
    [HttpDelete("{id}")]
    public Task<RegistryModel> DeleteById(string id)
    {
        return this.registryService.DeleteById(id);
    }
}

