using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Builder.Model.Registry;
using Shoc.Core;

namespace Shoc.Builder.Data.Sql
{
    /// <summary>
    /// The docker registry repository implementation
    /// </summary>
    public class DockerRegistryRepository : IDockerRegistryRepository
    {
        /// <summary>
        /// The data operations instance
        /// </summary>
        private readonly DataOperations dataOps;

        /// <summary>
        /// Creates new instance of docker registry repository implementation
        /// </summary>
        /// <param name="dataOps">A DataOps instance</param>
        public DockerRegistryRepository(DataOperations dataOps)
        {
            this.dataOps = dataOps;
        }

        /// <summary>
        /// Gets all the docker registry instances
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<DockerRegistry>> GetAll()
        {
            return this.dataOps.Connect().Query("DockerRegistry", "GetAll").ExecuteAsync<DockerRegistry>();
        }

        /// <summary>
        /// Gets the docker registry by id
        /// </summary>
        /// <param name="id">The id of registry</param>
        /// <returns></returns>
        public Task<DockerRegistry> GetById(string id)
        {
            return this.dataOps.Connect().QueryFirst("DockerRegistry", "GetById").ExecuteAsync<DockerRegistry>(new { Id = id });
        }

        /// <summary>
        /// Creates a docker registry with given input
        /// </summary>
        /// <param name="input">The registry creation input</param>
        /// <returns></returns>
        public Task<DockerRegistry> Create(CreateDockerRegistry input)
        {
            // generate id if necessary
            input.Id ??= StdIdGenerator.Next(BuilderObjects.DOCKER_REGISTRY)?.ToLowerInvariant();

            // add object to the database
            return this.dataOps.Connect().QueryFirst("DockerRegistry", "Create").ExecuteAsync<DockerRegistry>(input);
        }

        /// <summary>
        /// Deletes the registry by given id
        /// </summary>
        /// <param name="id">The id of registry</param>
        /// <returns></returns>
        public Task<DockerRegistry> DeleteById(string id)
        {
            // delete object from the database
            return this.dataOps.Connect().QueryFirst("DockerRegistry", "DeleteById").ExecuteAsync<DockerRegistry>(new
            {
                Id = id
            });
        }
    }
}