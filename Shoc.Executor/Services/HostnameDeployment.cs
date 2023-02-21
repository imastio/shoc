using System.Threading.Tasks;
using Shoc.Executor.Model.Deployment;
using Shoc.Executor.Services.Interfaces;

namespace Shoc.Executor.Services
{
    /// <summary>
    /// Class for Hostname project type deployment
    /// </summary>
    public class HostnameDeployment : DeploymentBase, IDeployment
    {
        /// <summary>
        /// Create new instance of class
        /// </summary>
        /// <param name="input">The input of deployment</param>
        public HostnameDeployment(CreateDeploymentInput input) : base(input)
        {

        }

        /// <summary>
        /// Deploys the required kinds into Kubernetes
        /// </summary>
        public async Task Deploy()
        {
            // make sure namespace exists
            await this.AssureNamespace();

            // create the secret for pulling images
            await this.CreateSecret();

            // creates batch job in kubernetes
            await this.CreateJob();
        }
    }
}
