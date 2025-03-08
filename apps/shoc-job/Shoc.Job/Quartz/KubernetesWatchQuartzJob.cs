using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quartz;
using Shoc.Job.Data;
using Shoc.Job.K8s;
using Shoc.Job.Services;

namespace Shoc.Job.Quartz;

/// <summary>
/// A quartz job to sync Kubernetes and Shoc job and task statuses 
/// </summary>
public class KubernetesWatchQuartzJob : IJob
{
    /// <summary>
    /// The job repository
    /// </summary>
    private readonly IJobRepository jobRepository;

    /// <summary>
    /// The task status repository
    /// </summary>
    private readonly IJobTaskStatusRepository taskStatusRepository;
    
    /// <summary>
    /// The task client factory for Kubernetes
    /// </summary>
    protected readonly KubernetesTaskClientFactory taskClientFactory;
    
    /// <summary>
    /// The protection provider
    /// </summary>
    protected readonly JobProtectionProvider jobProtectionProvider;

    /// <summary>
    /// Creates new instance of watch job
    /// </summary>
    /// <param name="jobRepository">The job repository</param>
    /// <param name="taskStatusRepository">The task status repository</param>
    /// <param name="taskClientFactory">The task client factory for Kubernetes</param>
    /// <param name="jobProtectionProvider">The protection provider</param>
    public KubernetesWatchQuartzJob(IJobRepository jobRepository, IJobTaskStatusRepository taskStatusRepository, KubernetesTaskClientFactory taskClientFactory, JobProtectionProvider jobProtectionProvider)
    {
        this.jobRepository = jobRepository;
        this.taskStatusRepository = taskStatusRepository;
        this.taskClientFactory = taskClientFactory;
        this.jobProtectionProvider = jobProtectionProvider;
    }

    /// <summary>
    /// Sync the statuses from Kubernetes with Shoc objects
    /// </summary>
    /// <param name="context">The execution context</param>
    /// <returns></returns>
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            // execute the job logic
            await this.Execute(context);
        }
        catch (Exception e)
        {
            // should fire again
            var refireImmediately = e is FatalQuartzException;
            
            // fire the trigger again immediately
            throw new JobExecutionException(e, refireImmediately);
        }
    }

    /// <summary>
    /// Implementation of job execution
    /// </summary>
    /// <param name="context">The execution context</param>
    /// <returns></returns>
    private async Task ExecuteImpl(IJobExecutionContext context)
    {
        // the data
        var data = context.MergedJobDataMap;

        // the workspace id
        var workspaceId = data.GetValueOrDefault("workspaceId") as string;

        // the job id
        var jobId = data.GetValueOrDefault("jobId") as string;

        // the task id
        var taskId = data.GetValueOrDefault("taskId") as string;

        // ensure workspace id is given
        if (string.IsNullOrWhiteSpace(workspaceId))
        {
            throw new FatalQuartzException("The workspace id is required");
        }
        
        // ensure job id is given
        if (string.IsNullOrWhiteSpace(jobId))
        {
            throw new FatalQuartzException("The job id is required");
        }
        
        // ensure task id is given
        if (string.IsNullOrWhiteSpace(taskId))
        {
            throw new FatalQuartzException("The task id is required");
        }
        
        // require the job 
        var job = await this.jobRepository.GetById(workspaceId, jobId);

        // ensure job exist to continue
        if (job == null)
        {
            return;
        }
        
        Console.WriteLine($"Doing something important");
    }
}