using k8s.Models;

namespace Shoc.Job.K8s.Model;

/// <summary>
/// The Kubernetes namespace init result
/// </summary>
public class InitNamespaceResult
{
    /// <summary>
    /// The namespace object
    /// </summary>
    public V1Namespace Namespace { get; set; }
}