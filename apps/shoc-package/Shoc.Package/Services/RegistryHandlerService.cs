using System;
using System.Threading.Tasks;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Package.Model;
using Shoc.Package.Model.Package;
using Shoc.Package.Model.Registry;
using Shoc.Registry.Grpc.Registries;

namespace Shoc.Package.Services;

/// <summary>
/// The registry handler service
/// </summary>
public class RegistryHandlerService
{
    /// <summary>
    /// The grpc client provider
    /// </summary>
    private readonly IGrpcClientProvider grpcClientProvider;
    
    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    public RegistryHandlerService(IGrpcClientProvider grpcClientProvider)
    {
        this.grpcClientProvider = grpcClientProvider;
    }

    /// <summary>
    /// Builds the image tag by context
    /// </summary>
    /// <param name="context">The context</param>
    /// <returns></returns>
    public virtual string BuildImageTag(RegistryImageContext context)
    {
        // for shoc provider use nested scheme
        if (context.Provider == KnownRegistryProviders.SHOC)
        {
            // handle both for user-scoped and workspace-scoped packages
            return context.TargetPackageScope == PackageScopes.WORKSPACE ? 
                $"{context.Registry}/w/{context.TargetWorkspaceId}/{context.TargetPackageId}" : 
                $"{context.Registry}/w/{context.TargetWorkspaceId}/u/{context.TargetUserId}/{context.TargetPackageId}";
        }
        
        // for other providers do with given namespace
        return $"{context.Registry}/{context.Namespace}/{context.TargetWorkspaceId}{context.TargetPackageId}";
    }
    
    /// <summary>
    /// Gets the registry to store the package
    /// </summary>
    /// <param name="id">The workspace id</param>
    /// <returns></returns>
    public async Task<RegistryGrpcModel> GetRegistryById(string id)
    {
        // try getting object
        try {
            var result = await this.grpcClientProvider
                .Get<RegistryServiceGrpc.RegistryServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByIdAsync(new GetRegistryByIdRequest{Id = id}, metadata));

            return result.Registry;
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_REGISTRY).AsException();
        }
    }
    
    /// <summary>
    /// Gets the registry to store the package
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    public async Task<RegistryGrpcModel> GetDefaultRegistryId(string workspaceId)
    {
        // try getting object
        try {
            var result = await this.grpcClientProvider
                .Get<WorkspaceDefaultRegistryServiceGrpc.WorkspaceDefaultRegistryServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByWorkspaceIdAsync(new GetWorkspaceDefaultRegistryRequest{WorkspaceId = workspaceId}, metadata));

            return result.Registry;
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_REGISTRY).AsException();
        }
    }

    /// <summary>
    /// Gets the push credential for the registry
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public async Task<RegistryPlainCredentialGrpcModel> GetPushCredential(string registryId, string workspaceId, string userId)
    {
        // try getting object
        try {
            var result = await this.grpcClientProvider
                .Get<RegistryPlainCredentialServiceGrpc.RegistryPlainCredentialServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetPushCredentialOrCreateAsync(new GetRegistryPlainCredentialRequest
                {
                    RegistryId = registryId,
                    WorkspaceId = workspaceId,
                    UserId = userId
                }, metadata));

            return result.Credential;
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_REGISTRY_CREDENTIALS).AsException();
        }
    }
    
    /// <summary>
    /// Gets the pull credential for the registry
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public async Task<RegistryPlainCredentialGrpcModel> GetPullCredential(string registryId, string workspaceId, string userId)
    {
        // try getting object
        try {
            var result = await this.grpcClientProvider
                .Get<RegistryPlainCredentialServiceGrpc.RegistryPlainCredentialServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetPullCredentialOrCreateAsync(new GetRegistryPlainCredentialRequest
                {
                    RegistryId = registryId,
                    WorkspaceId = workspaceId,
                    UserId = userId
                }, metadata));

            return result.Credential;
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_REGISTRY_CREDENTIALS).AsException();
        }
    }
}