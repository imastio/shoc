using System;
using System.IO;

namespace Shoc.Cli.Utility
{
    /// <summary>
    /// The shoc root utilities
    /// </summary>
    public static class ShocRoot
    {
        /// <summary>
        /// Gets the user configuration
        /// </summary>
        /// <returns></returns>
        public static DirectoryInfo GetOrCreateShocRoot()
        {
            // gets the home directory
            var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // get path to .shoc directory
            return Directory.CreateDirectory(Path.Combine(home, ".shoc"));
        }
    }
}