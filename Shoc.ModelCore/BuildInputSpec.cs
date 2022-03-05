using System.Collections.Generic;

namespace Shoc.ModelCore
{
    /// <summary>
    /// The build input spec
    /// </summary>
    public class BuildInputSpec
    {
        /// <summary>
        /// The files copy specification
        /// </summary>
        public List<FileCopySpec> Copy { get; set; }
    }
}