using System.Collections.Generic;

namespace Shoc.Core
{
    /// <summary>
    /// The aggregated error definition
    /// </summary>
    public class AggregateErrorDefinition
    {
        /// <summary>
        /// The underlying error definitions
        /// </summary>
        public List<ErrorDefinition> Errors { get; set; }
    }
}