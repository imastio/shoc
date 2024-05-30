using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.Flow;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The OTP repository interface
/// </summary>
public interface IOtpRepository
{

    /// <summary>
    /// Gets all the one-time passwords associated with user
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    Task<IEnumerable<OneTimePassModel>> GetAll(string userId);

    /// <summary>
    /// Gets one-time password by link if exists
    /// </summary>
    /// <param name="link">The link fragment</param>
    /// <returns></returns>
    Task<OneTimePassModel> GetByLink(string link);

    /// <summary>
    /// Creates a one-time password
    /// </summary>
    /// <param name="input">The one-time password instance</param>
    /// <returns></returns>
    Task<OneTimePassModel> Create(OneTimePassModel input);

    /// <summary>
    /// Delete one-time passwords associated by id
    /// </summary>
    /// <param name="id">The id of OTP</param>
    /// <returns></returns>
    Task<int> DeleteById(string id);
}
