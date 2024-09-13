using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Shoc.Identity.Model;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Provider.Stores;

/// <summary>
/// The signin key store implementation
/// </summary>
public class SigningKeyStore : ISigningKeyStore
{
    /// <summary>
    /// The key usage for 'signing'
    /// </summary>
    private const string USE = "signing";
    
    /// <summary>
    /// The key repository
    /// </summary>
    private readonly ISigningKeyRepository signingKeyRepository;

    /// <summary>
    /// Creates new instance for signing key store
    /// </summary>
    /// <param name="signingKeyRepository">The signing key repository</param>
    public SigningKeyStore(ISigningKeyRepository signingKeyRepository)
    {
        this.signingKeyRepository = signingKeyRepository;
    }

    /// <summary>
    /// Returns all the keys in storage.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<SerializedKey>> LoadKeysAsync()
    {
        // get items by use
        var items = await this.signingKeyRepository.GetBy(USE);

        // return mapped entities
        return items.Select(MapFromModel);
    }
    
    /// <summary>
    /// Persists new key in storage.
    /// </summary>
    /// <param name="key">The key to store</param>
    /// <returns></returns>
    public Task StoreKeyAsync(SerializedKey key)
    {
        return this.signingKeyRepository.Create(new SigningKey
        {
            Id = key.Id,
            Algorithm = key.Algorithm,
            Use = USE,
            Data = key.Data,
            DataProtected = key.DataProtected,
            IsX509Certificate = key.IsX509Certificate,
            Version = key.Version,
            Created = key.Created
        });
    }

    /// <summary>
    /// Deletes key from storage.
    /// </summary>
    /// <param name="id">The id of key</param>
    /// <returns></returns>
    public async Task DeleteKeyAsync(string id)
    {
        // try get by id
        var existing = await this.signingKeyRepository.GetById(id);

        // if does not exist or not the required use just skip
        if (existing == null || !string.Equals(existing.Use, USE))
        {
            return;
        }

        // just delete if key is found
        await this.signingKeyRepository.DeleteById(id);
    }
    
    /// <summary>
    /// Maps signing key to serialized key
    /// </summary>
    /// <param name="key">The key to map</param>
    /// <returns></returns>
    private static SerializedKey MapFromModel(SigningKey key)
    {
        // handle if absent
        if (key == null)
        {
            return null;
        }

        // map the key
        return new SerializedKey
        {
            Id = key.Id,
            Algorithm = key.Algorithm,
            Data = key.Data,
            DataProtected = key.DataProtected,
            IsX509Certificate = key.IsX509Certificate,
            Version = key.Version,
            Created = key.Created
        };
    }
}
