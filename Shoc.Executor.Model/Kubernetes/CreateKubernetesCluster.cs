namespace Shoc.Executor.Model.Kubernetes
{
    /// <summary>
    /// The kubernetes cluster creating model
    /// </summary>
    public class CreateKubernetesCluster
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
        /// The kubeconfig in plain text form
        /// </summary>
        public string KubeConfigPlaintext { get; set; }

        /// <summary>
        /// The kubeconfig in encrypted form
        /// </summary>
        public string EncryptedKubeConfig { get; set; }
    }
}