using System;

namespace Shoc.Cluster.Model.Cluster;

/// <summary>
/// The cluster model
/// </summary>
public class ClusterModel
{
    /// <summary>
    /// The id of the cluster in the system
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace associated with the cluster
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The name of the cluster (should be unique within the workspace)
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The description of the cluster
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// The type of the cluster
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// The status of the cluster
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// The configuration including authentication, endpoints, etc.
    /// </summary>
    public string Configuration { get; set; }
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}