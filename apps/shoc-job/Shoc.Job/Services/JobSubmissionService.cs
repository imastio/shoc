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
using Shoc.Secret.Grpc.Secrets;

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
    protected const string DEFAULT_JOB_ARRAY_COUNT = "SHOC_JOB_ARRAY_COUNT";

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
    /// The secret resolver
    /// </summary>
    protected readonly JobSecretResolver secretResolver;

    /// <summary>
    /// The resource parser
    /// </summary>
    protected readonly ResourceParser resourceParser;

    /// <summary>
    /// Creates new instance of job submission service
    /// </summary>
    /// <param name="jobRepository">The job repository</param>
    /// <param name="validationService">The validation service</param>
    /// <param name="jobProtectionProvider">The job protection provider</param>
    /// <param name="packageResolver">The package resolver</param>
    /// <param name="clusterResolver">The cluster resolver</param>
    /// <param name="secretResolver">The secret resolver</param>
    /// <param name="resourceParser">The resource parser</param>
    public JobSubmissionService(IJobRepository jobRepository, JobValidationService validationService, JobProtectionProvider jobProtectionProvider, JobPackageResolver packageResolver, JobClusterResolver clusterResolver, JobSecretResolver secretResolver, ResourceParser resourceParser) 
        : base(jobRepository, validationService, jobProtectionProvider)
    {
        this.packageResolver = packageResolver;
        this.clusterResolver = clusterResolver;
        this.secretResolver = secretResolver;
        this.resourceParser = resourceParser;
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
        var result = await this.jobRepository.GetById(workspaceId, id);

        // ensure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
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

        // the protection provider
        var protector = this.jobProtectionProvider.Create();
        
        // parse the given resources
        var resources = ParseResources(input.Manifest.Resources);
        
        // validate resources
        this.validationService.ValidateResources(resources);
        
        // build wrapped arguments model
        var args = new JobTaskArgsModel
        {
            Args = input.Manifest.Args
        };
        
        // arguments in json
        var argsJson = ToJsonString(args);
        
        // resolve the referenced package
        var package = await this.packageResolver.ResolveById(input.WorkspaceId, input.Manifest.PackageId);

        // deserialize the runtime model
        var runtime = DeserializeRuntime(package.Runtime);

        // the runtime as json back serialized
        var runtimeJson = ToJsonString(runtime);
        
        // gets the user's pull credentials for the package's registry within the workspace
        var credentials = await this.packageResolver.GetPullCredential(package.RegistryId, input.WorkspaceId, input.UserId);
        
        // build package reference
        var packageReference = new JobTaskPackageReferenceModel
        {
            Image = package.Image,
            PullUsername = credentials.Username,
            PullPasswordPlain = credentials.PasswordPlain
        };
        
        // the package reference json
        var packageReferenceJson = ToJsonString(packageReference);
        var packageReferenceJsonProtected = protector.Protect(packageReferenceJson);
        
        // resolve the cluster with proper validation
        var cluster = await this.clusterResolver.ResolveById(input.WorkspaceId, input.Manifest.ClusterId);

        // combined set of decrypted environment key values
        var env = await this.ResolveEnvironment(input);
        
        // the resolved env json
        var resolvedEnvJson = ToJsonString(env);
        var resolvedEnvJsonProtected = protector.Protect(resolvedEnvJson);
        
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
        
        // the spec as json
        var specJson = ToJsonString(input.Manifest.Spec);
        
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
                Runtime = runtimeJson,
                Args = argsJson,
                PackageReferenceEncrypted = packageReferenceJsonProtected,
                ArrayReplicas = job.TotalTasks,
                ArrayIndexer = input.Manifest.Array.Indexer,
                ArrayCounter = input.Manifest.Array.Counter,
                ResolvedEnvEncrypted = resolvedEnvJsonProtected,
                MemoryRequested = resources.Memory,
                CpuRequested = resources.Cpu,
                NvidiaGpuRequested = resources.NvidiaGpu,
                AmdGpuRequested = resources.AmdGpu,
                Spec = specJson,
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
    /// Submit the created job to the target cluster
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The job id</param>
    /// <returns></returns>
    public async Task<JobModel> Submit(string userId, string workspaceId, string id)
    {
        // get the job instance
        var job = await this.GetById(workspaceId, id);

        // the user initiating the job is not matching the submitting user
        if (!string.Equals(job.UserId, userId))
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_USER, "The submitting user does not match the invoking user").AsException();
        }

        // ensure only job is just created
        if (job.Status != JobStatuses.CREATED)
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_STATUS, "The job was already submitted").AsException();
        }

        // load the tasks associated with the job
        var tasks = (await this.jobRepository.GetTasksById(workspaceId, id)).OrderBy(task => task.Sequence).ToList();

        // check if number of existing tasks matches the jobs total tasks
        if (tasks.Count != job.TotalTasks)
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_TASKS, "The job tasks are not valid").AsException();
        }

        // check if there is any task not in the created state
        if (tasks.Any(task => task.Status != JobTaskStatuses.CREATED))
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_STATUS, "At least one task has been already submitted").AsException();
        }
        
        tasks[0].
    }

    /// <summary>
    /// Resolves the environment for submission
    /// </summary>
    /// <param name="input">The input</param>
    /// <returns></returns>
    private async Task<JobTaskEnvModel> ResolveEnvironment(JobSubmissionInput input)
    {
        // get the unique set of names
        var names = input.Manifest.Env.Use.ToHashSet();
        
        // do not consider names of secrets that are subject of overriding 
        names.ExceptWith(input.Manifest.Env.Override.Keys);

        // get the items 
        var items = (await this.secretResolver.ResolveByNames(input.WorkspaceId, input.UserId, names)).ToList();

        // separate workspace secrets
        var workspaceSecrets = items.Where(item => item.Kind == UnifiedSecretKind.Workspace && !item.Disabled).ToList();

        // separate user secrets 
        var userSecrets = items.Where(item => item.Kind == UnifiedSecretKind.User && !item.Disabled).ToList();

        // prepare sets 
        var plain = new Dictionary<string, string>();
        var encrypted = new Dictionary<string, string>();
        
        // add workspaces values first
        foreach (var secret in workspaceSecrets)
        {
            // the target collection
            var target = secret.Encrypted ? encrypted : plain;
            
            // add to the collection
            target[secret.Name] = secret.Value;
        }
        
        // then add user values to override workspace values if any
        foreach (var secret in userSecrets)
        {
            // the target collection
            var target = secret.Encrypted ? encrypted : plain;
            
            // add to the collection
            target[secret.Name] = secret.Value;
        }
        
        // add overriding values to plain values
        foreach (var pairs in input.Manifest.Env.Override)
        {
            plain[pairs.Key] = pairs.Value;
        }
        
        // build result
        return new JobTaskEnvModel
        {
            Plain = plain,
            Encrypted = encrypted
        };
    }

    /// <summary>
    /// Parses the given resource strings into valid resource quantities
    /// </summary>
    /// <param name="resources">The resources to parse</param>
    /// <returns></returns>
    private JobTaskResourcesModel ParseResources(JobRunManifestResourcesModel resources)
    {
        return new JobTaskResourcesModel
        {
            Cpu = this.resourceParser.ParseToMillicores(resources?.Cpu),
            Memory = this.resourceParser.ParseToBytes(resources?.Memory),
            NvidiaGpu = this.resourceParser.ParseToGpu(resources?.NvidiaGpu),
            AmdGpu = this.resourceParser.ParseToGpu(resources?.AmdGpu)
        };
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