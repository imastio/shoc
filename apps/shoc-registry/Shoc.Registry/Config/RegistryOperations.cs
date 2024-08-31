using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Shoc.Registry.Config;

/// <summary>
/// The service operation source declaration
/// </summary>
public static class RegistryOperations
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
            Path.Combine(sourceDirectory, "Operations", "Access.xml"),
            Path.Combine(sourceDirectory, "Operations", "WorkspaceAccess.xml"),
            Path.Combine(sourceDirectory, "Operations", "Registry.xml"),
            Path.Combine(sourceDirectory, "Operations", "Registry.Credential.xml")
        };
    }
}
