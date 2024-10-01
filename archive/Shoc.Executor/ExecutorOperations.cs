using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Shoc.Executor
{
    /// <summary>
    /// The executor operations declaration
    /// </summary>
    public class ExecutorOperations
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
                Path.Combine(sourceDirectory, "Operations", "Job.xml"),
                Path.Combine(sourceDirectory, "Operations", "KubernetesCluster.xml")
            };
        }
    }
}