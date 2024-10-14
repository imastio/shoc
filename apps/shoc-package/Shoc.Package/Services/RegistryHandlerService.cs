using Shoc.Package.Model.Package;
using Shoc.Package.Model.Registry;

namespace Shoc.Package.Services;

/// <summary>
/// The registry handler service
/// </summary>
public class RegistryHandlerService
{
    /// <summary>
    /// Builds the image tag by context
    /// </summary>
    /// <param name="context">The context</param>
    /// <returns></returns>
    public string BuildImageTag(RegistryImageContext context)
    {
        // for shoc provider use nested scheme
        if (context.Provider == KnownRegistryProviders.SHOC)
        {
            // handle both for user-scoped and workspace-scoped packages
            return context.TargetPackageScope == PackageScopes.WORKSPACE ? 
                $"{context.Registry}/w/{context.TargetWorkspaceId}/{context.TargetPackageId}" : 
                $"{context.Registry}/w/{context.TargetWorkspaceId}/u/{context.TargetUserId}/{context.TargetPackageId}";
        }
        
        // for other providers do with given namespace
        return $"{context.Registry}/{context.Namespace}/{context.TargetWorkspaceId}{context.TargetPackageId}";
    }
}