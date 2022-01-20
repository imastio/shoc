using System;
using System.Collections.Generic;
using Imast.Ext.DiscoveryCore;

namespace Shoc.Core.Discovery
{
    /// <summary>
    /// The static discovery for shoc services
    /// </summary>
    public class ShocStaticDiscovery : StaticDiscoveryClient
    {
        /// <summary>
        /// Creates new instance of shoc static discovery
        /// </summary>
        /// <param name="protocol">The protocol</param>
        /// <param name="host">The host</param>
        public ShocStaticDiscovery(string protocol, string host) : base(protocol, host, GetFallbackPort(protocol), GetPorts(protocol))
        {
        }

        /// <summary>
        /// Creates new instance of shoc static discovery
        /// </summary>
        public ShocStaticDiscovery() : this("http", "localhost")
        {
        }

        /// <summary>
        /// Gets the fallback port based on protocol
        /// </summary>
        /// <param name="protocol">The protocol</param>
        /// <returns></returns>
        private static int GetFallbackPort(string protocol)
        {
            return string.Equals(protocol, "https", StringComparison.CurrentCultureIgnoreCase) ? 443 : 80;
        }

        /// <summary>
        /// Gets the fallback port based on protocol
        /// </summary>
        /// <param name="protocol">The protocol</param>
        /// <returns></returns>
        private static IDictionary<string, int> GetPorts(string protocol)
        {
            // indicates if https is enforced
            var useHttps = string.Equals(protocol, "https", StringComparison.CurrentCultureIgnoreCase);
            
            // define known services for discovery
            var services = new Dictionary<string, int>
            {
                {"shoc-database-sql", useHttps ?  0 : 11001},
                {"shoc-dind", useHttps ? 0 : 11003},
                {"shoc-docker-registry", useHttps ? 0 : 11004},
                {"shoc-builder", useHttps ? 11007 : 11006}

            };

            return services;
        }
    }
}
