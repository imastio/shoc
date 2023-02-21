using System.Threading.Tasks;

namespace Shoc.Executor.Services.Interfaces
{
    /// <summary>
    /// Interfaces for Deployment
    /// </summary>
    public interface IDeployment
    {
        /// <summary>
        /// Deploys the required kinds into Kubernetes
        /// </summary>
        Task Deploy();
    }
}
