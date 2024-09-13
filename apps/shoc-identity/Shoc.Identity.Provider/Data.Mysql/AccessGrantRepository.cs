using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.OpenId;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The access grant repository
/// </summary>
public class AccessGrantRepository : IAccessGrantRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of access grant repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public AccessGrantRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all grants
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<AccessGrant>> GetAll()
    {
        return this.dataOps.Connect().Query("Identity.Grant", "GetAll").ExecuteAsync<AccessGrant>();
    }

    /// <summary>
    /// Gets all grants by given filter
    /// </summary>
    /// <param name="filter">The filter of grants</param>
    /// <returns></returns>
    public Task<IEnumerable<AccessGrant>> GetAllBy(AccessGrantFilter filter)
    {
        return this.dataOps.Connect().Query("Identity.Grant", "GetAllBy")
            .WithBinding("BySession", filter.SessionId != null)
            .WithBinding("ByClient", filter.ClientId != null)
            .WithBinding("BySubject", filter.SubjectId != null)
            .WithBinding("ByType", filter.Type != null)
            .ExecuteAsync<AccessGrant>(filter);
    }

    /// <summary>
    /// Gets the entity by identifier
    /// </summary>
    /// <param name="key">The entity key</param>
    /// <returns></returns>
    public Task<AccessGrant> GetByKey(string key)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Grant", "GetByKey").ExecuteAsync<AccessGrant>(new
        {
            Key = key
        });
    }

    /// <summary>
    /// Saves given entity 
    /// </summary>
    /// <param name="entity">The entity to save</param>
    /// <returns></returns>
    public Task<AccessGrant> Save(AccessGrant entity)
    {
        // give a new id
        entity.Id ??= StdIdGenerator.Next(IdentityObjects.ACCESS_GRANT);
        
        return this.dataOps.Connect().QueryFirst("Identity.Grant", "Save").ExecuteAsync<AccessGrant>(entity);
    }

    /// <summary>
    /// Deletes the entity by identifier
    /// </summary>
    /// <param name="key">The key entity</param>
    /// <returns></returns>
    public Task<AccessGrant> DeleteByKey(string key)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Grant", "DeleteByKey")
            .ExecuteAsync<AccessGrant>(new {Key = key});
    }

    /// <summary>
    /// Deletes all grants by given filter
    /// </summary>
    /// <param name="filter">The filter of grants</param>
    /// <returns></returns>
    public Task<int> DeleteAllBy(AccessGrantFilter filter)
    {
        return this.dataOps.Connect().NonQuery("Identity.Grant", "DeleteAllBy")
            .WithBinding("BySession", filter.SessionId != null)
            .WithBinding("ByClient", filter.ClientId != null)
            .WithBinding("BySubject", filter.SubjectId != null)
            .WithBinding("ByType", filter.Type != null)
            .ExecuteAsync(filter);
    }

    /// <summary>
    /// Deletes all entities
    /// </summary>
    /// <returns></returns>
    public Task<int> DeleteAll()
    {
        return this.dataOps.Connect().NonQuery("Identity.Grant", "DeleteAll").ExecuteAsync();
    }
}