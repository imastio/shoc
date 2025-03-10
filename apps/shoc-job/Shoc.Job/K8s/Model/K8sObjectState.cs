namespace Shoc.Job.K8s.Model;

/// <summary>
/// The state of Kubernetes object in the cluster
/// </summary>
public class K8sObjectState
{
    /// <summary>
    /// The object is not found
    /// </summary>
    public const string NOT_FOUND = "not_found";
    
    /// <summary>
    /// The duplicate of the object was detected
    /// </summary>
    public const string DUPLICATE_OBJECT = "duplicate_object";

    /// <summary>
    /// The object is valid in the cluster
    /// </summary>
    public const string OK = "ok";
}