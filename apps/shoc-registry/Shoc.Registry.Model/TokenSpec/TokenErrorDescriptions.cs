using System.Collections.Generic;

namespace Shoc.Registry.Model.TokenSpec;

/// <summary>
/// The token error descriptions
/// </summary>
public static class TokenErrorDescriptions
{
    /// <summary>
    /// The set of descriptions
    /// </summary>
    public static readonly IDictionary<string, string> DESCRIPTIONS = new Dictionary<string, string>
    {
        {
            TokenErrorCodes.INVALID_REQUEST,
            "The request is missing a required parameter, includes an unsupported parameter or parameter value, repeats the same parameter, or is otherwise malformed."
        },
        {
            TokenErrorCodes.INVALID_CLIENT,
            "Client authentication failed (e.g., unknown client, no client authentication included, or unsupported authentication method)."
        },
        {
            TokenErrorCodes.INVALID_GRANT,
            "The provided authorization grant (e.g., authorization code, resource owner credentials) or refresh token is invalid, expired, revoked, or does not match the redirection URI used in the authorization request."
        },
        {
            TokenErrorCodes.UNAUTHORIZED_CLIENT,
            "The authenticated client is not authorized to use this authorization grant type."
        },
        {
            TokenErrorCodes.UNSUPPORTED_GRANT_TYPE,
            "The authorization grant type is not supported by the authorization server."
        },
        {
            TokenErrorCodes.INVALID_SCOPE,
            "The requested scope is invalid, unknown, or malformed."
        },
        {
            TokenErrorCodes.SERVER_ERROR,
            "The authorization server encountered an unexpected condition that prevented it from fulfilling the request."
        },
        {
            TokenErrorCodes.TEMPORARILY_UNAVAILABLE,
            "The authorization server is currently unable to handle the request due to a temporary overloading or maintenance of the server."
        }
    };
}