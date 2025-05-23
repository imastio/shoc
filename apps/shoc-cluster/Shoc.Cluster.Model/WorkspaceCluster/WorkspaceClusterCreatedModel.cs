namespace Shoc.Cluster.Model.WorkspaceCluster;

/// <summary>
/// The workspace cluster created model
/// </summary>
public class WorkspaceClusterCreatedModel
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
    /// The cluster name
    /// </summary>
    public string Name { get; set; }
}