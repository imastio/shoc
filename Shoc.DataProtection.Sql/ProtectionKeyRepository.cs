using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.DataProtection;

namespace Shoc.Data.DataProtection.Sql
{
    /// <summary>
    /// The protection key repository interface
    /// </summary>
    public class ProtectionKeyRepository : IProtectionKeyRepository
    {
        /// <summary>
        /// The data operations instance
        /// </summary>
        private readonly DataOperations dataOps;

        /// <summary>
        /// Creates new instance of access grant repository implementation
        /// </summary>
        /// <param name="dataOps">A DataOps instance</param>
        public ProtectionKeyRepository(DataOperations dataOps)
        {
            this.dataOps = dataOps;
        }

        /// <summary>
        /// Gets all the protection keys
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<ProtectionKeyModel>> GetAll()
        {
            return this.dataOps.Connect().Query("ProtectionKey", "GetAll").ExecuteAsync<ProtectionKeyModel>();
        }

        /// <summary>
        /// Saves the model
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<int> Create(ProtectionKeyModel input)
        {
            return this.dataOps.Connect().NonQuery("ProtectionKey", "Create").ExecuteAsync(input);
        }
    }
}