using System.Collections.Generic;

namespace Shoc.ModelCore
{
    /// <summary>
    /// The build action specification
    /// </summary>
    public class BuildHooksSpec
    {
        /// <summary>
        /// The to execute before packaging
        /// </summary>
        public List<string> BeforePackage { get; set; }
    }
}