namespace Shoc.Registry.Model.Registry;

/// <summary>
/// The registry update model
/// </summary>
public class RegistryUpdateModel
{
    /// <summary>
    /// The id of the registry in the system
    /// </summary>
    public string Id { get; set; }
    
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
    /// The Root URI of the container registry (without trailing slash) 
    /// </summary>
    public string Registry { get; set; }
    
    /// <summary>
    /// A namespace in the registry (mandatory expect for Shoc provider)
    /// </summary>
    public string Namespace { get; set; }
}