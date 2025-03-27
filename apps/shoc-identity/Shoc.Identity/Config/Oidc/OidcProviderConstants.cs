namespace Shoc.Identity.Config.Oidc;

/// <summary>
/// The oidc provider constants
/// </summary>
public static class OidcProviderConstants
{
    /// <summary>
    /// The dynamic oidc scheme
    /// </summary>
    public const string DYNAMIC_OIDC_SCHEME = "DynamicOidc";

    /// <summary>
    /// The provider code key
    /// </summary>
    public const string PROVIDER_CODE_KEY = "providerCode";

    /// <summary>
    /// The oidc callback path
    /// </summary>
    public const string CALLBACK_PATH = "/api-auth/signin-oidc";
}