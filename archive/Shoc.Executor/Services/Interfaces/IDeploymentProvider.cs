using Shoc.Executor.Model.Deployment;

namespace Shoc.Executor.Services.Interfaces
{
    /// <summary>
    /// Interface for Providing Deployments
    /// </summary>
    public interface IDeploymentProvider
    {
        /// <summary>
        /// Gets the Deployment instance from name
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        IDeployment Create(CreateDeploymentInput input);
    }
}
