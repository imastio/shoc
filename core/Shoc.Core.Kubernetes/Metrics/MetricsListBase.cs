using System.Collections.Generic;

namespace Shoc.Core.Kubernetes.Metrics;

/// <summary>
/// Base class for metrics lists containing common properties
/// </summary>
/// <typeparam name="T">Type of the metrics items in the list</typeparam>
public class MetricsListBase<T>
{
    /// <summary>
    /// Kind is a string value representing the REST resource this object represents.
    /// </summary>
    /// <example>PodMetricsList</example>
    /// <example>NodeMetricsList</example>
    public string Kind { get; set; }

    /// <summary>
    /// APIVersion defines the versioned schema of this representation of an object.
    /// </summary>
    /// <example>metrics.k8s.io/v1beta1</example>
    public string ApiVersion { get; set; }

    /// <summary>
    /// Standard list metadata
    /// </summary>
    public V1ListMeta Metadata { get; set; }

    /// <summary>
    /// List of metrics items
    /// </summary>
    public List<T> Items { get; set; }
}