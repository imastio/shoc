namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The sign-out flow result
/// </summary>
public class SignOutFlowResult
{
    /// <summary>
    /// The sign-out iframe url
    /// </summary>
    public string SignOutIframeUrl { get; set; }

    /// <summary>
    /// The Post-Logout redirect URI
    /// </summary>
    public string PostLogoutRedirectUri { get; set; }

    /// <summary>
    /// Indicates if currently in sing-out flow
    /// </summary>
    public bool ContinueFlow { get; set; }
}