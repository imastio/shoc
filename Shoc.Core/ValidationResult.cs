using System.Collections.Generic;

namespace Shoc.Core
{
    /// <summary>
    /// The validation result
    /// </summary>
    public class ValidationResult<T>
    {
        /// <summary>
        /// The validation errors
        /// </summary>
        public List<ErrorDefinition> Errors { get; set; }

        /// <summary>
        /// The valid value
        /// </summary>
        public T Value { get; set; }
    }
}