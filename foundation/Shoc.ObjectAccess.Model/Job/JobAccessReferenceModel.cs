namespace Shoc.ObjectAccess.Model.Job;

/// <summary>
/// The package access reference model
/// </summary>
public class JobAccessReferenceModel
{
    /// <summary>
    /// The object id
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
    /// The scope of the job
    /// </summary>
    public string Scope { get; set; }
}