using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Secret.Model.UnifiedSecret;

namespace Shoc.Secret.Data.Sql;

/// <summary>
/// The repository implementation
/// </summary>
public class UnifiedSecretRepository : IUnifiedSecretRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public UnifiedSecretRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets objects by names
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<UnifiedSecretModel>> GetByNames(string workspaceId, string userId, IEnumerable<string> names)
    {
        return this.dataOps.Connect().Query("Secret.UnifiedSecret", "GetByNames").ExecuteAsync<UnifiedSecretModel>(new
        {
            WorkspaceId = workspaceId,
            UserId = userId,
            Names = names
        });
    }
}