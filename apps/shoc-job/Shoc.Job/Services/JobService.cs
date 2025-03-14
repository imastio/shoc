using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Job.Data;
using Shoc.Job.Model.Job;

namespace Shoc.Job.Services;

/// <summary>
/// The job service
/// </summary>
public class JobService : JobServiceBase
{
    /// <summary>
    /// The default page size
    /// </summary>
    private const int DEFAULT_PAGE_SIZE = 20;
    
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
    /// Gets page of objects by filter
    /// </summary>
    /// <returns></returns>
    public async Task<JobPageResult<JobExtendedModel>> GetExtendedPageBy(string workspaceId, JobFilter filter, int page, int? size)
    {
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);

        // get from the storage
        return await this.jobRepository.GetExtendedPageBy(workspaceId, filter, Math.Abs(page), Math.Abs(size ?? DEFAULT_PAGE_SIZE));
    }
        
    /// <summary>
    /// Gets the extended object by id
    /// </summary>
    /// <returns></returns>
    public async Task<JobExtendedModel> GetExtendedById(string workspaceId, string id)
    {
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);

        // try load the object
        var result = await this.jobRepository.GetExtendedById(workspaceId, id);

        // check if object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        // return result
        return result;
    }
}