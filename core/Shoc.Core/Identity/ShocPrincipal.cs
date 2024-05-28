using System.Collections.Generic;

namespace Shoc.Core.Identity;

/// <summary>
/// The principal model
/// </summary>
public class ShocPrincipal
{
    /// <summary>
    /// The principal id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The set of accesses
    /// </summary>
    public ISet<string> Accesses { get; set; }
}