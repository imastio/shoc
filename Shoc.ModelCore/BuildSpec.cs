using System.Collections.Generic;

namespace Shoc.ModelCore
{
    /// <summary>
    /// The project build specification
    /// </summary>
    public class BuildSpec
    {
        /// <summary>
        /// The code name of technology
        /// </summary>
        public string Technology { get; set; }

        /// <summary>
        /// The build action specification
        /// </summary>
        public BuildActionsSpec Actions { get; set; }

        /// <summary>
        /// The required package references
        /// </summary>
        public List<PackageReference> Packages { get; set; }

        /// <summary>
        /// The files to add
        /// </summary>
        public List<string> Files { get; set; }

        /// <summary>
        /// The files to exclude
        /// </summary>
        public List<string> ExcludeFiles { get; set; }

        /// <summary>
        /// The entry point of the project
        /// </summary>
        public string EntryPoint { get; set; }

        /// <summary>
        /// The arguments for entry point
        /// </summary>
        public string Args { get; set; }
    }
}