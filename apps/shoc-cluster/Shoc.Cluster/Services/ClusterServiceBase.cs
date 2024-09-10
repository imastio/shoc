using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Shoc.ApiCore.GrpcClient;
using Shoc.Cluster.Data;
using Shoc.Cluster.Model;
using Shoc.Cluster.Model.Cluster;
using Shoc.Core;
using Shoc.Identity.Grpc.Users;
using Shoc.Workspace.Grpc.Workspaces;

namespace Shoc.Cluster.Services;

/// <summary>
/// The base service for cluster
/// </summary>
public abstract class ClusterServiceBase
{
    /// <summary>
    /// The name pattern for cluster
    /// </summary>
    protected static readonly Regex NAME_PATTERN = new(@"^[a-z\d](?:[a-z\d]|-(?=[a-z\d])){0,38}$");
    
    /// <summary>
    /// The object repository
    /// </summary>
    protected readonly IClusterRepository clusterRepository;

    /// <summary>
    /// The configuration protection provider
    /// </summary>
    protected readonly ConfigurationProtectionProvider configurationProtectionProvider;

    /// <summary>
    /// The grpc client provider
    /// </summary>
    protected readonly IGrpcClientProvider grpcClientProvider;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="clusterRepository">The cluster repository</param>
    /// <param name="configurationProtectionProvider">The configuration protection provider</param>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    protected ClusterServiceBase(IClusterRepository clusterRepository, ConfigurationProtectionProvider configurationProtectionProvider, IGrpcClientProvider grpcClientProvider)
    {
        this.clusterRepository = clusterRepository;
        this.configurationProtectionProvider = configurationProtectionProvider;
        this.grpcClientProvider = grpcClientProvider;
    }

    /// <summary>
    /// Requires the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    protected async Task<ClusterModel> RequireClusterById(string workspaceId, string id)
    {
        // id should be a valid string
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try getting object by id
        var result = await this.clusterRepository.GetById(workspaceId, id);

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
        // no workspace id given
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.Validation(ClusterErrors.INVALID_WORKSPACE).AsException();
        }
        
        // try getting object
        try {
            await this.grpcClientProvider
                .Get<WorkspaceServiceGrpc.WorkspaceServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByIdAsync(new GetWorkspaceByIdRequest{Id = id}, metadata));
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(ClusterErrors.INVALID_WORKSPACE).AsException();
        }
    }
    
    /// <summary>
    /// Validate user by id
    /// </summary>
    /// <param name="id">The id to validate</param>
    protected async Task RequireUser(string id)
    {
        // no user id given
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.Validation(ClusterErrors.INVALID_USER).AsException();
        }
        
        // try getting object
        try {
            await this.grpcClientProvider
                .Get<UserServiceGrpc.UserServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByIdAsync(new GetUserByIdRequest{Id = id}, metadata));
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(ClusterErrors.INVALID_USER).AsException();
        }
    }
    /// <summary>
    /// Validate the cluster name
    /// </summary>
    /// <param name="name">The name of the cluster</param>
    /// <exception cref="ShocException"></exception>
    protected static void ValidateName(string name)
    {
        // check if empty or not matching the pattern
        if (string.IsNullOrWhiteSpace(name) || !NAME_PATTERN.IsMatch(name))
        {
            throw ErrorDefinition.Validation(ClusterErrors.INVALID_NAME).AsException();
        }
    }
    
    /// <summary>
    /// Validate object provider
    /// </summary>
    /// <param name="type">The type to validate</param>
    protected static void ValidateType(string type)
    {
        // make sure input is valid 
        if (ClusterTypes.ALL.Contains(type))
        {
            return;
        }

        throw ErrorDefinition.Validation(ClusterErrors.INVALID_TYPE).AsException();
    }
    
    /// <summary>
    /// Validate object status
    /// </summary>
    /// <param name="status">The status to validate</param>
    protected static void ValidateStatus(string status)
    {
        // make sure input is valid 
        if (ClusterStatuses.ALL.Contains(status))
        {
            return;
        }

        throw ErrorDefinition.Validation(ClusterErrors.INVALID_STATUS).AsException();
    }
}