using System;

namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The sign-in history record model
/// </summary>
public class SigninHistoryRecordModel
{
    /// <summary>
    /// The record id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The session id
    /// </summary>
    public string SessionId { get; set; }

    /// <summary>
    /// The user id
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// The IP address
    /// </summary>
    public string Ip { get; set; }

    /// <summary>
    /// The identity provider
    /// </summary>
    public string Provider { get; set; }
        
    /// <summary>
    /// The signin method
    /// </summary>
    public string MethodType { get; set; }
        
    /// <summary>
    /// The signin method
    /// </summary>
    public string MultiFactorType { get; set; }

    /// <summary>
    /// The user agent
    /// </summary>
    public string UserAgent { get; set; }

    /// <summary>
    /// The time of sign-in
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// The time of refresh
    /// </summary>
    public DateTime Refreshed { get; set; }
}