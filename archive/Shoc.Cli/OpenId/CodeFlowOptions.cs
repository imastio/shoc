using System;
using System.Threading;

namespace Shoc.Cli.OpenId
{
    /// <summary>
    /// The options of code flow
    /// </summary>
    public class CodeFlowOptions
    {
        /// <summary>
        /// The port to use
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The timeout to wait for
        /// </summary>
        public TimeSpan Timeout { get; set; }
    }
}