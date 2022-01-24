using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;

namespace Shoc.Identity.Data.Sql
{
    /// <summary>
    /// The confirmation code implementation
    /// </summary>
    public class ConfirmationCodeRepository : IConfirmationCodeRepository
    {
        /// <summary>
        /// The data operations instance
        /// </summary>
        private readonly DataOperations dataOps;

        /// <summary>
        /// Creates new instance of users repository implementation
        /// </summary>
        /// <param name="dataOps">A DataOps instance</param>
        public ConfirmationCodeRepository(DataOperations dataOps)
        {
            this.dataOps = dataOps;
        }
        
        /// <summary>
        /// Gets active confirmations
        /// </summary>
        /// <param name="email">The email</param>
        /// <returns></returns>
        public Task<IEnumerable<ConfirmationCode>> GetByEmail(string email)
        {
            return this.dataOps.Connect().Query("Confirmation", "GetByEmail").ExecuteAsync<ConfirmationCode>(new
            {
                Email = email
            });
        }

        /// <summary>
        /// Gets the confirmation code by link
        /// </summary>
        /// <param name="link">The link fragment</param>
        /// <returns></returns>
        public Task<ConfirmationCode> GetByLink(string link)
        {
            return this.dataOps.Connect().QueryFirst("Confirmation", "GetByLink").ExecuteAsync<ConfirmationCode>(new
            {
                Link = link
            });
        }

        /// <summary>
        /// Create a confirmation code
        /// </summary>
        /// <param name="input">The code input</param>
        /// <returns></returns>
        public Task<ConfirmationCode> Create(ConfirmationCode input)
        {
            // generate id if necessary
            input.Id ??= StdIdGenerator.Next(IdentityObjects.CNF)?.ToLowerInvariant();

            return this.dataOps.Connect().QueryFirst("Confirmation", "Create").ExecuteAsync<ConfirmationCode>(input);
        }

        /// <summary>
        /// Delete confirmation code associated by id
        /// </summary>
        /// <param name="id">The id of code</param>
        /// <returns></returns>
        public Task<int> DeleteById(string id)
        {
            return this.dataOps.Connect().NonQuery("Confirmation", "DeleteById").ExecuteAsync(new
            {
                Id = id
            });
        }

        /// <summary>
        /// Delete confirmation codes for the email
        /// </summary>
        /// <param name="email">The target to confirm</param>
        /// <returns></returns>
        public Task<int> DeleteByEmail(string email)
        {
            return this.dataOps.Connect().NonQuery("Confirmation", "DeleteByEmail").ExecuteAsync(new
            {
                Email = email
            });
        }
    }
}