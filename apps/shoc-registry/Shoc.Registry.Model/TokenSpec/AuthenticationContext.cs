namespace Shoc.Registry.Model.TokenSpec;

/// <summary>
/// The authentication context
/// </summary>
public class AuthenticationContext
{
    /// <summary>
    /// The party issuing the token
    /// </summary>
    public string Issuer { get; set; }
    
    /// <summary>
    /// The target audience
    /// </summary>
    public string Audience { get; set; }
}