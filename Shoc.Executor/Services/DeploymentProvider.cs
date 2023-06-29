using Shoc.Executor.Model.Deployment;
using Shoc.Executor.Services.Interfaces;

namespace Shoc.Executor.Services
{
    /// <summary>
    /// Service for providing deployments.
    /// </summary>
    public class DeploymentProvider : IDeploymentProvider
    {
        /// <summary>
        /// Get deployment by type
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IDeployment Create(CreateDeploymentInput input)
        {
            return input.Type switch
            {
                "list" => new ListDeployment(input),
                "hostname" => new HostnameDeployment(input),
                "python-pip" => new PythonPipDeployment(input),
                "python-mpi" => new PythonMpiDeployment(input),
                _ => null
            };
        }
    }
}
