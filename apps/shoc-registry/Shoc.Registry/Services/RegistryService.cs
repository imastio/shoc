using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Registry.Data;
using Shoc.Registry.Model;
using Shoc.Registry.Model.Registry;

namespace Shoc.Registry.Services;

/// <summary>
/// The registry service
/// </summary>
public class RegistryService : RegistryServiceBase
{
    /// <summary>
    /// Creates new instance of registry service
    /// </summary>
    /// <param name="registryRepository">The registry repository</param>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    /// <param name="dataProtectionProvider">The data protection provider</param>
    public RegistryService(IRegistryRepository registryRepository, IGrpcClientProvider grpcClientProvider, IDataProtectionProvider dataProtectionProvider) : base(registryRepository, grpcClientProvider, dataProtectionProvider)
    {
    }
    
    /// <summary>
    /// Gets all objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<RegistryModel>> GetAll()
    {
        return this.registryRepository.GetAll();
    }
    
    /// <summary>
    /// Gets all extended objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<RegistryExtendedModel>> GetAllExtended()
    {
        return this.registryRepository.GetAllExtended();
    }
    
    /// <summary>
    /// Gets the workspace referential records
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<RegistryReferentialValueModel>> GetAllReferentialValues()
    {
        // load matching objects
        return this.registryRepository.GetAllReferentialValues();
    }
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<RegistryModel> GetById(string id)
    {
        return this.RequireRegistryById(id);
    }

    /// <summary>
    /// Gets the object by workspace id and the name 
    /// </summary>
    /// <returns></returns>
    public async Task<RegistryModel> GetByName(string workspaceId, string name)
    {
        // make sure workspace and name are given
        if (string.IsNullOrWhiteSpace(workspaceId) || string.IsNullOrWhiteSpace(name))
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        // try getting the result
        var result = await this.registryRepository.GetByName(workspaceId, name);

        // check if object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    } 
    
    /// <summary>
    /// Gets the object by global name 
    /// </summary>
    /// <returns></returns>
    public async Task<RegistryModel> GetByGlobalName(string name)
    {
        // make sure workspace and name are given
        if (string.IsNullOrWhiteSpace(name))
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        // try getting the result
        var result = await this.registryRepository.GetByGlobalName(name);

        // check if object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<RegistryModel> Create(RegistryCreateModel input)
    {
        // active on creation
        input.Status = RegistryStatuses.ACTIVE;
        
        // use name as display name if missing
        input.DisplayName ??= input.Name;
        
        // empty namespace if not given
        input.Namespace ??= string.Empty;
        
        // validate the name
        ValidateName(input.Name);
        
        // validate the provider
        ValidateProvider(input.Provider);
        
        // validate the status
        ValidateStatus(input.Status);
        
        // validate the uri
        ValidateRegistry(input.Registry);
        
        // validate the display name
        ValidateDisplayName(input.DisplayName);
        
        // validates the usage scope
        ValidateUsageScope(input.UsageScope);
        
        // validate the namespace for the given provider
        ValidateNamespace(input.Provider, input.Namespace);
        
        // try getting an object by name (global or workspace-level)
        var existingByName = input.WorkspaceId == null
            ? await this.registryRepository.GetByGlobalName(input.Name)
            : await this.registryRepository.GetByName(input.WorkspaceId, input.Name);
        
        // report error if object by name exists (global or workspace-level)
        if (existingByName != null)
        {
            throw ErrorDefinition.Validation(RegistryErrors.EXISTING_NAME).AsException();
        }
        
        // in case if usage scope is workspace require the workspace to exist
        if (input.UsageScope == RegistryUsageScopes.WORKSPACE)
        {
            await this.RequireWorkspace(input.WorkspaceId);
        }

        // for the global registry we should not have any workspace id
        if (input.UsageScope == RegistryUsageScopes.GLOBAL && !string.IsNullOrWhiteSpace(input.WorkspaceId))
        {
            throw ErrorDefinition.Validation(RegistryErrors.INVALID_WORKSPACE).AsException();
        }

        // create in the storage
        return await this.registryRepository.Create(input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public async Task<RegistryModel> UpdateById(string id, RegistryUpdateModel input)
    {
        // make sure referring to the correct object
        input.Id = id;
        
        // ensure object exists and throw otherwise
        var existing = await this.RequireRegistryById(input.Id);

        // use name as display name if not given
        input.DisplayName ??= input.Name;
        
        // empty namespace if not given
        input.Namespace ??= string.Empty;
        
        // validate the name
        ValidateName(input.Name);
                
        // validate the status
        ValidateStatus(input.Status);
        
        // validate the uri
        ValidateRegistry(input.Registry);
        
        // validate the display name
        ValidateDisplayName(input.DisplayName);
        
        // validate the namespace for the given provider
        ValidateNamespace(existing.Provider, input.Namespace);

        // try getting an object by name (global or workspace-level)
        var existingByName = existing.WorkspaceId == null
            ? await this.registryRepository.GetByGlobalName(input.Name)
            : await this.registryRepository.GetByName(existing.WorkspaceId, input.Name);
        
        // if object exists but not referring to the current object then report name conflict
        if (existingByName != null && existingByName.Id != existing.Id)
        {
            throw ErrorDefinition.Validation(RegistryErrors.EXISTING_NAME).AsException();
        }

        // update in the storage
        return await this.registryRepository.UpdateById(id, input);
    }
    
    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="id">The id of object</param>
    /// <returns></returns>
    public async Task<RegistryModel> DeleteById(string id)
    {
        // try deleting object by id
        var result = await this.registryRepository.DeleteById(id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
    }
}