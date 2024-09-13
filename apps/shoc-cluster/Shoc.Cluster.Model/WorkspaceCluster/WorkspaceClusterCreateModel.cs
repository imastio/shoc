namespace Shoc.Cluster.Model.WorkspaceCluster;

/// <summary>
/// The workspace cluster create model
/// </summary>
public class WorkspaceClusterCreateModel
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
    /// The type of the cluster
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// The configuration including authentication, endpoints, etc.
    /// </summary>
    public string Configuration { get; set; }
}