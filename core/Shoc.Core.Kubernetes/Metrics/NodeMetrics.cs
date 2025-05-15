using System;
using System.Collections.Generic;

namespace Shoc.Core.Kubernetes.Metrics;

/// <summary>
/// NodeMetrics represents the metrics of a specific node
/// </summary>
public class NodeMetrics
{
    /// <summary>
    /// Standard object metadata
    /// </summary>
    public V1ObjectMeta Metadata { get; set; }

    /// <summary>
    /// The timestamp for when the metrics were collected
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// The time window for which the metrics were collected
    /// </summary>
    /// <example>30s</example>
    public string Window { get; set; }

    /// <summary>
    /// Resource usage metrics for the node
    /// </summary>
    /// <remarks>
    /// The usage data includes:
    /// - cpu: CPU usage (e.g., "250m" for 250 millicores)
    /// - memory: Memory usage (e.g., "4Gi" for 4 gibibytes)
    /// </remarks>
    public Dictionary<string, string> Usage { get; set; }
}