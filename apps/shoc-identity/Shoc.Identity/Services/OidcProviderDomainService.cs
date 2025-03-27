using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Provider;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The service
/// </summary>
public class OidcProviderDomainService : OidcProviderServiceBase
{
    /// <summary>
    /// The domain repository
    /// </summary>
    protected readonly IOidcProviderDomainRepository domainRepository;
    
    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    public OidcProviderDomainService(IOidcProviderRepository oidcProviderRepository, IdentityProviderProtectionProvider protectionProvider, IOidcProviderDomainRepository domainRepository): base(oidcProviderRepository, protectionProvider)
    {
        this.domainRepository = domainRepository;
    }
    
    /// <summary>
    /// Gets the objects
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<OidcProviderDomainModel>> GetAll(string providerId)
    {
        await this.RequireById(providerId);
        
        return await this.domainRepository.GetAll(providerId);
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public async Task<OidcProviderDomainModel> GetById(string providerId, string id)
    {
        await this.RequireById(providerId);

        // try getting the object by id
        var result = await this.domainRepository.GetById(providerId, id);

        // ensure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        return result;
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<OidcProviderDomainModel> Create(string providerId, OidcProviderDomainCreateModel input)
    {
        // ensure referring to a correct object
        input.ProviderId = providerId;
        
        // validate domain name
        ValidateDomainName(input.DomainName);
        
        // require parent object
        await this.RequireById(providerId);
        
        // try getting by domain name
        var existsByName = await this.domainRepository.GetByDomainName(providerId, input.DomainName);
        
        // check if domain name is used
        if (existsByName != null)
        {
            throw ErrorDefinition.Validation(IdentityErrors.EXISTING_PROVIDER_DOMAIN_NAME).AsException();
        }
        
        // create in the storage
        return await this.domainRepository.Create(providerId, input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public async Task<OidcProviderDomainModel> UpdateById(string providerId, string id, OidcProviderDomainUpdateModel input)
    {
        // make sure referring to the proper object
        input.ProviderId = providerId;
        input.Id = id;
        
        // validate domain name
        ValidateDomainName(input.DomainName);
        
        // make sure object exists
        await this.RequireById(providerId);
        
        // try getting by code
        var existingByName = await this.domainRepository.GetByDomainName(providerId, input.DomainName);
        
        // check if name is free
        if (existingByName != null && existingByName.Id != input.Id)
        {
            throw ErrorDefinition.Validation(IdentityErrors.EXISTING_PROVIDER_DOMAIN_NAME).AsException();
        }

        // update in the storage
        return await this.domainRepository.UpdateById(providerId, id, input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public async Task<OidcProviderDomainModel> DeleteById(string providerId, string id)
    {
        // make sure object exists
        await this.RequireById(providerId);
        
        // delete the object
        var existing = await this.domainRepository.DeleteById(providerId, id);

        // check if exists
        if (existing == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        return existing;
    }
}