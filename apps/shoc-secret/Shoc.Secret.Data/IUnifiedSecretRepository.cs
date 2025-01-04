using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Secret.Model.UnifiedSecret;

namespace Shoc.Secret.Data;

/// <summary>
/// The repository interface
/// </summary>
public interface IUnifiedSecretRepository
{
    /// <summary>
    /// Gets objects by names
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<UnifiedSecretModel>> GetByNames(string workspaceId, string userId, IEnumerable<string> names);
}