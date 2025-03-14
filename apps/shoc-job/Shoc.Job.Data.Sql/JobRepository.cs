using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Job.Model;
using Shoc.Job.Model.Job;
using Shoc.Job.Model.JobGitRepo;
using Shoc.Job.Model.JobLabel;
using Shoc.Job.Model.JobTask;

namespace Shoc.Job.Data.Sql;

/// <summary>
/// The repository interface implementation
/// </summary>
public class JobRepository : IJobRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public JobRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<JobModel>> GetAll(string workspaceId)
    {
        return this.dataOps.Connect().Query("Job", "GetAll").ExecuteAsync<JobModel>(new
        {
            WorkspaceId = workspaceId
        });
    }

    /// <summary>
    /// Gets page of the extended objects
    /// </summary>
    /// <returns></returns>
    public async Task<JobPageResult<JobExtendedModel>> GetExtendedPageBy(string workspaceId, JobFilter filter, int page, int size)
    {
        // the query argument
        var arg = new
        {
            WorkspaceId = workspaceId,
            filter.UserId,
            filter.Scope,
            filter.Status,
            filter.ClusterId,
            Offset = page * size,
            Count = size
        };
        
        // load page of objects
        var items = await this.dataOps.Connect().Query("Job", "GetExtendedPageBy")
            .WithBinding("ByUser", !string.IsNullOrWhiteSpace(arg.UserId))
            .WithBinding("ByScope", !string.IsNullOrWhiteSpace(arg.Scope))
            .WithBinding("ByStatus", !string.IsNullOrWhiteSpace(arg.Status))
            .WithBinding("ByCluster", !string.IsNullOrWhiteSpace(arg.ClusterId))
            .ExecuteAsync<JobExtendedModel>(arg);
        
        // count total count separately by now
        // fix the multi-query with binding
        var totalCount = await this.dataOps.Connect().QueryFirst("Job", "CountBy")
            .WithBinding("ByUser", !string.IsNullOrWhiteSpace(arg.UserId))
            .WithBinding("ByScope", !string.IsNullOrWhiteSpace(arg.Scope))
            .WithBinding("ByStatus", !string.IsNullOrWhiteSpace(arg.Status))
            .WithBinding("ByCluster", !string.IsNullOrWhiteSpace(arg.ClusterId))
            .ExecuteAsync<long>(arg);
        
        return new JobPageResult<JobExtendedModel>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public Task<JobModel> GetById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Job", "GetById").ExecuteAsync<JobModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }

    /// <summary>
    /// Gets the extended object by id
    /// </summary>
    /// <returns></returns>
    public Task<JobExtendedModel> GetExtendedById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Job", "GetExtendedById").ExecuteAsync<JobExtendedModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }

    /// <summary>
    /// Gets the task objects by id
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<JobTaskModel>> GetTasksById(string workspaceId, string id)
    {
        return this.dataOps.Connect().Query("Job", "GetTasksById").ExecuteAsync<JobTaskModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<JobModel> Create(string workspaceId, JobCreateModel input)
    {
        // the job object
        var job = input.Job;

        // the task list
        var tasks = input.Tasks?.ToList() ?? [];
        
        // the job id
        var jobId = StdIdGenerator.Next(JobObjects.JOB);
        
        // initialize job id and other fields for consistency
        job.WorkspaceId = workspaceId;
        job.Id = jobId;
        job.TotalTasks = tasks.Count;
        
        // bind tasks to job before inserting
        for (var i = 0; i < tasks.Count; ++i)
        {
            // the current task
            var task = tasks[i];
            
            // initialize task fields
            task.Id = StdIdGenerator.Next(JobObjects.JOB_TASK);
            task.WorkspaceId = workspaceId;
            task.JobId = jobId;
            task.UserId = job.UserId;
            task.ClusterId = job.ClusterId;
            
            // the next task 
            task.Sequence = i;
        }

        // the labels
        var labels = input.Labels?.ToList() ?? [];

        // prepare labels
        foreach (var label in labels)
        {
            label.Id = StdIdGenerator.Next(JobObjects.JOB_LABEL);
            label.WorkspaceId = workspaceId;
            label.JobId = jobId;
        }

        // the jobs git repo
        var gitRepo = input.GitRepo;

        // in case if git repo is given process it
        if (gitRepo != null)
        {
            gitRepo.Id = StdIdGenerator.Next(JobObjects.JOB_GIT_REPOSITORY);
            gitRepo.WorkspaceId = workspaceId;
            gitRepo.JobId = jobId;
        }

        // perform the data operation
        return await this.CreateImpl(job, tasks, labels, gitRepo);
    }

    /// <summary>
    /// The implementation of the creation data operation
    /// </summary>
    /// <param name="job">The job object</param>
    /// <param name="tasks">The job tasks</param>
    /// <param name="labels">The job labels</param>
    /// <param name="gitRepo">The job git repo if given</param>
    /// <returns></returns>
    private async Task<JobModel> CreateImpl(JobModel job, IEnumerable<JobTaskModel> tasks, ICollection<JobLabelModel> labels, JobGitRepoModel gitRepo)
    {
        // creates a new transaction scope to ensure all the operations are happening within the scope
        using var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted
        }, TransactionScopeAsyncFlowOption.Enabled);

        // build args for job creation
        var jobArg = new DynamicParameters();
        
        // add job
        jobArg.AddDynamicParams(job);
        
        // add other params
        jobArg.AddDynamicParams(new { JobIdentityType = JobIdentityObjectTypes.JOB });
        
        // create a job record
        var result = await this.dataOps.Connect().QueryFirst("Job", "Create").ExecuteAsync<JobModel>(jobArg);
        
        // create the set of tasks
        await this.dataOps.Connect().NonQuery("Job", "CreateTask").ExecuteAsync(tasks);

        // if there are labels insert those
        if (labels.Count > 0)
        {
            // create the set of tasks
            await this.dataOps.Connect().NonQuery("Job", "CreateLabel").ExecuteAsync(labels);
        }

        // if repository is attached create a record
        if (gitRepo != null)
        {
            await this.dataOps.Connect().NonQuery("Job", "CreateGitRepo").ExecuteAsync(gitRepo);
        }
        
        // complete the transaction
        transactionScope.Complete();

        // return the job
        return result;
    }

    /// <summary>
    /// Updates the namespaces by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="ns">The namespace value</param>
    /// <returns></returns>
    public Task<JobModel> UpdateNamespaceById(string workspaceId, string id, string ns)
    {
        return this.dataOps.Connect().QueryFirst("Job", "UpdateNamespaceById").ExecuteAsync<JobModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id,
            Namespace = ns
        });
    }

    /// <summary>
    /// Update the job to fail all the tasks of job and the job itself
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The job fail input</param>
    /// <returns></returns>
    public Task<JobModel> FailById(string workspaceId, string id, JobFailInputModel input)
    {
        // refer to correct object
        input.WorkspaceId = workspaceId;
        input.Id = id;
        
        return this.dataOps.Connect().QueryFirst("Job", "FailById").ExecuteAsync<JobModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id,
            input.Message,
            input.CompletedAt,
            Status = JobStatuses.FAILED,
            TaskStatus = JobStatuses.FAILED
        });
    }
    
    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<JobModel> DeleteById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Job", "DeleteById").ExecuteAsync<JobModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }
}