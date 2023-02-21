using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Shoc.Builder.Model;
using Shoc.Core;
using Shoc.Executor.Data;
using Shoc.Executor.Model;
using Shoc.Executor.Model.Kubernetes;
using Shoc.ModelCore;

namespace Shoc.Executor.Services
{
    /// <summary>
    /// The kubernetes cluster service implementation
    /// </summary>
    public class KubernetesClusterService
    {
        /// <summary>
        /// The name validation regex
        /// </summary>
        private static readonly Regex NAME_REGEX = new("^[a-zA-Z0-9_]+$");

        /// <summary>
        /// The kubernetes cluster repository
        /// </summary>
        private readonly IKubernetesClusterRepository kubernetesClusterRepository;

        /// <summary>
        /// The data protection provider 
        /// </summary>
        private readonly IDataProtectionProvider dataProtectionProvider;

        /// <summary>
        /// Creates new instance of kubernetes cluster service
        /// </summary>
        /// <param name="kubernetesClusterRepository">The kubernetes cluster repository</param>
        /// <param name="dataProtectionProvider">The data protection provider</param>
        public KubernetesClusterService(IKubernetesClusterRepository kubernetesClusterRepository, IDataProtectionProvider dataProtectionProvider)
        {
            this.kubernetesClusterRepository = kubernetesClusterRepository;
            this.dataProtectionProvider = dataProtectionProvider;
        }

        /// <summary>
        /// Gets all the kubernetes cluster instances
        /// </summary>
        /// <param name="principal">The requesting principal</param>
        /// <param name="query">The query to lookup</param>
        /// <returns></returns>
        public Task<IEnumerable<KubernetesCluster>> GetBy(ShocPrincipal principal, KubernetesClusterQuery query)
        {
            // gets all the entries by owner
            return this.kubernetesClusterRepository.GetBy(query);
        }

        /// <summary>
        /// Gets the kubernetes cluster by id
        /// </summary>
        /// <param name="principal">The current principal</param>
        /// <param name="id">The id of cluster</param>
        /// <returns></returns>
        public async Task<KubernetesCluster> GetById(ShocPrincipal principal, string id)
        {
            // try load the result
            var result = await this.kubernetesClusterRepository.GetById(id);

            // not found
            if (result == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }
            
            // the result
            return result;
        }

        /// <summary>
        /// Creates a kubernetes cluster with given input
        /// </summary>
        /// <param name="principal">The authenticated principal</param>
        /// <param name="input">The cluster creation input</param>
        /// <returns></returns>
        public async Task<KubernetesCluster> Create(ShocPrincipal principal, CreateKubernetesCluster input)
        {
            // name should be given
            if (string.IsNullOrEmpty(input.Name))
            {
                throw ErrorDefinition.Validation(BuilderErrors.INVALID_NAME).AsException();
            }

            // check if name does not pass the pattern
            if (!NAME_REGEX.IsMatch(input.Name))
            {
                throw ErrorDefinition.Validation(BuilderErrors.INVALID_NAME).AsException();
            }

            // check if kubeconfig is missing
            if (string.IsNullOrWhiteSpace(input.KubeConfigPlaintext))
            {
                throw ErrorDefinition.Validation(BuilderErrors.INVALID_KUBE_CONFIG).AsException();
            }

            // create a protector
            var protector = this.dataProtectionProvider.CreateProtector(ExecutorProtection.CLUSTER_CONFIG);

            // encrypt kubeconfig
            input.EncryptedKubeConfig = protector.Protect(input.KubeConfigPlaintext);

            // try load an existing item with the name
            var existing = await this.kubernetesClusterRepository.GetBy(new KubernetesClusterQuery()
            {
                Name = input.Name
            });

            // there is an object with the name
            if (existing.Any())
            {
                throw ErrorDefinition.Validation(BuilderErrors.EXISTING_NAME).AsException();
            }

            // create the cluster
            return await this.kubernetesClusterRepository.Create(input);
        }

        /// <summary>
        /// Deletes the cluster by given id
        /// </summary>
        /// <param name="principal">The principal</param>
        /// <param name="id">The id of cluster</param>
        /// <returns></returns>
        public async Task<KubernetesCluster> DeleteById(ShocPrincipal principal, string id)
        {
            // get the object by id
            var result = await this.GetById(principal, id);

            // if object is available delete from repository
            return await this.kubernetesClusterRepository.DeleteById(result.Id);
        }
    }
}