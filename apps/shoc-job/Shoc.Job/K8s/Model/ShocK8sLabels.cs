namespace Shoc.Job.K8s.Model;

/// <summary>
/// The definitions of job annotations for Kubernetes
/// </summary>
public class ShocK8sLabels
{
    /// <summary>
    /// The base namespace for kubernetes annotations
    /// </summary>
    public const string BASE_NS = "k8s.shoc.dev";

    /// <summary>
    /// The shoc workspace label
    /// </summary>
    public const string SHOC_WORKSPACE = $"{BASE_NS}/workspace";
    
    /// <summary>
    /// The shoc job label
    /// </summary>
    public const string SHOC_JOB = $"{BASE_NS}/job";
    
    /// <summary>
    /// The shoc task label
    /// </summary>
    public const string SHOC_JOB_TASK = $"{BASE_NS}/task";
    
    /// <summary>
    /// The shoc pod role label
    /// </summary>
    public const string SHOC_POD_ROLE = $"{BASE_NS}/pod-role";
}