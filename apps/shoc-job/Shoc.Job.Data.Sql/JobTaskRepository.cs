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
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public Task<JobTaskModel> GetById(string workspaceId, string jobId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Job.Task", "GetById").ExecuteAsync<JobTaskModel>(new
        {
            WorkspaceId = workspaceId,
            JobId = jobId,
            Id = id
        });
    }
}