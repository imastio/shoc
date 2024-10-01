using System.Collections.Generic;

namespace Shoc.Kube.Model
{
    /// <summary>
    /// The headless service creation input representation
    /// </summary>
    public class CreateHeadlessServiceInput
    {
        /// <summary>
        /// The name for headless service
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The pod selectors
        /// </summary>
        public Dictionary<string, string> Selector { get; set; }

        /// <summary>
        /// The name of the port protocol
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// The port number
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The target port number
        /// </summary>
        public int TargetPort { get; set; }
    }
}