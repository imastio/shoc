namespace Shoc.Identity.Model;

/// <summary>
/// The identity object types
/// </summary>
public static class IdentityObjects
{
    /// <summary>
    /// The client object
    /// </summary>
    public const string APPLICATION = "app";

    /// <summary>
    /// The application secret object
    /// </summary>
    public const string APPLICATION_SECRET = "app-sec";

    /// <summary>
    /// The application uri object
    /// </summary>
    public const string APPLICATION_URI = "app-uri";
    
    /// <summary>
    /// The application claim object
    /// </summary>
    public const string APPLICATION_CLAIM = "app-clm";

    /// <summary>
    /// The user object
    /// </summary>
    public const string USER = "usr";

    /// <summary>
    /// The user group object
    /// </summary>
    public const string USER_GROUP = "ugp";

    /// <summary>
    /// The privilege object.
    /// </summary>
    public const string PRIVILEGE = "prv";

    /// <summary>
    /// The privilege object.
    /// </summary>
    public const string ROLE = "rl";

    /// <summary>
    /// The sign-in record
    /// </summary>
    public const string HIST = "hst";

    /// <summary>
    /// The confirmation code object
    /// </summary>
    public const string CNF = "cnf";

    /// <summary>
    /// The one-time password object type
    /// </summary>
    public const string OTP = "otp";

    /// <summary>
    /// The access definition object
    /// </summary>
    public const string ACC = "acc";

    /// <summary>
    /// The privilege access definition object.
    /// </summary>
    public const string PACC = "pacc";

    /// <summary>
    /// The user access definition object.
    /// </summary>
    public const string UACC = "uacc";

    /// <summary>
    /// The user group access definition object.
    /// </summary>
    public const string UGACC = "ugacc";

    /// <summary>
    /// The access grant object
    /// </summary>
    public const string ACCESS_GRANT = "ag";

    /// <summary>
    /// The OIDC provider
    /// </summary>
    public const string OIDC_PROVIDER = "oidc";
    
    /// <summary>
    /// The OIDC provider domain
    /// </summary>
    public const string OIDC_PROVIDER_DOMAIN = "oidc-dn";
}
