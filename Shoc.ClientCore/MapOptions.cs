using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shoc.ClientCore
{
    /// <summary>
    /// The options to map result
    /// </summary>
    public class MapOptions<T>
    {
        /// <summary>
        /// The status handlers
        /// </summary>
        private readonly Dictionary<int, Func<HttpResponseMessage, Task<T>>> statusHandlers;

        /// <summary>
        /// Creates new instance of map options
        /// </summary>
        public MapOptions()
        {
            this.statusHandlers = new Dictionary<int, Func<HttpResponseMessage, Task<T>>>();
        }

        /// <summary>
        /// Adds the status handling supplier
        /// </summary>
        /// <param name="status">The status to handle</param>
        /// <param name="supplier">The external supplier</param>
        public MapOptions<T> OnStatus(int status, Func<HttpResponseMessage, Task<T>> supplier)
        {
            this.statusHandlers[status] = supplier;
            return this;
        }

        /// <summary>
        /// Uses the default value on not found
        /// </summary>
        /// <param name="defaultValue">The default value to use</param>
        public MapOptions<T> OnNotFoundDefault(T defaultValue = default)
        {
            this.statusHandlers[404] = response => Task.FromResult(defaultValue);
            return this;
        }

        /// <summary>
        /// Gets the status handler if available
        /// </summary>
        /// <param name="status">The status code</param>
        /// <returns></returns>
        public Func<HttpResponseMessage, Task<T>> GetStatusHandler(int status)
        {
            return this.statusHandlers.GetValueOrDefault(status, null);
        }
    }
}
