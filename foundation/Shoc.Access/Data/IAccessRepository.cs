using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Access.Model;

namespace Shoc.Access.Data;

/// <summary>
/// The access repository
/// </summary>
public interface IAccessRepository
{
    /// <summary>
    /// Gets the principal access definitions
    /// </summary>
    /// <param name="userId">The user identity</param>
    /// <returns></returns>
    Task<IEnumerable<AccessValueModel>> GetEffectiveByUser(string userId);
}
