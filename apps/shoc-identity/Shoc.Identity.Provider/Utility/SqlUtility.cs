namespace Shoc.Identity.Provider.Utility;

/// <summary>
/// The SQL utilities
/// </summary>
public class SqlUtility
{
    /// <summary>
    /// Escapes like symbols with the default escape character
    /// </summary>
    /// <param name="input">The string to escape</param>
    /// <returns></returns>
    public static string EscapeLikeSymbols(string input)
    {
        return input?.Replace("%", string.Empty).Replace("_", @"\_").Replace("%", @"\%");
    }

    /// <summary>
    /// Build a value for sql like operator to match anything with escaped special symbols
    /// </summary>
    /// <param name="input">The input value string</param>
    /// <returns></returns>
    public static string AnyMatchLikeValue(string input)
    {
        return input == null ? null : $"%{EscapeLikeSymbols(input)}%";
    }
}