using System.Collections.Generic;

namespace Shoc.Job.Model.Job;

/// <summary>
/// The job page result
/// </summary>
public class JobPageResult<TItem>
{
    /// <summary>
    /// The result items
    /// </summary>
    public IEnumerable<TItem> Items { get; set; }

    /// <summary>
    /// The total count 
    /// </summary>
    public long TotalCount { get; set; }
}