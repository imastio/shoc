using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Shoc.Settings.Config;

/// <summary>
/// The service operation source declaration
/// </summary>
public static class SettingsOperations
{
    /// <summary>
    /// Gets the data sources declared in a module
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<string> GetSources()
    {
        // get the execution directory
        var sourceDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        // return sources
        return new[]
        {
            Path.Combine(sourceDirectory, "Operations", "ProtectionKey.xml"),
            Path.Combine(sourceDirectory, "Operations", "Mailing.xml"),
            Path.Combine(sourceDirectory, "Operations", "Access.xml")
        };
    }
}
