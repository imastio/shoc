using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Shoc.Workspace.Config;

/// <summary>
/// The service operation source declaration
/// </summary>
public static class WorkspaceOperations
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
            Path.Combine(sourceDirectory, "Operations", "Workspace.xml"),
            Path.Combine(sourceDirectory, "Operations", "Workspace.UserWorkspace.xml"),
            Path.Combine(sourceDirectory, "Operations", "Workspace.User.xml"),
            Path.Combine(sourceDirectory, "Operations", "Workspace.Member.xml"),
            Path.Combine(sourceDirectory, "Operations", "Workspace.Invitation.xml")
        };
    }
}
