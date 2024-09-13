using System;

namespace Shoc.Cluster.Model.WorkspaceCluster;

/// <summary>
/// The workspace cluster model
/// </summary>
public class WorkspaceClusterModel
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
    /// The name of the workspace
    /// </summary>
    public string WorkspaceName { get; set; }
    
    /// <summary>
    /// The name of the cluster (should be unique within the workspace)
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The type of the cluster
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// The status of the cluster
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
    
}