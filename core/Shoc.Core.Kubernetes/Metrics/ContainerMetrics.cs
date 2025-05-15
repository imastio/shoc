using System.Collections.Generic;

namespace Shoc.Core.Kubernetes.Metrics;

/// <summary>
/// ContainerMetrics represents the metrics of a specific container within a pod
/// </summary>
public class ContainerMetrics
{
    /// <summary>
    /// Name of the container
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Resource usage metrics for the container
    /// </summary>
    /// <remarks>
    /// The usage data includes:
    /// - cpu: CPU usage (e.g., "5m" for 5 millicores)
    /// - memory: Memory usage (e.g., "128Mi" for 128 mebibytes)
    /// </remarks>
    public Dictionary<string, string> Usage { get; set; }
}