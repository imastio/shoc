using System.Threading.Tasks;
using Shoc.ObjectAccess.Model.Job;

namespace Shoc.ObjectAccess.Job;

/// <summary>
/// Job access repository
/// </summary>
public interface IJobAccessRepository
{
    /// <summary>
    /// Gets the job access reference
    /// </summary>
    /// <param name="workspaceId">The workspace</param>
    /// <param name="id">The id</param>
    /// <returns></returns>
    Task<JobAccessReferenceModel> GetAccessReferenceById(string workspaceId, string id);
}