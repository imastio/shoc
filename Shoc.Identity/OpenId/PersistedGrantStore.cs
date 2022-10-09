using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Shoc.Identity.Data;
using Shoc.Identity.Model;

namespace Shoc.Identity.OpenId
{
    /// <summary>
    /// The persisted access grant store implementation
    /// </summary>
    public class PersistedGrantStore : IPersistedGrantStore
    {
        /// <summary>
        /// The grants repository
        /// </summary>
        private readonly IAccessGrantRepository grantsRepository;

        /// <summary>
        /// Creates new instance of persisted grants store
        /// </summary>
        /// <param name="grantsRepository">The grants repository</param>
        public PersistedGrantStore(IAccessGrantRepository grantsRepository)
        {
            this.grantsRepository = grantsRepository;
        }

        /// <summary>
        /// Stores given accessGrant in the 
        /// </summary>
        /// <param name="grant"></param>
        /// <returns></returns>
        public Task StoreAsync(PersistedGrant grant)
        {
            return this.grantsRepository.Save(ToModel(grant));
        }

        /// <summary>
        /// Gets the stored persisted grant
        /// </summary>
        /// <param name="key">The key of grant</param>
        /// <returns></returns>
        public async Task<PersistedGrant> GetAsync(string key)
        {
            return FromModel(await this.grantsRepository.GetByKey(key));
        }

        /// <summary>
        /// Gets all the persisted grants from storage
        /// </summary>
        /// <param name="filter">The filter of grants</param>
        /// <returns></returns>
        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            // get the set of grants
            var grants = await this.grantsRepository.GetAllBy(ToFilterModel(filter));

            // return as IS entities
            return grants.Select(FromModel);
        }

        /// <summary>
        /// Removes the grant with given key
        /// </summary>
        /// <param name="key">The grant key</param>
        /// <returns></returns>
        public Task RemoveAsync(string key)
        {
            return this.grantsRepository.DeleteByKey(key);
        }

        /// <summary>
        /// Delete all the grants with given filter
        /// </summary>
        /// <param name="filter">The filter to delete</param>
        /// <returns></returns>
        public Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            return this.grantsRepository.DeleteAllBy(ToFilterModel(filter));
        }

        /// <summary>
        /// Converts the grants filter into corresponding model
        /// </summary>
        /// <param name="filter">The grants filter</param>
        /// <returns></returns>
        private static AccessGrantFilter ToFilterModel(PersistedGrantFilter filter)
        {
            return new AccessGrantFilter
            {
                SessionId = filter.SessionId,
                ClientId = filter.ClientId,
                SubjectId = filter.SubjectId,
                Type = filter.Type
            };
        }

        /// <summary>
        /// Convert persisted accessGrant into access accessGrant to store
        /// </summary>
        /// <param name="grant">The persisted accessGrant</param>
        /// <returns></returns>
        private static AccessGrant ToModel(PersistedGrant grant)
        {
            if (grant == null)
            {
                return null;
            }

            return new AccessGrant
            {
                Key = grant.Key,
                ClientId = grant.ClientId,
                SubjectId = grant.SubjectId,
                Type = grant.Type,
                ConsumedTime = grant.ConsumedTime,
                CreationTime = grant.CreationTime,
                SessionId = grant.SessionId,
                Data = grant.Data,
                Description = grant.Description,
                Expiration = grant.Expiration
            };
        }

        /// <summary>
        /// Convert stored access grant into persisted grant in IS4
        /// </summary>
        /// <param name="accessGrant">The stored access grant</param>
        /// <returns></returns>
        private static PersistedGrant FromModel(AccessGrant accessGrant)
        {
            if (accessGrant == null)
            {
                return null;
            }

            return new PersistedGrant
            {
                Key = accessGrant.Key,
                ClientId = accessGrant.ClientId,
                SubjectId = accessGrant.SubjectId,
                Type = accessGrant.Type,
                ConsumedTime = accessGrant.ConsumedTime,
                CreationTime = accessGrant.CreationTime,
                SessionId = accessGrant.SessionId,
                Data = accessGrant.Data,
                Description = accessGrant.Description,
                Expiration = accessGrant.Expiration
            };
        }
    }
}