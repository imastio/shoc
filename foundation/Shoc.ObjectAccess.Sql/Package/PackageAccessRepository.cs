using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.ObjectAccess.Model.Package;
using Shoc.ObjectAccess.Package;

namespace Shoc.ObjectAccess.Sql.Package;

/// <summary>
/// Package access repository
/// </summary>
public class PackageAccessRepository : IPackageAccessRepository
{
    /// <summary>
    /// The data ops reference
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of the repository
    /// </summary>
    /// <param name="dataOps">The data operations</param>
    public PackageAccessRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets the package access reference
    /// </summary>
    /// <param name="workspaceId">The workspace</param>
    /// <param name="id">The id</param>
    /// <returns></returns>
    public Task<PackageAccessReferenceModel> GetAccessReferenceById(string workspaceId, string id)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("PackageAccess", "GetAccessReferenceById").ExecuteAsync<PackageAccessReferenceModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        }));
    }
}