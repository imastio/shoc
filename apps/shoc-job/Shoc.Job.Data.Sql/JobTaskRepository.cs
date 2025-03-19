using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Job.Model.JobTask;

namespace Shoc.Job.Data.Sql;

/// <summary>
/// The repository interface implementation
/// </summary>
public class JobTaskRepository : IJobTaskRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public JobTaskRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<JobTaskModel>> GetAll(string workspaceId, string jobId)
    {
        return this.dataOps.Connect().Query("Job.Task", "GetAll").ExecuteAsync<JobTaskModel>(new
        {
            WorkspaceId = workspaceId,
            JobId = jobId
        });
    }

    /// <summary>
    /// Gets all the extended objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<JobTaskExtendedModel>> GetAllExtended(string workspaceId, string jobId)
    {
        return this.dataOps.Connect().Query("Job.Task", "GetAllExtended").ExecuteAsync<JobTaskExtendedModel>(new
        {
            WorkspaceId = workspaceId,
            JobId = jobId
        });
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public Task<JobTaskModel> GetById(string workspaceId, string jobId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Job.Task", "GetByIdOrSequence")
            .WithBinding("ById", true)
            .WithBinding("BySequence", false)
            .ExecuteAsync<JobTaskModel>(new
            {
                WorkspaceId = workspaceId,
                JobId = jobId,
                Id = id
            });
    }

    /// <summary>
    /// Gets the object by sequence
    /// </summary>
    /// <returns></returns>
    public Task<JobTaskModel> GetBySequence(string workspaceId, string jobId, long sequence)
    {
        return this.dataOps.Connect().QueryFirst("Job.Task", "GetByIdOrSequence")
            .WithBinding("ById", false)
            .WithBinding("BySequence", true)
            .ExecuteAsync<JobTaskModel>(new
            {
                WorkspaceId = workspaceId,
                JobId = jobId,
                Sequence = sequence
            });
    }

    /// <summary>
    /// Gets the extended object by id
    /// </summary>
    /// <returns></returns>
    public Task<JobTaskExtendedModel> GetExtendedById(string workspaceId, string jobId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Job.Task", "GetExtendedByIdOrSequence")
            .WithBinding("ById", true)
            .WithBinding("BySequence", false)
            .ExecuteAsync<JobTaskExtendedModel>(new
            {
                WorkspaceId = workspaceId,
                JobId = jobId,
                Id = id
            });
    }

    /// <summary>
    /// Gets the extended object by sequence
    /// </summary>
    /// <returns></returns>
    public Task<JobTaskExtendedModel> GetExtendedBySequence(string workspaceId, string jobId, long sequence)
    {
        return this.dataOps.Connect().QueryFirst("Job.Task", "GetExtendedByIdOrSequence")
            .WithBinding("ById", false)
            .WithBinding("BySequence", true)
            .ExecuteAsync<JobTaskExtendedModel>(new
            {
                WorkspaceId = workspaceId,
                JobId = jobId,
                Sequence = sequence
            });
    }
}