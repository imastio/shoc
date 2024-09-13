using System;

namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The sign-in attempt success model
/// </summary>
public class SigninAttemptSuccessModel
{
    /// <summary>
    /// The user logged in
    /// </summary>
    public string Id { get; set; }
        
    /// <summary>
    /// The last ip of login
    /// </summary>
    public string Ip { get; set; }

    /// <summary>
    /// The last login time to update
    /// </summary>
    public DateTime Time { get; set; }
}