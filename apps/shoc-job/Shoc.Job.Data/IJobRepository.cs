using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Job.Model.Job;

namespace Shoc.Job.Data;

/// <summary>
/// The repository interface
/// </summary>
public interface IJobRepository
{
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<JobModel>> GetAll(string workspaceId);
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    Task<JobModel> GetById(string workspaceId, string id);
    
    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<JobModel> Create(string workspaceId, JobCreateModel input);
    
    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<JobModel> DeleteById(string workspaceId, string id);
}