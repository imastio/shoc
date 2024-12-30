using System.Threading.Tasks;
using Shoc.ObjectAccess.Model.Package;

namespace Shoc.ObjectAccess.Package;

/// <summary>
/// Package access repository
/// </summary>
public interface IPackageAccessRepository
{
    /// <summary>
    /// Gets the package access reference
    /// </summary>
    /// <param name="workspaceId">The workspace</param>
    /// <param name="id">The id</param>
    /// <returns></returns>
    Task<PackageAccessReferenceModel> GetAccessReferenceById(string workspaceId, string id);
}