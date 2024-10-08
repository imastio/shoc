﻿namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The sign-in input
/// </summary>
public class SignInFlowInput
{
    /// <summary>
    /// The email of user
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The password to sign in
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// The input language
    /// </summary>
    public string Lang { get; set; }

    /// <summary>
    /// The return url
    /// </summary>
    public string ReturnUrl { get; set; }
}