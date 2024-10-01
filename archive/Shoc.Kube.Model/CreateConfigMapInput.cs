using System.Collections.Generic;

namespace Shoc.Kube.Model
{
    /// <summary>
    /// The config map creation input representation
    /// </summary>
    public class CreateConfigMapInput
    {
        /// <summary>
        /// The name for stateful set
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The dictionary of configs
        /// </summary>
        public Dictionary<string, string> Configs { get; set; }
    }
}