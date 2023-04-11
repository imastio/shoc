using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Identity.Model;

namespace Shoc.Identity.Data.Sql
{
    /// <summary>
    /// The external users repository implementation
    /// </summary>
    public class ExternalUserRepository : IExternalUserRepository
    {
        /// <summary>
        /// The data operations instance
        /// </summary>
        private readonly DataOperations dataOps;

        /// <summary>
        /// Creates new instance of external users repository implementation
        /// </summary>
        /// <param name="dataOps">A DataOps instance</param>
        public ExternalUserRepository(DataOperations dataOps)
        {
            this.dataOps = dataOps;
        }

        /// <summary>
        /// Gets the external user by email and provider
        /// </summary>
        /// <param name="email">The user email</param>
        /// <param name="provider">The external provider name</param>
        /// <returns></returns>
        public Task<ExternalUserModel> GetByEmailAndProvider(string email, string provider)
        {
            // try load from database
            return this.dataOps.Connect().QueryFirst("ExternalUser", "GetByEmailAndProvider").ExecuteAsync<ExternalUserModel>(new
            {
                Email = email.ToLowerInvariant(),
                Provider = provider.ToLowerInvariant()
            });
        }

        /// <summary>
        /// Creates entity based on input
        /// </summary>
        /// <param name="input">The creation input</param>
        /// <returns></returns>
        public Task<ExternalUserModel> Create(CreateExternalUserModel input)
        {
            // handle case sensitivity of identifiers
            input.Email = input.Email?.ToLowerInvariant();

            // add user to the database
            return this.dataOps.Connect().QueryFirst("ExternalUser", "Create").ExecuteAsync<ExternalUserModel>(input);
        }
    }
}