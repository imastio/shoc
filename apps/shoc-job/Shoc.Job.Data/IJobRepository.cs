using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Job.Model.Job;
using Shoc.Job.Model.JobTask;

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
    /// Gets the task objects by id
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<JobTaskModel>> GetTasksById(string workspaceId, string id);
    
    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<JobModel> Create(string workspaceId, JobCreateModel input);

    /// <summary>
    /// Updates the namespaces by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="ns">The namespace value</param>
    /// <returns></returns>
    Task<JobModel> UpdateNamespaceById(string workspaceId, string id, string ns);
    
    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<JobModel> DeleteById(string workspaceId, string id);
}