namespace Shoc.Core.OpenId;

/// <summary>
/// The definitions of known scopes
/// </summary>
public static class KnownScopes
{
    /// <summary>
    /// The openid scope
    /// </summary>
    public const string OPENID = "openid";

    /// <summary>
    /// The email scope
    /// </summary>
    public const string EMAIl = "email";

    /// <summary>
    /// The profile scope
    /// </summary>
    public const string PROFILE = "profile";
        
    /// <summary>
    /// The shoc scope
    /// </summary>
    public const string SHOC = "shoc";

    /// <summary>
    /// The service scope
    /// </summary>
    public const string SVC = "svc";
    
    /// <summary>
    /// The infrastructure read scope
    /// </summary>
    public const string INFRASTRUCTURE_READ = "infrastructure:read";
    
    /// <summary>
    /// The infrastructure write scope
    /// </summary>
    public const string INFRASTRUCTURE_WRITE = "infrastructure:write";
}