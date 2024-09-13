namespace Shoc.Registry.Model.Registry;

/// <summary>
/// The registry referential model
/// </summary>
public class RegistryReferentialValueModel
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
    /// The usage scope
    /// </summary>
    public string UsageScope { get; set; }
}