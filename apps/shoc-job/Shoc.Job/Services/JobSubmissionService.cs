using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Shoc.Core;
using Shoc.Job.Data;
using Shoc.Job.Model;
using Shoc.Job.Model.Job;
using Shoc.Job.Model.JobGitRepo;
using Shoc.Job.Model.JobLabel;
using Shoc.Job.Model.JobTask;

namespace Shoc.Job.Services;

/// <summary>
/// The job submission service
/// </summary>
public class JobSubmissionService : JobServiceBase
{
    /// <summary>
    /// The default indexer for job array
    /// </summary>
    protected const string DEFAULT_JOB_ARRAY_INDEXER = "SHOC_JOB_ARRAY_INDEXER";

    /// <summary>
    /// The default job array count
    /// </summary>
    protected const string DEFAULT_JOB_ARRAY_COUNT = "SHOC_JOB_ARRAY_INDEXER";

    /// <summary>
    /// The default number of replicas in job array
    /// </summary>
    protected const int DEFAULT_JOB_ARRAY_REPLICAS = 1;

    /// <summary>
    /// The package resolver
    /// </summary>
    protected readonly JobPackageResolver packageResolver;

    /// <summary>
    /// The cluster resolver
    /// </summary>
    protected readonly JobClusterResolver clusterResolver;

    /// <summary>
    /// Creates new instance of job submission service
    /// </summary>
    /// <param name="jobRepository">The job repository</param>
    /// <param name="validationService">The validation service</param>
    /// <param name="jobProtectionProvider">The job protection provider</param>
    /// <param name="packageResolver">The package resolver</param>
    /// <param name="clusterResolver">The cluster resolver</param>
    public JobSubmissionService(IJobRepository jobRepository, JobValidationService validationService, JobProtectionProvider jobProtectionProvider, JobPackageResolver packageResolver, JobClusterResolver clusterResolver) 
        : base(jobRepository, validationService, jobProtectionProvider)
    {
        this.packageResolver = packageResolver;
        this.clusterResolver = clusterResolver;
    }
    
    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<JobModel> Create(string workspaceId, JobSubmissionInput input)
    {
        // enrich with defaults
        input = BuildInputWithDefaults(workspaceId, input);

        // validate the given input
        await this.ValidateInput(input);

        // build wrapped arguments model
        var args = new JobTaskArgsModel
        {
            Args = input.Manifest.Args
        };
        
        // resolve the referenced package
        var package = await this.packageResolver.ResolveById(input.WorkspaceId, input.Manifest.PackageId);

        // deserialize the runtime model
        var runtime = DeserializeRuntime(package.Runtime);
        
        // gets the user's pull credentials for the package's registry within the workspace
        var credentials = await this.packageResolver.GetPullCredential(package.RegistryId, input.WorkspaceId, input.UserId);
        
        // build package reference
        var packageReference = new JobTaskPackageReferenceModel
        {
            Image = package.Image,
            PullUsername = credentials.Username,
            PullPasswordPlain = credentials.PasswordPlain
        };
        
        // resolve the cluster with proper validation
        var cluster = await this.clusterResolver.ResolveById(input.WorkspaceId, input.Manifest.ClusterId);

        // combined set of decrypted environment key values
        // TODO: replace with resolved secret values
        var env = new JobTaskEnvModel
        {
            Env = new Dictionary<string, string>()
        };
        
        // the protection provider
        var protector = this.jobProtectionProvider.Create();
        
        // create a job instance
        var job = new JobModel
        {
            WorkspaceId = input.WorkspaceId,
            ClusterId = cluster.Id,
            UserId = input.UserId,
            Scope = input.Scope,
            Manifest = ToJsonString(input.Manifest),
            ClusterConfigEncrypted = protector.Protect(cluster.Configuration),
            TotalTasks = input.Manifest.Array.Replicas.GetValueOrDefault(DEFAULT_JOB_ARRAY_REPLICAS),
            CompletedTasks = 0,
            Status = JobStatuses.CREATED,
            Message = string.Empty,
            PendingAt = null,
            RunningAt = null,
            CompletedAt = null
        };

        // a set of tasks
        var tasks = new List<JobTaskModel>();

        // create tasks corresponding to number of replicas
        for (var i = 0; i < job.TotalTasks; ++i)
        {
            tasks.Add(new JobTaskModel
            {
                WorkspaceId = input.WorkspaceId,
                ClusterId = input.Manifest.ClusterId,
                PackageId = input.Manifest.PackageId,
                UserId = input.UserId,
                Type = MapTaskType(runtime.Type),
                Runtime = ToJsonString(runtime),
                Args = ToJsonString(args),
                PackageReferenceEncrypted = protector.Protect(ToJsonString(packageReference)),
                ArrayReplicas = job.TotalTasks,
                ArrayIndexer = input.Manifest.Array.Indexer,
                ArrayCounter = input.Manifest.Array.Counter,
                ResolvedEnvEncrypted = protector.Protect(ToJsonString(env)),
                MemoryRequested = input.Manifest.Resources.Memory,
                CpuRequested = input.Manifest.Resources.Cpu,
                NvidiaGpuRequested = input.Manifest.Resources.NvidiaGpu,
                AmdGpuRequested = input.Manifest.Resources.AmdGpu,
                Spec = ToJsonString(input.Manifest.Spec),
                Status = JobTaskStatuses.CREATED,
                Message = string.Empty,
                PendingAt = null,
                RunningAt = null,
                CompletedAt = null
            });
        }

        // build job and label associations for every job 
        var labels = input.Manifest
            .LabelIds
            .Select(labelId => new JobLabelModel { WorkspaceId = input.WorkspaceId, LabelId = labelId })
            .ToList();

        // git repo association instance if given
        var gitRepo = string.IsNullOrWhiteSpace(input.Manifest.GitRepoId) ? null : new JobGitRepoModel
        {
            WorkspaceId = input.WorkspaceId,
            GitRepoId = input.Manifest.GitRepoId
        };

        // returns the created object
        return await this.jobRepository.Create(input.WorkspaceId, new JobCreateModel
        {
            Job = job,
            Tasks = tasks,
            Labels = labels,
            GitRepo = gitRepo
        });
    }

    /// <summary>
    /// Maps the runtime type into a task type
    /// </summary>
    /// <param name="runtimeType">The runtime type</param>
    /// <returns></returns>
    private static string MapTaskType(string runtimeType)
    {
        return runtimeType switch
        {
            "function" => JobTaskTypes.FUNCTION,
            "mpi" => JobTaskTypes.MPI,
            _ => throw ErrorDefinition.Validation(JobErrors.INVALID_RUNTIME_TYPE, $"The {runtimeType} is not valid runtime type").AsException()
        };
    }

    /// <summary>
    /// Performs the initial validation of given input
    /// </summary>
    /// <param name="input">The input to validate</param>
    private async Task ValidateInput(JobSubmissionInput input)
    {
        // validate the scope
        this.validationService.ValidateScope(input.Scope);
        
        // validate arguments
        this.validationService.ValidateArgs(input.Manifest.Args);
        
        // validate array of job
        this.validationService.ValidateArray(input.Manifest.Array);
        
        // validate resources
        this.validationService.ValidateResources(input.Manifest.Resources);
        
        // validate environment
        this.validationService.ValidateEnv(input.Manifest.Env);
        
        // require the parent object
        await this.validationService.RequireWorkspace(input.WorkspaceId);

        // require valid user
        await this.validationService.RequireUser(input.UserId);
        
        // validate referenced labels
        await this.validationService.ValidateLabels(input.WorkspaceId, input.Manifest.LabelIds);

        // validate referenced git repository
        await this.validationService.ValidateGitRepo(input.WorkspaceId, input.Manifest.GitRepoId);
    }
    
    /// <summary>
    /// Builds a submission input object enriched with default values 
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The input object</param>
    /// <returns></returns>
    private static JobSubmissionInput BuildInputWithDefaults(string workspaceId, JobSubmissionInput input)
    {
        // clone the object not to mutate
        input = JsonSerializer.Deserialize<JobSubmissionInput>(JsonSerializer.Serialize(input));

        // initialize parent object
        input.WorkspaceId = workspaceId;
        input.Scope ??= JobScopes.USER;

        // initialize with default if missing
        input.Manifest ??= new JobRunManifestModel();
        input.Manifest.Args ??= [];
        input.Manifest.Array ??= new JobRunManifestArrayModel();
        input.Manifest.LabelIds ??= [];
        input.Manifest.Env ??= new JobRunManifestEnvModel();
        input.Manifest.Env.Use ??= [];
        input.Manifest.Env.Override ??= new Dictionary<string, string>();
        input.Manifest.Spec ??= new JobRunManifestSpecModel();
        input.Manifest.Resources ??= new JobRunManifestResourcesModel();
        
        // perform basic transformations and apply defaults
        input.Manifest.LabelIds = input.Manifest.LabelIds.Distinct().ToArray();
        input.Manifest.Array.Indexer ??= DEFAULT_JOB_ARRAY_INDEXER;
        input.Manifest.Array.Counter ??= DEFAULT_JOB_ARRAY_COUNT;
        input.Manifest.Array.Replicas ??= DEFAULT_JOB_ARRAY_REPLICAS;
        
        return input;
    }

    /// <summary>
    /// Deserialize the package runtime into an executable model
    /// </summary>
    /// <param name="runtime">The runtime in json</param>
    /// <returns></returns>
    private static JobTaskRuntimeModel DeserializeRuntime(string runtime)
    {
        try
        {
            // deserialize the runtime into a proper model
            return FromJsonString<JobTaskRuntimeModel>(runtime);
        }
        catch (Exception e)
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_PACKAGE_RUNTIME, $"The package runtime is not valid: {e.Message}").AsException();
        }
    }
}