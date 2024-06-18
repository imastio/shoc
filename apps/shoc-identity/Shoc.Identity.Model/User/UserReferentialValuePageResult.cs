using System.Collections.Generic;

namespace Shoc.Identity.Model.User;

/// <summary>
/// The users page result
/// </summary>
public class UserReferentialValuePageResult
{
    /// <summary>
    /// The result items
    /// </summary>
    public IEnumerable<UserReferentialValueModel> Items { get; set; }

    /// <summary>
    /// The total count 
    /// </summary>
    public long TotalCount { get; set; }
}