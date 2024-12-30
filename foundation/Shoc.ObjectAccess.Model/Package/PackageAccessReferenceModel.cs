namespace Shoc.ObjectAccess.Model.Package;

/// <summary>
/// The package access reference model
/// </summary>
public class PackageAccessReferenceModel
{
    /// <summary>
    /// The package id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The user id
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// The scope of the package
    /// </summary>
    public string Scope { get; set; }
}