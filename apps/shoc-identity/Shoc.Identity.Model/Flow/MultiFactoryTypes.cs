namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The known multi-factor auth types
/// </summary>
public static class MultiFactoryTypes
{
    /// <summary>
    /// No multi-factor stage is used
    /// </summary>
    public const string NONE = "none";

    /// <summary>
    /// The google authenticator
    /// </summary>
    public const string GOOGLE_AUTHENTICATOR = "g_authenticator";

    /// <summary>
    /// The microsoft authenticator
    /// </summary>
    public const string MICROSOFT_AUTHENTICATOR = "m_authenticator";
}