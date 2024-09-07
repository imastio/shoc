using System;
using System.Threading.Tasks;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Registry.Crypto;
using Shoc.Registry.Model.Registry;
using Shoc.Workspace.Grpc.Workspaces;

namespace Shoc.Registry.Services;

/// <summary>
/// The authentication service base
/// </summary>
public abstract class AuthenticationServiceBase
{
    /// <summary>
    /// The registry service
    /// </summary>
    protected readonly RegistryService registryService;
    
    /// <summary>
    /// The registry signing key service
    /// </summary>
    protected readonly RegistrySigningKeyService registrySigningKeyService;
    
    /// <summary>
    /// The key provider service
    /// </summary>
    protected readonly KeyProviderService keyProviderService;

    /// <summary>
    /// The grpc client provider
    /// </summary>
    protected readonly IGrpcClientProvider grpcClientProvider;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="registryService">The registry service</param>
    /// <param name="registrySigningKeyService">The signing key service</param>
    /// <param name="keyProviderService">The key provider service</param>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    protected AuthenticationServiceBase(RegistryService registryService, RegistrySigningKeyService registrySigningKeyService, KeyProviderService keyProviderService, IGrpcClientProvider grpcClientProvider)
    {
        this.registryService = registryService;
        this.registrySigningKeyService = registrySigningKeyService;
        this.keyProviderService = keyProviderService;
        this.grpcClientProvider = grpcClientProvider;
    }
    

    /// <summary>
    /// Gets the registry based on workspace and registry name
    /// </summary>
    /// <param name="workspaceName">The workspace name (optional)</param>
    /// <param name="registryName">The registry name</param>
    /// <returns></returns>
    /// <exception cref="ShocException"></exception>
    protected async Task<RegistryModel> GetRegistry(string workspaceName, string registryName)
    {
        // require a non-empty name for the registry
        if (string.IsNullOrWhiteSpace(registryName))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // detect if global registry is requested
        var global = string.IsNullOrWhiteSpace(workspaceName) || string.Equals(workspaceName, "_");
        
        // the id of the workspace
        var workspaceId = default(string);

        // if workspace name is given try lookup
        if (!global)
        {
            // get workspace by name
            var workspace = await this.GetWorkspaceByName(workspaceName);
            
            // use the id of the workspace
            workspaceId = workspace.Id;
        }
        
        // get the registry object by the name (global or workspace)
        return global
            ? await this.registryService.GetByGlobalName(registryName)
            : await this.registryService.GetByName(workspaceId, registryName);
    }

    /// <summary>
    /// Gets the workspace by the name
    /// </summary>
    /// <param name="name">The workspace name</param>
    /// <returns></returns>
    /// <exception cref="ShocException"></exception>
    protected async Task<WorkspaceGrpcModel> GetWorkspaceByName(string name)
    {
        // try getting object
        try {
            var result =  await this.grpcClientProvider
                .Get<WorkspaceServiceGrpc.WorkspaceServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByNameAsync(new GetWorkspaceByNameRequest{Name = name}, metadata));

            return result.Workspace;
        }
        catch(Exception)
        {
            throw ErrorDefinition.NotFound().AsException();
        }
    }
}