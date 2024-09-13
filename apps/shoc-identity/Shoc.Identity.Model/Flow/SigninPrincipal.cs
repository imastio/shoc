namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The sign-in attempt 
/// </summary>
public class SigninPrincipal
{
    /// <summary>
    /// The subject of sign-in
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// The display name of subject
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// The email of attempt subject
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The identity provider
    /// </summary>
    public string Provider { get; set; }
        
    /// <summary>
    /// The method type
    /// </summary>
    public string MethodType { get; set; }
        
    /// <summary>
    /// The multi-factory type used
    /// </summary>
    public string MultiFactorType { get; set; }
}