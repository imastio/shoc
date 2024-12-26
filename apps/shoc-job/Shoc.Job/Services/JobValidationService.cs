using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Job.Data;
using Shoc.Job.Model;
using Shoc.Job.Model.Job;

namespace Shoc.Job.Services;

/// <summary>
/// The job validation service
/// </summary>
public class JobValidationService : ValidationServiceBase
{
    /// <summary>
    /// Maximum allowed number of labels
    /// </summary>
    protected const int MAX_LABEL_REFERENCES = 10;
    
    /// <summary>
    /// The label repository
    /// </summary>
    protected readonly ILabelRepository labelRepository;

    /// <summary>
    /// The git repo repository
    /// </summary>
    protected readonly IGitRepoRepository gitRepoRepository;

    /// <summary>
    /// The validation service 
    /// </summary>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    /// <param name="labelRepository"></param>
    /// <param name="gitRepoRepository"></param>
    public JobValidationService(IGrpcClientProvider grpcClientProvider, ILabelRepository labelRepository, IGitRepoRepository gitRepoRepository) : base(grpcClientProvider)
    {
        this.labelRepository = labelRepository;
        this.gitRepoRepository = gitRepoRepository;
    }
    
    /// <summary>
    /// Validate object scope
    /// </summary>
    /// <param name="scope">The scope to validate</param>
    public virtual void ValidateScope(string scope)
    {
        // make sure valid status
        if (JobScopes.ALL.Contains(scope))
        {
            return;
        }

        throw ErrorDefinition.Validation(JobErrors.INVALID_JOB_SCOPE, "The scope is not valid").AsException();
    }

    /// <summary>
    /// Validate given set of label identifiers
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="labelIds">The label identifiers</param>
    /// <returns></returns>
    public async Task ValidateLabels(string workspaceId, ICollection<string> labelIds)
    {
        // empty list of labels is valid
        if (labelIds.Count == 0)
        {
            return;
        }

        // too many labels
        if (labelIds.Count > MAX_LABEL_REFERENCES)
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_JOB_LABELS_LIMIT, "Too many labels").AsException();
        }

        // count actual labels by given ids
        var actualCount = await this.labelRepository.CountByIds(workspaceId, labelIds);

        // counts not matching
        if (actualCount != labelIds.Count)
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_JOB_LABEL_REFERENCE, "Invalid labels referenced").AsException();
        }
    }
    
    /// <summary>
    /// Validate given git repository
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="gitRepoId">The git repo reference</param>
    /// <returns></returns>
    public async Task ValidateGitRepo(string workspaceId, string gitRepoId)
    {
        // git repo is not given
        if (gitRepoId == null)
        {
            return;
        }

        // check if empty string is given
        if (gitRepoId.Trim() == string.Empty)
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_JOB_GIT_REPO, "Empty git repository referenced").AsException();
        }

        // get existing object
        var existing = await this.gitRepoRepository.GetById(workspaceId, gitRepoId);

        // object does not exist
        if (existing == null)
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_JOB_GIT_REPO, "Invalid reference to git repository").AsException();
        }
    }
    
}