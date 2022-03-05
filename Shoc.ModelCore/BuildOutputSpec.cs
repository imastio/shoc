using System.Collections.Generic;

namespace Shoc.ModelCore
{
    /// <summary>
    /// The build output spec
    /// </summary>
    public class BuildOutputSpec
    {
        /// <summary>
        /// The stdout file path
        /// </summary>
        public string StdOut { get; set; }

        /// <summary>
        /// The stderr file path
        /// </summary>
        public string StdErr { get; set; }

        /// <summary>
        /// The set of required files
        /// </summary>
        public List<string> RequiredFiles { get; set; }
    }
}