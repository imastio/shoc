namespace Shoc.Package.Model.Package;

/// <summary>
/// The package 'duplicate-from' model
/// </summary>
public class PackageDuplicateFromModel
{
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The user id
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// The registry id
    /// </summary>
    public string RegistryId { get; set; }
    
    /// <summary>
    /// The listing checksum
    /// </summary>
    public string ListingChecksum { get; set; }
    
    /// <summary>
    /// The package scope
    /// </summary>
    public string Scope { get; set; }
}