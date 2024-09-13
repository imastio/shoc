using System;

namespace Shoc.Identity.Model.User;

/// <summary>
/// The user model
/// </summary>
public class UserModel
{
    /// <summary>
    /// The user id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The email address
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// If email is verified
    /// </summary>
    public bool EmailVerified { get; set; }

    /// <summary>
    /// The username
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The user type
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// The user state
    /// </summary>
    public string UserState { get; set; }

    /// <summary>
    /// The full name of user
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// The user timezone
    /// </summary>
    public string Timezone { get; set; }

    /// <summary>
    /// The last ip address
    /// </summary>
    public string LastIp { get; set; }

    /// <summary>
    /// The last login
    /// </summary>
    public DateTime? LastLogin { get; set; }

    /// <summary>
    /// If the multi-factor auth is enabled
    /// </summary>
    public bool MultiFactor { get; set; }

    /// <summary>
    /// The lockout expiration time
    /// </summary>
    public DateTime? LockedUntil { get; set; }

    /// <summary>
    /// The number of continuous failed attempts
    /// </summary>
    public int FailedAttempts { get; set; }

    /// <summary>
    /// The deletion indicator
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}