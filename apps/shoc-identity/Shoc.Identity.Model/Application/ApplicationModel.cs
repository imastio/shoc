using System;

namespace Shoc.Identity.Model.Application;

/// <summary>
/// The identity application client model 
/// </summary>
public class ApplicationModel
{
    /// <summary>
    /// The client identifier
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Specifies if client is enabled
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// The application client id for the client
    /// </summary>
    public string ApplicationClientId { get; set; }
    
    /// <summary>
    /// The protocol type
    /// </summary>
    public string ProtocolType { get; set; }
    
    /// <summary>
    /// Client display name (used for logging and consent screen)
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the client.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// If set to false, no client secret is needed to request tokens at the token endpoint (defaults to <c>true</c>)
    /// </summary>
    public bool SecretRequired { get; set; }

    /// <summary>
    /// URI to further information about client (used on consent screen)
    /// </summary>
    public string ApplicationUri { get; set; }

    /// <summary>
    /// URI to client logo (used on consent screen)
    /// </summary>
    public string LogoUri { get; set; }

    /// <summary>
    /// Specifies whether a consent screen is required (defaults to <c>false</c>)
    /// </summary>
    public bool ConsentRequired { get; set; }

    /// <summary>
    /// Specifies whether user can choose to store consent decisions (defaults to <c>true</c>)
    /// </summary>
    public bool AllowRememberConsent { get; set; }
    
    /// <summary>
    /// The space delimited list of allowed grant types
    /// </summary>
    public string AllowedGrantTypes { get; set; }
    
    /// <summary>
    /// Specifies whether a proof key is required for authorization code based token requests (defaults to <c>true</c>).
    /// </summary>
    public bool PkceRequired { get; set; }
    
    /// <summary>
    /// Specifies whether a proof key can be sent using plain method (not recommended and defaults to <c>false</c>.)
    /// </summary>
    public bool AllowPlainTextPkce { get; set; }

    /// <summary>
    /// Specifies whether the client must use a request object on authorize requests (defaults to <c>false</c>.)
    /// </summary>
    public bool RequireRequestObject { get; set; }
    
    /// <summary>
    /// Controls whether access tokens are transmitted via the browser for this client (defaults to <c>false</c>).
    /// This can prevent accidental leakage of access tokens when multiple response types are allowed.
    /// </summary>
    /// <value>
    /// <c>true</c> if access tokens can be transmitted via the browser; otherwise, <c>false</c>.
    /// </value>
    public bool AllowAccessTokensViaBrowser { get; set; }
    
    /// <summary>
    /// The DPoP required
    /// </summary>
    public bool DpopRequired { get; set; }
    
    /// <summary>
    /// The DPoP validation mode
    /// </summary>
    public string DpopValidationMode { get; set; }
    
    /// <summary>
    /// The DPoP clock skew in seconds
    /// </summary>
    public int DpopClockSkewSeconds { get; set; }
    
    /// <summary>
    /// Specifies logout URI at client for HTTP front-channel based logout.
    /// </summary>
    public string FrontChannelLogoutUri { get; set; }

    /// <summary>
    /// Specifies if the user's session id should be sent to the FrontChannelLogoutUri. Defaults to <c>true</c>.
    /// </summary>
    public bool FrontChannelLogoutSessionRequired { get; set; }

    /// <summary>
    /// Specifies logout URI at client for HTTP back-channel based logout.
    /// </summary>
    public string BackChannelLogoutUri { get; set; }

    /// <summary>
    /// Specifies if the user's session id should be sent to the BackChannelLogoutUri. Defaults to <c>true</c>.
    /// </summary>
    public bool BackChannelLogoutSessionRequired { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [allow offline access]. Defaults to <c>false</c>.
    /// </summary>
    public bool AllowOfflineAccess { get; set; }

    /// <summary>
    /// Specifies the api scopes that the client is allowed to request. If empty, the client can't access any scope
    /// </summary>
    public string AllowedScopes { get; set; }
    
    /// <summary>
    /// When requesting both an id token and access token, should the user claims always be added to the id token instead of requiring the client to use the userinfo endpoint.
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

    /// <summary>
    /// Lifetime of identity token in seconds (defaults to 300 seconds / 5 minutes)
    /// </summary>
    public int IdentityTokenLifetime { get; set; }

    /// <summary>
    /// Signing algorithm for identity token. If empty, will use the server default signing algorithm.
    /// </summary>
    public string AllowedIdentityTokenSigningAlgorithms { get; set; }

    /// <summary>
    /// Lifetime of access token in seconds (defaults to 3600 seconds / 1 hour)
    /// </summary>
    public int AccessTokenLifetime { get; set; }

    /// <summary>
    /// Lifetime of authorization code in seconds (defaults to 300 seconds / 5 minutes)
    /// </summary>
    public int AuthorizationCodeLifetime { get; set; }

    /// <summary>
    /// Maximum lifetime of a refresh token in seconds. Defaults to 2592000 seconds / 30 days
    /// </summary>
    public int AbsoluteRefreshTokenLifetime { get; set; }

    /// <summary>
    /// Sliding lifetime of a refresh token in seconds. Defaults to 1296000 seconds / 15 days
    /// </summary>
    public int SlidingRefreshTokenLifetime { get; set; }

    /// <summary>
    /// Lifetime of a user consent in seconds. Defaults to null (no expiration)
    /// </summary>
    public int ConsentLifetime { get; set; }

    /// <summary>
    /// ReUse: the refresh token handle will stay the same when refreshing tokens
    /// OneTime: the refresh token handle will be updated when refreshing tokens
    /// </summary>
    public string RefreshTokenUsage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the access token (and its claims) should be updated on a refresh token request.
    /// Defaults to <c>false</c>.
    /// </summary>
    /// <value>
    /// <c>true</c> if the token should be updated; otherwise, <c>false</c>.
    /// </value>
    public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

    /// <summary>
    /// Absolute: the refresh token will expire on a fixed point in time (specified by the AbsoluteRefreshTokenLifetime)
    /// Sliding: when refreshing the token, the lifetime of the refresh token will be renewed (by the amount specified in SlidingRefreshTokenLifetime). The lifetime will not exceed AbsoluteRefreshTokenLifetime.
    /// </summary>        
    public string RefreshTokenExpiration { get; set; }

    /// <summary>
    /// Specifies whether the access token is a reference token or a self contained JWT token (defaults to Jwt).
    /// </summary>
    public string AccessTokenType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the local login is allowed for this client. Defaults to <c>true</c>.
    /// </summary>
    /// <value>
    ///   <c>true</c> if local logins are enabled; otherwise, <c>false</c>.
    /// </value>
    public bool EnableLocalLogin { get; set; }

    /// <summary>
    /// Specifies which external IdPs can be used with this client (if list is empty all IdPs are allowed). Defaults to empty.
    /// </summary>
    public string IdentityProviderRestrictions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether JWT access tokens should include an identifier. Defaults to <c>true</c>.
    /// </summary>
    /// <value>
    /// <c>true</c> to add an id; otherwise, <c>false</c>.
    /// </value>
    public bool IncludeJwtId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether client claims should be always included in the access tokens - or only for client credentials flow.
    /// Defaults to <c>false</c>
    /// </summary>
    /// <value>
    /// <c>true</c> if claims should always be sent; otherwise, <c>false</c>.
    /// </value>
    public bool AlwaysSendClientClaims { get; set; }

    /// <summary>
    /// Gets or sets a value to prefix it on client claim types.
    /// </summary>
    /// <value>
    /// Any non empty string if claims should be prefixed with the value; otherwise, <c>null</c>.
    /// </value>
    public string ClientClaimsPrefix { get; set; }

    /// <summary>
    /// Gets or sets a salt value used in pair-wise subjectId generation for users of this client.
    /// </summary>
    public string PairWiseSubjectSalt { get; set; }

    /// <summary>
    /// The maximum duration (in seconds) since the last time the user authenticated.
    /// </summary>
    public int UserSsoLifetime { get; set; }

    /// <summary>
    /// Gets or sets the type of the device flow user code.
    /// </summary>
    /// <value>
    /// The type of the device flow user code.
    /// </value>
    public string UserCodeType { get; set; }

    /// <summary>
    /// Gets or sets the device code lifetime.
    /// </summary>
    /// <value>
    /// The device code lifetime.
    /// </value>
    public int DeviceCodeLifetime { get; set; }

    /// <summary>
    /// Gets or sets the backchannel authentication request lifetime in seconds.
    /// </summary>
    public int CibaLifetime { get; set; }

    /// <summary>
    /// Gets or sets the backchannel polling interval in seconds.
    /// </summary>
    public int PollingInterval { get; set; }
    
    /// <summary>
    /// When enabled, the client's token lifetimes (e.g. refresh tokens) will be tied to the user's session lifetime.
    /// This means when the user logs out, any revokable tokens will be removed.
    /// If using server-side sessions, expired sessions will also remove any revokable tokens, and backchannel logout will be triggered.
    /// This client's setting overrides the global CoordinateTokensWithUserSession configuration setting.
    /// </summary>
    public bool CoordinateLifetimeWithUserSession { get; set; }
    
    /// <summary>
    /// The initial login uri
    /// </summary>
    public string InitiateLoginUri { get; set; }
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}