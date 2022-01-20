using System;
using System.Collections.Generic;
using System.Linq;

namespace Shoc.Core
{
    /// <summary>
    /// The exception of shoc application
    /// </summary>
    public class ShocException : Exception
    {
        /// <summary>
        /// The set of errors
        /// </summary>
        public List<ErrorDefinition> Errors { get; }

        /// <summary>
        /// Creates new instance for error definition
        /// </summary>
        public ShocException(List<ErrorDefinition> errors = null, string message = null, Exception innerException = null) : base(message, innerException)
        {
            this.Errors = errors ?? new List<ErrorDefinition>();
        }

        /// <summary>
        /// Creates new instance for error definition
        /// </summary>
        public ShocException(string message) : this(new List<ErrorDefinition> { ErrorDefinition.Unknown(message) }, message)
        {
        }

        /// <summary>
        /// Creates new instance for error definition
        /// </summary>
        public ShocException(params ErrorDefinition[] errors) : this(errors.ToList())
        {
        }
    }
}