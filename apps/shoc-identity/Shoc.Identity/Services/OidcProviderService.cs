using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Flow;
using Shoc.Identity.Model.Provider;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The service
/// </summary>
public class OidcProviderService : OidcProviderServiceBase
{
    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    public OidcProviderService(IOidcProviderRepository oidcProviderRepository, IdentityProviderProtectionProvider protectionProvider): base(oidcProviderRepository, protectionProvider)
    {
    }
    
    /// <summary>
    /// Gets the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<OidcProviderModel>> GetAll()
    {
        return this.oidcProviderRepository.GetAll();
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public Task<OidcProviderModel> GetById(string id)
    {
        return this.RequireById(id);
    }
    
    /// <summary>
    /// Gets the object by code
    /// </summary>
    /// <returns></returns>
    public Task<OidcProviderModel> GetByCodeOrNull(string code)
    {
        return this.oidcProviderRepository.GetByCode(code);
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<OidcProviderModel> Create(OidcProviderCreateModel input)
    {
        // default values
        input.Scope ??= DEFAULT_SCOPE;
        input.IconUrl ??= string.Empty;
        input.Type ??= IdentityProviders.OTHER;
        
        // validate name
        ValidateName(input.Name);
        
        // validate code
        ValidateCode(input.Code);
        
        // validate type
        ValidateType(input.Type);
        
        // validate scope
        ValidateScope(input.Scope);
        
        // validate client id
        ValidateClientId(input.ClientId);
        
        // validate client secret
        ValidateClientSecret(input.ClientSecret);
        
        // validate icon url
        ValidateIconUrl(input.IconUrl);
        
        // validate authority
        ValidateAuthority(input.Authority);
        
        // try getting by code
        var existsByCode = await this.GetByCodeOrNull(input.Code);
        
        // check if code is used
        if (existsByCode != null)
        {
            throw ErrorDefinition.Validation(IdentityErrors.EXISTING_PROVIDER_CODE).AsException();
        }

        // create protector
        var protector = this.protectionProvider.Create();

        // encrypt the secret
        input.ClientSecretEncrypted = protector.Protect(input.ClientSecret);
        
        // create in the storage
        return await this.oidcProviderRepository.Create(input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public async Task<OidcProviderModel> UpdateById(string id, OidcProviderUpdateModel input)
    {
        // make sure referring to the proper object
        input.Id = id;

        // default values
        input.Scope ??= DEFAULT_SCOPE;
        input.IconUrl ??= string.Empty;
        input.Type ??= IdentityProviders.OTHER;
        
        // validate name
        ValidateName(input.Name);
        
        // validate code
        ValidateCode(input.Code);
        
        // validate type
        ValidateType(input.Type);
        
        // validate scope
        ValidateScope(input.Scope);
        
        // validate client id
        ValidateClientId(input.ClientId);
        
        // validate icon url
        ValidateIconUrl(input.IconUrl);
        
        // validate authority
        ValidateAuthority(input.Authority);
        
        // make sure object exists
        await this.RequireById(input.Id);
        
        // try getting by code
        var existsByCode = await this.oidcProviderRepository.GetByCode(input.Code);
        
        // check if application id is free
        if (existsByCode != null && existsByCode.Id != input.Id)
        {
            throw ErrorDefinition.Validation(IdentityErrors.EXISTING_PROVIDER_CODE).AsException();
        }

        // update in the storage
        return await this.oidcProviderRepository.UpdateById(id, input);
    }
    
    /// <summary>
    /// Updates the object secret by id
    /// </summary>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public async Task<OidcProviderModel> UpdateClientSecretById(string id, OidcProviderClientSecretUpdateModel input)
    {
        // make sure referring to the proper object
        input.Id = id;
        
        // make sure object exists
        await this.RequireById(input.Id);
        
        // create protector
        var protector = this.protectionProvider.Create();

        // encrypt the secret
        input.ClientSecretEncrypted = protector.Protect(input.ClientSecret);
        
        // update in the storage
        return await this.oidcProviderRepository.UpdateClientSecretById(id, input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public async Task<OidcProviderModel> DeleteById(string id)
    {
        // delete the object
        var existing = await this.oidcProviderRepository.DeleteById(id);

        // check if exists
        if (existing == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        return existing;
    }
}