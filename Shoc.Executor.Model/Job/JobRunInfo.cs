using System.Collections.Generic;

namespace Shoc.Executor.Model.Job
{
    /// <summary>
    /// Class for job running info
    /// </summary>
    public class JobRunInfo
    {
        /// <summary>
        /// The arguments list
        /// </summary>
        public IEnumerable<string> Args { get; set; }

        /// <summary>
        /// The environments
        /// </summary>
        public Dictionary<string, object> Environments { get; set; }
    }
}
