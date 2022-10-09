using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Identity.Model;

namespace Shoc.Identity.Data.Sql
{
    /// <summary>
    /// The signing key repository implementation
    /// </summary>
    public class SigningKeyRepository : ISigningKeyRepository
    {
        /// <summary>
        /// The data operations instance
        /// </summary>
        private readonly DataOperations dataOps;

        /// <summary>
        /// Creates new instance of repository implementation
        /// </summary>
        /// <param name="dataOps">A DataOps instance</param>
        public SigningKeyRepository(DataOperations dataOps)
        {
            this.dataOps = dataOps;
        }

        /// <summary>
        /// Get all keys by use indicator
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<SigningKey>> GetBy(string use)
        {
            return this.dataOps.Connect().Query("SigningKey", "GetBy").ExecuteAsync<SigningKey>(new
            {
                Use = use
            });
        }

        /// <summary>
        /// Get all keys by id and use indicator
        /// </summary>
        /// <returns></returns>
        public Task<SigningKey> GetById(string id)
        {
            return this.dataOps.Connect().QueryFirst("SigningKey", "GetById").ExecuteAsync<SigningKey>(new
            {
                Id = id
            });
        }

        /// <summary>
        /// Stores the key in the storage
        /// </summary>
        /// <param name="key">The key to store</param>
        /// <returns></returns>
        public Task<SigningKey> Create(SigningKey key)
        {
            return this.dataOps.Connect().QueryFirst("SigningKey", "Create").ExecuteAsync<SigningKey>(key);
        }

        /// <summary>
        /// Delete the key by given id and use
        /// </summary>
        /// <param name="id">The key id</param>
        /// <returns></returns>
        public Task<SigningKey> DeleteById(string id)
        {
            return this.dataOps.Connect().QueryFirst("SigningKey", "DeleteById").ExecuteAsync<SigningKey>(new
            {
                Id = id
            });
        }
    }
}