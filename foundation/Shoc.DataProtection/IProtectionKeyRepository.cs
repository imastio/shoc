using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoc.DataProtection;

/// <summary>
/// The protection key repository interface
/// </summary>
public interface IProtectionKeyRepository
{
    /// <summary>
    /// Gets all the protection keys
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ProtectionKeyModel>> GetAll();

    /// <summary>
    /// Saves the model
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<int> Create(ProtectionKeyModel input);
}
