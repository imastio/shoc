using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Access.Model;

namespace Shoc.Access.Data.Sql;

/// <summary>
/// The access repository implementation
/// </summary>
public class AccessRepository : IAccessRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of access grant repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public AccessRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets the principal access definitions
    /// </summary>
    /// <param name="userId">The user identity</param>
    /// <returns></returns>
    public Task<IEnumerable<AccessValueModel>> GetEffectiveByUser(string userId)
    {
        return this.dataOps.Connect().Query("Access", "GetEffectiveByUser").ExecuteAsync<AccessValueModel>(new
        {
            UserId = userId
        });

    }
}