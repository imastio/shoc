namespace Shoc.Job.Model.Job;

/// <summary>
/// The definitions of job annotations for Kubernetes
/// </summary>
public class JobAnnotations
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
}