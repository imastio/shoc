using System;
using System.Threading.Tasks;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Job.Model;
using Shoc.Package.Grpc.Packages;
using Shoc.Registry.Grpc.Registries;

namespace Shoc.Job.Services;

/// <summary>
/// The package resolver
/// </summary>
public class JobPackageResolver
{
    /// <summary>
    /// The grpc client provider
    /// </summary>
    private readonly IGrpcClientProvider grpcClientProvider;
    
    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    public JobPackageResolver(IGrpcClientProvider grpcClientProvider)
    {
        this.grpcClientProvider = grpcClientProvider;
    }
    
    /// <summary>
    /// Gets the package object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <returns></returns>
    public async Task<PackageGrpcModel> ResolveById(string workspaceId, string id)
    {
        // no id provided
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_PACKAGE, "No package provided").AsException();
        }
        
        // try getting object
        try {
            var result = await this.grpcClientProvider
                .Get<PackageServiceGrpc.PackageServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByIdAsync(new GetPackageByIdRequest
                {
                    WorkspaceId = workspaceId,
                    Id = id
                }, metadata));

            return result.Package;
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_PACKAGE, "Could not find the referenced package").AsException();
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
            throw ErrorDefinition.Validation(JobErrors.INVALID_REGISTRY_CREDENTIALS, "Could not find credentials to pull the image").AsException();
        }
    }
}