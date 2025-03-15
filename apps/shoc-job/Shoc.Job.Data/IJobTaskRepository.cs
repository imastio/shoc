using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Job.Model.JobTask;

namespace Shoc.Job.Data;

/// <summary>
/// The repository interface
/// </summary>
public interface IJobTaskRepository
{
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<JobTaskModel>> GetAll(string workspaceId, string jobId);
    
    /// <summary>
    /// Gets all the extended objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<JobTaskExtendedModel>> GetAllExtended(string workspaceId, string jobId);
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    Task<JobTaskModel> GetById(string workspaceId, string jobId, string id);
    
    /// <summary>
    /// Gets the extended object by id
    /// </summary>
    /// <returns></returns>
    Task<JobTaskExtendedModel> GetExtendedById(string workspaceId, string jobId, string id);
}