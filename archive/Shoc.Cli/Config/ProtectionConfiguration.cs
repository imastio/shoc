using System.IO;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Shoc.Cli.Utility;

namespace Shoc.Cli.Config
{
    /// <summary>
    /// The protection configuration
    /// </summary>
    public static class ProtectionConfiguration
    {
        /// <summary>
        /// Adds the data protection into the chain
        /// </summary>
        /// <param name="services">The services</param>
        /// <returns></returns>
        public static IServiceCollection AddProtection(this IServiceCollection services)
        {
            // the data protection chain
            var builder = services.AddDataProtection()
                .SetApplicationName("shocctl")
                .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(ShocRoot.GetOrCreateShocRoot().FullName, "p-keys")));

            // in case of windows protect with DPAPI
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                builder.ProtectKeysWithDpapi();
            }

            return services;
        }
    }
}