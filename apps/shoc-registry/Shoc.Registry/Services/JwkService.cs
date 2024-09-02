using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Registry.Crypto;
using Shoc.Workspace.Grpc.Workspaces;

namespace Shoc.Registry.Services;

/// <summary>
/// The JWK key service
/// </summary>
public class JwkService
{
    /// <summary>
    /// The registry service
    /// </summary>
    private readonly RegistryService registryService;
    
    /// <summary>
    /// The registry signing key service
    /// </summary>
    private readonly RegistrySigningKeyService registrySigningKeyService;
    
    /// <summary>
    /// The key provider service
    /// </summary>
    private readonly KeyProviderService keyProviderService;

    /// <summary>
    /// The grpc client provider
    /// </summary>
    private readonly IGrpcClientProvider grpcClientProvider;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="registryService">The registry service</param>
    /// <param name="registrySigningKeyService">The signing key service</param>
    /// <param name="keyProviderService">The key provider service</param>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    public JwkService(RegistryService registryService, RegistrySigningKeyService registrySigningKeyService, KeyProviderService keyProviderService, IGrpcClientProvider grpcClientProvider)
    {
        this.registryService = registryService;
        this.registrySigningKeyService = registrySigningKeyService;
        this.keyProviderService = keyProviderService;
        this.grpcClientProvider = grpcClientProvider;
    }

    /// <summary>
    /// Gets the set of JWK keys for the given registry (global or workspace)
    /// </summary>
    /// <param name="workspaceName">The workspace name (optional)</param>
    /// <param name="registryName">The registry name</param>
    /// <returns></returns>
    public async Task<IEnumerable<JsonWebKey>> GetJwks(string workspaceName, string registryName)
    {
        // require a non-empty name for the registry
        if (string.IsNullOrWhiteSpace(registryName))
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        // the id of the workspace
        var workspaceId = default(string);

        // if workspace name is given try lookup
        if (!string.IsNullOrWhiteSpace(workspaceName))
        {
            // get workspace by name
            var workspace = await this.GetWorkspaceByName(workspaceName);
            
            // use the id of the workspace
            workspaceId = workspace.Id;
        }

        // if no workspace is given we need a global registry
        var global = string.IsNullOrWhiteSpace(workspaceId);

        // get the registry object by the name (global or workspace)
        var registry = global
            ? await this.registryService.GetByGlobalName(registryName)
            : await this.registryService.GetByName(workspaceId, registryName);

        // gets all the keys in the registry
        var payloads = await this.registrySigningKeyService.GetAllPayloads(registry.Id);

        // transform the record into a JWK 
        return payloads.Select(payload =>
        {
            // convert to asymmetric key
            var key = this.keyProviderService.ToSecurityKey(payload);

            // convert to JWK
            var jwk = JsonWebKeyConverter.ConvertFromSecurityKey(key);
            
            key.
        });

    }


    /// <summary>
    /// Gets the workspace by the name
    /// </summary>
    /// <param name="name">The workspace name</param>
    /// <returns></returns>
    /// <exception cref="ShocException"></exception>
    private async Task<WorkspaceGrpcModel> GetWorkspaceByName(string name)
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