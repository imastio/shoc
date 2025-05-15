using System;
using System.Collections.Generic;

namespace Shoc.Core.Kubernetes.Metrics;

/// <summary>
/// PodMetrics represents the metrics of a specific pod
/// </summary>
public class PodMetrics
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
    /// List of container metrics within this pod
    /// </summary>
    public List<ContainerMetrics> Containers { get; set; }
}