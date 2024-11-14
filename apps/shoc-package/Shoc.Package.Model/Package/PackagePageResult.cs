using System.Collections.Generic;

namespace Shoc.Package.Model.Package;

/// <summary>
/// The package page result
/// </summary>
public class PackagePageResult<TItem>
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