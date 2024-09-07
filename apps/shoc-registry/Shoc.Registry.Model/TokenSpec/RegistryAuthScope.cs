namespace Shoc.Registry.Model.TokenSpec;

/// <summary>
/// The parsed representation of registry auth scope
/// </summary>
public class RegistryAuthScope
{
    /// <summary>
    /// The type of the resources (repository)
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// The name of the repository
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The supported actions on the resource
    /// </summary>
    public string[] Actions { get; set; }
}