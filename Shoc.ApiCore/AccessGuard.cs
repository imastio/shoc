using System;
using Shoc.Core;

namespace Shoc.ApiCore
{
    /// <summary>
    /// The access guard utility
    /// </summary>
    public static class AccessGuard
    {
        /// <summary>
        /// Require the given predicate to evaluate successfully for the access
        /// </summary>
        /// <param name="predicate">The access criteria</param>
        /// <param name="errorCode">The error code</param>
        /// <param name="message">The error message</param>
        public static void Require(Func<bool> predicate, string errorCode = null, string message = null)
        {
            // make sure access requirement is satisfied
            if (!predicate.Invoke())
            {
                throw ErrorDefinition.Access(errorCode, message).AsException();
            }
        }
    }
}