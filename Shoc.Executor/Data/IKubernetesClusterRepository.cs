using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Executor.Model.Kubernetes;

namespace Shoc.Executor.Data
{
    /// <summary>
    /// The repository of kubernetes cluster
    /// </summary>
    public interface IKubernetesClusterRepository
    {
        /// <summary>
        /// Gets all the kubernetes cluster instances
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<KubernetesCluster>> GetAll();

        /// <summary>
        /// Gets all the kubernetes cluster instances by query
        /// </summary>
        /// <param name="query">The lookup query</param>
        /// <returns></returns>
        Task<IEnumerable<KubernetesCluster>> GetBy(KubernetesClusterQuery query);

        /// <summary>
        /// Gets the kubernetes cluster by id
        /// </summary>
        /// <param name="id">The id of cluster</param>
        /// <returns></returns>
        Task<KubernetesCluster> GetById(string id);

        /// <summary>
        /// Creates a kubernetes cluster with given input
        /// </summary>
        /// <param name="input">The cluster creation input</param>
        /// <returns></returns>
        Task<KubernetesCluster> Create(CreateKubernetesCluster input);

        /// <summary>
        /// Deletes the cluster by given id
        /// </summary>
        /// <param name="id">The id of cluster</param>
        /// <returns></returns>
        Task<KubernetesCluster> DeleteById(string id);
    }
}