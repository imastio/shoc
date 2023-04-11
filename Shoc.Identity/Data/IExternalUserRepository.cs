using System.Threading.Tasks;
using Shoc.Identity.Model;

namespace Shoc.Identity.Data
{
    /// <summary>
    /// The external users repository
    /// </summary>
    public interface IExternalUserRepository
    {
        /// <summary>
        /// Gets the external user by email and provider
        /// </summary>
        /// <param name="email">The user email</param>
        /// <param name="provider">The external provider name</param>
        /// <returns></returns>
        Task<ExternalUserModel> GetByEmailAndProvider(string email, string provider);

        /// <summary>
        /// Creates entity based on input
        /// </summary>
        /// <param name="input">The creation input</param>
        /// <returns></returns>
        Task<ExternalUserModel> Create(CreateExternalUserModel input);
    }
}