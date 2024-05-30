using System;

namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The one-time password
/// </summary>
public class OneTimePassModel
{
    /// <summary>
    /// The OTP identity
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The user identifier
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Target of signing in (email or phone value)
    /// </summary>
    public string Target { get; set; }

    /// <summary>
    /// The type of target
    /// </summary>
    public string TargetType { get; set; }

    /// <summary>
    /// The OTP delivery method
    /// </summary>
    public string DeliveryMethod { get; set; }

    /// <summary>
    /// The hash of one-time password
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// The magic link fragment
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    /// The language in context
    /// </summary>
    public string Lang { get; set; }

    /// <summary>
    /// The expiration time
    /// </summary>
    public DateTime ValidUntil { get; set; }

    /// <summary>
    /// The return url if context is available
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
}