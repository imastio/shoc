namespace Shoc.Job.K8s;

/// <summary>
/// The Kubernetes constants
/// </summary>
public static class K8sConstants
{
    /// <summary>
    /// The service account name used inside the job namespace
    /// </summary>
    public const string SHOC_SERVICE_ACCOUNT_NAME = "shoc-sa";
    
    /// <summary>
    /// The role name for the shoc service accounts
    /// </summary>
    public const string SHOC_ROLE_NAME = "shoc-role";
    
    /// <summary>
    /// The role binding name for shoc roles
    /// </summary>
    public const string SHOC_ROLE_BINDING_NAME = "shoc-role-binding";
}