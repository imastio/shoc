using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The signing key repository interface
/// </summary>
public interface ISigningKeyRepository
{
    /// <summary>
    /// Get all keys by use indicator
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<SigningKey>> GetBy(string use);
        
    /// <summary>
    /// Get all keys by id and use indicator
    /// </summary>
    /// <returns></returns>
    Task<SigningKey> GetById(string id);

    /// <summary>
    /// Stores the key in the storage
    /// </summary>
    /// <param name="key">The key to store</param>
    /// <returns></returns>
    Task<SigningKey> Create(SigningKey key);

    /// <summary>
    /// Delete the key by given id and use
    /// </summary>
    /// <param name="id">The key id</param>
    /// <returns></returns>
    Task<SigningKey> DeleteById(string id);
}