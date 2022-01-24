using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model;

namespace Shoc.Identity.Data
{
    /// <summary>
    /// The confirmation code repository
    /// </summary>
    public interface IConfirmationCodeRepository
    {
        /// <summary>
        /// Gets active confirmations
        /// </summary>
        /// <param name="email">The email</param>
        /// <returns></returns>
        Task<IEnumerable<ConfirmationCode>> GetByEmail(string email);

        /// <summary>
        /// Gets the confirmation code by link
        /// </summary>
        /// <param name="link">The link fragment</param>
        /// <returns></returns>
        Task<ConfirmationCode> GetByLink(string link);

        /// <summary>
        /// Create a confirmation code
        /// </summary>
        /// <param name="input">The code input</param>
        /// <returns></returns>
        Task<ConfirmationCode> Create(ConfirmationCode input);

        /// <summary>
        /// Delete confirmation code associated by id
        /// </summary>
        /// <param name="id">The id of code</param>
        /// <returns></returns>
        Task<int> DeleteById(string id);

        /// <summary>
        /// Delete confirmation codes for the email
        /// </summary>
        /// <param name="email">The target to confirm</param>
        /// <returns></returns>
        Task<int> DeleteByEmail(string email);
    }
}