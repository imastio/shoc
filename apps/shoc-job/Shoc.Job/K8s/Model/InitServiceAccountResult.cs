using k8s.Models;

namespace Shoc.Job.K8s.Model;

/// <summary>
/// The Kubernetes init result
/// </summary>
public class InitServiceAccountResult
{
    /// <summary>
    /// The service account object
    /// </summary>
    public V1ServiceAccount ServiceAccount { get; set; }
    
    /// <summary>
    /// The role object
    /// </summary>
    public V1Role Role { get; set; }
    
    /// <summary>
    /// The role binding
    /// </summary>
    public V1RoleBinding RoleBinding { get; set; }
}