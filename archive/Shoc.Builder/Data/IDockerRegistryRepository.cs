using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Builder.Model.Registry;

namespace Shoc.Builder.Data
{
    /// <summary>
    /// The repository of docker registries
    /// </summary>
    public interface IDockerRegistryRepository
    {
        /// <summary>
        /// Gets all the docker registry instances
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DockerRegistry>> GetAll();

        /// <summary>
        /// Gets all the docker registry instances by query
        /// </summary>
        /// <param name="query">The lookup query</param>
        /// <returns></returns>
        Task<IEnumerable<DockerRegistry>> GetBy(DockerRegistryQuery query);

        /// <summary>
        /// Gets the docker registry by id
        /// </summary>
        /// <param name="id">The id of registry</param>
        /// <returns></returns>
        Task<DockerRegistry> GetById(string id);

        /// <summary>
        /// Creates a docker registry with given input
        /// </summary>
        /// <param name="input">The registry creation input</param>
        /// <returns></returns>
        Task<DockerRegistry> Create(CreateDockerRegistry input);

        /// <summary>
        /// Deletes the registry by given id
        /// </summary>
        /// <param name="id">The id of registry</param>
        /// <returns></returns>
        Task<DockerRegistry> DeleteById(string id);
    }
}