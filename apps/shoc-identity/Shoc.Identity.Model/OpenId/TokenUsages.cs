namespace Shoc.Identity.Model;

/// <summary>
/// The types of token usages
/// </summary>
public static class TokenUsages
{
    /// <summary>
    /// The token should be used only once
    /// </summary>
    public const string ONE_TIME = "one_time";

    /// <summary>
    /// The token can be reused
    /// </summary>
    public const string REUSE = "reuse";
}