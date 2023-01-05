using System;

namespace Shoc.Builder.Model.Kubernetes
{
    /// <summary>
    /// The kubernetes cluster model
    /// </summary>
    public class KubernetesCluster
    {
        /// <summary>
        /// The cluster id
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// The cluster name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The api server uri value
        /// </summary>
        public string ApiServerUri { get; set; }

        /// <summary>
        /// The kubeconfig in encrypted form
        /// </summary>
        public string EncryptedKubeConfig { get; set; }

        /// <summary>
        /// The creation time
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The update time
        /// </summary>
        public DateTime Updated { get; set; }
    }
}