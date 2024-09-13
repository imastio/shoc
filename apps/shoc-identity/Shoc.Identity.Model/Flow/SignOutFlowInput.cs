namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The sign-out flow input
/// </summary>
public class SignOutFlowInput
{
    /// <summary>
    /// The indicator to require a valid context (for auto sign-out)
    /// </summary>
    public bool RequireValidContext { get; set; }

    /// <summary>
    /// The logout id
    /// </summary>
    public string LogoutId { get; set; }
}