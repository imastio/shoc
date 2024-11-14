namespace Shoc.Package.Model.Package;

/// <summary>
/// The package filter
/// </summary>
public class PackageFilter
{   
    /// <summary>
    /// The owner of the package
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// The target scope
    /// </summary>
    public string Scope { get; set; }
}