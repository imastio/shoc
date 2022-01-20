using System;

namespace Shoc.Core
{
    /// <summary>
    /// The standard id generator
    /// </summary>
    public static class StdIdGenerator
    {
        /// <summary>
        /// The next random unique id for the object
        /// </summary>
        /// <param name="obj">The target object</param>
        /// <returns></returns>
        public static string Next(string obj)
        {
            return $"{obj}-{Guid.NewGuid():N}";
        }
    }
}