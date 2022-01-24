using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Shoc.Identity
{
    /// <summary>
    /// The operations manifest 
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
                Path.Combine(sourceDirectory, "Operations", "Identity.xml"),
                Path.Combine(sourceDirectory, "Operations", "User.xml"),
                Path.Combine(sourceDirectory, "Operations", "Confirmation.xml")
            };
        }
    }
}