using System;
using System.Threading.Tasks;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Identity.Grpc.Users;
using Shoc.Package.Model;
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
    /// Validate user to be a member of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    public async Task RequireMembership(string workspaceId, string userId)
    {
        // no user id given
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_USER).AsException();
        }
        
        // try getting object
        try {
            await this.grpcClientProvider
                .Get<WorkspaceMemberServiceGrpc.WorkspaceMemberServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByUserIdAsync(new GetWorkspaceMemberByUserIdRequest
                {
                    WorkspaceId = workspaceId,
                    UserId = userId
                }, metadata));
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(PackageErrors.INVALID_USER).AsException();
        }
    }
}