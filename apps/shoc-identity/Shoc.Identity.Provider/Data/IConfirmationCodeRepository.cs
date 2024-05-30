using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.Flow;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The confirmation code repository interface
/// </summary>
public interface IConfirmationCodeRepository
{
    /// <summary>
    /// Gets active confirmations
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ConfirmationCodeModel>> GetAll(string target, string type);

    /// <summary>
    /// Gets the confirmation code by link
    /// </summary>
    /// <param name="link">The link fragment</param>
    /// <returns></returns>
    Task<ConfirmationCodeModel> GetByLink(string link);

    /// <summary>
    /// Create a confirmation code
    /// </summary>
    /// <param name="input">The code input</param>
    /// <returns></returns>
    Task<ConfirmationCodeModel> Create(ConfirmationCodeModel input);

    /// <summary>
    /// Delete confirmation code associated by id
    /// </summary>
    /// <param name="id">The id of code</param>
    /// <returns></returns>
    Task<int> DeleteById(string id);

    /// <summary>
    /// Delete confirmation codes
    /// </summary>
    /// <param name="target">The target to confirm</param>
    /// <param name="type">The target type</param>
    /// <returns></returns>
    Task<int> DeleteAll(string target, string type);
}
