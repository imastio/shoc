using System.Threading.Tasks;
using Shoc.Executor.Model.Deployment;
using Shoc.Executor.Services.Interfaces;

namespace Shoc.Executor.Services
{
    /// <summary>
    /// Class for Python-pip project type deployment
    /// </summary>
    public class PythonPipDeployment : DeploymentBase, IDeployment
    {
        /// <summary>
        /// Creates new instance of class
        /// </summary>
        /// <param name="input"></param>
        public PythonPipDeployment(CreateDeploymentInput input) : base(input)
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
            await this.CreatePullSecret();

            // creates batch job in kubernetes
            await this.CreateJob();
        }
    }
}