using System.Collections.Generic;

namespace Shoc.ModelCore
{
    /// <summary>
    /// The project manifest 
    /// </summary>
    public class ShocManifest
    {
        /// <summary>
        /// The registry name 
        /// </summary>
        public string RegistryName { get; set; }

        /// <summary>
        /// The files for build environment
        /// </summary>
        public Dictionary<string, object> Properties { get; set; }
    }
}