using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.OpenId;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The access grant repository interface
/// </summary>
public interface IAccessGrantRepository
{
    /// <summary>
    /// Gets all grants
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<AccessGrant>> GetAll();

    /// <summary>
    /// Gets all grants by given filter
    /// </summary>
    /// <param name="filter">The filter of grants</param>
    /// <returns></returns>
    Task<IEnumerable<AccessGrant>> GetAllBy(AccessGrantFilter filter);

    /// <summary>
    /// Gets the entity by identifier
    /// </summary>
    /// <param name="key">The entity key</param>
    /// <returns></returns>
    Task<AccessGrant> GetByKey(string key);

    /// <summary>
    /// Saves given entity 
    /// </summary>
    /// <param name="entity">The entity to save</param>
    /// <returns></returns>
    Task<AccessGrant> Save(AccessGrant entity);

    /// <summary>
    /// Deletes the entity by identifier
    /// </summary>
    /// <param name="key">The key entity</param>
    /// <returns></returns>
    Task<AccessGrant> DeleteByKey(string key);

    /// <summary>
    /// Deletes all grants by given filter
    /// </summary>
    /// <param name="filter">The filter of grants</param>
    /// <returns></returns>
    Task<int> DeleteAllBy(AccessGrantFilter filter);

    /// <summary>
    /// Deletes all entities
    /// </summary>
    /// <returns></returns>
    Task<int> DeleteAll();
}