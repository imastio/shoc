using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Shoc.Database.Migrator.Config;

/// <summary>
/// The service operation source declaration
/// </summary>
public static class DatabaseMigratorOperations
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
            Path.Combine(sourceDirectory, "Operations", "Access.xml")
        };
    }
}
