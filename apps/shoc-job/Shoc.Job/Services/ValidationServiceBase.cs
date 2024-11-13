using System;
using System.Threading.Tasks;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Identity.Grpc.Users;
using Shoc.Job.Model;
using Shoc.Workspace.Grpc.Workspaces;

namespace Shoc.Job.Services;

/// <summary>
/// The secret validation service
/// </summary>
public class ValidationServiceBase
{
    /// <summary>
    /// The grpc client provider
    /// </summary>
    protected readonly IGrpcClientProvider grpcClientProvider;

    /// <summary>
    /// Create a new service
    /// </summary>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    protected ValidationServiceBase(IGrpcClientProvider grpcClientProvider)
    {
        this.grpcClientProvider = grpcClientProvider;
    }
    
    /// <summary>
    /// Validate workspace by id
    /// </summary>
    /// <param name="id">The id to validate</param>
    public async Task RequireWorkspace(string id)
    {
        // no workspace id given
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_WORKSPACE).AsException();
        }
        
        // try getting object
        try {
            await this.grpcClientProvider
                .Get<WorkspaceServiceGrpc.WorkspaceServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByIdAsync(new GetWorkspaceByIdRequest{Id = id}, metadata));
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_WORKSPACE).AsException();
        }
    }
    
    /// <summary>
    /// Validate user by id
    /// </summary>
    /// <param name="id">The id to validate</param>
    public async Task RequireUser(string id)
    {
        // no user id given
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_USER).AsException();
        }
        
        // try getting object
        try {
            await this.grpcClientProvider
                .Get<UserServiceGrpc.UserServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByIdAsync(new GetUserByIdRequest{Id = id}, metadata));
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_USER).AsException();
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
            throw ErrorDefinition.Validation(JobErrors.INVALID_USER).AsException();
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
            throw ErrorDefinition.Validation(JobErrors.INVALID_USER).AsException();
        }
    }
}