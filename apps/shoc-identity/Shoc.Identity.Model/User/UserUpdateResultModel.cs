using System;

namespace Shoc.Identity.Model.User;

/// <summary>
/// The user update result model
/// </summary>
public class UserUpdateResultModel
{
    /// <summary>
    /// The user id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}