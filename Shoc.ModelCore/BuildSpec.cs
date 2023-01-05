using System.Collections.Generic;

namespace Shoc.ModelCore
{
    /// <summary>
    /// The project build specification
    /// </summary>
    public class BuildSpec
    {
        /// <summary>
        /// The registry name 
        /// </summary>
        public string RegistryName { get; set; }

        /// <summary>
        /// The base technology string
        /// </summary>
        public string Base { get; set; }

        /// <summary>
        /// The name of user inside the package
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// The build hooks
        /// </summary>
        public BuildHooksSpec Hooks { get; set; }

        /// <summary>
        /// The build input specification
        /// </summary>
        public BuildInputSpec Input { get; set; }

        /// <summary>
        /// The entry point specification
        /// </summary>
        public List<string> EntryPoint { get; set; }
    }
}