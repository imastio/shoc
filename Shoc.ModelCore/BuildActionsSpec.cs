using System.Collections.Generic;

namespace Shoc.ModelCore
{
    /// <summary>
    /// The build action specification
    /// </summary>
    public class BuildActionsSpec
    {
        /// <summary>
        /// The to execute before upload
        /// </summary>
        public List<string> PreUpload { get; set; }
    }
}