using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Job.Data;
using Shoc.Job.Model.Job;

namespace Shoc.Job.Services;

/// <summary>
/// The job service
/// </summary>
public class JobService : JobServiceBase
{
    /// <summary>
    /// Creates new instance of the job service
    /// </summary>
    /// <param name="jobRepository">The job repository</param>
    /// <param name="validationService">The validation service</param>
    /// <param name="jobProtectionProvider">The protection provider</param>
    public JobService(IJobRepository jobRepository, JobValidationService validationService, JobProtectionProvider jobProtectionProvider) 
        : base(jobRepository, validationService, jobProtectionProvider)
    {
    }
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<JobModel>> GetAll(string workspaceId)
    {
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);

        // get from the storage
        return await this.jobRepository.GetAll(workspaceId);
    }
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public async Task<JobModel> GetById(string workspaceId, string id)
    {
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);

        // get from the storage
        return await this.jobRepository.GetById(workspaceId, id);
    }
}