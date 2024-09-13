using Shoc.Identity.Model.OpenId;

namespace Shoc.Identity.Model.Application;

/// <summary>
/// The definitions of application defaults
/// </summary>
public class ApplicationDefaults
{
    /// <summary>
    /// The client is enabled if not given
    /// </summary>
    public const string DEFAULT_PROTOCOL_TYPE = "oidc";

    /// <summary>
    /// The secret is required by default
    /// </summary>
    public const bool DEFAULT_SECRET_REQUIRED = true;

    /// <summary>
    /// Consent is not required by default
    /// </summary>
    public const bool DEFAULT_CONSENT_REQUIRED = false;
    
    /// <summary>
    /// Allow remember consent by default
    /// </summary>
    public const bool DEFAULT_ALLOW_REMEMBER_CONSENT = true;
    
    /// <summary>
    /// The default grant types
    /// </summary>
    public const string DEFAULT_ALLOWED_GRANT_TYPES = "authorization_code";
    
    /// <summary>
    /// PKCE is required by default
    /// </summary>
    public const bool DEFAULT_PKCE_REQUIRED = true;
    
    /// <summary>
    /// Indicates if plain text pkce value is allowed
    /// </summary>
    public const bool DEFAULT_ALLOW_PLAIN_TEXT_PKCE = false;

    /// <summary>
    /// Indicates if request object is required or not
    /// </summary>
    public const bool DEFAULT_REQUIRE_REQUEST_OBJECT = false;

    /// <summary>
    /// Indicates if access tokens are allowed via browser
    /// </summary>
    public const bool DEFAULT_ALLOW_ACCESS_TOKENS_VIA_BROWSER = false;

    /// <summary>
    /// The DPoP is not required by default
    /// </summary>
    public const bool DEFAULT_DPOP_REQUIRED = false;
    
    /// <summary>
    /// The default clock skew for DPoP
    /// </summary>
    public const int DEFAULT_DPOP_CLOCK_SKEW_SECONDS = 5 * 60;
    
    /// <summary>
    /// The default DPoP validation mode
    /// </summary>
    public const string DEFAULT_DPOP_VALIDATION_MODE = "";

    /// <summary>
    /// Require front channel logout session
    /// </summary>
    public const bool DEFAULT_FRONT_CHANNEL_LOGOUT_SESSION_REQUIRED = true;

    /// <summary>
    /// Require back channel logout session
    /// </summary>
    public const bool DEFAULT_BACK_CHANNEL_LOGOUT_SESSION_REQUIRED = true;

    /// <summary>
    /// Disallow offline access by default
    /// </summary>
    public const bool DEFAULT_ALLOW_OFFLINE_ACCESS = false;

    /// <summary>
    /// The default allowed scopes
    /// </summary>
    public const string DEFAULT_ALLOWED_SCOPES = "openid";

    /// <summary>
    /// Disable include user claims in id token by default 
    /// </summary>
    public const bool DEFAULT_INCLUDE_USER_CLAIMS_IN_ID_TOKEN = false;

    /// <summary>
    /// The default identity token lifetime
    /// </summary>
    public const int DEFAULT_IDENTITY_TOKEN_LIFETIME = 300;

    /// <summary>
    /// Allowed signing algorithms
    /// </summary>
    public const string DEFAULT_ALLOWED_IDENTITY_TOKEN_SIGNING_ALGORITHMS = "";

    /// <summary>
    /// The default access token lifetime
    /// </summary>
    public const int DEFAULT_ACCESS_TOKEN_LIFETIME = 3600;

    /// <summary>
    /// The default authorization code lifetime
    /// </summary>
    public const int DEFAULT_AUTHORIZATION_CODE_LIFETIME = 300;

    /// <summary>
    /// The default absolute refresh token lifetime
    /// </summary>
    public const int DEFAULT_ABSOLUTE_REFRESH_TOKEN_LIFETIME = 2592000;

    /// <summary>
    /// The default sliding refresh token lifetime
    /// </summary>
    public const int DEFAULT_SLIDING_REFRESH_TOKEN_LIFETIME = 1296000;

    /// <summary>
    /// The default consent lifetime
    /// </summary>
    public const int DEFAULT_CONSENT_LIFETIME = 0;

    /// <summary>
    /// The default usage type of refresh token
    /// </summary>
    public const string DEFAULT_REFRESH_TOKEN_USAGE = TokenUsages.ONE_TIME;

    /// <summary>
    /// Do not update access token claims on refresh by default
    /// </summary>
    public const bool DEFAULT_UPDATE_ACCESS_TOKEN_CLAIMS_ON_REFRESH = false;

    /// <summary>
    /// The default expiration of refresh token
    /// </summary>
    public const string DEFAULT_REFRESH_TOKEN_EXPIRATION = TokenExpirations.ABSOLUTE;

    /// <summary>
    /// The default access token type
    /// </summary>
    public const string DEFAULT_ACCESS_TOKEN_TYPE = AccessTokenTypes.JWT;

    /// <summary>
    /// Indicates if enabling local login by default or not
    /// </summary>
    public const bool DEFAULT_ENABLE_LOCAL_LOGIN = true;

    /// <summary>
    /// Indicates if including jwt id by default
    /// </summary>
    public const bool DEFAULT_INCLUDE_JWT_ID = true;

    /// <summary>
    /// Indicates if always sending client claims by default
    /// </summary>
    public const bool DEFAULT_ALWAYS_SEND_CLIENT_CLAIMS = false;

    /// <summary>
    /// The default sso lifetime
    /// </summary>
    public const int DEFAULT_USER_SSO_LIFETIME = 0;

    /// <summary>
    /// The default ciba lifetime
    /// </summary>
    public const int DEFAULT_CIBA_LIFETIME = 300;

    /// <summary>
    /// The default polling interval
    /// </summary>
    public const int DEFAULT_POLLING_INTERVAL = 5;

    /// <summary>
    /// The default device code lifetime
    /// </summary>
    public const int DEFAULT_DEVICE_CODE_LIFETIME = 300;

    /// <summary>
    /// Coordinate lifetime with user session by default
    /// </summary>
    public const bool DEFAULT_COORDINATE_LIFETIME_WITH_USER_SESSION = false;
}