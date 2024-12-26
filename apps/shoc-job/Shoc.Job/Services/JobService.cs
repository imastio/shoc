using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Job.Data;
using Shoc.Job.Model;
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

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<JobModel> Create(string workspaceId, JobSubmissionInput input)
    {
        // the parent object
        input.WorkspaceId = workspaceId;
        
        // require the parent object
        await this.validationService.RequireWorkspace(input.WorkspaceId);

        // require valid user
        await this.validationService.RequireUser(input.UserId);
        
        // validate the scope
        this.validationService.ValidateScope(input.Scope);

        // manifest is missing
        if (input.Manifest == null)
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_JOB_MANIFEST).AsException();
        }

        // get distinct labels
        var labelIds = input.Manifest.LabelIds?.Distinct().ToList() ?? [];

        // validate referenced labels
        await this.validationService.ValidateLabels(input.WorkspaceId, labelIds);

        // the git repo referenced
        var gitRepoId = input.Manifest.GitRepoId;
        
        // validate referenced git repository
        await this.validationService.ValidateGitRepo(input.WorkspaceId, gitRepoId);

        // TODO: continue validation 
        return null;
    }
}