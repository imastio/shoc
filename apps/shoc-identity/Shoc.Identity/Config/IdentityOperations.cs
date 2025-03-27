using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Shoc.Identity.Config;

/// <summary>
/// The service operation source declaration
/// </summary>
public static class IdentityOperations
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
            Path.Combine(sourceDirectory, "Operations", "Identity.User.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.User.Access.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.User.Internal.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.Grant.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.Key.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.Otp.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.SigninHistory.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.ConfirmationCode.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.UserGroup.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.UserGroup.Access.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.UserGroup.Member.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.Application.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.Application.Secret.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.Application.Uri.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.Application.Claim.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.Privilege.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.Privilege.Access.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.Role.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.Role.Privilege.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.Role.Member.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.OidcProvider.xml"),
            Path.Combine(sourceDirectory, "Operations", "Identity.OidcProvider.Domain.xml"),
            Path.Combine(sourceDirectory, "Operations", "ProtectionKey.xml"),
            Path.Combine(sourceDirectory, "Operations", "Mailing.xml"),
            Path.Combine(sourceDirectory, "Operations", "Access.xml")
        };
    }
}