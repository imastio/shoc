using System;

namespace Shoc.Core.Kubernetes.Metrics;

/// <summary>
/// Standard object metadata
/// </summary>
public class V1ObjectMeta
{
    /// <summary>
    /// Name of the object
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Namespace of the object (for namespaced objects)
    /// </summary>
    public string Namespace { get; set; }

    /// <summary>
    /// SelfLink is a URL representing this object.
    /// </summary>
    public string SelfLink { get; set; }

    /// <summary>
    /// CreationTimestamp is a timestamp representing the server time when this object was created.
    /// </summary>
    public DateTime CreationTimestamp { get; set; }
}