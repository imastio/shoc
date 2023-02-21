using Shoc.Executor.Model.Job;

namespace Shoc.Executor.Model.Deployment
{
    /// <summary>
    /// Class for create deployments
    /// </summary>
    public class CreateDeploymentInput
    {
        /// <summary>
        /// 
        /// </summary>
        public JobModel Job { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Kubeconfig { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string RegistryUsername { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string RegistryPassword { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RegistryEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RegistryUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Image { get; set; }
    }
}
