namespace Shoc.Registry.Model.TokenSpec;

/// <summary>
/// The token error codes
/// </summary>
public static class TokenErrorCodes
{
    /// <summary>
    /// The request is missing a required parameter, includes an unsupported parameter or parameter value, repeats the same parameter, or is otherwise malformed.
    /// </summary>
    public const string INVALID_REQUEST = "invalid_request";

    /// <summary>
    /// Client authentication failed (e.g., unknown client, no client authentication included, or unsupported authentication method).
    /// </summary>
    public const string INVALID_CLIENT = "invalid_client";
    
    /// <summary>
    /// The provided authorization grant (e.g., authorization code, resource owner credentials) or refresh token is invalid, expired, revoked, or does not match the redirection URI used in the authorization request.
    /// </summary>
    public const string INVALID_GRANT = "invalid_grant";

    /// <summary>
    /// The authenticated client is not authorized to use this authorization grant type.
    /// </summary>
    public const string UNAUTHORIZED_CLIENT = "unauthorized_client";

    /// <summary>
    /// The authorization grant type is not supported by the authorization server.
    /// </summary>
    public const string UNSUPPORTED_GRANT_TYPE = "unsupported_grant_type";

    /// <summary>
    /// The requested scope is invalid, unknown, or malformed.
    /// </summary>
    public const string INVALID_SCOPE = "invalid_scope";

    /// <summary>
    /// The authorization server encountered an unexpected condition that prevented it from fulfilling the request.
    /// </summary>
    public const string SERVER_ERROR = "server_error";

    /// <summary>
    /// The authorization server is currently unable to handle the request due to a temporary overloading or maintenance of the server.
    /// </summary>
    public const string TEMPORARILY_UNAVAILABLE = "temporarily_unavailable";
}