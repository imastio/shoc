using Shoc.Executor.Model.Job;

namespace Shoc.Executor.Model.Deployment
{
    /// <summary>
    /// Class for creating deployments
    /// </summary>
    public class CreateDeploymentInput
    {
        /// <summary>
        /// The job to deploy
        /// </summary>
        public JobModel Job { get; set; }

        /// <summary>
        /// The project type
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// The kubernetes configuration for connection
        /// </summary>
        public string Kubeconfig { get; set; }
        
        /// <summary>
        /// The image registry username
        /// </summary>
        public string RegistryUsername { get; set; }
        
        /// <summary>
        /// The image registry password
        /// </summary>
        public string RegistryPassword { get; set; }

        /// <summary>
        /// The image registry email
        /// </summary>
        public string RegistryEmail { get; set; }

        /// <summary>
        /// The image registry url
        /// </summary>
        public string RegistryUrl { get; set; }

        /// <summary>
        /// The image name
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Count of the nodes
        /// </summary>
        public int NodeCount { get; set; }

        /// <summary>
        /// The service port
        /// </summary>
        public int ServicePort { get; set; }
    }
}
