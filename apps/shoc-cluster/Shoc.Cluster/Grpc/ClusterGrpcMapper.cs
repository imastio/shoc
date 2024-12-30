using Shoc.Cluster.Grpc.Clusters;
using Shoc.Cluster.Model;
using Shoc.Cluster.Model.Cluster;
using Shoc.Core;

namespace Shoc.Cluster.Grpc;

/// <summary>
/// The cluster grpc mapper
/// </summary>
public class ClusterGrpcMapper
{
    /// <summary>
    /// Maps to the Grpc representation
    /// </summary>
    /// <param name="input">The input to map</param>
    /// <returns></returns>
    public ClusterGrpcModel Map(ClusterModel input)
    {
        return new ClusterGrpcModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId,
            Name = input.Name,
            Description = input.Description,
            Type = input.Type,
            Status = MapStatus(input.Status),
            Configuration = input.Configuration
        };
    }

    /// <summary>
    /// Maps the status of the cluster 
    /// </summary>
    /// <param name="status">The status to map</param>
    /// <returns></returns>
    public static ClusterStatus MapStatus(string status)
    {
        return status switch
        {
            ClusterStatuses.ACTIVE => ClusterStatus.Active,
            ClusterStatuses.ARCHIVED => ClusterStatus.Archived,
            _ => throw ErrorDefinition.Validation(ClusterErrors.INVALID_STATUS, "Cluster status is not valid").AsException()
        };
    }
}