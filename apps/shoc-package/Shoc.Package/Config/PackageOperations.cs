﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Shoc.Package.Config;

/// <summary>
/// The service operation source declaration
/// </summary>
public static class PackageOperations
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
            Path.Combine(sourceDirectory, "Operations", "PackageAccess.xml"),
            Path.Combine(sourceDirectory, "Operations", "JobAccess.xml"),
            Path.Combine(sourceDirectory, "Operations", "Package.xml"),
            Path.Combine(sourceDirectory, "Operations", "Package.BuildTask.xml")
        };
    }
}
