using System;
using System.Threading.Tasks;
using Shoc.ApiCore.GrpcClient;
using Shoc.Cluster.Grpc.Clusters;
using Shoc.Core;
using Shoc.Job.Model;

namespace Shoc.Job.Services;

/// <summary>
/// The cluster resolver
/// </summary>
public class JobClusterResolver
{
    /// <summary>
    /// The grpc client provider
    /// </summary>
    private readonly IGrpcClientProvider grpcClientProvider;
    
    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    public JobClusterResolver(IGrpcClientProvider grpcClientProvider)
    {
        this.grpcClientProvider = grpcClientProvider;
    }
    
    /// <summary>
    /// Gets the cluster object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <returns></returns>
    public async Task<ClusterGrpcModel> ResolveById(string workspaceId, string id)
    {
        // no id provided
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_CLUSTER, "No cluster provided").AsException();
        }

        // the resulting cluster
        ClusterGrpcModel result;
        
        // try getting object
        try {
            result = (await this.grpcClientProvider
                .Get<ClusterServiceGrpc.ClusterServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByIdAsync(new GetClusterByIdRequest
                {
                    WorkspaceId = workspaceId,
                    Id = id,
                    Plain = true
                }, metadata))).Cluster;
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_CLUSTER, "Could not find the referenced cluster").AsException();
        }

        // cluster should be active
        if (result.Status != ClusterStatus.Active)
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_CLUSTER, "The cluster is not active").AsException();
        }

        // check if configuration is empty
        if (string.IsNullOrWhiteSpace(result.Configuration))
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_CLUSTER, "The cluster has no valid configuration").AsException();
        }
        
        return result;
    }
}