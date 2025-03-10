namespace Shoc.Core.Kubernetes;

/// <summary>
/// The well-known labels
/// </summary>
public static class WellKnownLabels
{
    /// <summary>
    /// The name of the application
    /// </summary>
    public const string NAME = "app.kubernetes.io/name";
    
    /// <summary>
    /// A unique name identifying the instance of an application
    /// </summary>
    public const string INSTANCE = "app.kubernetes.io/instance";
    
    /// <summary>
    /// The current version of the application (e.g., a SemVer 1.0, revision hash, etc.)
    /// </summary>
    public const string VERSION = "app.kubernetes.io/version";
    
    /// <summary>
    /// The component within the architecture
    /// </summary>
    public const string COMPONENT = "app.kubernetes.io/component";
    
    /// <summary>
    /// The name of a higher level application this one is part of
    /// </summary>
    public const string PART_OF = "app.kubernetes.io/part-of";
    
    /// <summary>
    /// The tool being used to manage the operation of an application
    /// </summary>
    public const string MANAGED_BY = "app.kubernetes.io/managed-by";
}