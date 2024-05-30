using System.Collections.Generic;

namespace Shoc.Identity.Model.Application;

/// <summary>
/// The application client model
/// </summary>
public class ApplicationClientModel
{
    /// <summary>
    /// The application model
    /// </summary>
    public ApplicationModel Application { get; set; }
    
    /// <summary>
    /// The application secrets model collection
    /// </summary>
    public IEnumerable<ApplicationSecretModel> Secrets { get; set; }
    
    /// <summary>
    /// The application uris model collection
    /// </summary>
    public IEnumerable<ApplicationUriModel> Uris { get; set; }
    
    /// <summary>
    /// The application claim model
    /// </summary>
    public IEnumerable<ApplicationClaimModel> Claims { get; set; }
}