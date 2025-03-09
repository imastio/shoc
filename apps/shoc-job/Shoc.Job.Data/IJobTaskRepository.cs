using System.Threading.Tasks;
using Shoc.Job.Model.JobTask;

namespace Shoc.Job.Data;

/// <summary>
/// The repository interface
/// </summary>
public interface IJobTaskRepository
{
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    Task<JobTaskModel> GetById(string workspaceId, string jobId, string id);
}