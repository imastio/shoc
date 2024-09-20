using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Identity.Grpc.Users;
using Shoc.Secret.Model;
using Shoc.Workspace.Grpc.Workspaces;

namespace Shoc.Secret.Services;

/// <summary>
/// The secret validation service
/// </summary>
public class SecretValidationService
{
    /// <summary>
    /// The name pattern for cluster
    /// </summary>
    protected static readonly Regex NAME_PATTERN = new("^[a-zA-Z_]{1,}[a-zA-Z0-9_]{0,250}$");

    /// <summary>
    /// The maximum description length
    /// </summary>
    protected const int MAX_DESCRIPTION_LENGTH = 256;
    
    /// <summary>
    /// The grpc client provider
    /// </summary>
    private readonly IGrpcClientProvider grpcClientProvider;

    /// <summary>
    /// Create a new service
    /// </summary>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    public SecretValidationService(IGrpcClientProvider grpcClientProvider)
    {
        this.grpcClientProvider = grpcClientProvider;
    }
    
    /// <summary>
    /// Validate the description of the secret
    /// </summary>
    /// <param name="description">The description to validate</param>
    public void ValidateDescription(string description)
    {
        // check if empty or not matching the pattern
        if (description == null || description.Length > MAX_DESCRIPTION_LENGTH)
        {
            throw ErrorDefinition.Validation(SecretErrors.INVALID_DESCRIPTION).AsException();
        }
    }
    
    /// <summary>
    /// Validate the name of the secret
    /// </summary>
    /// <param name="name">The name to validate</param>
    public void ValidateName(string name)
    {
        // check if empty or not matching the pattern
        if (string.IsNullOrWhiteSpace(name) || !NAME_PATTERN.IsMatch(name))
        {
            throw ErrorDefinition.Validation(SecretErrors.INVALID_NAME).AsException();
        }
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
            throw ErrorDefinition.Validation(SecretErrors.INVALID_WORKSPACE).AsException();
        }
        
        // try getting object
        try {
            await this.grpcClientProvider
                .Get<WorkspaceServiceGrpc.WorkspaceServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByIdAsync(new GetWorkspaceByIdRequest{Id = id}, metadata));
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(SecretErrors.INVALID_WORKSPACE).AsException();
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
            throw ErrorDefinition.Validation(SecretErrors.INVALID_USER).AsException();
        }
        
        // try getting object
        try {
            await this.grpcClientProvider
                .Get<UserServiceGrpc.UserServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByIdAsync(new GetUserByIdRequest{Id = id}, metadata));
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(SecretErrors.INVALID_USER).AsException();
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
            throw ErrorDefinition.Validation(SecretErrors.INVALID_USER).AsException();
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
            throw ErrorDefinition.Validation(SecretErrors.INVALID_USER).AsException();
        }
    }
}