using System.Collections.Generic;
using System.Linq;
using Duende.IdentityServer.Models;

namespace Shoc.Identity.Provider.Stores;

/// <summary>
/// The in memory clients provider
/// </summary>
public class InMemoryClientsProvider
{
    /// <summary>
    /// The storage for clients
    /// </summary>
    private readonly Dictionary<string, Client> inMemory;

    /// <summary>
    /// Creates new instance of in memory clients provider
    /// </summary>
    /// <param name="inMemory">The in memory clients provider</param>
    public InMemoryClientsProvider(IEnumerable<Client> inMemory)
    {
        this.inMemory = inMemory?.ToDictionary(client => client.ClientId, client => client) ?? new Dictionary<string, Client>();
    }

    /// <summary>
    /// Gets all the in memory clients
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, Client> GetAll()
    {
        return this.inMemory;
    }

    /// <summary>
    /// Try get the client from the provider
    /// </summary>
    /// <param name="id">The key of client</param>
    /// <param name="client">The client</param>
    /// <returns></returns>
    public bool TryGet(string id, out Client client)
    {
        return this.inMemory.TryGetValue(id, out client);
    }
}
