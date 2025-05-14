using System;

namespace Shoc.Cluster.Model.Cluster;

/// <summary>
/// The cluster connectivity info
/// </summary>
public class ClusterConnectivityModel
{
    /// <summary>
    /// The cluster id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// Indicates if successfully established the connection
    /// </summary>
    public bool Connected { get; set; }
    
    /// <summary>
    /// The message of connectivity
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}