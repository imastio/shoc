using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Shoc.Builder
{
    /// <summary>
    /// The builder operations declaration
    /// </summary>
    public class BuilderOperations
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
                Path.Combine(sourceDirectory, "Operations", "Project.xml"),
                Path.Combine(sourceDirectory, "Operations", "DockerRegistry.xml")
            };
        }
    }
}