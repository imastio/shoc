namespace Shoc.Core;

/// <summary>
/// The URL additions
/// </summary>
public static class UrlExt
{
    /// <summary>
    /// Ensures the trailing slash to the url
    /// </summary>
    /// <param name="url">The url</param>
    /// <returns></returns>
    public static string EnsureSlash(string url)
    {
        return url.EndsWith('/') ? url : $"{url}/";
    }
}