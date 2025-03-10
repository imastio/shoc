namespace Shoc.Job.K8s.Model;

/// <summary>
/// The Kubernetes pod roles of Shoc
/// </summary>
public static class ShocK8sPodRoles
{
    /// <summary>
    /// The executor role
    /// </summary>
    public const string EXECUTOR = "executor";

    /// <summary>
    /// The worker role
    /// </summary>
    public const string WORKER = "worker";
}