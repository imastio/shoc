using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Protection;
using Shoc.Executor.Model.Kubernetes;
using Shoc.Executor.Services;
using Shoc.Identity.Model;

namespace Shoc.Executor.Controllers
{
    /// <summary>
    /// The kubernetes clusters controller
    /// </summary>
    [Route("api/kubernetes-clusters")]
    [ApiController]
    [ShocExceptionHandler]
    [AuthorizedSubject]
    [AuthorizeMinUserType(UserTypes.ADMIN)]
    public class KubernetesClustersController : ControllerBase
    {
        /// <summary>
        /// The kubernetes cluster service
        /// </summary>
        private readonly KubernetesClusterService kubernetesClusterService;

        /// <summary>
        /// Creates new instance of kubernetes cluster controller
        /// </summary>
        /// <param name="kubernetesClusterService">The kubernetes cluster service</param>
        public KubernetesClustersController(KubernetesClusterService kubernetesClusterService)
        {
            this.kubernetesClusterService = kubernetesClusterService;
        }

        /// <summary>
        /// Gets all the objects
        /// </summary>
        /// <param name="name">The name of the cluster</param>
        /// <returns></returns>
        [HttpGet]
        public Task<IEnumerable<KubernetesCluster>> GetAll([FromQuery] string name = null)
        {
            // the request principal
            var principal = this.HttpContext.GetShocPrincipal();

            // get the entities by owner
            return this.kubernetesClusterService.GetBy(principal, new KubernetesClusterQuery()
            {
                Name = name
            });
        }

        /// <summary>
        /// Creates an object with the given input
        /// </summary>
        /// <param name="input">The entity creation input</param>
        /// <returns></returns>
        [HttpPost]
        public Task<KubernetesCluster> Create([FromBody] CreateKubernetesCluster input)
        {
            return this.kubernetesClusterService.Create(this.HttpContext.GetShocPrincipal(), input);
        }

        /// <summary>
        /// Deletes a entity with the given id
        /// </summary>
        /// <param name="id">The id of entity to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task<KubernetesCluster> DeleteById(string id)
        {
            return this.kubernetesClusterService.DeleteById(this.HttpContext.GetShocPrincipal(), id);
        }
    }
}
