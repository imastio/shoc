using System;
using System.Threading.Tasks;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Identity.Grpc.Users;
using Shoc.Package.Model;
using Shoc.Package.Model.Package;
using Shoc.Workspace.Grpc.Workspaces;

namespace Shoc.Package.Services;

/// <summary>
/// The validation service
/// </summary>
public class ValidationService
{
    /// <summary>
    /// The grpc client provider
    /// </summary>
    private readonly IGrpcClientProvider grpcClientProvider;

    /// <summary>
    /// Create a new service
    /// </summary>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    public ValidationService(IGrpcClientProvider grpcClientProvider)
    {
        this.grpcClientProvider = grpcClientProvider;
    }
    
    /// <summary>
    /// Validate workspace by id
    /// </summary>
    /// <param name="workspaceId">The id to validate</param>
    public async Task RequireWorkspace(string workspaceId)
    {
        // no workspace id given
        if (string.IsNullOrWhiteSpace(workspaceId))
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_WORKSPACE).AsException();
        }
        
        // try getting object
        try {
            await this.grpcClientProvider
                .Get<WorkspaceServiceGrpc.WorkspaceServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByIdAsync(new GetWorkspaceByIdRequest{Id = workspaceId}, metadata));
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_WORKSPACE).AsException();
        }
    }
    
    /// <summary>
    /// Validate user by id
    /// </summary>
    /// <param name="userId">The id to validate</param>
    public async Task RequireUser(string userId)
    {
        // no user id given
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_USER).AsException();
        }
        
        // try getting object
        try {
            await this.grpcClientProvider
                .Get<UserServiceGrpc.UserServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByIdAsync(new GetUserByIdRequest{Id = userId}, metadata));
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_USER).AsException();
        }
    }
    
    /// <summary>
    /// Validate object scope
    /// </summary>
    /// <param name="scope">The scope to validate</param>
    public void ValidateScope(string scope)
    {
        // make sure valid status
        if (PackageScopes.ALL.Contains(scope))
        {
            return;
        }

        throw ErrorDefinition.Validation(PackageErrors.INVALID_PACKAGE_SCOPE).AsException();
    }

    /// <summary>
    /// Validates the listing checksum
    /// </summary>
    /// <param name="checksum">The checksum</param>
    public void ValidateListingChecksum(string checksum)
    {
        if (string.IsNullOrWhiteSpace(checksum))
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_LISTING_CHECKSUM).AsException();
        }
    }
}