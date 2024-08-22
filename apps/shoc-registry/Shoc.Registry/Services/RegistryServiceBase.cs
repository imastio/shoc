using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Imast.DataOps.Definitions;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Registry.Data;
using Shoc.Registry.Model;
using Shoc.Registry.Model.Registry;
using Shoc.Workspace.Grpc.Workspaces;

namespace Shoc.Registry.Services;

/// <summary>
/// The base service for registry
/// </summary>
public abstract class RegistryServiceBase
{
    /// <summary>
    /// The name pattern for registry
    /// </summary>
    protected static readonly Regex NAME_PATTERN = new(@"^[a-z\d](?:[a-z\d]|-(?=[a-z\d])){0,38}$");
    
    /// <summary>
    /// The namespace pattern for registry
    /// </summary>
    protected static readonly Regex NAMESPACE_PATTERN = new(@"^[a-z\d](?:[a-z\d]|-(?=[a-z\d])){0,38}$");

    /// <summary>
    /// The pattern to validate the registry string
    /// </summary>
    protected static readonly Regex REGISTRY_PATTERN = new(@"^(?:[a-zA-Z0-9.-]+(?:\.[a-zA-Z]{2,})?(?::\d{1,5})?)$");
    
    /// <summary>
    /// The maximum length of the display length
    /// </summary>
    protected const int MAX_DISPLAY_NAME_LENGTH = 64;
    
    /// <summary>
    /// The object repository
    /// </summary>
    protected readonly IRegistryRepository registryRepository;
    
    /// <summary>
    /// The grpc client provider
    /// </summary>
    private readonly IGrpcClientProvider grpcClientProvider;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="registryRepository">The registry repository</param>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    protected RegistryServiceBase(IRegistryRepository registryRepository, IGrpcClientProvider grpcClientProvider)
    {
        this.registryRepository = registryRepository;
        this.grpcClientProvider = grpcClientProvider;
    }
    
    /// <summary>
    /// Requires the object by id
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    protected async Task<RegistryModel> RequireRegistryById(string id)
    {
        // id should be a valid string
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try getting object by id
        var result = await this.registryRepository.GetById(id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
    }
    
    /// <summary>
    /// Validate workspace by id
    /// </summary>
    /// <param name="id">The id to validate</param>
    protected async Task RequireWorkspace(string id)
    {
        // no user id given
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.Validation(RegistryErrors.INVALID_WORKSPACE).AsException();
        }
        
        // try getting object
        try {
            await this.grpcClientProvider
                .Get<WorkspaceServiceGrpc.WorkspaceServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByIdAsync(new GetWorkspaceByIdRequest{Id = id}, metadata));
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(RegistryErrors.INVALID_WORKSPACE).AsException();
        }
    }

    /// <summary>
    /// Validate the registry name
    /// </summary>
    /// <param name="name">The name of the registry</param>
    /// <exception cref="ShocException"></exception>
    protected static void ValidateName(string name)
    {
        // check if empty or not matching the pattern
        if (string.IsNullOrWhiteSpace(name) || !NAME_PATTERN.IsMatch(name))
        {
            throw ErrorDefinition.Validation(RegistryErrors.INVALID_NAME).AsException();
        }
    }
    
    /// <summary>
    /// Validate object provider
    /// </summary>
    /// <param name="provider">The provider to validate</param>
    protected static void ValidateProvider(string provider)
    {
        // make sure input is valid 
        if (RegistryProviderTypes.ALL.Contains(provider))
        {
            return;
        }

        throw ErrorDefinition.Validation(RegistryErrors.INVALID_PROVIDER).AsException();
    }
    
    /// <summary>
    /// Validate object usage scope
    /// </summary>
    /// <param name="usageScope">The usage scope to validate</param>
    protected static void ValidateUsageScope(string usageScope)
    {
        // make sure input is valid 
        if (RegistryUsageScopes.ALL.Contains(usageScope))
        {
            return;
        }

        throw ErrorDefinition.Validation(RegistryErrors.INVALID_USAGE_SCOPE).AsException();
    }
    
    /// <summary>
    /// Validate object status
    /// </summary>
    /// <param name="status">The status to validate</param>
    protected static void ValidateStatus(string status)
    {
        // make sure input is valid 
        if (RegistryStatuses.ALL.Contains(status))
        {
            return;
        }

        throw ErrorDefinition.Validation(RegistryErrors.INVALID_STATUS).AsException();
    }
    
    /// <summary>
    /// Validate the registry display name
    /// </summary>
    /// <param name="displayName">The display name of the registry</param>
    /// <exception cref="ShocException"></exception>
    protected static void ValidateDisplayName(string displayName)
    {
        // check if empty or not matching the pattern
        if (string.IsNullOrWhiteSpace(displayName) || displayName.Length > MAX_DISPLAY_NAME_LENGTH)
        {
            throw ErrorDefinition.Validation(RegistryErrors.INVALID_DISPLAY_NAME).AsException();
        }
    }

    /// <summary>
    /// Validate the given registry uri
    /// </summary>
    /// <param name="registry">The registry uri to validate</param>
    /// <exception cref="ShocException"></exception>
    protected static void ValidateRegistry(string registry)
    {
        // check if empty or not matching the pattern
        if (string.IsNullOrWhiteSpace(registry) || !REGISTRY_PATTERN.IsMatch(registry))
        {
            throw ErrorDefinition.Validation(RegistryErrors.INVALID_REGISTRY_URI).AsException();
        }
    }

    /// <summary>
    /// Validate the registry namespace
    /// </summary>
    /// <param name="provider">The registry provider</param>
    /// <param name="ns">The namespace of the registry</param>
    /// <exception cref="ShocException"></exception>
    protected static void ValidateNamespace(string provider, string ns)
    {
        // namespace cannot be null
        if (ns == null)
        {
            throw ErrorDefinition.Validation(RegistryErrors.INVALID_NAMESPACE).AsException();
        }
        
        // for shoc provider the namespace always should be empty
        if (provider == RegistryProviderTypes.SHOC && ns != string.Empty)
        {
            throw ErrorDefinition.Validation(RegistryErrors.INVALID_NAMESPACE).AsException();
        }
        
        // otherwise namespace should be a valid string matching the pattern
        if (!NAMESPACE_PATTERN.IsMatch(ns))
        {
            throw ErrorDefinition.Validation(RegistryErrors.INVALID_NAMESPACE).AsException();
        }
    }
}