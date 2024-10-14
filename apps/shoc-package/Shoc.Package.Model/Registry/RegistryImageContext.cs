namespace Shoc.Package.Model.Registry;

/// <summary>
/// The registry image context
/// </summary>
public class RegistryImageContext
{
    /// <summary>
    /// The target workspace id
    /// </summary>
    public string TargetWorkspaceId { get; set; }
    
    /// <summary>
    /// The target user id
    /// </summary>
    public string TargetUserId { get; set; }
    
    /// <summary>
    /// The registry provider
    /// </summary>
    public string Provider { get; set; }
    
    /// <summary>
    /// The Root URI of the container registry (without trailing slash) 
    /// </summary>
    public string Registry { get; set; }
    
    /// <summary>
    /// A namespace in the registry (mandatory expect for Shoc provider)
    /// </summary>
    public string Namespace { get; set; }
    
    /// <summary>
    /// The target package scope
    /// </summary>
    public string TargetPackageScope { get; set; }
    
    /// <summary>
    /// The target package id
    /// </summary>
    public string TargetPackageId { get; set; }
}