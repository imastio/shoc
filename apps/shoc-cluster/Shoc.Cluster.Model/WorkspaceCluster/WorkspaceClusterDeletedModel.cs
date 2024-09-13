namespace Shoc.Cluster.Model.WorkspaceCluster;

/// <summary>
/// The workspace cluster deleted model
/// </summary>
public class WorkspaceClusterDeletedModel
{
    /// <summary>
    /// The id of the cluster in the system
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace associated with the cluster
    /// </summary>
    public string WorkspaceId { get; set; }
}