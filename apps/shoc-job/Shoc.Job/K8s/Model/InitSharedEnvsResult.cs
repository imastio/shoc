using System.Collections.Generic;
using k8s.Models;

namespace Shoc.Job.K8s.Model;

/// <summary>
/// The Kubernetes secrets and config maps init result
/// </summary>
public class InitSharedEnvsResult
{
    /// <summary>
    /// The secrets
    /// </summary>
    public List<V1Secret> Secrets { get; set; }
    
    /// <summary>
    /// The config maps
    /// </summary>
    public List<V1ConfigMap> ConfigMaps { get; set; }
}