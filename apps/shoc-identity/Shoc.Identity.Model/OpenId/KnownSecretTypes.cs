namespace Shoc.Identity.Model;

/// <summary>
/// The definitions of known secret types
/// </summary>
public class KnownSecretTypes
{
    /// <summary>
    /// The shared secret
    /// </summary>
    public const string SHARED_SECRET = "shared_secret";

    /// <summary>
    /// The certificate thumbprint
    /// </summary>
    public const string CERT_THUMBPRINT = "cert_thumbprint";

    /// <summary>
    /// The certificate name
    /// </summary>
    public const string CERT_NAME = "cert_name";

    /// <summary>
    /// The base64 encoded certificate
    /// </summary>
    public const string CERT_BASE64 = "cert_base64";

    /// <summary>
    /// The JSON Web Key 
    /// </summary>
    public const string JWK = "jwk";
}