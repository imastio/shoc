using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.ObjectAccess.Job;
using Shoc.ObjectAccess.Model.Job;

namespace Shoc.ObjectAccess.Sql.Job;

/// <summary>
/// Job access repository
/// </summary>
public class JobAccessRepository : IJobAccessRepository
{
    /// <summary>
    /// The data ops reference
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of the repository
    /// </summary>
    /// <param name="dataOps">The data operations</param>
    public JobAccessRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets the job access reference
    /// </summary>
    /// <param name="workspaceId">The workspace</param>
    /// <param name="id">The id</param>
    /// <returns></returns>
    public Task<JobAccessReferenceModel> GetAccessReferenceById(string workspaceId, string id)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("JobAccess", "GetAccessReferenceById").ExecuteAsync<JobAccessReferenceModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        }));
    }
}