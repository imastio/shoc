namespace Shoc.Core.Kubernetes.Metrics;

/// <summary>
/// Metadata for list operations
/// </summary>
public class V1ListMeta
{
    /// <summary>
    /// SelfLink is a URL representing this object.
    /// </summary>
    public string SelfLink { get; set; }
}