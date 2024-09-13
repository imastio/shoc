using System;

namespace Shoc.Registry.Model.Registry;

/// <summary>
/// The registry extended model
/// </summary>
public class RegistryExtendedModel
{
    /// <summary>
    /// The id of the registry in the system
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace associated with the registry if any (optional)
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The workspace name if available
    /// </summary>
    public string WorkspaceName { get; set; }
    
    /// <summary>
    /// The name of the registry (should be unique within the workspace)
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// A friendly name for the registry to display
    /// </summary>
    public string DisplayName { get; set; }
    
    /// <summary>
    /// The status of the registry
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// The provider of registry
    /// </summary>
    public string Provider { get; set; }
    
    /// <summary>
    /// The usage scope
    /// </summary>
    public string UsageScope { get; set; }
    
    /// <summary>
    /// The Root URI of the container registry (without trailing slash) 
    /// </summary>
    public string Registry { get; set; }
    
    /// <summary>
    /// A namespace in the registry (mandatory expect for Shoc provider)
    /// </summary>
    public string Namespace { get; set; }
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}