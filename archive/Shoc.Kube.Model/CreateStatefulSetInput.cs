using System.Collections.Generic;

namespace Shoc.Kube.Model
{
    /// <summary>
    /// The stateful set creation input representation
    /// </summary>
    public class CreateStatefulSetInput
    {
        /// <summary>
        /// The name for stateful set
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The service name
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// The service selectors to match labels
        /// </summary>
        public Dictionary<string, string> ServiceSelector { get; set; }

        /// <summary>
        /// The replicas count
        /// </summary>
        public int Replicas { get; set; }

        /// <summary>
        /// The labels for pods
        /// </summary>
        public Dictionary<string, string> PodLabels { get; set; }

        /// <summary>
        /// The name for containers
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// The image uri for container
        /// </summary>
        public string ContainerImageUri { get; set; }

        /// <summary>
        /// The container port number
        /// </summary>
        public int ContainerPort { get; set; }
    }
}