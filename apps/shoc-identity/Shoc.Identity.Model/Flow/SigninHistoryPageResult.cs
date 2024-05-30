using System.Collections.Generic;

namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The sign-in history page result
/// </summary>
public class SigninHistoryPageResult
{
    /// <summary>
    /// The sign-in history page result
    /// </summary>
    public IEnumerable<SigninHistoryRecordModel> Items { get; set; }

    /// <summary>
    /// The total number of items
    /// </summary>
    public long TotalCount { get; set; }
}