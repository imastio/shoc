using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Builder.Model.Kubernetes;
using Shoc.Core;

namespace Shoc.Builder.Data.Sql
{
    /// <summary>
    /// The kubernetes cluster repository implementation
    /// </summary>
    public class KubernetesClusterRepository : IKubernetesClusterRepository
    {
        /// <summary>
        /// The data operations instance
        /// </summary>
        private readonly DataOperations dataOps;

        /// <summary>
        /// Creates new instance of kubernetes cluster repository implementation
        /// </summary>
        /// <param name="dataOps">A DataOps instance</param>
        public KubernetesClusterRepository(DataOperations dataOps)
        {
            this.dataOps = dataOps;
        }

        /// <summary>
        /// Gets all the kubernetes cluster instances
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<KubernetesCluster>> GetAll()
        {
            return this.dataOps.Connect().Query("KubernetesCluster", "GetAll").ExecuteAsync<KubernetesCluster>();
        }

        /// <summary>
        /// Gets all the kubernetes cluster instances by query
        /// </summary>
        /// <param name="query">The lookup query</param>
        /// <returns></returns>
        public Task<IEnumerable<KubernetesCluster>> GetBy(KubernetesClusterQuery query)
        {
            return this.dataOps.Connect().Query("KubernetesCluster", "GetBy")
                .WithBinding("ByName", query.Name != null)
                .ExecuteAsync<KubernetesCluster>(query);
        }

        /// <summary>
        /// Gets the kubernetes cluster by id
        /// </summary>
        /// <param name="id">The id of cluster</param>
        /// <returns></returns>
        public Task<KubernetesCluster> GetById(string id)
        {
            return this.dataOps.Connect().QueryFirst("KubernetesCluster", "GetById").ExecuteAsync<KubernetesCluster>(new { Id = id });
        }

        /// <summary>
        /// Creates a kubernetes cluster with given input
        /// </summary>
        /// <param name="input">The cluster creation input</param>
        /// <returns></returns>
        public Task<KubernetesCluster> Create(CreateKubernetesCluster input)
        {
            // generate id if necessary
            input.Id ??= StdIdGenerator.Next(BuilderObjects.KUBERNETES_CLUSTER)?.ToLowerInvariant();

            // add object to the database
            return this.dataOps.Connect().QueryFirst("KubernetesCluster", "Create").ExecuteAsync<KubernetesCluster>(input);
        }

        /// <summary>
        /// Deletes the cluster by given id
        /// </summary>
        /// <param name="id">The id of cluster</param>
        /// <returns></returns>
        public Task<KubernetesCluster> DeleteById(string id)
        {
            // delete object from the database
            return this.dataOps.Connect().QueryFirst("KubernetesCluster", "DeleteById").ExecuteAsync<KubernetesCluster>(new
            {
                Id = id
            });
        }
    }
}