using k8s.Models;

namespace Shoc.Job.K8s.Model;

/// <summary>
/// The Kubernetes pull secret init result
/// </summary>
public class InitPullSecretResult
{
    /// <summary>
    /// The pull secret
    /// </summary>
    public V1Secret PullSecret { get; set; }
    
    /// <summary>
    /// The reference to the target image
    /// </summary>
    public string Image { get; set; }
}