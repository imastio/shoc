using System.Collections.Generic;
using System.Threading.Tasks;
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
    public RegistryService(IRegistryRepository registryRepository, IGrpcClientProvider grpcClientProvider) : base(registryRepository, grpcClientProvider)
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

        return await this.registryRepository.Create(input);
    }
}