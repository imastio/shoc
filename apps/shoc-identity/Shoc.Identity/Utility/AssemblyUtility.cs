using System.IO;
using System.Reflection;

namespace Shoc.Identity.Utility;

/// <summary>
/// The assembly level utilities
/// </summary>
public class AssemblyUtility
{
    /// <summary>
    /// Resolves the relative path into full one
    /// </summary>
    /// <param name="parts">The path parts</param>
    /// <returns></returns>
    public static string ResolveRelative(params string[] parts)
    {
        // source directory
        var sourceDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        // build full path
        return Path.GetFullPath(Path.Combine(parts), sourceDirectory);
    }
}